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
using HetsData.Model;

namespace HetsImport.Import
{
    /// <summary>
    /// Import District Equipment Type Records
    /// </summary>
    public static class ImportDistrictEquipmentType
    {
        public const string OldTable = "Equip_Type";
        public const string NewTable = "HET_DISTRICT_EQUIPMENT_TYPE";
        public const string XmlFileName = "Equip_Type.xml";

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
                performContext.WriteLine("*** Resetting HET_DISTRICT_EQUIPMENT_TYPE database sequence after import ***");
                Debug.WriteLine("Resetting HET_DISTRICT_EQUIPMENT_TYPE database sequence after import");

                if (dbContext.HetDistrictEquipmentType.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetDistrictEquipmentType.Max(x => x.DistrictEquipmentTypeId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_DISTRICT_EQUIPMENT_TYPE_DISTRICT_EQUIPMENT_TYPE_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_DISTRICT_EQUIPMENT_TYPE database sequence after import ***");
                Debug.WriteLine("Resetting HET_DISTRICT_EQUIPMENT_TYPE database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Import District Equipment Types
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

            int maxEquipTypeIndex = 0;

            if (dbContext.HetDistrictEquipmentType.Any())
            {
                maxEquipTypeIndex = dbContext.HetDistrictEquipmentType.Max(x => x.DistrictEquipmentTypeId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.EquipType[] legacyItems = (ImportModels.EquipType[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing DistrictEquipmentType Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.EquipType item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Equip_Type_Id.ToString() &&
                                             x.NewTable == NewTable);

                    // new entry
                    if (importMap == null && item.Equip_Type_Id > 0)
                    {
                        HetDistrictEquipmentType equipType = null;
                        CopyToInstance(dbContext, item, ref equipType, systemId, ref maxEquipTypeIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, equipType.DistrictEquipmentTypeId);

                        // save has to be done immediately because we need access to the records
                        dbContext.SaveChangesForImport();
                    }

                    // periodically save change to the status
                    if (ii++ % 250 == 0)
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
                    string temp = string.Format("Error saving data (DistrictEquipmentTypeIndex: {0}): {1}", maxEquipTypeIndex, e.Message);
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
        /// Copy xml item to instance (table entries)
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="equipType"></param>
        /// <param name="systemId"></param>
        /// <param name="maxEquipTypeIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.EquipType oldObject, 
            ref HetDistrictEquipmentType equipType, string systemId, ref int maxEquipTypeIndex)
        {
            try
            {                
                if (equipType != null)
                {
                    return;
                }

                // ***********************************************
                // get the equipment type
                // ***********************************************
                float? tempBlueBookRate = ImportUtility.GetFloatValue(oldObject.Equip_Rental_Rate_No);

                if (tempBlueBookRate == null)
                {
                    return;
                }

                // ***********************************************
                // get the parent equipment type
                // ***********************************************
                HetEquipmentType type = dbContext.HetEquipmentType.FirstOrDefault(x => x.BlueBookSection == tempBlueBookRate);

                // if it's not found - try to round up to the "parent" blue book section
                if (type == null)
                {
                    int tempIntBlueBookRate = Convert.ToInt32(tempBlueBookRate);
                    type = dbContext.HetEquipmentType.FirstOrDefault(x => x.BlueBookSection == tempIntBlueBookRate);
                }

                // finally - if that's not found - map to 0 (MISCELLANEOUS)
                if (type == null)
                {
                    int tempIntBlueBookRate = 0;
                    type = dbContext.HetEquipmentType.FirstOrDefault(x => x.BlueBookSection == tempIntBlueBookRate);
                }

                if (type == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Equipment Type (Blue Book Rate Number: {0} | Equipment Type Id: {1})",
                            tempBlueBookRate.ToString(), oldObject.Equip_Type_Id));
                }

                // ***********************************************
                // get the description
                // ***********************************************
                string tempDistrictDescriptionOnly = ImportUtility.CleanString(oldObject.Equip_Type_Desc);
                tempDistrictDescriptionOnly = ImportUtility.GetCapitalCase(tempDistrictDescriptionOnly);

                // cleaning up a few data errors from BC Bid
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("  ", " ");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" Lrg", "Lgr");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" LoadLgr", " Load Lgr");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("23000 - 37999", "23000-37999");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator- Class 1", "Excavator-Class 1");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" 3-4(19.05-23.13)Tonnes", " 3-4 (19.05-23.13)Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" 3 - 4 (19.05-23.13)Tonnes", " 3-4 (19.05-23.13)Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" 5(23.13-26.76)Tonnes", " 5 (23.13-26.76)Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" 6(26.76-30.84)Tonnes", " 6 (26.76-30.84)Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" 7(30.84-39.92)Tonnes", " 7 (30.84-39.92)Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator-Class 8 - 12", "Excavator-Class 8-12");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(")Tonnes", ") Tonnes");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("(Trailer/Skidmou", "(Trailer/Skidmou)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(" Lgr", "Large");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Compressors", "Compressor");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Bulldozers-Mini", "Bulldozer Mini");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Curbing Machines", "Curbing Machine");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Drilling, Specialized Equipment", "Specialized Drilling Equipment");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Geotch. Drilling Companie", "Geotch. Drilling Company");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavators", "Excavator");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavatr", "Excavator");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excvtr", "Excavator");                
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Attachements", "Attachments");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 2 To 41,000 Lbs", "Class 2 To 41,999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Heavy Excavator Class 5 To 58,999", "Heavy Excavator Class 5 To 58,999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Heavy Excavator Class 6 To 67,999", "Heavy Excavator Class 6 To 67,999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Heavy Excavator Class 7 To 87.999", "Heavy Excavator Class 7 To 87.999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Heavy Excavator Class 9 To 152,999 Lbs", "Heavy Excavator Class Class 9-12 To 152,999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 1 (Under 32000 Lbs)", "Excav Class 1 (Under 32000 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 2 (32000 - 41999 Lbs)", "Excav Class 2 (32000-41999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 3 (42000 - 44999 Lbs)", "Excav Class 3 (42000-44999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 4 (45000 - 50999 Lbs)", "Excav Class 4 (45000-50999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 5 (51000 - 58999 Lbs)", "Excav Class 5 (51000-58999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 6 (59000 - 67999 Lbs)", "Excav Class 6 (59000-67999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 8 (88000 - 95999 Lbs)", "Excav Class 8 (88000-95999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 9 (96000 - 102999 Lbs)", "Excav Class 9 (96000-102999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 10 (103000 - 118999 Lbs)", "Excav Class 10 (103000-118999 Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excav Class 11 (119000 - 151999 Lbs)", "Excav Class 11 (119000-151999 Lbs)");                
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator-Class 8-12(39.92-68.95+)", "Excavator-Class 8-12 (39.92-68.95+)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace(") Lbs", " Lbs)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Lbs Lbs", "Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Lbs.", "Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Lb", "Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Lbss", "Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("999Lbs", "999 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("000Lbs+", "000 Lbs+");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("000 Lbs +", "000 Lbs+");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("000+ - 152,000Lbs", "000-152,000 Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tract.1", "Tract. 1");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tract.2", "Tract. 2");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class Class", "Class");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Graders 130 - 144 Fwhp", "Graders 130-144 Fwhp");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Graders Under 100 - 129 Fwhp", "Graders Under 100-129 Fwhp");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Craw.64-90,999Lbs", "Craw 64-90,999Lbs");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator Mini-", "Excavator Mini -");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator Mini -2", "Excavator Mini - 2");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator Mini 2", "Excavator Mini - 2");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Flat Decks", "Flat Deck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Misc. Equipment", "Misc Equipment");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Misc. Equip", "Miscellaneous");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Pipe Crews", "Pipe Crew");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Pipecrew", "Pipe Crew");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roll.TandemLarge.", "Roll. Tandem Large");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roll. TandemLarge", "Roll. Tandem Large");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roll.Tandem Med.", "Roll. Tandem Med");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Scraper - Two Engine -", "Scraper - Two Engines -");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("14.2 Scrapers", "14.2 Scraper");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Skidders", "Skidder");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Selfprop Scrapr", "Self Prop Scrapr");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Bellydump", "Belly Dump");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Dump Combo Pup", "Dump Combo (Pup)");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tandem Dump Trk", "Tandem Dump Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tandem Dump Trks", "Tandem Dump Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tandem Dump Truc", "Tandem Dump Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tandem Dump Truckk", "Tandem Dump Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tractor/Trailers", "Tractor Trailers");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Truck - Tractor Trucks", "Truck - Tractor Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Loaders-Rubber Tired", "Loaders - Rubber Tired");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tractor-Crawler-Class ", "Tractor - Crawler - Class ");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Heavy Hydraulic Excav Class 11 (119000 - 151999 Lbs)", "Heavy Hydraulic Excav Class 11 (119000-151999 Lbs)");               
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 1-3 Under", "Class 1-3 - Under");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 4-5 2", "Class 4-5 - 2");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 6-7 3", "Class 6-7 - 3");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 8-9 4", "Class 8-9 - 4");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 10-11 5", "Class 10-11 - 5");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 12-16 6", "Class 12-16 - 6");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Tractors-Rubber Tired", "Tractors - Rubber Tired");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("(Rubber Tired)6.", "(Rubber Tired) 6");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Rubber Tired-Class ", "Rubber Tired - Class ");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("20 - 59.9 Fhwp", "20-59.9 Fhwp");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("20 - 150+ Fhwp", "20-150+ Fhwp");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 6-11 60-119.9 Fwhp", "Class 6-11 - 60-119.9 Fwhp");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Water Trucks", "Water Truck");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Welding Outfit/Truck Mounted", "Welding Outfit-Truck Mounted");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator-Wheeled-Class", "Excavator-Wheeled - Class");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 1-2(", "Class 1-2 - (");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Class 3(", "Class 3 - (");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Backhoe Sm.-40", "Backhoe Sm. -40");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Water Truck -Tandem", "Water Truck - Tandem");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer-Class ", "Roller/Packer - Class ");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer - Class 7 - 11", "Roller/Packer - Class 7-11");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer - Class 5 8", "Roller/Packer - Class 5 - 8");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer - Class 2-4", "Roller/Packer - Class 2 - 4");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer - Class 7-11", "Roller/Packer - Class 7 - 11");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Sp,Vib,Rubber", "Sp, Vib, Rubber");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Roller/Packer 10-15.9T, ", "Roller/Packer 10-15.9T ");                
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator-Class", "Excavator - Class");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator - Class 3 6", "Excavator - Class 3 - 6");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator - Class 2 3", "Excavator - Class 2 - 3");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Excavator - Class 5 13", "Excavator - Class 5 - 13");
                tempDistrictDescriptionOnly = tempDistrictDescriptionOnly.Replace("Graders-Class", "Graders - Class");

                // get the abbreviation portion
                string tempAbbrev = ImportUtility.CleanString(oldObject.Equip_Type_Cd).ToUpper();

                // cleaning up a few data errors from BC Bid
                tempAbbrev = tempAbbrev.Replace("BUCTRK", "BUCKET");                
                tempAbbrev = tempAbbrev.Replace("DUMP 1", "DUMP1");
                tempAbbrev = tempAbbrev.Replace("DUMP 2", "DUMP2");
                tempAbbrev = tempAbbrev.Replace("MIS", "MISC");
                tempAbbrev = tempAbbrev.Replace("MISCC", "MISC");
                tempAbbrev = tempAbbrev.Replace("HEL", "HELICOP");
                tempAbbrev = tempAbbrev.Replace("SCRNGEQ", "SCRPLA");
                tempAbbrev = tempAbbrev.Replace("SECT. ", "SECT.");
                tempAbbrev = tempAbbrev.Replace("SKI", "SKID");
                tempAbbrev = tempAbbrev.Replace("SKIDD", "SKID");
                tempAbbrev = tempAbbrev.Replace("SSM", "SSS");
                tempAbbrev = tempAbbrev.Replace("SSP", "SSS");
                tempAbbrev = tempAbbrev.Replace("TFC", "TFD");
                tempAbbrev = tempAbbrev.Replace("TRUCKS/", "TRUCKS");
                tempAbbrev = tempAbbrev.Replace("TDB", "TBD");

                string tempDistrictDescriptionFull = string.Format("{0} - {1}", tempAbbrev, tempDistrictDescriptionOnly);

                // add new district equipment type
                int tempId = type.EquipmentTypeId;

                equipType = new HetDistrictEquipmentType
                {
                    DistrictEquipmentTypeId = ++maxEquipTypeIndex,
                    EquipmentTypeId = tempId,
                    DistrictEquipmentName = tempDistrictDescriptionFull
                };

                // ***********************************************
                // set the district
                // ***********************************************
                HetServiceArea serviceArea = dbContext.HetServiceArea.AsNoTracking()
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.MinistryServiceAreaId == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Service Area (Service Area Id: {0} | Equipment Type Id: {1})",
                            oldObject.Service_Area_Id, oldObject.Equip_Type_Id));
                }

                int districtId = serviceArea.District.DistrictId;
                equipType.DistrictId = districtId;

                // ***********************************************
                // check that we don't have this equipment type 
                // already (from another service area - but same district)
                // ***********************************************                
                HetDistrictEquipmentType existingEquipmentType = dbContext.HetDistrictEquipmentType.AsNoTracking()
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.DistrictEquipmentName == tempDistrictDescriptionFull &&
                                         x.District.DistrictId == districtId);

                if (existingEquipmentType != null)
                {
                    equipType.DistrictEquipmentTypeId = existingEquipmentType.DistrictEquipmentTypeId;
                    return; // not adding a duplicate
                }

                // ***********************************************
                // save district equipment type record
                // ***********************************************
                equipType.AppCreateUserid = systemId;
                equipType.AppCreateTimestamp = DateTime.UtcNow;
                equipType.AppLastUpdateUserid = systemId;
                equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetDistrictEquipmentType.Add(equipType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - (Old) Equipment Code: " + oldObject.Equip_Type_Cd);
                Debug.WriteLine("***Error*** - Master District Equipment Index: " + maxEquipTypeIndex);
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

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.EquipType[] legacyItems = (ImportModels.EquipType[])ser.Deserialize(memoryStream);

                // no fields to mask for Equip Type - straight copy
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


