///  Import Equipment.xml into HET_EQUIPMENT TABLE.
///  1. Using Import_MAP to find out the Owner_ID through Owner_Prop_ID
///  2. Using the address information here to update the Equipment Owner's main contact address
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
        public static string oldTable_Progress = oldTable + "_Progress";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // Check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, oldTable_Progress, BCBidImport.sigId);
            if (startPoint == BCBidImport.sigId)    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + xmlFileName + " is complete from the former process ***");
                return;
            }

            string rootAttr = "ArrayOf" + oldTable;

            //Create Processer progress indicator
            performContext.WriteLine("Processing " + oldTable);
            var progress = performContext.WriteProgressBar();
            progress.SetValue(0);

            // create serializer and serialize xml file
            XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
            MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
            HETSAPI.Import.Equip[] legacyItems = (HETSAPI.Import.Equip[])ser.Deserialize(memoryStream);

            int ii = startPoint;
            if (startPoint > 0)    // Skip the portion already processed
            {
                legacyItems = legacyItems.Skip(ii).ToArray();
            }

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
                    if (instance == null && item.Equip_Id > 0) // record was deleted
                    {
                        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                        // update the import map.
                        importMap.NewKey = instance.Id;
                        dbContext.ImportMaps.Update(importMap);
                    }
                    else // ordinary update.
                    {
                        if (item.Equip_Id > 0)
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }
                }

                if (ii++ % 1000 == 0)   // Save change to database once a while to avoid frequent writing to the database.
                {
                    try
                    {
                        ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, ii.ToString(), BCBidImport.sigId);
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
                performContext.WriteLine("*** Importing " + xmlFileName + " is Done ***");
                ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, BCBidImport.sigId.ToString(), BCBidImport.sigId);
                int iResult = dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
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

                if (oldObject.Approved_Dt != null)
                {
                    instance.ApprovedDate = DateTime.ParseExact(oldObject.Approved_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (oldObject.Received_Dt != null)
                {
                    instance.ReceivedDate = DateTime.ParseExact(oldObject.Received_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.Comment != null)
                {
                    instance.Notes = new List<Note>();
                    Models.Note note = new Note();
                    note.Text = new string(oldObject.Comment.Take(2048).ToArray());
                    note.IsNoLongerRelevant = true;
                    instance.Notes.Add(note);
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
                        instance.PayRate = (float)0.0;
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
                        instance.Seniority = (float)0.0;
                    }
                }
                // Find the owner which is referenced in the equipment of the xml file entry Through ImportMaps because owner_ID is not prop_ID
                ImportMap map = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == ImportOwner.oldTable && x.OldKey == oldObject.Owner_Popt_Id.ToString());
                if (map != null)
                {
                    Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == map.NewKey);
                    if (owner != null)
                    {
                        instance.Owner = owner;
                        Models.Contact con = dbContext.Contacts.FirstOrDefault(x => x.Id == owner.PrimaryContactId);
                        if (con != null)            //This is used to update owner contact address
                        {
                            try
                            {
                                con.Address1 = oldObject.Addr1;
                                con.Address2 = oldObject.Addr2;
                                con.City = oldObject.City;
                                con.PostalCode = oldObject.Postal;
                                con.Province = "BC";
                                dbContext.Contacts.Update(con);
                            }
                            catch (Exception e)
                            {
                                string str = e.ToString();
                            }
                        }
                    }
                }
                
                if (oldObject.Seniority != null)
                {
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch (Exception e)
                    {
                        instance.Seniority = (float)0.0;
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
                        instance.YearsOfService = (float)0.0;
                    }
                }
                if (oldObject.Block_Num != null)
                {
                    try
                    {
                        instance.BlockNumber = decimal.ToInt32(Decimal.Parse(oldObject.Block_Num, System.Globalization.NumberStyles.Float));
                    }
                    catch (Exception e)
                    {
 
                    }
                }
                if (oldObject.Size != null)
                {
                    try
                    {
                        instance.Size = oldObject.Size;
                    }
                    catch (Exception e)
                    {

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
                        instance.ServiceHoursLastYear = (float)0.0;
                    }
                    try
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)Decimal.Parse(oldObject.YTD2, System.Globalization.NumberStyles.Any); 
                        instance.ServiceHoursThreeYearsAgo = (float)Decimal.Parse(oldObject.YTD3, System.Globalization.NumberStyles.Any);  
                    }
                    catch (Exception e)
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)0.0;
                        instance.ServiceHoursThreeYearsAgo = (float)0.0;
                    }
                }

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
                    instance.LastUpdateTimestamp = DateTime.ParseExact(oldObject.Modified_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    string str = e.ToString();
                }
                dbContext.Equipments.Update(instance);
            }
        }
    }
}
