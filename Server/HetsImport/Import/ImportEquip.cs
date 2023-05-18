using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Console.Progress;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsImport.Import
{
    /// <summary>
    /// Import Equipment Records
    /// </summary>
    public static class ImportEquip
    {
        public const string OldTable = "Equip";
        public const string NewTable = "HET_EQUIPMENT";
        public const string XmlFileName = "Equip.xml";

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
                performContext.WriteLine("*** Resetting HET_EQUIPMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_EQUIPMENT database sequence after import");

                if (dbContext.HetEquipment.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetEquipment.Max(x => x.EquipmentId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_EQUIPMENT_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_EQUIPMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_EQUIPMENT database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Recalculate the block assignment for each piece of equipment
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="seniorityScoringRules"></param>
        /// <param name="dbContext"></param>
        /// <param name="systemId"></param>
        public static void ProcessBlocks(PerformContext performContext, string seniorityScoringRules, DbAppContext dbContext, string systemId)
        {
            try
            {
                performContext.WriteLine("*** Recalculating Equipment Block Assignment ***");
                Debug.WriteLine("Recalculating Equipment Block Assignment");

                int ii = 0;
                string _oldTableProgress = "BlockAssignment_Progress";
                string _newTable = "BlockAssignment";

                // check if the block assignment has already been completed
                int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, _oldTableProgress, BcBidImport.SigId, _newTable);

                if (startPoint == BcBidImport.SigId)    // this means the assignment job is complete
                {
                    performContext.WriteLine("*** Recalculating Equipment Block Assignment is complete from the former process ***");
                    return;
                }

                // ************************************************************
                // cleanup old block assignment status records
                // ************************************************************
                List<HetImportMap> importMapList = dbContext.HetImportMap
                    .Where(x => x.OldTable == _oldTableProgress &&
                                x.NewTable == _newTable)
                    .ToList();

                foreach (HetImportMap importMap in importMapList)
                {
                    dbContext.HetImportMap.Remove(importMap);                    
                }

                dbContext.SaveChangesForImport();

                // ************************************************************
                // get processing rules
                // ************************************************************
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(seniorityScoringRules, (errMessage, ex) => {
                    performContext.WriteLine(errMessage);
                    performContext.WriteLine(ex.ToString());
                });

                // ************************************************************
                // get all local areas 
                // (using active equipment to minimize the results)
                // ************************************************************
                List<HetLocalArea> localAreas = dbContext.HetEquipment.AsNoTracking()
                    .Include(x => x.EquipmentStatusType)
                    .Include(x => x.LocalArea)
                    .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved &&
                                x.ArchiveCode == "N")
                    .Select(x => x.LocalArea)
                    .Distinct()
                    .ToList();
                
                // ************************************************************************
                // iterate the data and update the assignment blocks 
                // (seniority is already calculated)
                // ************************************************************************
                Debug.WriteLine("Recalculating Equipment Block Assignment - Local Area Record Count: " + localAreas.Count);

                foreach (HetLocalArea localArea in localAreas)
                {
                    IQueryable<HetDistrictEquipmentType> equipmentTypes = dbContext.HetEquipment.AsNoTracking()
                        .Include(x => x.EquipmentStatusType)
                        .Include(x => x.DistrictEquipmentType)
                        .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved &&
                                    x.ArchiveCode == "N" &&
                                    x.LocalArea.LocalAreaId == localArea.LocalAreaId)
                        .Select(x => x.DistrictEquipmentType)
                        .Distinct();

                    foreach (HetDistrictEquipmentType districtEquipmentType in equipmentTypes)
                    {
                        // get the associated equipment type
                        HetEquipmentType equipmentTypeRecord = dbContext.HetEquipmentType.AsNoTracking()
                            .FirstOrDefault(x => x.EquipmentTypeId == districtEquipmentType.EquipmentTypeId);

                        if (equipmentTypeRecord == null)
                        {
                            throw new DataException(string.Format("Invalid District Equipment Type. No associated Equipment Type record (District Equipment Id: {0})", districtEquipmentType.DistrictEquipmentTypeId));
                        }

                        // get rules                  
                        int blockSize = equipmentTypeRecord.IsDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = equipmentTypeRecord.IsDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        // assign blocks
                        SeniorityListHelper.AssignBlocks(
                            localArea.LocalAreaId, districtEquipmentType.DistrictEquipmentTypeId, blockSize, totalBlocks, dbContext, 
                            (errMessage, ex) => {
                                performContext.WriteLine(errMessage);
                                performContext.WriteLine(ex.ToString());
                            },
                            null);

                        // save change to database
                        if (ii++ % 100 == 0)
                        {
                            try
                            {
                                Debug.WriteLine("Recalculating Equipment Block Assignment - Index: " + ii);
                                ImportUtility.AddImportMapForProgress(dbContext, _oldTableProgress, ii.ToString(), BcBidImport.SigId, _newTable);
                                dbContext.SaveChangesForImport();
                            }
                            catch (Exception e)
                            {
                                performContext.WriteLine("Error saving data " + e.Message);
                            }
                        }
                    }
                }

                // ************************************************************
                // save final set of updates
                // ************************************************************
                try
                {
                    performContext.WriteLine("*** Recalculating Equipment Block Assignment is Done ***");
                    Debug.WriteLine("Recalculating Equipment Block Assignment is Done");
                    ImportUtility.AddImportMapForProgress(dbContext, _oldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, _newTable);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string temp = string.Format("Error saving data (Record: {0}): {1}", ii, e.Message);
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
        /// Import Equipment
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxEquipmentIndex = 0;

            if (dbContext.HetEquipment.Any())
            {
                maxEquipmentIndex = dbContext.HetEquipment.Max(x => x.EquipmentId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.Equip[] legacyItems = (ImportModels.Equip[]) ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Equipment Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.Equip item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Equip_Id.ToString());

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        HetEquipment instance = null;
                        CopyToInstance(dbContext, item, ref instance, systemId, ref maxEquipmentIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable, instance.EquipmentId);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 1000 == 0)
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
                    string temp = string.Format("Error saving data (EquipmentIndex: {0}): {1}", maxEquipmentIndex, e.Message);
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
        /// Map data 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="equipment"></param>
        /// <param name="systemId"></param>
        /// <param name="maxEquipmentIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Equip oldObject, 
            ref HetEquipment equipment, string systemId, ref int maxEquipmentIndex)
        {
            try
            {
                int? statusIdApproved = StatusHelper.GetStatusId("Approved", "equipmentStatus", dbContext);
                int? statusIdUnapproved = StatusHelper.GetStatusId("Unapproved", "equipmentStatus", dbContext);
                int? statusIdArchived = StatusHelper.GetStatusId("Archived", "equipmentStatus", dbContext);                

                if (oldObject.Equip_Id <= 0)
                {
                    return;
                }

                equipment = new HetEquipment { EquipmentId = ++maxEquipmentIndex };

                // there is a problem with 1 equipment record
                // Per BC Bid - the correct Owner Id should be: 8786195
                if (oldObject.Equip_Id == 19165)
                {
                    oldObject.Owner_Popt_Id = 8786195;
                }

                // ***********************************************
                // equipment code
                // ***********************************************
                string tempEquipmentCode = ImportUtility.CleanString(oldObject.Equip_Cd).ToUpper();

                if (!string.IsNullOrEmpty(tempEquipmentCode))
                {
                    equipment.EquipmentCode = tempEquipmentCode;
                }
                else
                {
                    // must have an equipment code: HETS-817
                    return;
                }

                // ***********************************************
                // set the equipment status
                // ***********************************************
                string tempArchive = oldObject.Archive_Cd;
                string tempStatus = oldObject.Status_Cd.Trim();

                if (tempArchive == "Y")
                {
                    if (statusIdArchived == null)
                    {
                        throw new DataException(string.Format("Status Id cannot be null (EquipmentIndex: {0})", maxEquipmentIndex));
                    }

                    // archived!
                    equipment.ArchiveCode = "Y";
                    equipment.ArchiveDate = DateTime.UtcNow;
                    equipment.ArchiveReason = "Imported from BC Bid";
                    equipment.EquipmentStatusTypeId = (int)statusIdArchived;

                    string tempArchiveReason = ImportUtility.CleanString(oldObject.Archive_Reason);

                    if (!string.IsNullOrEmpty(tempArchiveReason))
                    {
                        equipment.ArchiveReason = ImportUtility.GetUppercaseFirst(tempArchiveReason);
                    }
                }
                else
                {                    
                    if (statusIdApproved == null)
                    {
                        throw new DataException(string.Format("Status Id cannot be null (EquipmentIndex: {0})", maxEquipmentIndex));
                    }
                    
                    if (statusIdUnapproved == null)
                    {
                        throw new DataException(string.Format("Status Id cannot be null (EquipmentIndex: {0})", maxEquipmentIndex));
                    }

                    equipment.ArchiveCode = "N";
                    equipment.ArchiveDate = null;
                    equipment.ArchiveReason = null;
                    equipment.EquipmentStatusTypeId = tempStatus == "A" ? (int)statusIdApproved : (int)statusIdUnapproved;
                    equipment.StatusComment = string.Format("Imported from BC Bid ({0})", tempStatus);
                }

                // ***********************************************
                // set equipment attributes
                // ***********************************************
                string tempLicense = ImportUtility.CleanString(oldObject.Licence).ToUpper();

                if (!string.IsNullOrEmpty(tempLicense))
                {
                    equipment.LicencePlate = tempLicense;
                }

                equipment.ApprovedDate = ImportUtility.CleanDate(oldObject.Approved_Dt);

                DateTime? tempReceivedDate = ImportUtility.CleanDate(oldObject.Received_Dt);

                if (tempReceivedDate != null)
                {
                    equipment.ReceivedDate = (DateTime)tempReceivedDate;
                }
                else
                {
                    if (equipment.ArchiveCode == "N" &&                         
                        equipment.EquipmentStatusTypeId == statusIdApproved)
                    {
                        throw new DataException(string.Format("Received Date cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                    }                    
                }

                // get the created date and use the timestamp to fix the 
                DateTime? tempCreatedDate = ImportUtility.CleanDateTime(oldObject.Created_Dt);

                if (tempCreatedDate != null)
                {
                    int hours = Convert.ToInt32(tempCreatedDate?.ToString("HH"));
                    int minutes = Convert.ToInt32(tempCreatedDate?.ToString("mm"));
                    int secs = Convert.ToInt32(tempCreatedDate?.ToString("ss"));

                    equipment.ReceivedDate = equipment.ReceivedDate.AddHours(hours);
                    equipment.ReceivedDate = equipment.ReceivedDate.AddMinutes(minutes);
                    equipment.ReceivedDate = equipment.ReceivedDate.AddSeconds(secs);                    
                }

                // pay rate
                float? tempPayRate = ImportUtility.GetFloatValue(oldObject.Pay_Rate);

                if (tempPayRate != null)
                {
                    equipment.PayRate = tempPayRate;                    
                }

                // ***********************************************
                // make, model, year, etc.
                // ***********************************************
                string tempType = ImportUtility.CleanString(oldObject.Type);

                if (!string.IsNullOrEmpty(tempType))
                {
                    tempType = ImportUtility.GetCapitalCase(tempType);
                    equipment.Type = tempType;
                }

                string tempMake = ImportUtility.CleanString(oldObject.Make);

                if (!string.IsNullOrEmpty(tempMake))
                {
                    tempMake = ImportUtility.GetCapitalCase(tempMake);
                    equipment.Make = tempMake;
                }

                // model
                string tempModel = ImportUtility.CleanString(oldObject.Model).ToUpper();

                if (!string.IsNullOrEmpty(tempModel))
                {
                    equipment.Model = tempModel;
                }

                // year
                string tempYear = ImportUtility.CleanString(oldObject.Year);

                if (!string.IsNullOrEmpty(tempYear))
                {
                    equipment.Year = tempYear;
                }

                // size
                string tempSize = ImportUtility.CleanString(oldObject.Size);

                if (!string.IsNullOrEmpty(tempSize))
                {
                    tempSize = ImportUtility.GetCapitalCase(tempSize);

                    equipment.Size = tempSize;
                }                

                // serial number
                string tempSerialNumber = ImportUtility.CleanString(oldObject.Serial_Num).ToUpper();

                if (!string.IsNullOrEmpty(tempSerialNumber))
                {
                    equipment.SerialNumber = tempSerialNumber;
                }

                // operator
                string tempOperator = ImportUtility.CleanString(oldObject.Operator);

                if (!string.IsNullOrEmpty(tempOperator))
                {
                    equipment.Operator = tempOperator ?? null;
                }                                                        

                // ***********************************************
                // add comment into the notes field
                // ***********************************************
                string tempComment = ImportUtility.CleanString(oldObject.Comment);
                
                if (!string.IsNullOrEmpty(tempComment))
                {
                    tempComment = ImportUtility.GetUppercaseFirst(tempComment);

                    HetNote note = new HetNote
                    {
                        Text = tempComment,
                        IsNoLongerRelevant = false
                    };

                    if (equipment.HetNote == null)
                    {
                        equipment.HetNote = new List<HetNote>();
                    }

                    equipment.HetNote.Add(note);
                }

                // ***********************************************
                // add equipment to the correct area
                // ***********************************************
                if (oldObject.Area_Id != null)
                {
                    HetLocalArea area = dbContext.HetLocalArea.AsNoTracking()
                        .FirstOrDefault(x => x.LocalAreaNumber == oldObject.Area_Id);

                    if (area != null)
                    {
                        int tempAreaId = area.LocalAreaId;
                        equipment.LocalAreaId = tempAreaId;
                    }
                }

                if (equipment.LocalAreaId == null && equipment.ArchiveCode == "N" && 
                    equipment.EquipmentStatusTypeId == statusIdApproved)
                {
                    throw new DataException(string.Format("Local Area cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                }

                // ***********************************************
                // set the equipment type
                // ***********************************************
                if (oldObject.Equip_Type_Id != null)
                {
                    // get the new id for the "District" Equipment Type
                    string tempEquipmentTypeId = oldObject.Equip_Type_Id.ToString();

                    HetImportMap equipMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == ImportDistrictEquipmentType.OldTable &&
                                             x.OldKey == tempEquipmentTypeId &&
                                             x.NewTable == ImportDistrictEquipmentType.NewTable);

                    if (equipMap != null)
                    {
                        HetDistrictEquipmentType distEquipType = dbContext.HetDistrictEquipmentType
                            .FirstOrDefault(x => x.DistrictEquipmentTypeId == equipMap.NewKey);

                        if (distEquipType != null)
                        {
                            int tempEquipmentId = distEquipType.DistrictEquipmentTypeId;
                            equipment.DistrictEquipmentTypeId = tempEquipmentId;
                        }                        
                    }
                }

                if (equipment.DistrictEquipmentTypeId == null && equipment.ArchiveCode == "N" &&
                    equipment.EquipmentStatusTypeId == statusIdApproved)
                {
                    throw new DataException(string.Format("Equipment Type cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                }

                // ***********************************************
                // set the equipment owner
                // ***********************************************                
                HetImportMap ownerMap = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldTable == ImportOwner.OldTable && 
                                         x.OldKey == oldObject.Owner_Popt_Id.ToString());

                if (ownerMap != null)
                {
                    HetOwner owner = dbContext.HetOwner.FirstOrDefault(x => x.OwnerId == ownerMap.NewKey);

                    if (owner != null)
                    {
                        int tempOwnerId = owner.OwnerId;
                        equipment.OwnerId = tempOwnerId;

                        // set address fields on the owner record
                        if (string.IsNullOrEmpty(owner.Address1))
                        {
                            string tempAddress1 = ImportUtility.CleanString(oldObject.Addr1);
                            tempAddress1 = ImportUtility.GetCapitalCase(tempAddress1);

                            string tempAddress2 = ImportUtility.CleanString(oldObject.Addr2);
                            tempAddress2 = ImportUtility.GetCapitalCase(tempAddress2);

                            string tempCity = ImportUtility.CleanString(oldObject.City);
                            tempCity = ImportUtility.GetCapitalCase(tempCity);

                            owner.Address1 = tempAddress1;
                            owner.Address2 = tempAddress2;
                            owner.City = tempCity;
                            owner.PostalCode = ImportUtility.CleanString(oldObject.Postal).ToUpper();
                            owner.Province = "BC";

                            dbContext.HetOwner.Update(owner);
                        }                        
                    }
                }

                if (equipment.OwnerId == null && equipment.ArchiveCode != "Y")
                {
                    throw new DataException(string.Format("Owner cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                }

                // ***********************************************                
                // set seniority and hours
                // ***********************************************                
                float? tempSeniority = ImportUtility.GetFloatValue(oldObject.Seniority);
                equipment.Seniority = tempSeniority ?? 0.0F;

                float? tempYearsOfService = ImportUtility.GetFloatValue(oldObject.Num_Years);
                equipment.YearsOfService = tempYearsOfService ?? 0.0F;

                int? tempBlockNumber = ImportUtility.GetIntValue(oldObject.Block_Num);
                equipment.BlockNumber = tempBlockNumber ?? null;

                float? tempServiceHoursLastYear = ImportUtility.GetFloatValue(oldObject.YTD1);
                equipment.ServiceHoursLastYear = tempServiceHoursLastYear ?? 0.0F;

                float? tempServiceHoursTwoYearsAgo = ImportUtility.GetFloatValue(oldObject.YTD2);
                equipment.ServiceHoursTwoYearsAgo = tempServiceHoursTwoYearsAgo ?? 0.0F;

                float? tempServiceHoursThreeYearsAgo = ImportUtility.GetFloatValue(oldObject.YTD3);
                equipment.ServiceHoursThreeYearsAgo = tempServiceHoursThreeYearsAgo ?? 0.0F;

                // ***********************************************
                // using the "to date" field to store the
                // equipment "Last_Dt" (hopefully the last time
                // this equipment was hired)
                // ***********************************************                
                DateTime? tempLastDate = ImportUtility.CleanDate(oldObject.Last_Dt);

                if (tempLastDate != null)
                {
                    equipment.ToDate = (DateTime)tempLastDate;
                }

                // ***********************************************
                // set last verified date (default to March 31, 2018)
                // ***********************************************                
                equipment.LastVerifiedDate = DateTime.Parse("2018-03-31 0:00:01");                        

                // ***********************************************
                // create equipment
                // ***********************************************                            
                equipment.AppCreateUserid = systemId;
                equipment.AppCreateTimestamp = DateTime.UtcNow;
                equipment.AppLastUpdateUserid = systemId;
                equipment.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetEquipment.Add(equipment);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Equipment Code: " + equipment.EquipmentCode);
                Debug.WriteLine("***Error*** - Master Equipment Index: " + maxEquipmentIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Equip[] legacyItems = (ImportModels.Equip[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating Equip data");
                progress.SetValue(0);

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (ImportModels.Equip item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                    if (item.Modified_By != null)
                    {
                        item.Modified_By = systemId;
                    }

                    Random random = new Random();
                    string newSerialNum = random.Next(10000).ToString();

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Serial_Num",
                        OriginalValue = item.Serial_Num,
                        NewValue = newSerialNum
                    };

                    importMapRecords.Add(importMapRecordOrganization);

                    item.Serial_Num = newSerialNum;
                    item.Addr1 = ImportUtility.ScrambleString(item.Addr1);
                    item.Addr2 = ImportUtility.ScrambleString(item.Addr2);
                    item.Addr3 = ImportUtility.ScrambleString(item.Addr3);
                    item.Addr4 = ImportUtility.ScrambleString(item.Addr4);
                    item.Postal = ImportUtility.ScrambleString(item.Postal);
                    item.Licence = ImportUtility.ScrambleString(ImportUtility.CleanString(item.Licence));
                    item.Operator = ImportUtility.ScrambleString(item.Operator);                    
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // write out the spreadsheet of import records
                ImportUtility.WriteImportRecordsToExcel(destinationLocation, importMapRecords, OldTable);
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
