using System;
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
using HetsCommon;

namespace HetsImport.Import
{
    /// <summary>
    /// Import Equipment Usage Records
    /// </summary>
    public static class ImportEquipUsage
    {
        public const string OldTable = "Equip_Usage";
        public const string NewTable = "HET_TIME_RECORD";
        public const string XmlFileName = "Equip_Usage.xml";

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
                // **************************************
                // Time Records
                // **************************************
                performContext.WriteLine("*** Resetting HET_TIME_RECORD database sequence after import ***");
                Debug.WriteLine("Resetting HET_TIME_RECORD database sequence after import");

                if (dbContext.HetTimeRecord.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetTimeRecord.Max(x => x.TimeRecordId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_TIME_RECORD_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_TIME_RECORD database sequence after import ***");
                Debug.WriteLine("Resetting HET_TIME_RECORD database sequence after import - Done!");
                
                // **************************************
                // Rental Agreements
                // **************************************
                performContext.WriteLine("*** Resetting HET_RENTAL_AGREEMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_RENTAL_AGREEMENT database sequence after import");

                if (dbContext.HetRentalAgreement.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetRentalAgreement.Max(x => x.RentalAgreementId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_RENTAL_AGREEMENT_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_RENTAL_AGREEMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_RENTAL_AGREEMENT database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Import Equipment Usage
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)   // this means the import job completed for all the records in this file
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxTimeSheetIndex = 0;

            if (dbContext.HetTimeRecord.Any())
            {
                maxTimeSheetIndex = dbContext.HetTimeRecord.Max(x => x.TimeRecordId);
            }

            try
            {           
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.EquipUsage[] legacyItems = (ImportModels.EquipUsage[])ser.Deserialize(memoryStream);                
                
                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing TimeSheet Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.EquipUsage item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    string oldProjectKey = item.Project_Id.ToString();
                    string oldEquipKey = item.Equip_Id.ToString();
                    string oldCreatedDate = item.Created_Dt;

                    string oldKey = string.Format("{0}-{1}-{2}", oldProjectKey, oldEquipKey, oldCreatedDate);

                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable &&
                                             x.OldKey == oldKey);

                    // new entry
                    if (importMap == null && item.Equip_Id > 0 && item.Project_Id > 0)
                    {
                        HetTimeRecord instance = null;
                        CopyToTimeRecorded(dbContext, item, ref instance, systemId, ref maxTimeSheetIndex);

                        if (instance != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldKey, NewTable, instance.TimeRecordId);
                        }

                        // save change to database
                        if (++ii % 2000 == 0)
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId, NewTable);
                            dbContext.SaveChanges();
                        }
                    }                                        
                }

                try
                {
                    performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, NewTable);
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    string temp = string.Format("Error saving data (TimeRecordIndex: {0}): {1}", maxTimeSheetIndex, e.Message);
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
        /// <param name="timeRecord"></param>
        /// <param name="systemId"></param>
        /// <param name="maxTimeSheetIndex"></param>
        private static void CopyToTimeRecorded(DbAppContext dbContext, ImportModels.EquipUsage oldObject, 
            ref HetTimeRecord timeRecord, string systemId, ref int maxTimeSheetIndex)
        {
            try
            {
                if (oldObject.Equip_Id <= 0)
                {
                    return;
                }

                if (oldObject.Project_Id <= 0)
                {
                    return;
                }

                // ***********************************************
                // we only need records from the current fiscal
                // so ignore all others
                // ***********************************************
                DateTime fiscalStart;
                DateTime now = DateTime.Now;
                if (now.Month == 1 || now.Month == 2 || now.Month == 3)
                {
                    fiscalStart = DateUtils.ConvertPacificToUtcTime(
                        new DateTime(now.AddYears(-1).Year, 4, 1, 0, 0, 0));
                }
                else
                {
                    fiscalStart = DateUtils.ConvertPacificToUtcTime(
                        new DateTime(now.Year, 4, 1, 0, 0, 0));
                }

                string tempRecordDate = oldObject.Worked_Dt;

                if (string.IsNullOrEmpty(tempRecordDate))
                {
                    return; // ignore if we don't have a created date
                }                

                if (!string.IsNullOrEmpty(tempRecordDate))
                {
                    DateTime? recordDate = ImportUtility.CleanDate(tempRecordDate);

                    if (recordDate == null || recordDate < fiscalStart)
                    {
                        return; // ignore this record - it is outside of the fiscal years
                    }
                }
                
                // ************************************************
                // get the imported equipment record map
                // ************************************************
                string tempId = oldObject.Equip_Id.ToString();

                HetImportMap mapEquip = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempId &&
                                         x.OldTable == ImportEquip.OldTable &&
                                         x.NewTable == ImportEquip.NewTable);

                if (mapEquip == null)
                {
                    return; // ignore and move to the next record
                }

                // ***********************************************
                // find the equipment record
                // ***********************************************
                HetEquipment equipment = dbContext.HetEquipment.AsNoTracking()
                    .Include(x => x.LocalArea)
                        .ThenInclude(y => y.ServiceArea)
                            .ThenInclude(z => z.District)
                    .FirstOrDefault(x => x.EquipmentId == mapEquip.NewKey);

                if (equipment == null)
                {
                    return; // ignore and move to the next record
                }

                // ************************************************
                // get the imported project record map
                // ************************************************
                string tempProjectId = oldObject.Project_Id.ToString();

                HetImportMap mapProject = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempProjectId &&
                                         x.OldTable == ImportProject.OldTable &&
                                         x.NewTable == ImportProject.NewTable);

