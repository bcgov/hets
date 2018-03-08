using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        const string OldTable = "Equip_Usage";
        const string NewTable = "HET_RENTAL_AGREEMENT";
        const string XmlFileName = "Equip_Usage.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipUsage[] legacyItems = (EquipUsage[])ser.Deserialize(memoryStream);

                //Use this list to save a trip to query database in each iteration
                List<Equipment> equip_list = dbContext.Equipments
                        .Include(x => x.DumpTruck)
                        .Include(x => x.DistrictEquipmentType)
                        .ToList();
                Dictionary<int, Equipment> equips = new Dictionary<int, Equipment>();

                foreach (Equipment equip_item in equip_list)
                {
                    equips.Add(equip_item.Id, equip_item);
                }

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    performContext.WriteLine("*** skipping " + startPoint + " records done in a former process ***");
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    string oldKey = (item.Equip_Id ?? 0) + (item.Project_Id ?? 0).ToString() + (item.Service_Area_Id ?? 0);
                    string workedDate =  item.Worked_Dt.Trim().Substring(0, 10);
                    string note = oldKey + "-" + workedDate.Substring(0, 4);
                    string oldKeyAll = oldKey + "-" + workedDate.Substring(0, 10);

                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldKeyAll);

                    // new entry
                    if (importMap == null) 
                    {
                        if (item.Equip_Id > 0)
                        {
                            RentalAgreement rentalAgreement = dbContext.RentalAgreements.FirstOrDefault(x => x.Note == note);
                            CopyToTimeRecorded(dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, oldKeyAll, NewTable, rentalAgreement.Id);
                        }
                    }
                    else // update
                    {
                        RentalAgreement rentalAgreement = dbContext.RentalAgreements.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (rentalAgreement == null) 
                        {
                            CopyToTimeRecorded(dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);

                            // update the import map
                            importMap.NewKey = rentalAgreement.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update
                        {
                            CopyToTimeRecorded(dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }
                    ii++;
                    // save change to database periodically to avoid frequent writing to the database
                    if (ii % 100 == 0)
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
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rentalAgreement"></param>
        /// <param name="note"></param>
        /// <param name="workedDate"></param>
        /// <param name="equips"></param>
        /// <param name="systemId"></param>
        private static void CopyToTimeRecorded(DbAppContext dbContext, EquipUsage oldObject, 
            ref RentalAgreement rentalAgreement, string note, string workedDate, Dictionary<int,Equipment> equips, string systemId)
        {            
            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (rentalAgreement == null)
            {
                rentalAgreement = new RentalAgreement
                {
                    RentalAgreementRates = new List<RentalAgreementRate>(),
                    TimeRecords = new List<TimeRecord>()
                };

                Equipment equip = null;
                

                if (oldObject.Equip_Id != null && equips.ContainsKey ((int)oldObject.Equip_Id))
                {
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == "Equip" && x.OldKey == oldObject.Equip_Id.ToString());

                    if (importMap != null)
                    {
                        equip = equips[importMap.NewKey];
                    }
                    
                }
                
                if (equip != null)
                {
                    rentalAgreement.Equipment = equip;
                    rentalAgreement.EquipmentId = equip.Id;
                }

                Models.Project proj = null;

                ImportMap importMapProject = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == "Project" && x.OldKey == oldObject.Project_Id.ToString()); 
                if (importMapProject != null)
                {
                    proj = dbContext.Projects.FirstOrDefault(x => x.Id == importMapProject.NewKey);
                }

                if (proj != null)
                {
                    rentalAgreement.Project = proj;
                    rentalAgreement.ProjectId = proj.Id;
                }

                // adding rental agreement rates and Time_Records: The two are added together becase Time Record reference rental agreement rate.
                AddingRateTimeForRentaAgreement(dbContext, oldObject, ref rentalAgreement,  workedDate, systemId);

                rentalAgreement.Status = "Imported from BCBid";
                rentalAgreement.Note = note;
                rentalAgreement.EquipmentRate = (float)Decimal.Parse(oldObject.Rate ?? "0", System.Globalization.NumberStyles.Any);

                if (oldObject.Entered_Dt != null)
                {
                    rentalAgreement.DatedOn =  DateTime.ParseExact(oldObject.Entered_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.Created_Dt != null)
                {
                    try
                    {
                        rentalAgreement.AppCreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        rentalAgreement.AppCreateTimestamp = DateTime.UtcNow;
                    }
                }

                rentalAgreement.AppCreateUserid = createdBy.SmUserId;           
                dbContext.RentalAgreements.Add(rentalAgreement);
            }
            else
            {
                rentalAgreement = dbContext.RentalAgreements.First(x => x.Note == note);

                if (rentalAgreement.RentalAgreementRates == null)
                    rentalAgreement.RentalAgreementRates = new List<RentalAgreementRate>();

                if (rentalAgreement.TimeRecords == null)
                    rentalAgreement.TimeRecords = new List<TimeRecord>();

                AddingRateTimeForRentaAgreement(dbContext, oldObject, ref rentalAgreement,  workedDate, systemId);
                rentalAgreement.AppLastUpdateUserid = modifiedBy.SmUserId;
                rentalAgreement.AppLastUpdateTimestamp = DateTime.UtcNow;
                dbContext.RentalAgreements.Update(rentalAgreement);
            }
        }

        /// <summary>
        /// Adding Rental Agreement Rate and Time Records to rental agreement
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rentalAgreement"></param>
        /// <param name="workedDate"></param>
        /// <param name="systemId"></param>
        private static void AddingRateTimeForRentaAgreement(DbAppContext dbContext, EquipUsage oldObject,
            ref RentalAgreement rentalAgreement, string workedDate, string systemId)
        {
            // adding rental agreement rates and time records: 
            // the two are added together because time record reference rental agreement rate           
            
            // adding general properties for Rental Agreement Rate
            DateTime lastUpdateTimestamp = DateTime.UtcNow;
            if (oldObject.Entered_Dt != null)
            {
                lastUpdateTimestamp = DateTime.ParseExact(oldObject.Entered_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            string lastUpdateUserid = oldObject.Created_By == null ? systemId : ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId).SmUserId;

            // adding general properties for Time Record
            DateTime workedDateTime;
            if (oldObject.Worked_Dt != null)
            {
                workedDateTime = DateTime.ParseExact(workedDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                workedDateTime = DateTime.UtcNow;
            }

            DateTime createdDate;
            if (oldObject.Created_Dt != null)
            {
                createdDate = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                createdDate = DateTime.UtcNow;
            }           

            // adding three rates and hours using a Dictionary
            Dictionary<int, Pair> f = new Dictionary<int, Pair>();

            float rate = (float)Decimal.Parse(oldObject.Rate ?? "0", System.Globalization.NumberStyles.Any);
            float rate2 = (float)Decimal.Parse(oldObject.Rate2 ?? "0", System.Globalization.NumberStyles.Any);
            float rate3 = (float)Decimal.Parse(oldObject.Rate3 ?? "0", System.Globalization.NumberStyles.Any);
            float hours = (float)Decimal.Parse(oldObject.Hours ?? "0", System.Globalization.NumberStyles.Any);
            float hours2 = (float)Decimal.Parse(oldObject.Hours2 ?? "0", System.Globalization.NumberStyles.Any);
            float hours3 = (float)Decimal.Parse(oldObject.Hours3 ?? "0", System.Globalization.NumberStyles.Any);

            // add items to dictionary
            if (hours != 0.0 || rate != 0.0)
                f.Add(1, new Pair(hours, rate));

            if (hours2!=0.0 || rate2 !=0.0)
                f.Add(2, new Pair(hours2, rate2));

            if (hours3 != 0.0 || rate3 != 0.0)
                f.Add(3, new Pair(hours3, rate3));

            // use var in foreach loop.
            int ii = 0;
            RentalAgreementRate [] rateA= new RentalAgreementRate[3];
            TimeRecord [] tRecA = new TimeRecord[3];

            foreach (var pair in f)
            {
                RentalAgreementRate exitingRate = rentalAgreement.RentalAgreementRates.FirstOrDefault(x => x.Rate == pair.Value.Rate);

                // rate does not exist
                if (exitingRate == null)  
                {  
                    // adding the new rate  
                    rateA[ii] = new RentalAgreementRate
                    {
                        Comment = "Import from BCBid",
                        IsAttachment = false,
                        AppLastUpdateTimestamp = lastUpdateTimestamp,
                        AppLastUpdateUserid = lastUpdateUserid,
                        AppCreateTimestamp = createdDate,
                        AppCreateUserid = lastUpdateUserid,
                        Rate = pair.Value.Rate
                    };

                    rentalAgreement.RentalAgreementRates.Add(rateA[ii]);
                    
                    // add time Record
                    tRecA[ii] = new TimeRecord
                    {
                        EnteredDate = lastUpdateTimestamp,
                        AppLastUpdateUserid = lastUpdateUserid,
                        WorkedDate = workedDateTime,
                        Hours = pair.Value.Hours,
                        AppCreateTimestamp = createdDate,
                        AppCreateUserid = lastUpdateUserid,
                        RentalAgreementRate = rateA[ii]
                    };

                    rentalAgreement.TimeRecords.Add(tRecA[ii]);
                }
                else   
                {   
                    //the rate already existed which is exitingRate, no need to add rate, just add Time Record
                    TimeRecord existingTimeRec= rentalAgreement.TimeRecords.FirstOrDefault(x => x.WorkedDate == workedDateTime);

                    if (existingTimeRec == null)
                    {   
                        // the new Time Record is added if it does not exist, otherwise, it already existed
                        tRecA[ii] = new TimeRecord
                        {
                            EnteredDate = lastUpdateTimestamp,
                            AppLastUpdateUserid = lastUpdateUserid,
                            WorkedDate = workedDateTime,
                            Hours = pair.Value.Hours,
                            AppCreateTimestamp = createdDate,
                            AppCreateUserid = lastUpdateUserid,
                            RentalAgreementRate = exitingRate
                        };

                        rentalAgreement.TimeRecords.Add(tRecA[ii]);
                    }
                }

                ii++;
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
                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
                // no excel for EquipUsage.

            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


