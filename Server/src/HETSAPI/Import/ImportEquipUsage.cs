using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    /// Import Equip(ment) Usage Records
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

                if (dbContext.TimeRecords.Any())
                {
                    // get max key
                    int maxKey = dbContext.TimeRecords.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_TIME_RECORD_TIME_RECORD_ID_seq"" RESTART WITH {0};", maxKey);

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

                if (dbContext.RentalAgreements.Any())
                {
                    // get max key
                    int maxKey = dbContext.RentalAgreements.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_RENTAL_AGREEMENT_RENTAL_AGREEMENT_ID_seq"" RESTART WITH {0};", maxKey);

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
        /// Import Equip(ment) Usage
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

            int maxTimesheetIndex = 0;

            if (dbContext.RentalAgreements.Any())
            {
                maxTimesheetIndex = dbContext.RentalAgreements.Max(x => x.Id);
            }

            try
            {           
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipUsage[] legacyItems = (EquipUsage[])ser.Deserialize(memoryStream);                
                
                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing TimeSheet Data. Total Records: " + legacyItems.Length);

                foreach (EquipUsage item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    string oldProjectKey = item.Project_Id.ToString();
                    string oldEquipKey = item.Project_Id.ToString();
                    string oldCreatedDate = item.Created_Dt;

                    string oldKey = string.Format("{0}-{1}-{2}", oldProjectKey, oldEquipKey, oldCreatedDate);
                    
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && 
                                                                                   x.OldKey == oldKey);

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        TimeRecord instance = null;
                        CopyToTimeRecorded(dbContext, item, ref instance, systemId, ref maxTimesheetIndex);

                        if (instance != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldKey, NewTable, instance.Id);
                        }
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
                    string temp = string.Format("Error saving data (RentalAgreementIndex: {0}): {1}", maxTimesheetIndex, e.Message);
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
        /// <param name="maxTimesheetIndex"></param>
        private static void CopyToTimeRecorded(DbAppContext dbContext, EquipUsage oldObject, ref TimeRecord timeRecord, 
            string systemId, ref int maxTimesheetIndex)
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

                string tempRecordDate = oldObject.Worked_Dt;

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

                
                // ************************************************
                // get the imported equipment record map
                // ************************************************
                string tempId = oldObject.Equip_Id.ToString();

                ImportMap mapEquip = dbContext.ImportMaps.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempId &&
                                         x.OldTable == ImportEquip.OldTable &&
                                         x.NewTable == ImportEquip.NewTable);

                if (mapEquip == null)
                {
                    throw new DataException(string.Format("Cannot locate Equipment record (Timesheet Equip Id: {0}", tempId));
                }

                // ***********************************************
                // find the equipment record
                // ***********************************************
                Equipment equipment = dbContext.Equipments.AsNoTracking().FirstOrDefault(x => x.Id == mapEquip.NewKey);

                if (equipment == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate Equipment record (Timesheet Equip Id: {0}", tempId));
                }

                // ************************************************
                // get the imported project record map
                // ************************************************
                string tempProjectId = oldObject.Project_Id.ToString();

                ImportMap mapProject = dbContext.ImportMaps.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempProjectId &&
                                         x.OldTable == ImportProject.OldTable &&
                                         x.NewTable == ImportProject.NewTable);

                // ***********************************************
                // find the project record
                // (or create a project (inactive))
                // ***********************************************
                Models.Project project;

                if (mapProject != null)
                {
                    project = dbContext.Projects.AsNoTracking().FirstOrDefault(x => x.Id == mapProject.NewKey);

                    if (project == null)
                    {
                        throw new ArgumentException(string.Format("Cannot locate Project record (Timesheet Equip Id: {0}", tempId));
                    }
                }
                else
                {
                    // create new project
                    project = new Models.Project
                    {
                        Information = "Created to support Time Record import from BCBid",
                        Status = "Complete",
                        Name = "Legacy BCBid Project",
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    dbContext.Projects.Add(project);

                    // save now so we can access it for other time records
                    dbContext.SaveChanges();

                    // add mapping record
                    ImportUtility.AddImportMapForProgress(dbContext, ImportProject.OldTable, tempProjectId, project.Id, ImportProject.NewTable);
                    dbContext.SaveChanges();
                }

                // ***********************************************
                // find or create the rental agreement
                // ***********************************************
                DateTime? enteredDate = ImportUtility.CleanDateTime(oldObject.Entered_Dt); // use for the agreement

                RentalAgreement agreement = dbContext.RentalAgreements.AsNoTracking()
                    .FirstOrDefault(x => x.EquipmentId == equipment.Id &&
                                         x.ProjectId == project.Id);

                if (agreement == null)
                {
                    // create a new agreement record
                    agreement = new RentalAgreement
                    {
                        EquipmentId = equipment.Id,
                        ProjectId = project.Id,
                        Note = "Created to support Time Record import from BCBid",
                        Number = "Legacy BCBid Agreement",
                        DatedOn = enteredDate,
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    if (project.RentalAgreements == null)
                    {
                        project.RentalAgreements = new List<RentalAgreement>();
                    }

                    project.RentalAgreements.Add(agreement);

                    // save now so we can access it for other time records
                    dbContext.SaveChangesForImport();
                }

                // ***********************************************
                // create time record
                // ***********************************************
                timeRecord = new TimeRecord { Id = ++maxTimesheetIndex };

                // ***********************************************
                // set time record attributes
                // ***********************************************
                DateTime? workedDate = ImportUtility.CleanDateTime(oldObject.Worked_Dt);

                if (workedDate != null)
                {
                    timeRecord.WorkedDate = (DateTime)workedDate;
                }
                else
                {
                    throw new DataException(string.Format("Worked Date cannot be null (TimesheetIndex: {0}", maxTimesheetIndex));
                }

                // get hours worked
                float? tempHoursWorked = ImportUtility.GetFloatValue(oldObject.Hours);

                if (tempHoursWorked != null)
                {
                    timeRecord.Hours = tempHoursWorked;
                }
                else
                {
                    throw new DataException(string.Format("Hours cannot be null (TimesheetIndex: {0}", maxTimesheetIndex));
                }                

                if (enteredDate != null)
                {
                    timeRecord.EnteredDate = (DateTime)enteredDate;
                }
                else
                {
                    throw new DataException(string.Format("Entered Date cannot be null (TimesheetIndex: {0}", maxTimesheetIndex));
                }

                // ***********************************************
                // create time record
                // ***********************************************                            
                timeRecord.AppCreateUserid = systemId;
                timeRecord.AppCreateTimestamp = DateTime.UtcNow;
                timeRecord.AppLastUpdateUserid = systemId;
                timeRecord.AppLastUpdateTimestamp = DateTime.UtcNow;

                if (agreement.TimeRecords == null)
                {
                    agreement.TimeRecords = new List<TimeRecord>();
                }

                agreement.TimeRecords.Add(timeRecord);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Worked Date: " + oldObject.Worked_Dt);
                Debug.WriteLine("***Error*** - Master TimeRecord Index: " + maxTimesheetIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                EquipUsage[] legacyItems = (EquipUsage[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipUsage data");
                progress.SetValue(0);

                foreach (EquipUsage item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;                    
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // no excel for EquipUsage
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


