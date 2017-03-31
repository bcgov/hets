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

namespace HETSAPI.Import
{
    public class BCBidImport
    {
        /// <summary>
        /// Copy from legacy to new record
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="serviceArea"></param>
        static private void CopyToServiceArea(DbAppContext dbContext, HETSAPI.Import.Service_Area oldObject, ref ServiceArea serviceArea)
        {
            bool isNew = false;
            if (serviceArea == null)
            {
                isNew = true;
                serviceArea = new ServiceArea();
            }
            serviceArea.MinistryServiceAreaID = oldObject.Service_Area_Id;
            serviceArea.Name = oldObject.Service_Area_Desc;
            serviceArea.AreaNumber = oldObject.Service_Area_Cd;

            District district = dbContext.Districts.FirstOrDefault(x => x.MinistryDistrictID == oldObject.District_Area_Id);
            serviceArea.District = district;

            if (isNew)
            {
                dbContext.ServiceAreas.Add(serviceArea);
            }
            else
            {
                dbContext.ServiceAreas.Update(serviceArea);
            }
            dbContext.SaveChanges();
        }

        static private void CopyToLocalArea(DbAppContext dbContext, HETSAPI.Import.Area oldObject, ref LocalArea localArea)
        {
            bool isNew = false;
            if (localArea == null)
            {
                isNew = true;
                localArea = new LocalArea();
            }
            localArea.Name = oldObject.Area_Desc;            
            
            ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);
            localArea.ServiceArea = serviceArea;

            if (isNew)
            {
                dbContext.LocalAreas.Add(localArea);
            }
            else
            {
                dbContext.LocalAreas.Update(localArea);
            }
            dbContext.SaveChanges();
        }


