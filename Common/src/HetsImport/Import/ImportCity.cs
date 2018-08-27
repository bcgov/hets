using System;
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
        /// Fix the sequence for the tables populated by the import process
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        public static void ResetSequence(PerformContext performContext, DbAppContext dbContext)
        {
            try
            {
                performContext.WriteLine("*** Resetting HET_CITY database sequence after import ***");
                Debug.WriteLine("Resetting HET_CITY database sequence after import");

                if (dbContext.HetCity.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetCity.Max(x => x.CityId);
                    maxKey = maxKey + 1;
                    
                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_CITY_CITY_ID_seq"" RESTART WITH {0};", maxKey);
                        
                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_CITY database sequence after import ***");
                Debug.WriteLine("Resetting HET_CITY database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Import City Constructor
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

            if (dbContext.HetCity.Any())
            {
                maxCityIndex = dbContext.HetCity.Max(x => x.CityId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.HetsCity[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.HetsCity[] legacyItems = (ImportModels.HetsCity[])ser.Deserialize(memoryStream);

                foreach (ImportModels.HetsCity item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.City_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        HetCity city = null;
                        CopyToInstance(dbContext, item, ref city, systemId, ref maxCityIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.City_Id.ToString(), NewTable, city.CityId);
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
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.HetsCity oldObject, 
            ref HetCity city, string systemId, ref int maxCityIndex)
        {
            try
            {
                if (city != null)
                {
                    return;
                }

                city = new HetCity {  CityId = ++maxCityIndex };

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

                dbContext.HetCity.Add(city);                                
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

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.HetsCity[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.HetsCity[] legacyItems = (ImportModels.HetsCity[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating HetsCity data");
                progress.SetValue(0);

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
