using Hangfire.Console;
using Hangfire.Server;
using System;
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
    /// Import Equipment Type Records
    /// </summary>
    public static class ImportEquipmentType
    {
        const string OldTable = "Equip_Type";
        const string NewTable = "HET_EQUIPMENT_TYPE";
        const string XmlFileName = "Equip_Type.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Equipment Types
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

            if (dbContext.EquipmentTypes.Any())
            {
                maxEquipTypeIndex = dbContext.EquipmentTypes.Max(x => x.Id);
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

                Debug.WriteLine("Importing EquipmentType Data. Total Records: " + legacyItems.Length);

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    // new entry (only import new records of Equipment Type)
                    if (importMap == null && item.Equip_Type_Id > 0)
                    {
                        EquipmentType equipType = null;
                        CopyToInstance(dbContext, item, ref equipType, systemId, ref maxEquipTypeIndex);

                        if (equipType != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, equipType.Id);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 500 == 0)
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
                    string temp = string.Format("Error saving data (EquipmentTypeIndex: {0}): {1}", maxEquipTypeIndex, e.Message);
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
        /// <param name="equipType"></param>
        /// <param name="systemId"></param>
        /// <param name="maxEquipTypeIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipType oldObject, 
            ref EquipmentType equipType, string systemId, ref int maxEquipTypeIndex)
        {
            if (oldObject.Equip_Type_Id <= 0)
                return;

            if (equipType != null)
            {
                return; 
            }

            // get the equipment type
            string tempEquipTypeCode = ImportUtility.CleanString(oldObject.Equip_Type_Cd).ToUpper();

            // check if we have this type already
            bool exists = dbContext.EquipmentTypes.Any(x => x.Name == tempEquipTypeCode);

            if (exists)
            {
                return;
            }

            // add new equipment type
            equipType = new EquipmentType
            {
                Id = ++maxEquipTypeIndex,
                IsDumpTruck = false,
                ExtendHours = ImportUtility.GetFloatValue(oldObject.Extend_Hours),
                MaximumHours = ImportUtility.GetFloatValue(oldObject.Max_Hours),
                MaxHoursSub = ImportUtility.GetFloatValue(oldObject.Max_Hours_Sub),
                BlueBookRateNumber = ImportUtility.GetFloatValue(oldObject.Equip_Rental_Rate_No),
                BlueBookSection = ImportUtility.GetFloatValue(oldObject.Equip_Rental_Rate_Page)
            };
           
            if (!string.IsNullOrEmpty(tempEquipTypeCode))
            {
                equipType.Name = tempEquipTypeCode;
            }

            equipType.AppCreateUserid = systemId;
            equipType.AppCreateTimestamp = DateTime.UtcNow;
            equipType.AppLastUpdateUserid = systemId;
            equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

            dbContext.EquipmentTypes.Add(equipType);   
        }
    }
}

