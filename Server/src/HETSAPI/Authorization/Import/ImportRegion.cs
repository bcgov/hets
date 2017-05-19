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
    public class ImportRegion
    {
        const string oldTable = "HETS_Region";
        const string newTable = "HETS_Region";
        const string xmlFileName = "Region.xml";

        /// <summary>
        /// Import Regions
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETS_Region[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_Region[] legacyItems = (HETSAPI.Import.HETS_Region[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    Models.Region reg = null;
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Region_Id.ToString());

                    if (dbContext.LocalAreas.Where(x => x.Name.ToUpper() == item.Name.Trim().ToUpper()).Count() > 0)
                    {
                        reg = dbContext.Regions.FirstOrDefault(x => x.Name.ToUpper() == item.Name.Trim().ToUpper());
                    }

                    if (importMap == null && reg== null) // new entry
                    {
                        CopyToInstance(performContext, dbContext, item, ref reg, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.Region_Id.ToString(), newTable, reg.Id);
                    }
                    else // update
                    {
                        if (reg == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref reg, systemId);
                            // update the import map.
                            importMap.NewKey = reg.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref reg, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            int iResult = dbContext.SaveChangesForImport();
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

        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_Region oldObject, ref Models.Region reg, string systemId)
        {
            bool isNew = false;
            if (reg == null)
            {
                isNew = true;
                reg = new Models.Region();
            }

            if (dbContext.Regions.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0)
            {
                isNew = true;
                reg.Name = oldObject.Name.Trim();
                reg.Id = oldObject.Region_Id; // dbContext.Regions.Max(x => x.Id) + 1;   
                reg.MinistryRegionID = oldObject.Ministry_Region_Id;
                reg.RegionNumber = oldObject.Region_Number;
                reg.CreateTimestamp = DateTime.UtcNow;
                reg.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Regions.Add(reg);   //Adding the city to the database table of HET_CITY
            }
            try
            {
                dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Region ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

