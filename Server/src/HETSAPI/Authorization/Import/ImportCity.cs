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
    public class ImportCity
    {
        const string oldTable = "HETS_City";
        const string newTable = "HET_City";
        const string xmlFileName = "City.xml";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
        }
        /// <summary>
        /// Import existing Cities
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportCities(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETS_City[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_City[] legacyItems = (HETSAPI.Import.HETS_City[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.City_Id);

                    if (importMap == null) // new entry
                    {
                        Models.City city = null;
                        CopyToInstance(performContext, dbContext, item, ref city, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.City_Id, newTable, city.Id);
                    }
                    else // update
                    {
                        Models.City city = dbContext.Cities.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (city == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref city, systemId);
                            // update the import map.
                            importMap.NewKey = city.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref city, systemId);
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
        /// Copy from legacy to new record For the table of HET_City
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="city"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_City oldObject, ref Models.City city, string systemId)
        {
            bool isNew = false;
            if (city == null)
            {
                isNew = true;
                city = new Models.City();
            }

            if (dbContext.Cities.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0)
            {
                isNew = true;
                city.Name = oldObject.Name.Trim();
                city.Id = dbContext.Cities.Max(x => x.Id) + 1;   //oldObject.Seq_Num;  
                city.CreateTimestamp = DateTime.UtcNow;
                city.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Cities.Add(city);   //Adding the city to the database table of HET_CITY
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update City ***");
                performContext.WriteLine(e.ToString());
            }
        }

    }
}
