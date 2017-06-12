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
    public class ImportDistrictEquipmentType
    {

        const string oldTable = "EquipType";
        const string newTable = "HET_EQUIPMMENT_TYPE";
        const string xmlFileName = "Equip_Type.xml";
        const int sigId = 150000;

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            string completed = DateTime.Now.ToString("d") + "-" + "Completed";
            ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == completed && x.NewKey == sigId);
            if (importMap != null)
            {
                return;
            }
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.EquipType[] legacyItems = (HETSAPI.Import.EquipType[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            Models.DistrictEquipmentType instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, item.Equip_Type_Id.ToString(), newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.DistrictEquipmentType instance = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);
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
                    if (ii++ % 250 == 0)
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
                ImportUtility.AddImportMap(dbContext, oldTable, completed, newTable, sigId);
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy xml item to instance (table entries)
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipType oldObject, ref Models.DistrictEquipmentType instance, string systemId)
        {
            bool isNew = false;

            if (oldObject.Equip_Type_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.DistrictEquipmentType();
                instance.Id = oldObject.Equip_Type_Id;



                try  //Combining <Equip_Type_Cd> and < Equip_Type_Desc> together
                {
                    instance.DistrictEquipmentName = oldObject.Equip_Type_Cd.Length >= 10 ? oldObject.Equip_Type_Cd.Substring(0, 10) : oldObject.Equip_Type_Cd
                       +"-" +  ( oldObject.Equip_Type_Desc.Length >= 210 ? oldObject.Equip_Type_Desc.Substring(0, 210) : oldObject.Equip_Type_Desc);
                   // instance.DistrictEquipmentName = oldObject.ToDelimString(" | ");
                }
                catch
                {

                }

                ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);
                if (serviceArea != null)
                {
                    int districtId = serviceArea.DistrictId ?? 0;
                    District dis = dbContext.Districts.FirstOrDefault(x => x.RegionId == districtId);
                    instance.DistrictId = districtId;
                    instance.District = dis;
                }

                //    instance.EquipmentType = 
                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;              
                dbContext.DistrictEquipmentTypes.Add(instance);
            }

        }

    }
}


