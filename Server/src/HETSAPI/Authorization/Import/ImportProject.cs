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
    public class ImportProject
    {
        const string oldTable = "Project";
        const string newTable = "HET_PROJECT";
        const string xmlFileName = "Project.xml";

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
                XmlSerializer ser = new XmlSerializer(typeof(Project[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Project[] legacyItems = (HETSAPI.Import.Project[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Project_Id);

                    if (importMap == null) // new entry
                    {
                        if (item.Project_Id > 0)
                        {
                            Models.Project instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, item.Project_Id, newTable, instance.Id);
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
                        District dis = dbContext.Districts.FirstOrDefault(x => x.Id == oldObject.Service_Area_Id);
                        instance.District = dis;
                        instance.DistrictId = dis.Id;
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
                        instance.Notes = new List<Note>();
                        instance.Notes.Add(new Note(oldObject.Job_Desc2, true));
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    try
                    {   
                      //  instance.RentalAgreements
                     // instance.RentalRequests = oldObject.
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {   //9 properties
                        instance.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10));
                    }
                    catch (Exception e)
                    {
                        instance.CreateTimestamp = DateTime.UtcNow;
                    }

                    try
                    {   // 8 properties
                        instance.CreateUserid = createdBy.SmUserId;
                    }
                    catch (Exception e)
                    {
                        instance.CreateUserid = systemId;
                    }


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
                try
                {
                    instance.LastUpdateUserid = modifiedBy.SmUserId;
                    instance.LastUpdateTimestamp = DateTime.UtcNow;
                }
                catch
                {

                }
                dbContext.Projects.Update(instance);
            }
            try
            {
                //Di dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Project "+ newTable +" ***");
                performContext.WriteLine(e.ToString());
            }
        }

    }
}

