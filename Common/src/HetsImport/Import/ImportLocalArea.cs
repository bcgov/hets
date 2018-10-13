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
    /// Import Local Area Records
    /// </summary>
    public static class ImportLocalArea
    {
        public const string OldTable = "Area";
        public const string NewTable = "HET_LOCAL_AREA";
        public const string XmlFileName = "Area.xml";

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
                performContext.WriteLine("*** Resetting HET_LOCAL_AREA database sequence after import ***");
                Debug.WriteLine("Resetting HET_LOCAL_AREA database sequence after import");

                if (dbContext.HetLocalArea.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetLocalArea.Max(x => x.LocalAreaId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_LOCAL_AREA_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_LOCAL_AREA database sequence after import ***");
                Debug.WriteLine("Resetting HET_LOCAL_AREA database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

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

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Area[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.Area[] legacyItems = (ImportModels.Area[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing LocalArea Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.Area item in legacyItems.WithProgress(progress))
                {                   
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Area_Id.ToString());
                    
                    // new entry
                    if (importMap == null && item.Area_Id > 0)
                    {
                        HetLocalArea localArea = null;
                        CopyToInstance(dbContext, item, ref localArea, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Area_Id.ToString(), NewTable, localArea.LocalAreaId);
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
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Area oldObject, 
            ref HetLocalArea localArea, string systemId)
        {
            try
            {
                if (oldObject.Area_Id <= 0)
                {
                    return;
                }

                string tempLocalArea = ImportUtility.CleanString(oldObject.Area_Desc);
                tempLocalArea = ImportUtility.GetCapitalCase(tempLocalArea);

                localArea = new HetLocalArea
                {
                    LocalAreaId = oldObject.Area_Id,
                    LocalAreaNumber = oldObject.Area_Id,
                    Name = tempLocalArea
                };

                // map to the correct service area
                HetServiceArea serviceArea = dbContext.HetServiceArea.AsNoTracking()
                    .FirstOrDefault(x => x.MinistryServiceAreaId == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    // not mapped correctly
                    return;
                }

                localArea.ServiceAreaId = serviceArea.ServiceAreaId;
                        
                localArea.AppCreateUserid = systemId;
                localArea.AppCreateTimestamp = DateTime.UtcNow;
                localArea.AppLastUpdateUserid = systemId;
                localArea.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetLocalArea.Add(localArea);            
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

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Area[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Area[] legacyItems = (ImportModels.Area[])ser.Deserialize(memoryStream);

                foreach (ImportModels.Area item in legacyItems.WithProgress(progress))
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

