using Hangfire.Console;
using Hangfire.Server;
using System;
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
    /// Import City Records
    /// </summary>
    public static class ImportCity
    {
        private const string OldTable = "HETS_City";
        private const string NewTable = "HET_CITY";
        private const string XmlFileName = "HETS_City.xml";        

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// IMport City Constructor
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            ImportCities(performContext, dbContext, fileLocation, systemId);
        }

        /// <summary>
        /// Import existing Cities
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        private static void ImportCities(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxCityIndex = 0;

            if (dbContext.Cities.Any())
            {
                maxCityIndex = dbContext.Cities.Max(x => x.Id);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HetsCity[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                HetsCity[] legacyItems = (HetsCity[])ser.Deserialize(memoryStream);

                foreach (HetsCity item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.City_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        City city = null;
                        CopyToInstance(dbContext, item, ref city, systemId, ref maxCityIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.City_Id.ToString(), NewTable, city.Id);
                    }                    
                }

                performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, NewTable);
                dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Copy from legacy to new record For the table of HET_City
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="city"></param>
        /// <param name="systemId"></param>
        /// <param name="maxCityIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, HetsCity oldObject, ref City city, string systemId, ref int maxCityIndex)
        {
            try
            {
                if (city != null)
                {
                    return;
                }

                city = new City {  Id = ++maxCityIndex };

                // ***********************************************
                // set the city name
                // ***********************************************  
                string tempName = ImportUtility.CleanString(oldObject.Name);

                if (string.IsNullOrEmpty(tempName))
                {
                    return;
                }

                tempName = ImportUtility.GetCapitalCase(tempName);
                city.Name = tempName;                

                // ***********************************************
                // create city
                // ***********************************************                            
                city.AppCreateUserid = systemId;
                city.AppCreateTimestamp = DateTime.UtcNow;
                city.AppLastUpdateUserid = systemId;
                city.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.Cities.Add(city);                                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - City Id: " + oldObject.City_Id);
                Debug.WriteLine("***Error*** - Service Area Id: " + oldObject.Service_Area_Id);
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
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                EquipAttach[] legacyItems = (EquipAttach[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipAttach data");
                progress.SetValue(0);

                foreach (EquipAttach item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // no excel for city
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}
