using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using DumpTruck = HETSAPI.ImportModels.DumpTruck;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Dump Truck Records
    /// </summary>
    public static class ImportDumpTruck
    {
        const string OldTable = "Dump_Truck";
        const string NewTable = "Dump_Truck";
        const string XmlFileName = "Dump_Truck.xml";

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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

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

                foreach (DumpTruck item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Id.ToString());

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        CopyToInstance(dbContext, item, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable, 0);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
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
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, DumpTruck oldObject, string systemId)
        {
            if (oldObject.Equip_Id <= 0)
            {
                return;
            }

            // dump truck records update the equipment type - IS_DUMP_TRUCK
            string tempId = oldObject.Equip_Id.ToString();

            ImportMap map = dbContext.ImportMaps.FirstOrDefault(x => x.OldKey == tempId && 
                                                                     x.OldTable == "Equip_Type" &&
                                                                     x.NewKey > 0);

            if (map != null)
            {
                // get the equipment record and update
                EquipmentType equipType = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == map.NewKey);

                if (equipType == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate Equipment Type record (Dump Truck Equip Id: {0}", tempId));
                }

                if (equipType.IsDumpTruck)
                {
                    return; // already updated
                }

                equipType.IsDumpTruck = true;
                equipType.AppLastUpdateUserid = systemId;
                equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.EquipmentTypes.Update(equipType);
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
