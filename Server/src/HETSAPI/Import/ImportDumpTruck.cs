using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using DumpTruck = HETSAPI.ImportModels.DumpTruck;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Dump Truck Records
    /// </summary>
    public static class ImportDumpTruck
    {
        public const string OldTable = "Dump_Truck";
        public const string NewTable = "HET_EQUIPMENT";
        public const string XmlFileName = "Dump_Truck.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Dump Truck
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // Check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // This means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(DumpTruck[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                DumpTruck[] legacyItems = (DumpTruck[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Dump Truck Data. Total Records: " + legacyItems.Length);
                int lastEquipmentIndex = -1;

                foreach (DumpTruck item in legacyItems.WithProgress(progress))
                {
                    lastEquipmentIndex = item.Equip_Id;

                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Id.ToString());

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        CopyToInstance(dbContext, item, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable, item.Equip_Id);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
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
                    string temp = string.Format("Error saving data (Dumptruck EquipmentIndex: {0}): {1}", lastEquipmentIndex, e.Message);
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
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, DumpTruck oldObject, string systemId)
        {
            try
            {
                if (oldObject.Equip_Id <= 0)
                {
                    return;
                }

                // dump truck records update the equipment record
                // find the original equiopment record
                string tempId = oldObject.Equip_Id.ToString();

                ImportMap map = dbContext.ImportMaps.FirstOrDefault(x => x.OldKey == tempId && 
                                                                         x.OldTable == ImportEquip.OldTable &&
                                                                         x.NewTable == ImportEquip.NewTable);

                if (map == null)
                {
                    return; // ignore and move to the next record
                }

                // ************************************************
                // get the equipment record and update
                // ************************************************
                Equipment equipment = dbContext.Equipments.FirstOrDefault(x => x.Id == map.NewKey);

                if (equipment == null)
                {                
                    throw new ArgumentException(string.Format("Cannot locate Equipment record (DumpTruck Equip Id: {0}", tempId));
                }

                // set dump truck attributes
                string tempLicensedGvw = ImportUtility.CleanString(oldObject.Licenced_GVW);
                if (!string.IsNullOrEmpty(tempLicensedGvw))
                {
                    equipment.LicencedGvw = tempLicensedGvw;
                }

                string tempLegalCapacity = ImportUtility.CleanString(oldObject.Legal_Capacity);
                if (!string.IsNullOrEmpty(tempLegalCapacity))
                {
                    equipment.LegalCapacity = tempLegalCapacity;
                }

                string tempPupLegalCapacity = ImportUtility.CleanString(oldObject.Legal_PUP_Tare_Weight);

                if (!string.IsNullOrEmpty(tempPupLegalCapacity))
                {
                    equipment.PupLegalCapacity = tempPupLegalCapacity;
                }

                equipment.AppLastUpdateUserid = systemId;
                equipment.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.Equipments.Update(equipment);

                // ************************************************
                // get the equipment type record and update
                // ************************************************
                int? tempEquipTypeId = equipment.DistrictEquipmentTypeId;

                if (tempEquipTypeId == null) return;
            
                EquipmentType equipmentType = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == tempEquipTypeId);

                if (equipmentType != null && !equipmentType.IsDumpTruck)
                {
                    equipmentType.IsDumpTruck = true;
                    equipmentType.AppLastUpdateUserid = systemId;
                    equipmentType.AppLastUpdateTimestamp = DateTime.UtcNow;

                    dbContext.EquipmentTypes.Update(equipmentType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - (Old) Equipment Id: " + oldObject.Equip_Id);
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
                XmlSerializer ser = new XmlSerializer(typeof(DumpTruck[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                DumpTruck[] legacyItems = (DumpTruck[])ser.Deserialize(memoryStream);

                // no fields to mask for dump truck - straight copy
                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // no excel for DumpTruck.
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}
