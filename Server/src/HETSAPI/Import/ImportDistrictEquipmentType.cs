using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using ServiceArea = HETSAPI.Models.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import District Equipment Type Records
    /// </summary>
    public static class ImportDistrictEquipmentType
    {
        const string OldTable = "EquipType";
        const string NewTable = "HET_EQUIPMMENT_TYPE";
        const string XmlFileName = "Equip_Type.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import District Equipment Types
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);

                int ii = 0;

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            DistrictEquipmentType instance = null;
                            CopyToInstance(dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        DistrictEquipmentType instance = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // update the import map
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // touch the import map
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 250 == 0)
                    {
                        try
                        {
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            performContext.WriteLine("Error saving data " + e.Message);
                        }
                    }
                }

                try
                {
                    performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BCBidImport.SigId.ToString(), BCBidImport.SigId);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    performContext.WriteLine("Error saving data " + e.Message);
                }
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
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipType oldObject, ref DistrictEquipmentType instance, string systemId)
        {
            if (oldObject.Equip_Type_Id <= 0)
                return;

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new DistrictEquipmentType {Id = oldObject.Equip_Type_Id};

                try 
                {
                    instance.DistrictEquipmentName = oldObject.Equip_Type_Cd.Length >= 10 ? 
                        oldObject.Equip_Type_Cd.Substring(0, 10) : 
                        oldObject.Equip_Type_Cd
                       + "-" + ( oldObject.Equip_Type_Desc.Length >= 210 ? 
                            oldObject.Equip_Type_Desc.Substring(0, 210) : 
                            oldObject.Equip_Type_Desc);
                }
                catch
                {
                    // do nothing
                }

                ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);

                if (serviceArea != null)
                {
                    int districtId = serviceArea.DistrictId ?? 0;
                    District dis = dbContext.Districts.FirstOrDefault(x => x.RegionId == districtId);
                    instance.DistrictId = districtId;
                    instance.District = dis;
                }

                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;              
                dbContext.DistrictEquipmentTypes.Add(instance);
            }
        }
    }
}


