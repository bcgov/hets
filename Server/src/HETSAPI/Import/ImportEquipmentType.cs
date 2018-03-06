using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Equipment Type Records
    /// </summary>
    public static class ImportEquipmentType
    {
        const string OldTable = "Equip_Type";
        const string NewTable = "HET_EQUIPMMENT_TYPE";
        const string XmlFileName = "Equip_Type.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// IMport Equipment Types
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

                // create Processer progress indicator
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
                            EquipmentType instance = null;
                            CopyToInstance(dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        EquipmentType instance = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null) 
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
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BCBidImport.SigId);
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
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipType oldObject, ref EquipmentType instance, string systemId)
        {
            if (oldObject.Equip_Type_Id <= 0)
                return;

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new EquipmentType
                {
                    Id = oldObject.Equip_Type_Id,
                    IsDumpTruck = false
                };                

                try
                {
                    instance.ExtendHours = float.Parse(oldObject.Extend_Hours.Trim());
                    instance.MaximumHours = float.Parse(oldObject.Max_Hours.Trim());
                    instance.MaxHoursSub = float.Parse(oldObject.Max_Hours_Sub.Trim());
                }
                catch
                {
                    // do nothing
                }

                try
                {
                    instance.Name = oldObject.Equip_Type_Cd.Trim();
                }
                catch
                {
                    // do nothing
                }

                instance.AppCreateTimestamp = DateTime.UtcNow;
                instance.AppCreateUserid = createdBy.SmUserId;
                dbContext.EquipmentTypes.Add(instance);
            }
            else
            {
                instance = dbContext.EquipmentTypes.First(x => x.Id == oldObject.Equip_Type_Id);
                instance.AppLastUpdateUserid = modifiedBy.SmUserId;

                try
                {
                    instance.AppLastUpdateTimestamp =  DateTime.ParseExact(oldObject.Modified_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    // do nothing
                }

                dbContext.EquipmentTypes.Update(instance);
            }
        }
    }
}

