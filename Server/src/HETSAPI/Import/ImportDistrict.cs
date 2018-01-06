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
    /// Import District Records
    /// </summary>
    public static class ImportDistrict
    {
        const string OldTable = "HETS_District";
        const string NewTable = "HETS_District";
        const string XmlFileName = "District.xml";
        const int SigId = 150000;

        /// <summary>
        /// Import Districts
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
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
                XmlSerializer ser = new XmlSerializer(typeof(HetsDistrict[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                HetsDistrict[] legacyItems = (HetsDistrict[])ser.Deserialize(memoryStream);

                foreach (HetsDistrict item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.District_Id.ToString());

                    // new entry
                    if (importMap == null) 
                    {
                        District dis = null;
                        CopyToInstance(performContext, dbContext, item, ref dis, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.District_Id.ToString(), NewTable, dis.Id);
                    }
                    else // update
                    {
                        District dis = dbContext.Districts.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (dis == null) 
                        {
                            CopyToInstance(performContext, dbContext, item, ref dis, systemId);

                            // update the import map
                            importMap.NewKey = dis.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref dis, systemId);

                            // touch the import map
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                    }
                }

                performContext.WriteLine("*** Done ***");
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
        /// <param name="dis"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HetsDistrict oldObject, ref District dis, string systemId)
        {
            bool isNew = false;

            if (dis == null)
            {
                isNew = true;
                dis = new District();
            }

            if (dbContext.Districts.Count(x => String.Equals(x.Name, oldObject.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)) == 0)
            {
                isNew = true;
                dis.Name = oldObject.Name.Trim();
                dis.Id = oldObject.District_Id;
                dis.MinistryDistrictID = oldObject.Ministry_District_Id;
                dis.RegionId = oldObject.Region_ID;
                dis.CreateTimestamp = DateTime.UtcNow;
                dis.CreateUserid = systemId;
            }

            if (isNew)
            {
                // adding the city to the database table of HET_CITY
                dbContext.Districts.Add(dis);   
            }

            try
            {
                dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update District ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


