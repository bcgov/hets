using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Console.Progress;
using HetsData.Model;

namespace HetsImport.Import
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
        /// Fix the sequence for the tables populated by the import process
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        public static void ResetSequence(PerformContext performContext, DbAppContext dbContext)
        {
            try
            {
                performContext.WriteLine("*** Resetting HET_LOCAL_AREA_ROTATION_LIST database sequence after import ***");
                Debug.WriteLine("Resetting HET_LOCAL_AREA_ROTATION_LIST database sequence after import");

                if (dbContext.HetLocalAreaRotationList.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetLocalAreaRotationList.Max(x => x.LocalAreaRotationListId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_LOCAL_AREA_ROTATION_LIST_LOCAL_AREA_ROTATION_LIST_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_LOCAL_AREA_ROTATION_LIST database sequence after import ***");
                Debug.WriteLine("Resetting HET_LOCAL_AREA_ROTATION_LIST database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Import Rotation List
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxBlockIndex = 0;

            if (dbContext.HetRentalRequestRotationList.Any())
            {
                maxBlockIndex = dbContext.HetRentalRequestRotationList.Max(x => x.RentalRequestRotationListId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Block[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.Block[] legacyItems = (ImportModels.Block[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Block Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.Block item in legacyItems.WithProgress(progress))
                {
                    string areaId = item.Area_Id.ToString();
                    string equipmentTypeId = item.Equip_Type_Id.ToString();
                    string createdDate = item.Created_Dt;
                    string oldUniqueId = string.Format("{0}-{1}-{2}", areaId, equipmentTypeId, createdDate);

                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == oldUniqueId);

                    // new entry
                    if (importMap == null && item.Area_Id > 0)
                    {
                        HetLocalAreaRotationList instance = null;
                        CopyToInstance(dbContext, item, ref instance, systemId, ref maxBlockIndex);

                        if (instance != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldUniqueId, NewTable, instance.LocalAreaRotationListId);
                        }
                    }                    

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId, NewTable);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, NewTable);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string temp = string.Format("Error saving data (BlockIndex: {0}): {1}", maxBlockIndex, e.Message);
                    performContext.WriteLine(temp);
                    throw new DataException(temp);
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }            
        }

        /// <summary>
        /// Copy Block item of LocalAreaRotationList item
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rotationList"></param>
        /// <param name="systemId"></param>
        /// <param name="maxBlockIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Block oldObject, 
            ref HetLocalAreaRotationList rotationList, string systemId, ref int maxBlockIndex)
        {
            try
            {
                bool isNew = false;

                if (oldObject.Area_Id <= 0)
                {
                    return; // ignore these records
                }

                if (oldObject.Equip_Type_Id <= 0)
                {
                    return; // ignore these records
                }

                if (oldObject.Last_Hired_Equip_Id <= 0)
                {
                    return; // ignore these records
                }

                // ***********************************************
                // we only need records from the current fiscal
                // so ignore all others
                // ***********************************************
                DateTime fiscalStart;
                DateTime fiscalEnd;

                if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
                {
                    fiscalEnd = new DateTime(DateTime.UtcNow.Year, 3, 31);
                }
                else
                {
                    fiscalEnd = new DateTime(DateTime.UtcNow.AddYears(1).Year, 3, 31);
                }                

                if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
                }
                else
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
                }

                string tempRecordDate = oldObject.Created_Dt;

                if (string.IsNullOrEmpty(tempRecordDate))
                {
                    return; // ignore if we don't have a created date
                }

                if (!string.IsNullOrEmpty(tempRecordDate))
                {
                    DateTime? recordDate = ImportUtility.CleanDateTime(tempRecordDate);

                    if (recordDate == null ||
                        recordDate < fiscalStart ||
                        recordDate > fiscalEnd)
                    {
                        return; // ignore this record - it is outside of the current fiscal year
                    }
                }

                // ***********************************************
                // get the area record
                // ***********************************************
                string tempOldAreaId = oldObject.Area_Id.ToString();

                HetImportMap mapArea = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempOldAreaId &&
                                         x.OldTable == ImportLocalArea.OldTable &&
                                         x.NewTable == ImportLocalArea.NewTable);

                if (mapArea == null)
                {
                    throw new DataException(string.Format("Area Id cannot be null (BlockIndex: {0})", maxBlockIndex));
                }

                HetLocalArea area = dbContext.HetLocalArea.AsNoTracking()
                    .FirstOrDefault(x => x.LocalAreaId == mapArea.NewKey);

                if (area == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate Local Area record (Local Area Id: {0})", tempOldAreaId));
                }

                // ***********************************************
                // get the equipment type record
                // ***********************************************
                string tempOldEquipTypeId = oldObject.Equip_Type_Id.ToString();

                HetImportMap mapEquipType = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempOldEquipTypeId &&
                                         x.OldTable == ImportDistrictEquipmentType.OldTable &&
                                         x.NewTable == ImportDistrictEquipmentType.NewTable);

                if (mapEquipType == null)
                {
                    throw new DataException(string.Format("Equipment Type Id cannot be null (BlockIndex: {0})", maxBlockIndex));
                }

                HetDistrictEquipmentType equipmentType = dbContext.HetDistrictEquipmentType.AsNoTracking()
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == mapEquipType.NewKey);

                if (equipmentType == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate District Equipment Type record (Equipment Type Id: {0})", tempOldEquipTypeId));
                }

                // ***********************************************
                // see if a record already exists
                // ***********************************************
                rotationList = dbContext.HetLocalAreaRotationList
                    .FirstOrDefault(x => x.LocalAreaId == area.LocalAreaId &&
                                         x.DistrictEquipmentTypeId == equipmentType.DistrictEquipmentTypeId);

                if (rotationList == null)
                {
                    isNew = true;

                    // create new list
                    rotationList = new HetLocalAreaRotationList
                    {
                        LocalAreaRotationListId = ++maxBlockIndex,
                        LocalAreaId = area.LocalAreaId,
                        DistrictEquipmentTypeId = equipmentType.DistrictEquipmentTypeId,
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow
                    };
                }
                else
                {
                    // record already exists - just testing updates 
                    Debug.WriteLine("Record Id: " + rotationList.LocalAreaRotationListId);
                }

                // ***********************************************
                // get the equipment record
                // ***********************************************
                string tempOldEquipId = oldObject.Last_Hired_Equip_Id.ToString();

                HetImportMap mapEquip = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempOldEquipId &&
                                         x.OldTable == ImportEquip.OldTable &&
                                         x.NewTable == ImportEquip.NewTable);

                if (mapEquip == null)
                {
                    throw new DataException(string.Format("Equipment Id cannot be null (BlockIndex: {0})", maxBlockIndex));
                }

                HetEquipment equipment = dbContext.HetEquipment.AsNoTracking()
                    .FirstOrDefault(x => x.EquipmentId == mapEquip.NewKey);

                if (equipment == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate Equipment record (Equipment Id: {0})", tempOldEquipId));
                }

                // ***********************************************
                // update the "Ask Next" values
                // ***********************************************                
                float? blockNum = ImportUtility.GetFloatValue(oldObject.Block_Num);

                if (blockNum == null)
                {
                    throw new DataException(string.Format("Block Number cannot be null (BlockIndex: {0}", maxBlockIndex));
                }
                            
                // extract AskNextBlock*Id which is the secondary key of Equip.Id                
                switch (blockNum)
                {
                    case 1:
                        rotationList.AskNextBlock1Id = equipment.EquipmentId;
                        rotationList.AskNextBlock1Seniority = equipment.Seniority;
                        break;
                    case 2:
                        rotationList.AskNextBlock2Id = equipment.EquipmentId;
                        rotationList.AskNextBlock2Seniority = equipment.Seniority;
                        break;
                    case 3:                        
                        rotationList.AskNextBlockOpenId = equipment.EquipmentId;
                        break;                        
                }
                
                // ***********************************************
                // update or create equipment
                // ***********************************************  
                equipment.AppLastUpdateUserid = systemId;
                equipment.AppLastUpdateTimestamp = DateTime.UtcNow;

                if (isNew)
                {
                    dbContext.HetLocalAreaRotationList.Add(rotationList);
                }
                else
                {
                    dbContext.HetLocalAreaRotationList.Update(rotationList);
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Master Block Index: " + maxBlockIndex);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Obfuscating " + XmlFileName + " is complete from the former process ***");
                return;
            }
            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
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
                
                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


