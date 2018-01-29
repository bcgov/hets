using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Region Records
    /// </summary>
    public static class ImportRegion
    {
        const string OldTable = "HETS_Region";
        const string NewTable = "HETS_Region";
        const string XmlFileName = "Region.xml";
        const int SigId = 150000;

        /// <summary>
        /// Import Regions
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            string completed = DateTime.Now.ToString("d") + "-" + "Completed";

            ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == completed && x.NewKey == SigId);

            if (importMap != null)
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HetsRegion[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                HetsRegion[] legacyItems = (HetsRegion[])ser.Deserialize(memoryStream);

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    Region reg = null;
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Region_Id.ToString());

                    if (dbContext.LocalAreas.Count(x => String.Equals(x.Name, item.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)) > 0)
                    {
                        reg = dbContext.Regions.FirstOrDefault(x => String.Equals(x.Name, item.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    }

                    // new entry
                    if (importMap == null && reg == null) 
                    {
                        CopyToInstance(performContext, dbContext, item, ref reg, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Region_Id.ToString(), NewTable, reg.Id);
                    }
                    else // update
                    {
                        // record was deleted
                        if (reg == null) 
                        {
                            CopyToInstance(performContext, dbContext, item, ref reg, systemId);

                            // update the import map
                            importMap.NewKey = reg.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref reg, systemId);

                            // touch the import map
                            if (importMap != null)
                            {
                                importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                                dbContext.ImportMaps.Update(importMap);
                            }

                            dbContext.SaveChangesForImport();
                        }
                    }
                }

                performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                ImportUtility.AddImportMap(dbContext, OldTable, completed, NewTable, SigId);
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Map data
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="reg"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HetsRegion oldObject, ref Region reg, string systemId)
        {
            bool isNew = false;

            if (reg == null)
            {
                isNew = true;
                reg = new Region();
            }

            if (dbContext.Regions.Count(x => String.Equals(x.Name, oldObject.Name, StringComparison.CurrentCultureIgnoreCase)) == 0)
            {
                isNew = true;
                reg.Name = oldObject.Name.Trim();
                reg.Id = oldObject.Region_Id;
                reg.MinistryRegionID = oldObject.Ministry_Region_Id;
                reg.RegionNumber = oldObject.Region_Number;
                reg.AppCreateTimestamp = DateTime.UtcNow;
                reg.AppCreateUserid = systemId;
            }

            if (isNew)
            {
                // adding the city to the database table of HET_CITY
                dbContext.Regions.Add(reg);
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

