using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace HETSAPI.Import
{
    public class ImportEquip
    {

        const string oldTable = "Equip";
        const string newTable = "HET_EQUIPMMENT";
        const string xmlFileName = "Equip.xml";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            
            string rootAttr = "ArrayOf" + oldTable;

            //Create Processer progress indicator
            performContext.WriteLine("Processing " + oldTable);
            var progress = performContext.WriteProgressBar();
            progress.SetValue(0);

            // create serializer and serialize xml file
            XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
            MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
            HETSAPI.Import.Equip[] legacyItems = (HETSAPI.Import.Equip[])ser.Deserialize(memoryStream);
            int ii = 0;
            foreach (var item in legacyItems.WithProgress(progress))
            {
                // see if we have this one already.
                ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Id.ToString());

                if (importMap == null) // new entry
                {
                    if (item.Equip_Id > 0)
                    {
                        Models.Equipment instance = null;
                        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.Equip_Id.ToString(), newTable, instance.Id);
                    }
                }
                else // update
                {
                    Models.Equipment instance = dbContext.Equipments.FirstOrDefault(x => x.Id == importMap.NewKey);
                    if (instance == null) // record was deleted
                    {
                        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                        // update the import map.
                        importMap.NewKey = instance.Id;
                        dbContext.ImportMaps.Update(importMap);
                    }
                    else // ordinary update.
                    {
                        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                        // touch the import map.
                        importMap.LastUpdateTimestamp = DateTime.UtcNow;
                        dbContext.ImportMaps.Update(importMap);
                    }
                }

                if (ii++ % 1000 == 0)
                {
                    try
                    {
                        int iResult = dbContext.SaveChangesForImport();
                    }
                    catch (Exception e)
                    {
                        string iStr = e.ToString();
                    }
                }
            }
            try
            {
                int iResult = dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
            performContext.WriteLine("*** Done ***");
        }


        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Equip oldObject, ref Models.Equipment instance, string systemId)
        {
            bool isNew = false;

            if (oldObject.Equip_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.Equipment();
                instance.Id = oldObject.Equip_Id;
                
                // instance.DumpTruckId = oldObject.Reg_Dump_Trk;
                instance.ArchiveCode = oldObject.Archive_Cd == null ? "" : new string(oldObject.Archive_Cd.Take(50).ToArray());
                instance.ArchiveReason = oldObject.Archive_Reason == null ? "" : new string(oldObject.Archive_Reason.Take(2048).ToArray());
                instance.LicencePlate = oldObject.Licence == null ? "" : new string(oldObject.Licence.Take(20).ToArray());

                if (oldObject.Comment != null)
                {
                    instance.Notes = new List<Note>();
                    instance.Notes.Add(new Note(oldObject.Comment, true));
                }

                // instance.ArchiveDate = oldObject. 

                if (oldObject.Area_Id != null)
                {
                    LocalArea area = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);
                    if (area != null)
                        instance.LocalArea = area;
                }

                if (oldObject.Equip_Type_Id != null)
                {
                    //Equipment_TYPE_ID is copied to the table of HET_DISTRICT_DISTRICT_TYPE as key
                    DistrictEquipmentType equipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == oldObject.Equip_Type_Id);
                    if (equipType != null)
                    {
                        instance.DistrictEquipmentType = equipType;
                        instance.DistrictEquipmentTypeId = oldObject.Equip_Type_Id;
                    }
                }

                instance.EquipmentCode = oldObject.Equip_Cd == null ? "" : new string(oldObject.Equip_Cd.Take(25).ToArray());
                instance.Model = oldObject.Model == null ? "" : new string(oldObject.Model.Take(50).ToArray());
                instance.Make = oldObject.Make == null ? "" : new string(oldObject.Make.Take(50).ToArray());
                instance.Year = oldObject.Year == null ? "" : new string(oldObject.Year.Take(15).ToArray());
                instance.Operator = oldObject.Operator == null ? "" : new string(oldObject.Operator.Take(255).ToArray());

                // instance.RefuseRate = oldObject.Refuse_Rate == null ? "" : oldObject.Refuse_Rate;
                instance.SerialNumber = oldObject.Serial_Num == null ? "" : new string(oldObject.Serial_Num.Take(100).ToArray());
                instance.Status = oldObject.Status_Cd == null ? "" : new string(oldObject.Status_Cd.Take(50).ToArray());

                if (oldObject.Pay_Rate != null)
                {
                    try
                    {
                        instance.PayRate = float.Parse(oldObject.Pay_Rate.Trim());
                    }
                    catch (Exception e)
                    {
                        instance.PayRate = 0;
                    }

                }

                if (instance.Seniority != null)
                {
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch (Exception e)
                    {
                        instance.Seniority = 0;
                    }
                }
                // Find the owner which is referenced in the equipment of the xml file entry
                ImportMap map = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == ImportOwner.oldTable && x.OldKey == oldObject.Owner_Popt_Id.ToString());
                if (map != null)
                {
                    Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == map.NewKey);
                    if (owner != null)
                    {
                        instance.Owner = owner;
                    }
                }
                // instance.OwnerId = owner.Id;
                
                if (oldObject.Seniority != null)
                {
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch (Exception e)
                    {
                        instance.Seniority = 0;
                    }
                }
                if (oldObject.Num_Years != null)
                {
                    try
                    {
                        instance.YearsOfService = float.Parse(oldObject.Num_Years.Trim());
                    }
                    catch (Exception e)
                    {
                        instance.YearsOfService = 0;
                    }
                }

                if (oldObject.YTD1 != null && oldObject.YTD2 != null && oldObject.YTD3 != null)
                {
                    try
                    {
                        instance.ServiceHoursLastYear = (float)Decimal.Parse(oldObject.YTD1, System.Globalization.NumberStyles.Any);
                    }
                    catch (Exception e)
                    {
                        instance.ServiceHoursLastYear = (float)0;
                    }
                    try
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)Decimal.Parse(oldObject.YTD2, System.Globalization.NumberStyles.Any); 
                        instance.ServiceHoursThreeYearsAgo = (float)Decimal.Parse(oldObject.YTD3, System.Globalization.NumberStyles.Any);  
                    }
                    catch (Exception e)
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)0;
                        instance.ServiceHoursThreeYearsAgo = (float)0;
                    }
                }

                instance.ReceivedDate = DateTime.Parse(oldObject.Received_Dt == null ? "1900-01-01" : oldObject.Received_Dt.Trim().Substring(0, 10));

                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;
                dbContext.Equipments.Add(instance);
            }
            else
            {
                instance = dbContext.Equipments
                    .First(x => x.Id == oldObject.Equip_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                try
                {
                    instance.LastUpdateUserid = modifiedBy.SmUserId;
                    instance.LastUpdateTimestamp = DateTime.Parse(oldObject.Modified_Dt.Trim().Substring(0, 10));
                }
                catch
                {

                }
                dbContext.Equipments.Update(instance);
            }
        }
    }
}
