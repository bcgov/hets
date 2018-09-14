using System;
using System.Collections.Generic;
using System.Data;
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
using HetsData.Helpers;
using HetsImport.ImportModels;

namespace HetsImport.Import
{
    /// <summary>
    /// Import Project Records
    /// </summary>
    public static class ImportProject
    {
        public const string OldTable = "Project";
        public const string NewTable = "HET_PROJECT";
        public const string XmlFileName = "Project.xml";

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
                performContext.WriteLine("*** Resetting HET_PROJECT database sequence after import ***");
                Debug.WriteLine("Resetting HET_PROJECT database sequence after import");

                if (dbContext.HetProject.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetProject.Max(x => x.ProjectId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_PROJECT_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_PROJECT database sequence after import ***");
                Debug.WriteLine("Resetting HET_PROJECT database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }
        
        /// <summary>
        /// Import Projects
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

            int maxProjectIndex = 0;

            if (dbContext.HetProject.Any())
            {
                maxProjectIndex = dbContext.HetProject.Max(x => x.ProjectId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Project[]), new XmlRootAttribute(rootAttr));
                ser.UnknownAttribute += ImportUtility.UnknownAttribute;
                ser.UnknownElement += ImportUtility.UnknownElement;

                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Project[] legacyItems = (Project[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Project Data. Total Records: " + legacyItems.Length);

                foreach (Project item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Project_Id.ToString());

                    // new entry
                    if (importMap == null && item.Project_Id > 0)
                    {
                        HetProject instance = null;
                        CopyToInstance(dbContext, item, ref instance, systemId, ref maxProjectIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Project_Id.ToString(), NewTable, instance.ProjectId);

                        // save has to be done immediately because we need access to the records
                        dbContext.SaveChangesForImport();
                    }

                    // periodically save change to the progress record
                    if (++ii % 250 == 0)
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
                    string temp = string.Format("Error saving data (ProjectIndex: {0}): {1}", maxProjectIndex, e.Message);
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
        /// <param name="project"></param>
        /// <param name="systemId"></param>
        /// <param name="maxProjectIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, Project oldObject, 
            ref HetProject project, string systemId, ref int maxProjectIndex)
        {
            try
            {
                if (project != null)
                {
                    return;
                }

                project = new HetProject { ProjectId = ++maxProjectIndex };

                // ***********************************************
                // set project properties
                // ***********************************************
                string tempProjectNumber = ImportUtility.CleanString(oldObject.Project_Num).ToUpper();
                if (!string.IsNullOrEmpty(tempProjectNumber))
                {
                    project.ProvincialProjectNumber = tempProjectNumber;
                }

                // project name
                string tempName = ImportUtility.CleanString(oldObject.Job_Desc1).ToUpper();
                if (!string.IsNullOrEmpty(tempName))
                {
                    tempName = ImportUtility.GetCapitalCase(tempName);                    
                    project.Name = tempName;
                }

                // project information
                string tempInformation = ImportUtility.CleanString(oldObject.Job_Desc2);
                if (!string.IsNullOrEmpty(tempInformation))
                {
                    tempInformation = ImportUtility.GetUppercaseFirst(tempInformation);
                    project.Information = tempInformation;
                }                

                // ***********************************************
                // set service area for the project
                // ***********************************************
                HetServiceArea serviceArea = dbContext.HetServiceArea.AsNoTracking()
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.MinistryServiceAreaId == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    throw new DataException(string.Format("Service Area cannot be null (ProjectIndex: {0}", maxProjectIndex));
                }

                int tempDistrictId = serviceArea.District.DistrictId;

                project.DistrictId = tempDistrictId;

                // ***********************************************
                // check that we don't have this equipment type 
                // already (from another service area - but same district)
                // ***********************************************                
                HetProject existingProject = dbContext.HetProject.AsNoTracking()
                    .Include(x => x.ProjectStatusType)
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.Name == tempName &&
                                         x.District.DistrictId == tempDistrictId);

                if (existingProject != null)
                {
                    project.ProjectId = existingProject.ProjectId;
                    return; // not adding a duplicate
                }

                // ***********************************************
                // default the project to Active
                // ***********************************************
                int? statusId = StatusHelper.GetStatusId("Active", "projectStatus", dbContext);

                if (statusId == null)
                {
                    throw new DataException(string.Format("Status Id cannot be null (ProjectIndex: {0}", maxProjectIndex));
                }

                project.ProjectStatusTypeId = (int)statusId;

                // ***********************************************
                // create project
                // ***********************************************                            
                project.AppCreateUserid = systemId;
                project.AppCreateTimestamp = DateTime.UtcNow;
                project.AppLastUpdateUserid = systemId;
                project.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetProject.Add(project);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Project Name: " + oldObject.Job_Desc1);
                Debug.WriteLine("***Error*** - Master project Index: " + maxProjectIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(Project[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                Project[] legacyItems = (Project[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating Project data");
                progress.SetValue(0);

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (Project item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;

                    Random random = new Random();
                    string newProjectNum = random.Next(10000).ToString();

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Project_Num",
                        OriginalValue = item.Project_Num,
                        NewValue = newProjectNum
                    };

                    importMapRecords.Add(importMapRecordOrganization);

                    item.Project_Num = newProjectNum;
                    item.Job_Desc1 = ImportUtility.ScrambleString(item.Job_Desc1);
                    item.Job_Desc2 = ImportUtility.ScrambleString(item.Job_Desc2);                    
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // write out the spreadsheet of import records
                ImportUtility.WriteImportRecordsToExcel(destinationLocation, importMapRecords, OldTable);
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

