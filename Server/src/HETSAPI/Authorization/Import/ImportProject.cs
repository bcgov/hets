using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HETSAPI.Import

{
    public class ImportProject
    {
        const string oldTable = "Project";
        const string newTable = "HET_PROJECT";
        const string xmlFileName = "Project.xml";
        public static string oldTable_Progress = oldTable + "_Progress";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // Check the start point. If startPoint ==  
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, oldTable_Progress, BCBidImport.sigId);
            if (startPoint == BCBidImport.sigId)    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + xmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Project[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Project[] legacyItems = (HETSAPI.Import.Project[])ser.Deserialize(memoryStream);

                int ii = startPoint;
                if (startPoint > 0)    // Skip the portion already processed
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Project_Id.ToString());

                    if (importMap == null) // new entry
                    {
                        if (item.Project_Id > 0)
                        {
                            Models.Project instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, item.Project_Id.ToString(), newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.Project instance = dbContext.Projects.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, ii.ToString(), BCBidImport.sigId);
                            int iResult = dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            string iStr = e.ToString();
                        }
                    }
                }
                try
                {
                    performContext.WriteLine("*** Importing " + xmlFileName + " is Done ***");
                    ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, BCBidImport.sigId.ToString(), BCBidImport.sigId);
                    int iResult = dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string iStr = e.ToString();
                }
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }


        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Project oldObject, ref Models.Project instance, string systemId)
        {
            bool isNew = false;

            if (oldObject.Project_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.Project();
                instance.Id = oldObject.Project_Id;
                try
                {
                    // instance.ProjectId = oldObject.Reg_Dump_Trk;
                    try
                    {   //4 properties
                        instance.ProvincialProjectNumber = oldObject.Project_Num;
                        ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == oldObject.Service_Area_Id);
                        District dis = dbContext.Districts.FirstOrDefault(x => x.Id == serviceArea.DistrictId);
                        if (dis != null)   
                        {
                            instance.District = dis;
                            instance.DistrictId = dis.Id;
                        }
                        else   // This means that the District Id is not in the database.  
                        {      //This happens when the production data does not include district Other than "Lower Mainland" or all the districts                 
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        string iii = e.ToString();
                    }

                    try
                    {
                        instance.Name = oldObject.Job_Desc1;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.Information = oldObject.Job_Desc2;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    try
                    {
                        instance.Notes = new List<Note>();
                        Models.Note note = new Note();
                        note.Text = new string(oldObject.Job_Desc2.Take(2048).ToArray());
                        note.IsNoLongerRelevant = true;
                        instance.Notes.Add(note);
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    //  instance.RentalAgreements
                    // instance.RentalRequests = oldObject.
                    try
                    {   //9 properties
                        instance.CreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        instance.CreateTimestamp = DateTime.UtcNow;
                    }

                    instance.CreateUserid = createdBy.SmUserId;
                }
                catch (Exception e)
                {
                    string i = e.ToString();
                }

                dbContext.Projects.Add(instance);
            }
            else
            {
                instance = dbContext.Projects
                    .First(x => x.Id == oldObject.Project_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                instance.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.Projects.Update(instance);
            }
        }

    }
}

