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
using ServiceArea = HETSAPI.Models.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Local Area Records
    /// </summary>
    public static class ImportLocalArea
    {
        const string OldTable = "Area";
        const string NewTable = "HET_LOCAL_AREA";
        const string XmlFileName = "Area.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Local Areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(Area[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Area[] legacyItems = (Area[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing LocalArea Data. Total Records: " + legacyItems.Length);

                foreach (Area item in legacyItems.WithProgress(progress))
                {                   
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Area_Id.ToString());
                    
                    // new entry
                    if (importMap == null && item.Area_Id > 0)
                    {
                        LocalArea localArea = null;
                        CopyToInstance(dbContext, item, ref localArea, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Area_Id.ToString(), NewTable, localArea.Id);
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
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="localArea"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, Area oldObject, ref LocalArea localArea, string systemId)
        {
            try
            {
                if (oldObject.Area_Id <= 0)
                {
                    return;
                }

                localArea = new LocalArea
                {
                    Id = oldObject.Area_Id,
                    LocalAreaNumber = oldObject.Area_Id,
                    Name = ImportUtility.GetCapitalCase(oldObject.Area_Desc.Trim())
                };

                // map to the correct service area
                ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    // not mapped correctly
                    return;
                }

                localArea.ServiceAreaId = serviceArea.Id;
                        
                localArea.AppCreateUserid = systemId;
                localArea.AppCreateTimestamp = DateTime.UtcNow;
                localArea.AppLastUpdateUserid = systemId;
                localArea.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.LocalAreas.Add(localArea);            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                XmlSerializer ser = new XmlSerializer(typeof(Area[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                Area[] legacyItems = (Area[])ser.Deserialize(memoryStream);

                foreach (Area item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

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

