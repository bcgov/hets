using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Equip(ment) Records
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

            if (dbContext.Equipments.Any())
            {
                maxEquipmentIndex = dbContext.Equipments.Max(x => x.Id);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Equip[] legacyItems = (Equip[]) ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Equipment Data. Total Records: " + legacyItems.Length);

                foreach (Equip item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Id.ToString());

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        Equipment instance = null;
                        CopyToInstance(dbContext, item, ref instance, systemId, ref maxEquipmentIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable, instance.Id);
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
        private static void CopyToInstance(DbAppContext dbContext, Equip oldObject, ref Equipment equipment, string systemId, ref int maxEquipmentIndex)
        {
            try
            {
                if (oldObject.Equip_Id <= 0)
                {
                    return;
                }

                equipment = new Equipment { Id = ++maxEquipmentIndex };

                // there is a problem with 1 equipment record
                // Per BC Bid - the correct Owner Id should be: 8786195
                if (oldObject.Equip_Id == 19165)
                {
                    oldObject.Owner_Popt_Id = 8786195;
                }          

                // ***********************************************
                // set the equipment status
                // ***********************************************
                string tempArchive = oldObject.Archive_Cd;
                string tempStatus = oldObject.Status_Cd.Trim();

                if (tempArchive == "Y")
                {
                    // archived!
                    equipment.ArchiveCode = "Y";
                    equipment.ArchiveDate = DateTime.UtcNow;
                    equipment.ArchiveReason = "Imported from BC Bid";
                    equipment.Status = "Archived";

                    string tempArchiveReason = ImportUtility.CleanString(oldObject.Archive_Reason);

                    if (!string.IsNullOrEmpty(tempArchiveReason))
                    {
                        equipment.ArchiveReason = ImportUtility.GetUppercaseFirst(tempArchiveReason);
                    }
                }
                else
                {
                    equipment.ArchiveCode = "N";
                    equipment.ArchiveDate = null;
                    equipment.ArchiveReason = null;
                    equipment.Status = tempStatus == "A" ? "Approved" : "Unapproved";
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

                equipment.ApprovedDate = ImportUtility.CleanDateTime(oldObject.Approved_Dt);

                DateTime? tempReceivedDate = ImportUtility.CleanDateTime(oldObject.Received_Dt);

                if (tempReceivedDate != null)
                {
                    equipment.ReceivedDate = (DateTime)tempReceivedDate;
                }
                else
                {
                    if (equipment.ArchiveCode == "N" && equipment.Status == "Approved")
                    {
                        throw new DataException(string.Format("Received Date cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                    }                    
                }

                // equipment code
                string tempEquipmentCode = ImportUtility.CleanString(oldObject.Equip_Cd).ToUpper();

                if (!string.IsNullOrEmpty(tempEquipmentCode))
                {
                    equipment.EquipmentCode = tempEquipmentCode;
                }
                else
                {
                    if (equipment.ArchiveCode == "N" && equipment.Status == "Approved")
                    {
                        throw new DataException(string.Format("Equipment Code cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                    }                    
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
                    tempComment = ImportUtility.GetCapitalCase(tempComment);

                    equipment.Notes = new List<Note>();

                    Note note = new Note
                    {
                        Text = tempComment,
                        IsNoLongerRelevant = false
                    };

                    equipment.Notes.Add(note);
                }

                // ***********************************************
                // add equipment to the correct area
                // ***********************************************
                if (oldObject.Area_Id != null)
                {
                    LocalArea area = dbContext.LocalAreas.FirstOrDefault(x => x.LocalAreaNumber == oldObject.Area_Id);

                    if (area != null)
                    {
                        int tempAreaId = area.Id;
                        equipment.LocalAreaId = tempAreaId;
                    }
                }

                if (equipment.LocalAreaId == null && equipment.ArchiveCode == "N" && equipment.Status == "Approved")
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

                    ImportMap equipMap = dbContext.ImportMaps
                        .FirstOrDefault(x => x.OldTable == ImportDistrictEquipmentType.OldTable &&
                                             x.OldKey == tempEquipmentTypeId &&
                                             x.NewTable == ImportDistrictEquipmentType.NewTable);

                    if (equipMap != null)
                    {
                        DistrictEquipmentType distEquipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == equipMap.NewKey);

                        if (distEquipType != null)
                        {
                            int tempEquipmentId = distEquipType.Id;
                            equipment.DistrictEquipmentTypeId = tempEquipmentId;
                        }

                        // is this a dump truck?
                        if (!string.IsNullOrEmpty(oldObject.Reg_Dump_Trk) && oldObject.Reg_Dump_Trk == "Y")
                        {
                            // update the equipment type record -> IsDumpTruck = True
                            EquipmentType equipType = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == distEquipType.EquipmentTypeId);

                            if (equipType != null)
                            {
                                equipType.IsDumpTruck = true;
                                dbContext.EquipmentTypes.Update(equipType);
                            }
                        }
                    }
                }

                if (equipment.DistrictEquipmentTypeId == null && equipment.ArchiveCode == "N" && equipment.Status == "Approved")
                {
                    throw new DataException(string.Format("Equipment Type cannot be null (EquipmentIndex: {0}", maxEquipmentIndex));
                }

                // ***********************************************
                // set the equipment owner
                // ***********************************************                
                ImportMap ownerMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == ImportOwner.OldTable && 
                                                                              x.OldKey == oldObject.Owner_Popt_Id.ToString());

                if (ownerMap != null)
                {
                    Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == ownerMap.NewKey);

                    if (owner != null)
                    {
                        int tempOwnerId = owner.Id;
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

                            dbContext.Owners.Update(owner);
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

                dbContext.Equipments.Add(equipment);
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

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                Equip[] legacyItems = (Equip[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating Equip data");
                progress.SetValue(0);

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (Equip item in legacyItems.WithProgress(progress))
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
                    item.Licence = ImportUtility.ScrambleString(item.Licence);
                    item.Operator = ImportUtility.ScrambleString(item.Operator);                    
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // write out the spreadsheet of import records.
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
