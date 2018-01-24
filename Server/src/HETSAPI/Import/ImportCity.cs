using Hangfire.Console;
using Hangfire.Server;
using System;
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
        private const string NewTable = "HET_City";
        private const string XmlFileName = "City.xml";        
        private const int SigId = 150000;

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

                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HetsCity[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                HetsCity[] legacyItems = (HetsCity[])ser.Deserialize(memoryStream);

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.City_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        City city = null;
                        CopyToInstance(performContext, dbContext, item, ref city, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.City_Id.ToString(), NewTable, city.Id);
                    }
                    else // update
                    {
                        City city = dbContext.Cities.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (city == null) 
                        {
                            CopyToInstance(performContext, dbContext, item, ref city, systemId);

                            // update the import map
                            importMap.NewKey = city.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                        else // ordinary update
                        {
                            CopyToInstance(performContext, dbContext, item, ref city, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                    }
                }

                performContext.WriteLine("*** Done ***");
                ImportUtility.AddImportMap(dbContext, OldTable, completed, NewTable, SigId);
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy from legacy to new record For the table of HET_City
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="city"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HetsCity oldObject, ref City city, string systemId)
        {
            bool isNew = false;

            if (city == null)
            {
                isNew = true;
                city = new City();
            }

            if (!dbContext.Cities.Any(x => string.Equals(x.Name, oldObject.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                isNew = true;
                city.Name = oldObject.Name.Trim();
                city.Id = dbContext.Cities.Max(x => x.Id) + 1;
                city.AppCreateTimestamp = DateTime.UtcNow;
                city.AppCreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Cities.Add(city);   //Adding the city to the database table of HET_CITY
            }

            try
            {
                dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update City ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}
