using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using System.Collections.Generic;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Block Records
    /// </summary>
    public static class ImportBlock
    {
        const string OldTable = "Block";
        const string NewTable = "HET_LOCAL_AREA_ROTATION_LIST";
        const string XmlFileName = "Block.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Rotaion List
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(Block[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Block[] legacyItems = (Block[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (Block item in legacyItems.WithProgress(progress))
                {
                    int areaId = item.Area_Id ?? 0;
                    int equipmentTypeId = item.Equip_Type_Id ?? 0;
                    int blockNum = Convert.ToInt32(float.Parse(item.Block_Num ?? "0.0"));

                    // this is for conversion record hope this is unique
                    string oldUniqueId = ((areaId * 10000 + equipmentTypeId) * 100 + blockNum).ToString();

                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldUniqueId);

                    // new entry
                    if (importMap == null) 
                    {
                        if (areaId > 0)
                        {
                            LocalAreaRotationList instance = null;
                            CopyToInstance(dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, oldUniqueId, NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        LocalAreaRotationList instance = dbContext.LocalAreaRotationLists.FirstOrDefault(x => x.Id == importMap.NewKey);

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
                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId);
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
        /// Copy Block item of LocalAreaRotationList item
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, Block oldObject, ref LocalAreaRotationList instance, string systemId)
        {
            if (oldObject.Area_Id <= 0)
                return;

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            int equipmentTypeId = oldObject.Equip_Type_Id ?? 0;
            int blockNum = Convert.ToInt32(float.Parse(oldObject.Block_Num == null ? "0.0" : oldObject.Block_Num));
            
            if (instance == null)
            {
                instance = new LocalAreaRotationList();
                
                DistrictEquipmentType disEquipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == equipmentTypeId);
                if (disEquipType != null)
                {
                    instance.DistrictEquipmentType = disEquipType;
                    instance.DistrictEquipmentTypeId = disEquipType.Id;
                }

                // extract AskNextBlock*Id which is the secondary key of Equip.Id
                int equipId = oldObject.Last_Hired_Equip_Id ?? 0;

                if (dbContext.Equipments.Any(x => x.Id == equipId))   
                {
                    switch (blockNum)
                    {
                        case 1:
                            instance.AskNextBlockOpenId = equipId;
                            break;
                        case 2:
                            instance.AskNextBlock1Id = equipId;
                            break;
                        case 3:
                            instance.AskNextBlock2Id = equipId;
                            break;                        
                    }
                }

                instance.AppCreateUserid = createdBy.SmUserId;

                if (oldObject.Created_Dt != null)
                {
                    instance.AppCreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                dbContext.LocalAreaRotationLists.Add(instance);
            }            
        }


        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Obfuscating " + XmlFileName + " is complete from the former process ***");
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Block[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Block[] legacyItems = (ImportModels.Block[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating Block data");
                progress.SetValue(0);
                
                foreach (ImportModels.Block item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;                    
                    
                    item.Closed_Comments = ImportUtility.ScrambleString(item.Closed_Comments);
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);
                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
                // no excel for Block.

            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