                // ***********************************************
                // find the project record
                // (or create a project (inactive))
                // ***********************************************
                HetProject project;

                if (mapProject != null)
                {
                    project = dbContext.HetProject.AsNoTracking()
                        .FirstOrDefault(x => x.ProjectId == mapProject.NewKey);

                    if (project == null)
                    {
                        throw new ArgumentException(string.Format("Cannot locate Project record (Time Sheet Equip Id: {0}", tempId));
                    }
                }
                else
                {
                    int districtId = equipment.LocalArea.ServiceArea.District.DistrictId;

                    int? statusId = StatusHelper.GetStatusId(HetProject.StatusComplete, "projectStatus", dbContext);
                    if (statusId == null) throw new DataException(string.Format("Status Id cannot be null (Time Sheet Equip Id: {0}", tempId));

                    // create new project
                    project = new HetProject
                    {
                        DistrictId = districtId,
                        Information = "Created to support Time Record import from BCBid",
                        ProjectStatusTypeId = (int)statusId,
                        Name = "Legacy BCBid Project",
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    // save now so we can access it for other time records
                    dbContext.HetProject.Add(project);
                    dbContext.SaveChangesForImport();

                    // add mapping record
                    ImportUtility.AddImportMapForProgress(dbContext, ImportProject.OldTable, tempProjectId, project.ProjectId, ImportProject.NewTable);
                    dbContext.SaveChangesForImport();
                }

                // ***********************************************
                // find or create the rental agreement
                // ***********************************************
                DateTime? enteredDate = ImportUtility.CleanDate(oldObject.Entered_Dt); // use for the agreement

                HetRentalAgreement agreement = dbContext.HetRentalAgreement.AsNoTracking()
                    .FirstOrDefault(x => x.EquipmentId == equipment.EquipmentId &&
                                         x.ProjectId == project.ProjectId &&
                                         x.DistrictId == equipment.LocalArea.ServiceArea.District.DistrictId);

                if (agreement == null)
                {
                    int equipmentId = equipment.EquipmentId;
                    int projectId = project.ProjectId;
                    int districtId = equipment.LocalArea.ServiceArea.District.DistrictId;

                    int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusComplete, "rentalAgreementStatus", dbContext);
                    if (statusId == null) throw new DataException(string.Format("Status Id cannot be null (Time Sheet Equip Id: {0}", tempId));                    

                    int? agrRateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodDaily, dbContext);
                    if (agrRateTypeId == null) throw new DataException("Rate Period Id cannot be null");

                    int? year = (ImportUtility.CleanDate(oldObject.Worked_Dt))?.Year;

                    // create a new agreement record
                    agreement = new HetRentalAgreement
                    {
                        EquipmentId = equipmentId,
                        ProjectId = projectId,
                        DistrictId = districtId,
                        RentalAgreementStatusTypeId = (int)statusId,
                        RatePeriodTypeId = (int)agrRateTypeId,
                        Note = "Created to support Time Record import from BCBid",
                        Number = string.Format("BCBid{0}-{1}-{2}", projectId, equipmentId, year),
                        DatedOn = enteredDate,
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    // save now so we can access it for other time records
                    dbContext.HetRentalAgreement.Add(agreement);
                    dbContext.SaveChangesForImport();
                }

                // ***********************************************
                // create time record
                // ***********************************************
                timeRecord = new HetTimeRecord { TimeRecordId = ++maxTimeSheetIndex };

                // ***********************************************
                // set time period type
                // ***********************************************
                int? timePeriodTypeId = StatusHelper.GetTimePeriodId(HetTimePeriodType.PeriodDay, dbContext);
                if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

                timeRecord.TimePeriodTypeId = (int)timePeriodTypeId;

                // ***********************************************
                // set time record attributes
                // ***********************************************
                DateTime? workedDate = ImportUtility.CleanDate(oldObject.Worked_Dt);

                if (workedDate != null)
                {
                    timeRecord.WorkedDate = (DateTime)workedDate;
                }
                else
                {
                    throw new DataException(string.Format("Worked Date cannot be null (TimeSheet Index: {0}", maxTimeSheetIndex));
                }

                // get hours worked
                float? tempHoursWorked = ImportUtility.GetFloatValue(oldObject.Hours);

                if (tempHoursWorked != null)
                {
                    timeRecord.Hours = tempHoursWorked;
                }
                else
                {
                    throw new DataException(string.Format("Hours cannot be null (TimeSheet Index: {0}", maxTimeSheetIndex));
                }                

                if (enteredDate != null)
                {
                    timeRecord.EnteredDate = (DateTime)enteredDate;
                }
                else
                {
                    throw new DataException(string.Format("Entered Date cannot be null (TimeSheet Index: {0}", maxTimeSheetIndex));
                }

                // ***********************************************
                // create time record
                // ***********************************************  
                int raId = agreement.RentalAgreementId;

                timeRecord.RentalAgreementId = raId;
                timeRecord.AppCreateUserid = systemId;
                timeRecord.AppCreateTimestamp = DateTime.UtcNow;
                timeRecord.AppLastUpdateUserid = systemId;
                timeRecord.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetTimeRecord.Add(timeRecord);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Worked Date: " + oldObject.Worked_Dt);
                Debug.WriteLine("***Error*** - Master Time Record Index: " + maxTimeSheetIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.EquipUsage[] legacyItems = (ImportModels.EquipUsage[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipUsage data");
                progress.SetValue(0);

                foreach (ImportModels.EquipUsage item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;                    
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