        static private void CopyToOwner(DbAppContext dbContext, HETSAPI.Import.Owner oldObject, ref Models.Owner owner)
        {
            bool isNew = false;
            if (owner == null)
            {
                isNew = true;
                owner = new Models.Owner();
            }
            owner.OrganizationName = oldObject.CGL_company;

            // TODO finish mapping here 
            if (isNew)
            {
                dbContext.Owners.Add(owner);
            }
            else
            {
                dbContext.Owners.Update(owner);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Utility function to add a new ImportMap to the database
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable"></param>
        /// <param name="oldKey"></param>
        /// <param name="newTable"></param>
        /// <param name="newKey"></param>
        static private void AddImportMap(DbAppContext dbContext, string oldTable, int oldKey, string newTable, int newKey)
        {
            ImportMap importMap = new Models.ImportMap();
            importMap.OldTable = oldTable;
            importMap.OldKey = oldKey;
            importMap.NewTable = newTable;
            importMap.NewKey = newKey;
            dbContext.ImportMaps.Add(importMap);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Import existing service areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportServiceAreas(PerformContext performContext, DbAppContext dbContext, string fileLocation)
        {
            try
            {
                const string oldTable = "Service_Area";
                const string newTable = "ServiceArea";

                performContext.WriteLine("Processing Service Areas");

                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer
                XmlSerializer ser = new XmlSerializer(typeof(HETSAPI.Import.Service_Area[]));

                string fullPath = fileLocation + Path.DirectorySeparatorChar + "Service_Area.xml";
                string contents = File.ReadAllText(fullPath);

                // get the contents of the first tag.
                int startPos = contents.IndexOf('<') + 1;
                int endPos = contents.IndexOf('>');
                string tag = contents.Substring(startPos, endPos - startPos);

                contents = contents.Replace(tag, oldTable);

                string fixedXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                                  + "<ArrayOfService_Area xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n"
                                  + contents
                                  + "\n</ArrayOfService_Area>";

                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXML));
                HETSAPI.Import.Service_Area[] legacy_service_areas = (HETSAPI.Import.Service_Area[])ser.Deserialize(memoryStream);
                foreach (var item in legacy_service_areas.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Service_Area_Id);

                    if (importMap == null) // new entry
                    {
                        ServiceArea serviceArea = null;
                        CopyToServiceArea(dbContext, item, ref serviceArea);
                        AddImportMap(dbContext, oldTable, item.Service_Area_Id, newTable, serviceArea.Id);
                    }
                    else // update
                    {
                        ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (serviceArea == null) // record was deleted
                        {
                            CopyToServiceArea(dbContext, item, ref serviceArea);
                            // update the import map.
                            importMap.NewKey = serviceArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToServiceArea(dbContext, item, ref serviceArea);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Import local areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportLocalAreas(PerformContext performContext, DbAppContext dbContext, string fileLocation)
        {
            try
            {
                const string oldTable = "Area";
                const string newTable = "LocalArea";

                performContext.WriteLine("Processing Local Areas");

                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer
                XmlSerializer ser = new XmlSerializer(typeof(HETSAPI.Import.Area[]));

                string fullPath = fileLocation + Path.DirectorySeparatorChar + "Area.xml";
                string contents = File.ReadAllText(fullPath);

                // get the contents of the first tag.
                int startPos = contents.IndexOf('<') + 1;
                int endPos = contents.IndexOf('>');
                string tag = contents.Substring(startPos, endPos - startPos);

                contents = contents.Replace(tag, oldTable);

                string fixedXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                                  + "<ArrayOfArea xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n"
                                  + contents
                                  + "\n</ArrayOfArea>";

                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXML));
                HETSAPI.Import.Area[] legacy_service_areas = (HETSAPI.Import.Area[])ser.Deserialize(memoryStream);
                foreach (var item in legacy_service_areas.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Area_Id);

                    if (importMap == null) // new entry
                    {
                        LocalArea localArea = null;
                        CopyToLocalArea(dbContext, item, ref localArea);
                        AddImportMap(dbContext, oldTable, item.Service_Area_Id, newTable, localArea.Id);
                    }
                    else // update
                    {
                        LocalArea localArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (localArea == null) // record was deleted
                        {
                            CopyToLocalArea(dbContext, item, ref localArea);
                            // update the import map.
                            importMap.NewKey = localArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToLocalArea(dbContext, item, ref localArea);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// Import local areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportOwners(PerformContext performContext, DbAppContext dbContext, string fileLocation)
        {
            try
            {
                const string oldTable = "Owner";
                const string newTable = "Owner";

                performContext.WriteLine("Processing Owners");

                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer
                XmlSerializer ser = new XmlSerializer(typeof(HETSAPI.Import.Owner[]));

                string fullPath = fileLocation + Path.DirectorySeparatorChar + "Owner.xml";
                string contents = File.ReadAllText(fullPath);

                // get the contents of the first tag.
                int startPos = contents.IndexOf('<') + 1;
                int endPos = contents.IndexOf('>');
                string tag = contents.Substring(startPos, endPos - startPos);

                contents = contents.Replace(tag, oldTable);

                string fixedXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                                  + "<ArrayOfOwner xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n"
                                  + contents
                                  + "\n</ArrayOfOwner>";

                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXML));
                HETSAPI.Import.Owner[] legacy_owners = (HETSAPI.Import.Owner[])ser.Deserialize(memoryStream);
                foreach (var item in legacy_owners.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id);

                    if (importMap == null) // new entry
                    {
                        Models.Owner owner = null;
                        CopyToOwner(dbContext, item, ref owner);
                        AddImportMap(dbContext, oldTable, item.Popt_Id, newTable, owner.Id);
                    }
                    else // update
                    {
                        Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner == null) // record was deleted
                        {
                            CopyToOwner(dbContext, item, ref owner);
                            // update the import map.
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToOwner(dbContext, item, ref owner);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="connectionstring"></param>
        static public void ImportJob(PerformContext context, string connectionstring, string fileLocation)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");

            // start by importing the Service Areas.
            ImportServiceAreas(context, dbContext, fileLocation);
            // Local Areas
            ImportLocalAreas(context, dbContext, fileLocation);
            // Owners
            ImportOwners(context, dbContext, fileLocation);
        }
    }
}
