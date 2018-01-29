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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // This means the import job it has done today is complete for all the records in the xml file.
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
                    if (importMap == null)
                    {
                        if (item.Equip_Id > 0)
                        {
                            Models.DumpTruck instance = null;
                            CopyToInstance(dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.DumpTruck instance = dbContext.DumpTrucks.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null) 
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // update the import map
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref instance, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BCBidImport.SigId);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BCBidImport.SigId.ToString(), BCBidImport.SigId);
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
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, DumpTruck oldObject, ref Models.DumpTruck instance, string systemId)
        {
            if (oldObject.Equip_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, "", systemId);

            if (instance == null)
            {
                instance = new Models.DumpTruck {Id = oldObject.Equip_Id};

                if (oldObject.Single_Axle != null)
                {
                    instance.IsSingleAxle = (oldObject.Single_Axle.Trim() == "Y");
                }

                if (oldObject.Tandem_Axle != null)
                {
                    instance.IsTandemAxle = (oldObject.Tandem_Axle.Trim() == "Y");
                }

                if (oldObject.Tridem != null)
                {
                    instance.IsTridem = (oldObject.Tridem.Trim() == "Y");
                }

                if (oldObject.PUP != null)
                {
                    instance.HasPUP = (oldObject.PUP.Trim() == "Y");
                }

                if (oldObject.Belly_Dump != null)
                {
                    instance.HasBellyDump = (oldObject.Belly_Dump.Trim() == "Y");
                }

                if (oldObject.Rock_Box != null)
                {
                    instance.HasRockBox = (oldObject.Rock_Box.Trim() == "Y");
                }

                if (oldObject.Hilift_Gate != null)
                {
                    instance.HasHiliftGate = (oldObject.Hilift_Gate.Trim() == "Y");
                }

                if (oldObject.Water_Truck != null)
                {
                    instance.IsWaterTruck = (oldObject.Water_Truck.Trim() == "Y");
                }

                if (oldObject.Seal_Coat_Hitch != null)
                {
                    instance.HasSealcoatHitch = (oldObject.Seal_Coat_Hitch.Trim() == "Y");
                }
                
                if (oldObject.Rear_Axle_Spacing != null)
                {
                    instance.RearAxleSpacing = oldObject.Rear_Axle_Spacing;
                }

                if (oldObject.Front_Tire_Size != null)
                {
                    instance.FrontTireSize = oldObject.Front_Tire_Size;
                }

                if (oldObject.Front_Tire_UOM != null)
                {
                    instance.FrontTireUOM = oldObject.Front_Tire_UOM;
                }

                if (oldObject.Front_Axle_Capacity != null)
                {
                    instance.FrontAxleCapacity = oldObject.Front_Axle_Capacity;
                }

                if (oldObject.Rear_Axle_Capacity != null)
                {
                    instance.RearAxleCapacity = oldObject.Rear_Axle_Capacity;
                }

                if (oldObject.Legal_Load != null)
                {
                    instance.LegalLoad = oldObject.Legal_Load;
                }

                if (oldObject.Legal_Capacity != null)
                {
                    instance.LegalCapacity = oldObject.Legal_Capacity;
                }

                if (oldObject.Legal_PUP_Tare_Weight != null)
                {
                    instance.LegalPUPTareWeight = oldObject.Legal_PUP_Tare_Weight;
                }

                if (oldObject.Licenced_GVW != null)
                {
                    instance.LicencedGVW = oldObject.Licenced_GVW;
                }

                if (oldObject.Licenced_GVW_UOM != null)
                {
                    instance.LicencedGVWUOM = oldObject.Licenced_GVW_UOM;
                }

                if (oldObject.Licenced_Tare_Weight != null)
                {
                    instance.LicencedTareWeight = oldObject.Licenced_Tare_Weight;
                }

                if (oldObject.Licenced_PUP_Tare_Weight != null)
                {
                    instance.LicencedPUPTareWeight = oldObject.Licenced_PUP_Tare_Weight;
                }

                if (oldObject.Licenced_Load != null)
                {
                    instance.LicencedLoad = oldObject.Licenced_Load;
                }

                if (oldObject.Licenced_Capacity != null)
                {
                    instance.LicencedCapacity = oldObject.Licenced_Capacity;
                }

                if (oldObject.Box_Length != null)
                {
                    instance.BoxLength = oldObject.Box_Length;
                }

                if (oldObject.Box_Width != null)
                {
                    instance.BoxWidth = oldObject.Box_Width;
                }

                if (oldObject.Box_Height != null)
                {
                    instance.BoxHeight = oldObject.Box_Height;
                }

                if (oldObject.Box_Capacity != null)
                {
                    instance.BoxCapacity = oldObject.Box_Capacity;
                }

                if (oldObject.Trailer_Box_Length != null)
                {
                    instance.TrailerBoxLength = oldObject.Trailer_Box_Length;
                }

                if (oldObject.Trailer_Box_Width != null)
                {
                    instance.TrailerBoxWidth = oldObject.Trailer_Box_Width;
                }

                if (oldObject.Trailer_Box_Height != null)
                {
                    instance.TrailerBoxHeight = oldObject.Trailer_Box_Height;
                }

                if (oldObject.Trailer_Box_Capacity != null)
                {
                    instance.TrailerBoxCapacity = oldObject.Trailer_Box_Capacity;
                }
                     
                instance.AppCreateTimestamp = DateTime.UtcNow;
                instance.AppCreateUserid = createdBy.SmUserId;
                dbContext.DumpTrucks.Add(instance);
            }
            else
            {
                instance = dbContext.DumpTrucks.First(x => x.Id == oldObject.Equip_Id);
                instance.AppLastUpdateUserid = modifiedBy.SmUserId;
                instance.AppLastUpdateTimestamp = DateTime.UtcNow;
                dbContext.DumpTrucks.Update(instance);
            }
        }
    }
}
