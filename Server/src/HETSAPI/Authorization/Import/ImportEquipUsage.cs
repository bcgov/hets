using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace HETSAPI.Import
{
    public class ImportEquipUsage
    {
        const string oldTable = "EquipUsage";
        const string newTable = "HET_RENTAL_AGREEMENT";
        const string xmlFileName = "Equip_Usage.xml";

        /// <summary>
        /// Import from Equip_Usage.xml file to Three tables:
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipUsage[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.EquipUsage[] legacyItems = (HETSAPI.Import.EquipUsage[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    string oldKey = (item.Equip_Id ?? 0).ToString() + (item.Project_Id ?? 0).ToString() + (item.Service_Area_Id ?? 0).ToString();
                    string workedDate =  item.Worked_Dt.Trim().Substring(0, 10);
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldKey+ workedDate.Substring(0,4));

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Id > 0)
                        {
                            Models.RentalAgreement rentalAgreement = null;
                            Models.TimeRecord timeRecorded = null;


                            CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, ref timeRecorded, oldKey, workedDate, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, oldKey + workedDate.Substring(0, 4), newTable, rentalAgreement.Id);
                        }
                    }
                    else // update
                    {
                        Models.RentalAgreement rentalAgreement = dbContext.RentalAgreements.FirstOrDefault(x => x.Id == importMap.NewKey);
                        Models.TimeRecord timeRecorded = dbContext.TimeRecords.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (rentalAgreement == null) // record was deleted
                        {
                            CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, ref timeRecorded, oldKey, workedDate, systemId);
                            // update the import map.
                            importMap.NewKey = rentalAgreement.Id;
                            dbContext.ImportMaps.Update(importMap);
                            //  dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToTimeRecorded(performContext, dbContext, item, ref rentalAgreement, ref timeRecorded, oldKey, workedDate, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            // dbContext.SaveChanges();
                        }
                    }

                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch
                        {

                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
                try
                {
                    dbContext.SaveChanges();
                }
                catch
                {

                }
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }


        static private void CopyToTimeRecorded(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipUsage oldObject, 
            ref Models.RentalAgreement rentalAgreement, ref Models.TimeRecord timeRecorded, string oldKey, string workedDate, string systemId)
        {
            bool isNew = false;

            if (oldObject.Project_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (rentalAgreement == null)
            {
                isNew = true;
                rentalAgreement = new Models.RentalAgreement();
                rentalAgreement.RentalAgreementRates = new List<RentalAgreementRate>();
                rentalAgreement.TimeRecords = new List<TimeRecord>();
                Models.Equipment equip = dbContext.Equipments.FirstOrDefault(x => x.Id == oldObject.Equip_Id);
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
                addingRate_Time_For_RentaAgreement(dbContext, oldObject, ref rentalAgreement, oldKey, workedDate, systemId);

                rentalAgreement.Status = "Imported from BCBid";
                rentalAgreement.Note = oldKey+workedDate;
                rentalAgreement.EquipmentRate = (float)Decimal.Parse(oldObject.Rate == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
                if (oldObject.Entered_Dt != null)
                {
                    rentalAgreement.DatedOn = DateTime.Parse(oldObject.Entered_Dt.Trim().Substring(0, 10));
                }

                if (oldObject.Created_Dt != null)
                {
                    try
                    {
                        rentalAgreement.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10));
                    }
                    catch (Exception e)
                    {
                        rentalAgreement.CreateTimestamp = DateTime.UtcNow;
                    }
                }
                rentalAgreement.CreateUserid = createdBy.SmUserId;           

                dbContext.RentalAgreements.Add(rentalAgreement);
            }
            else
            {
                rentalAgreement = dbContext.RentalAgreements.First(x => x.Note == oldKey + workedDate);
                if (rentalAgreement.RentalAgreementRates==null)
                    rentalAgreement.RentalAgreementRates = new List<RentalAgreementRate>();
                if (rentalAgreement.TimeRecords == null)
                    rentalAgreement.TimeRecords = new List<TimeRecord>();
                addingRate_Time_For_RentaAgreement(dbContext, oldObject, ref rentalAgreement, oldKey, workedDate, systemId);
                try
                {
                    rentalAgreement.LastUpdateUserid = modifiedBy.SmUserId;
                    rentalAgreement.LastUpdateTimestamp = DateTime.UtcNow;
                }
                catch
                {

                }
                dbContext.RentalAgreements.Update(rentalAgreement);
            }
        }

        /// <summary>
        /// Adding (3) Rental Agreement Rate and (3) Time Records to rental agreement
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="rentalAgreement"></param>
        /// <param name="systemId"></param>
        static private void addingRate_Time_For_RentaAgreement(DbAppContext dbContext, HETSAPI.Import.EquipUsage oldObject,
            ref Models.RentalAgreement rentalAgreement, string oldKey, string workedDate, string systemId)
        {
            // Adding rental agreement rates and Time_Records: The two are added together becase Time Record reference rental agreement rate.
            Models.RentalAgreementRate rate = new RentalAgreementRate();
            Models.TimeRecord tRec = new TimeRecord();

            //Adding general properties for RentalAgreement Rate
            rate.Comment = "Import from BCBid";
            rate.IsAttachment = false;
            if (oldObject.Entered_Dt != null)
            {
                rate.LastUpdateTimestamp = DateTime.Parse(oldObject.Entered_Dt.Trim().Substring(0, 10));
            }
            else
            {
                rate.LastUpdateTimestamp = DateTime.UtcNow;
            }
            rate.LastUpdateUserid = oldObject.Created_By == null ? systemId : ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId).SmUserId;


            //Adding general properties for Time Records
            if (oldObject.Entered_Dt != null)
            {
                tRec.EnteredDate = DateTime.Parse(oldObject.Entered_Dt.Trim().Substring(0, 10));
            }
            else
            {
                tRec.EnteredDate = DateTime.UtcNow;
            }
            if (oldObject.Worked_Dt != null)
            {
                tRec.WorkedDate = DateTime.Parse(workedDate);
            }
            else
            {
                tRec.WorkedDate = DateTime.UtcNow;
            }

            if (oldObject.Created_Dt != null)
            {
                tRec.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10));
            }
            else
            {
                tRec.CreateTimestamp = DateTime.UtcNow;
            }
            tRec.LastUpdateUserid = oldObject.Created_By == null ? systemId : ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId).SmUserId;
            tRec.LastUpdateTimestamp = DateTime.UtcNow;

            //Adding three rates and hours using a Dictionary
            Dictionary<int, Pair> _f = new Dictionary<int, Pair>();
            float Rate   = (float)Decimal.Parse(oldObject.Rate   == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Rate2  = (float)Decimal.Parse(oldObject.Rate2  == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Rate3  = (float)Decimal.Parse(oldObject.Rate3  == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Hours  = (float)Decimal.Parse(oldObject.Hours  == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Hours2 = (float)Decimal.Parse(oldObject.Hours2 == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            float Hours3 = (float)Decimal.Parse(oldObject.Hours3 == null ? "0" : oldObject.Rate, System.Globalization.NumberStyles.Any);
            // Add items to dictionary.

            _f.Add(1, new Pair(Hours, Rate));
            _f.Add(2, new Pair(Hours2, Rate2));
            _f.Add(3, new Pair(Hours3, Rate3));

            // Use var in foreach loop.
            foreach (var pair in _f)
            {
                Models.RentalAgreementRate exitingRate = rentalAgreement.RentalAgreementRates.FirstOrDefault(x => x.Rate == pair.Value.Rate);
                if (exitingRate == null)
                {  //The new rate is added
                    rate.Rate = pair.Value.Rate;
                    rentalAgreement.RentalAgreementRates.Add(rate);
                    //Also add time Record
                    tRec.Hours = pair.Value.Hours;
                    tRec.RentalAgreementRate = rate;
                    rentalAgreement.TimeRecords.Add(tRec);
                }
                else
                {   //the rate already existed which is exitingRate, no need to add rate, just add Time Record
                    Models.TimeRecord existingTimeRec= rentalAgreement.TimeRecords.FirstOrDefault(x => x.WorkedDate == DateTime.Parse(workedDate) );
                    if (existingTimeRec == null)
                    {                                   //The new Time Record  is added if it does not existm, otherwise, it's already existed.
                        tRec.Hours = pair.Value.Hours;
                        tRec.RentalAgreementRate = exitingRate;
                        rentalAgreement.TimeRecords.Add(tRec);
                    }
                }
            }
            // Ended adding Rental Agreement rates and time records.
        }

    }
}


