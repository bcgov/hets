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
    public class ImportDistrict
    {
        const string oldTable = "HETS_District";
        const string newTable = "HETS_District";
        const string xmlFileName = "District.xml";

        /// <summary>
        /// Import Districts
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
                XmlSerializer ser = new XmlSerializer(typeof(HETS_District[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_District[] legacyItems = (HETSAPI.Import.HETS_District[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.District_Id);

                    if (importMap == null) // new entry
                    {
                        Models.District dis = null;
                        CopyToInstance(performContext, dbContext, item, ref dis, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.District_Id, newTable, dis.Id);
                    }
                    else // update
                    {
                        Models.District dis = dbContext.Districts.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (dis == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref dis, systemId);
                            // update the import map.
                            importMap.NewKey = dis.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref dis, systemId);
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

        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_District oldObject, ref Models.District dis, string systemId)
        {
            bool isNew = false;
            if (dis == null)
            {
                isNew = true;
                dis = new Models.District();
            }

            if (dbContext.Districts.Where(x => x.Name.ToUpper() == oldObject.Name.Trim().ToUpper()).Count() == 0)
            {
                isNew = true;
                dis.Name = oldObject.Name.Trim();
                dis.Id = oldObject.District_Id; //dbContext.Districts.Max(x => x.Id) + 1;   //oldObject.Seq_Num;  
                dis.MinistryDistrictID = oldObject.Ministry_District_Id;
                dis.RegionId = oldObject.Region_ID;
                dis.CreateTimestamp = DateTime.UtcNow;
                dis.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Districts.Add(dis);   //Adding the city to the database table of HET_CITY
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update District ***");
                performContext.WriteLine(e.ToString());
            }
        }


    }
}


