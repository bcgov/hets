using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using ServiceArea = HETSAPI.Models.ServiceArea;

namespace HETSAPI.Import
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

                if (dbContext.DistrictEquipmentTypes.Any())
                {
                    // get max key
                    int maxKey = dbContext.DistrictEquipmentTypes.Max(x => x.Id);
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

            if (dbContext.DistrictEquipmentTypes.Any())
            {
                maxEquipTypeIndex = dbContext.DistrictEquipmentTypes.Max(x => x.Id);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing DistrictEquipmentType Data. Total Records: " + legacyItems.Length);

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Equip_Type_Id.ToString() &&
                                             x.NewTable == NewTable);

                    // new entry
                    if (importMap == null && item.Equip_Type_Id > 0)
                    {
                        DistrictEquipmentType equipType = null;
                        CopyToInstance(dbContext, item, ref equipType, systemId, ref maxEquipTypeIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, equipType.Id);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 250 == 0)
                    {
                        try
                        {
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
        private static void CopyToInstance(DbAppContext dbContext, EquipType oldObject, 
            ref DistrictEquipmentType equipType, string systemId, ref int maxEquipTypeIndex)
        {
            try
            {                
                if (equipType != null)
                {
                    return;
                }

                // get the equipment type
                float? tempBlueBookRate = ImportUtility.GetFloatValue(oldObject.Equip_Rental_Rate_No);

                if (tempBlueBookRate == null)
                {
                    return;
                }

                // get the parent equipment type
                EquipmentType type = dbContext.EquipmentTypes.FirstOrDefault(x => x.BlueBookSection == tempBlueBookRate);

                // if it's not found - try to round up to the "parent" blue book section
                if (type == null)
                {
                    int tempIntBlueBookRate = Convert.ToInt32(tempBlueBookRate);
                    type = dbContext.EquipmentTypes.FirstOrDefault(x => x.BlueBookSection == tempIntBlueBookRate);
                }

                // finally - if that's not found - map to 0 (MISCELLANEOUS)
                if (type == null)
                {
                    int tempIntBlueBookRate = 0;
                    type = dbContext.EquipmentTypes.FirstOrDefault(x => x.BlueBookSection == tempIntBlueBookRate);
                }

                if (type == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Equipment Type (Blue Book Rate Number: {0} | Equipment Type Id: {1})",
                            tempBlueBookRate.ToString(), oldObject.Equip_Type_Id));
                }

                // get the description
                string tempDistrictDescription = ImportUtility.CleanString(oldObject.Equip_Type_Desc);
                tempDistrictDescription = ImportUtility.GetCapitalCase(tempDistrictDescription);

                string tempAbbrev = ImportUtility.CleanString(oldObject.Equip_Type_Cd).ToUpper();
                tempDistrictDescription = string.Format("{0} - {1}", tempAbbrev, tempDistrictDescription);

                // add new district equipment type
                int tempId = type.Id;

                equipType = new DistrictEquipmentType
                {
                    Id = ++maxEquipTypeIndex,
                    EquipmentTypeId = tempId,
                    DistrictEquipmentName = tempDistrictDescription
                };
                                      
                // set the district
                ServiceArea serviceArea = dbContext.ServiceAreas.AsNoTracking()
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Service Area (Service Area Id: {0} | Equipment Type Id: {1})",
                            oldObject.Service_Area_Id, oldObject.Equip_Type_Id));
                }

                int districtId = serviceArea.District.Id;
                equipType.DistrictId = districtId;

                // save district equipment type record
                equipType.AppCreateUserid = systemId;
                equipType.AppCreateTimestamp = DateTime.UtcNow;
                equipType.AppLastUpdateUserid = systemId;
                equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.DistrictEquipmentTypes.Add(equipType);
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

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);

                // no fields to mask for Equip Type - straight copy
                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // no excel for EquipType.
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


