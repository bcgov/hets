using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class ImportEquipUsage
    {
        const string oldTable = "EquipUsage";
        const string newTable = "HET_RENTAL_AGREEMENT";
        const string xmlFileName = "Equip_Usage.xml";
        public static string oldTable_Progress = oldTable + "_Progress";

        /// <summary>
        /// Import from Equip_Usage.xml file to Three tables:
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        static public void Import(PerformContext performContext, DbContextOptionsBuilder<DbAppContext> options, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // Check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, oldTable_Progress, BCBidImport.sigId);
            if (startPoint == BCBidImport.sigId)    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + xmlFileName + " is complete from the former process ***");
                return;
            }
            string rootAttr = "ArrayOf" + oldTable;

            //Create Processer progress indicator
            performContext.WriteLine("Processing " + oldTable);
            var progress = performContext.WriteProgressBar();
            progress.SetValue(0);

            // create serializer and serialize xml file
            XmlSerializer ser = new XmlSerializer(typeof(EquipUsage[]), new XmlRootAttribute(rootAttr));
            MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
            HETSAPI.Import.EquipUsage[] legacyItems = (HETSAPI.Import.EquipUsage[])ser.Deserialize(memoryStream);

            //Use this list to save a trip to query database in each iteration
            List<Models.Equipment> equips = dbContext.Equipments
                    .Include(x => x.DumpTruck)
                    .Include(x => x.DistrictEquipmentType)
                    .ToList();

            int ii = startPoint;
            if (startPoint > 0)    // Skip the portion already processed
            {
                legacyItems = legacyItems.Skip(ii).ToArray();
            }

            foreach (var item in legacyItems.WithProgress(progress))
            {
                // see if we have this one already.
                string oldKey = (item.Equip_Id ?? 0).ToString() + (item.Project_Id ?? 0).ToString() + (item.Service_Area_Id ?? 0).ToString();
                string workedDate =  item.Worked_Dt.Trim().Substring(0, 10);
                string note = oldKey + "-" + workedDate.Substring(0, 4);
                string oldKeyAll = oldKey + "-" + workedDate.Substring(0, 10);
                ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldKeyAll);

                if (importMap == null) // new entry
                {
                    if (item.Equip_Id > 0)
                    {
                        Models.RentalAgreement rentalAgreement = dbContext.RentalAgreements.FirstOrDefault(x => x.Note == note);
                        CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, oldKeyAll, newTable, rentalAgreement.Id);
                    }
                }
                else // update
                {
                    Models.RentalAgreement rentalAgreement = dbContext.RentalAgreements.FirstOrDefault(x => x.Id == importMap.NewKey);
                    if (rentalAgreement == null) // record was deleted
                    {
                        CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);
                        // update the import map.
                        importMap.NewKey = rentalAgreement.Id;
                        dbContext.ImportMaps.Update(importMap);
                    }
                    else // ordinary update.
                    {
                        CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, note, workedDate, equips, systemId);
                        // touch the import map.
                        importMap.LastUpdateTimestamp = DateTime.UtcNow;
                        dbContext.ImportMaps.Update(importMap);
                    }
                }

                if (++ii % 1000 == 0)   // Save change to database once a while to avoid frequent writing to the database.
                {                   
                    try
                    {
                        ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, ii.ToString(), BCBidImport.sigId);
                        int iResult = dbContext.SaveChangesForImport();
                        options = new DbContextOptionsBuilder<DbAppContext>();

                        dbContext = new DbAppContext(null, options.Options);
                    }
                    catch (Exception e)
                    {
                        string iStr = e.ToString();
                    }
                }
            }
            try
            {
                performContext.WriteLine("*** Importing " + xmlFileName + " is Done ***");
                ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, BCBidImport.sigId.ToString(), BCBidImport.sigId);
                int iResult = dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
        }

        /// <summary>
        /// Copy xml item to instance (table entries)
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rentalAgreement"></param>
        /// <param name="note"></param>
        /// <param name="workedDate"></param>
        /// <param name="equips"></param>
        /// <param name="systemId"></param>
        static private void CopyToTimeRecorded(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipUsage oldObject, 
            ref Models.RentalAgreement rentalAgreement, string note, string workedDate, List<Models.Equipment> equips, string systemId)
        {            
            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (rentalAgreement == null)
            {
                rentalAgreement = new Models.RentalAgreement();
                rentalAgreement.RentalAgreementRates = new List<RentalAgreementRate>();
                rentalAgreement.TimeRecords = new List<TimeRecord>();
                Models.Equipment equip = equips.FirstOrDefault(x => x.Id == oldObject.Equip_Id); //dbContext.Equipments.FirstOrDefault(x => x.Id == oldObject.Equip_Id);
                if (equip != null)
                {
                    rentalAgreement.Equipment = equip;
                    rentalAgreement.EquipmentId = equip.Id;
                }

                Models.Project proj = dbContext.Projects.FirstOrDefault(x => x.Id == oldObject.Project_Id);
                if (proj != null)
                {
                    rentalAgreement.Project = proj;
                    rentalAgreement.ProjectId = proj.Id;
                }

                // Adding rental agreement rates and Time_Records: The two are added together becase Time Record reference rental agreement rate.
                addingRate_Time_For_RentaAgreement(dbContext, oldObject, ref rentalAgreement,  workedDate, systemId);

                rentalAgreement.Status = "Imported from BCBid";
                rentalAgreement.Note = note;
                rentalAgreement.EquipmentRate = (float)Decimal.Parse(oldObject.Rate == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
                if (oldObject.Entered_Dt != null)
                {
                    rentalAgreement.DatedOn =  DateTime.ParseExact(oldObject.Entered_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.Created_Dt != null)
                {
                    try
                    {
                        rentalAgreement.CreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        rentalAgreement.CreateTimestamp = DateTime.UtcNow;
                    }
                }
                rentalAgreement.CreateUserid = createdBy.SmUserId;           
                dbContext.RentalAgreements.Add(rentalAgreement);
            }
            else
            {
                rentalAgreement = dbContext.RentalAgreements.First(x => x.Note == note);
                if (rentalAgreement.RentalAgreementRates==null)
                    rentalAgreement.RentalAgreementRates = new List<RentalAgreementRate>();
                if (rentalAgreement.TimeRecords == null)
                    rentalAgreement.TimeRecords = new List<TimeRecord>();
                addingRate_Time_For_RentaAgreement(dbContext, oldObject, ref rentalAgreement,  workedDate, systemId);
                rentalAgreement.LastUpdateUserid = modifiedBy.SmUserId;
                rentalAgreement.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.RentalAgreements.Update(rentalAgreement);
            }
        }

        /// <summary>
        /// Adding (3) Rental Agreement Rate and (3) Time Records to rental agreement
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rentalAgreement"></param>
        /// <param name="workedDate"></param>
        /// <param name="systemId"></param>
        static private void addingRate_Time_For_RentaAgreement(DbAppContext dbContext, HETSAPI.Import.EquipUsage oldObject,
            ref Models.RentalAgreement rentalAgreement, string workedDate, string systemId)
        {
            // Adding rental agreement rates and Time_Records: The two are added together becase Time Record reference rental agreement rate.
            Models.RentalAgreementRate rate = new RentalAgreementRate();
            Models.TimeRecord tRec = new TimeRecord();

            //Adding general properties for RentalAgreement Rate
            DateTime lastUpdateTimestamp = DateTime.UtcNow;
            if (oldObject.Entered_Dt != null)
            {
                lastUpdateTimestamp = DateTime.ParseExact(oldObject.Entered_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            string lastUpdateUserid = oldObject.Created_By == null ? systemId : ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId).SmUserId;

            //Adding general properties for Time Records
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

            //Adding three rates and hours using a Dictionary
            Dictionary<int, Pair> _f = new Dictionary<int, Pair>();
            float Rate   = (float)Decimal.Parse(oldObject.Rate   == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Rate2  = (float)Decimal.Parse(oldObject.Rate2  == null ? "0" : oldObject.Rate2, System.Globalization.NumberStyles.Any);
            float Rate3  = (float)Decimal.Parse(oldObject.Rate3  == null ? "0" : oldObject.Rate3, System.Globalization.NumberStyles.Any);
            float Hours  = (float)Decimal.Parse(oldObject.Hours  == null ? "0" : oldObject.Hours, System.Globalization.NumberStyles.Any);
            float Hours2 = (float)Decimal.Parse(oldObject.Hours2 == null ? "0" : oldObject.Hours2, System.Globalization.NumberStyles.Any);
            float Hours3 = (float)Decimal.Parse(oldObject.Hours3 == null ? "0" : oldObject.Hours3, System.Globalization.NumberStyles.Any);
            // Add items to dictionary.

            if (Hours != 0.0 || Rate != 0.0)
                _f.Add(1, new Pair(Hours, Rate));
            if (Hours2!=0.0 || Rate2 !=0.0)
                _f.Add(2, new Pair(Hours2, Rate2));
            if (Hours3 != 0.0 || Rate3 != 0.0)
                _f.Add(3, new Pair(Hours3, Rate3));

            // Use var in foreach loop.
            int ii = 0;
            Models.RentalAgreementRate [] rate_a= new RentalAgreementRate[3];
            Models.TimeRecord [] tRec_a = new TimeRecord[3];
            foreach (var pair in _f)
            {
                Models.RentalAgreementRate exitingRate = rentalAgreement.RentalAgreementRates.FirstOrDefault(x => x.Rate == pair.Value.Rate);
                if (exitingRate == null)  //rate does not exist
                {  //  Adding the new rate  
                    rate_a[ii] = new RentalAgreementRate();
                    rate_a[ii].Comment = "Import from BCBid";
                    rate_a[ii].IsAttachment = false;
                    rate_a[ii].LastUpdateTimestamp = lastUpdateTimestamp;
                    rate_a[ii].LastUpdateUserid = lastUpdateUserid;
                    rate_a[ii].CreateTimestamp = createdDate;
                    rate_a[ii].CreateUserid  = lastUpdateUserid;
                    rate_a[ii].Rate = pair.Value.Rate;
                    rentalAgreement.RentalAgreementRates.Add(rate_a[ii]);
                    //Also add time Record
                    tRec_a[ii] = new TimeRecord();
                    tRec_a[ii].EnteredDate = lastUpdateTimestamp;
                    tRec_a[ii].LastUpdateUserid = lastUpdateUserid;
                    tRec_a[ii].WorkedDate = workedDateTime;
                    tRec_a[ii].Hours = pair.Value.Hours;
                    tRec_a[ii].CreateTimestamp = createdDate;
                    tRec_a[ii].CreateUserid= lastUpdateUserid;
                    tRec_a[ii].RentalAgreementRate = rate_a[ii];
                    rentalAgreement.TimeRecords.Add(tRec_a[ii]);
                }
                else   
                {   //the rate already existed which is exitingRate, no need to add rate, just add Time Record
                    Models.TimeRecord existingTimeRec= rentalAgreement.TimeRecords.FirstOrDefault(x => x.WorkedDate == workedDateTime);
                    if (existingTimeRec == null)
                    {   //The new Time Record  is added if it does not existm, otherwise, it's already existed.
                        tRec_a[ii] = new TimeRecord();
                        tRec_a[ii].EnteredDate = lastUpdateTimestamp;
                        tRec_a[ii].LastUpdateUserid = lastUpdateUserid;
                        tRec_a[ii].WorkedDate = workedDateTime;
                        tRec_a[ii].Hours = pair.Value.Hours;
                        tRec_a[ii].CreateTimestamp = createdDate;
                        tRec_a[ii].CreateUserid = lastUpdateUserid;
                        tRec_a[ii].RentalAgreementRate = exitingRate;
                        rentalAgreement.TimeRecords.Add(tRec_a[ii]);
                    }
                }
                ii++;
            }
            // Ended adding Rental Agreement rates and time records.
        }

    }
}


