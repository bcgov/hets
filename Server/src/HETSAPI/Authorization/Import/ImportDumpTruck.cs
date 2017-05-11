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
    public class ImportDumpTruck
    {

        const string oldTable       = "Dump_Truck";
        const string newTable       = "Dump_Truck";
        const string xmlFileName    = "Dump_Truck.xml";

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
                XmlSerializer ser = new XmlSerializer(typeof(Dump_Truck[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Dump_Truck[] legacyItems = (HETSAPI.Import.Dump_Truck[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Id);

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Id > 0)
                        {
                            Models.DumpTruck instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, item.Equip_Id, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.DumpTruck instance = dbContext.DumpTrucks.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                            //  dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
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


        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Dump_Truck oldObject, ref Models.DumpTruck instance, string systemId)
        {
            bool isNew = false;

            if (oldObject.Equip_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, "", systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.DumpTruck();
                instance.Id = oldObject.Equip_Id;
                // instance.DumpTruckId = oldObject.Reg_Dump_Trk;
                if (oldObject.Single_Axle != null)
                {
                    instance.IsSingleAxle = (oldObject.Single_Axle.Trim() == "Y") ? true : false;
                }
                if (oldObject.Tandem_Axle != null)
                {
                    instance.IsTandemAxle = (oldObject.Tandem_Axle.Trim() == "Y") ? true : false;
                }
                if (oldObject.Tridem != null)
                {
                    instance.IsTridem = (oldObject.Tridem.Trim() == "Y") ? true : false;
                }
                if (oldObject.PUP != null)
                {
                    instance.HasPUP = (oldObject.PUP.Trim() == "Y") ? true : false;
                }

                //5 properties
                if (oldObject.Belly_Dump != null)
                {
                    instance.HasBellyDump = (oldObject.Belly_Dump.Trim() == "Y") ? true : false;
                }

                if (oldObject.Rock_Box != null)
                {
                    instance.HasRockBox = (oldObject.Rock_Box.Trim() == "Y") ? true : false;
                }
                if (oldObject.Hilift_Gate != null)
                {
                    instance.HasHiliftGate = (oldObject.Hilift_Gate.Trim() == "Y") ? true : false;
                }
                if (oldObject.Water_Truck != null)
                {
                    instance.IsWaterTruck = (oldObject.Water_Truck.Trim() == "Y") ? true : false;
                }
                if (oldObject.Seal_Coat_Hitch != null)
                {
                    instance.HasSealcoatHitch = (oldObject.Seal_Coat_Hitch.Trim() == "Y") ? true : false;
                }

                //5 properties
                // instance.ArchiveDate = oldObject. 
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
                if (oldObject.Rare_Axle_Capacity != null)
                {
                    instance.RearAxleCapacity = oldObject.Rare_Axle_Capacity;
                }

                //3 properties
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

                //6 properties
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

                // 8 properties
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
                     
                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;
                dbContext.DumpTrucks.Add(instance);
            }
            else
            {
                instance = dbContext.DumpTrucks
                    .First(x => x.Id == oldObject.Equip_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                instance.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.DumpTrucks.Update(instance);
            }
        }

    }
}
