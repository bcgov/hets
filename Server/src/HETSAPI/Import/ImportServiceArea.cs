using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.Models;
using ServiceArea = HETSAPI.ImportModels.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Service Area Records
    /// </summary>
    public static class ImportServiceArea
    {
        const string OldTable = "Service_Area";
        const string NewTable = "ServiceArea";
        const string XmlFileName = "Service_Area.xml";
        const int SigId = 150000;

        /// <summary>
        /// Import Service Areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            string completed = DateTime.Now.ToString("d") + "-" + "Completed";
            ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == completed && x.NewKey == SigId);

            if (importMap != null)
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;
                performContext.WriteLine("Processing Service Areas");
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ServiceArea[]), new XmlRootAttribute(rootAttr));
                ser.UnknownAttribute += ImportUtility.UnknownAttribute;
                ser.UnknownElement += ImportUtility.UnknownElement;
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ServiceArea[] legacyItems = (ServiceArea[])ser.Deserialize(memoryStream);

                foreach (ServiceArea item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Service_Area_Id.ToString());

                    Models.ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Name == item.Service_Area_Desc.Trim());

                    // new entry
                    if (importMap == null)
                    {
                        if (item.Service_Area_Cd != "000")
                        {
                            CopyToInstance(dbContext, item, ref serviceArea, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Service_Area_Id.ToString(), NewTable, serviceArea.Id);
                        }
                    }
                    else // update
                    {
                        // record was deleted
                        if (serviceArea != null && serviceArea.Name == null)
                        {
                            CopyToInstance(dbContext, item, ref serviceArea, systemId);

                            // update the import map
                            importMap.NewKey = serviceArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update
                        {
                            CopyToInstance(dbContext, item, ref serviceArea, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }
                }

                performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                ImportUtility.AddImportMap(dbContext, OldTable, completed, NewTable, SigId);

                dbContext.SaveChangesForImport();
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
        /// <param name="serviceArea"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, ServiceArea oldObject, ref Models.ServiceArea serviceArea, string systemId)
        {
            bool isNew = false;

            if (serviceArea == null)
            {
                isNew = true;
                serviceArea = new Models.ServiceArea();
            }

            if (oldObject.Service_Area_Id <= 0)
                return;

            serviceArea.Id = oldObject.Service_Area_Id;
            serviceArea.MinistryServiceAreaID = oldObject.Service_Area_Id;            
            serviceArea.Name = oldObject.Service_Area_Desc.Trim();

            // remove " CA" from Service Area Names
            if (serviceArea.Name.EndsWith(" CA"))
            {
                serviceArea.Name = serviceArea.Name.Replace(" CA", "");
            }

            // service area number
            if (oldObject.Service_Area_Cd != null)
            {
                serviceArea.AreaNumber = int.Parse(oldObject.Service_Area_Cd);
            }

            // get the district for this service area
            int tempServiceAreaId = GetServiceAreaId(serviceArea.Name);

            if (tempServiceAreaId > 0)
            {                

                District district = dbContext.Districts.FirstOrDefault(x => x.MinistryDistrictID == tempServiceAreaId);

                if (district != null)
                {
                    serviceArea.DistrictId = district.Id;
                }
            }

            if (oldObject.FiscalStart != null)
            {
                serviceArea.StartDate = DateTime.ParseExact(oldObject.FiscalStart.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            
            if (isNew)
            {
                serviceArea.AppCreateUserid = systemId;
                serviceArea.AppCreateTimestamp = DateTime.UtcNow;
                serviceArea.AppLastUpdateUserid = systemId;
                serviceArea.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.ServiceAreas.Add(serviceArea);
            }
            else
            {
                serviceArea.AppLastUpdateUserid = systemId;
                serviceArea.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.ServiceAreas.Update(serviceArea);
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ServiceArea[]), new XmlRootAttribute(rootAttr));
                ser.UnknownAttribute += ImportUtility.UnknownAttribute;
                ser.UnknownElement += ImportUtility.UnknownElement;
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ServiceArea[] legacyItems = (ServiceArea[])ser.Deserialize(memoryStream);

                foreach (ServiceArea item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array.
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

        /// <summary>
        /// Unfortunately this has to me manually mapped
        /// However, the data is static and won't change
        /// </summary>
        /// <param name="serviceAreaName"></param>
        /// <returns></returns>
        private static int GetServiceAreaId(string serviceAreaName)
        {
            switch (serviceAreaName.Trim().ToLower())
            {
                case "fraser valley":
                case "lower mainland":
                case "sunshine coast":
                case "howe sound":
                    return 1;

                case "south island":
                case "central island":
                case "north island":
                    return 2;

                case "selkirk":
                case "east kootenay":
                    return 3;

                case "central kootenay":
                case "kootenay boundary":
                    return 4;

                case "okanagan - shuswap":
                case "south okanagan":
                    return 5;

                case "thompson":
                case "nicola":
                    return 6;

                case "northriboo":
                case "centralriboo":
                case "Southriboo":
                    return 7;

                case "north peace":
                case "south peace":
                    return 8;

                case "nechako":
                case "robson":
                case "fort george":
                    return 9;

                case "stikine":
                case "bulkley - nass":
                case "lakes":
                    return 10;

                case "north coast":
                case "skeena":
                    return 11;

                case "non-bc":
                    return 12;

                case "unknown":
                    return 12;
            }

            return 0;
        }
    }
}
