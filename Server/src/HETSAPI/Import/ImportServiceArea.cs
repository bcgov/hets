using Hangfire.Console;
using Hangfire.Server;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.Models;
using ServiceArea = HETSAPI.ImportModels.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Service Area Records
    /// </summary>
    public static class ImportServiceArea
    {
        const string OldTable = "Service_Area";
        const string NewTable = "ServiceArea";
        const string XmlFileName = "Service_Area.xml";
        const int SigId = 150000;

        /// <summary>
        /// Import Service Areas
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
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                performContext.WriteLine("Processing Service Areas");
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ServiceArea[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ServiceArea[] legacyItems = (ServiceArea[])ser.Deserialize(memoryStream);

                foreach (ServiceArea item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Service_Area_Id.ToString());

                    Models.ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Name == item.Service_Area_Desc.Trim());

                    if (serviceArea == null)
                    {
                        serviceArea = new Models.ServiceArea();
                    }

                    // new entry
                    if (importMap == null) 
                    {
                        if (item.Service_Area_Cd > 0)
                        {
                            CopyToInstance(performContext, dbContext, item, ref serviceArea, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Service_Area_Id.ToString(), NewTable, serviceArea.Id);
                        }
                    }
                    else // update
                    {
                        // record was deleted
                        if (serviceArea.Name == null) 
                        {
                            CopyToInstance(performContext, dbContext, item, ref serviceArea, systemId);

                            // update the import map
                            importMap.NewKey = serviceArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChangesForImport();
                        }
                        else // ordinary update
                        {
                            CopyToInstance(performContext, dbContext, item, ref serviceArea, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
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
        /// <param name="serviceArea"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, ServiceArea oldObject, ref Models.ServiceArea serviceArea, string systemId)
        {
            bool isNew = false;

            if (serviceArea == null)
            {
                isNew = true;
                serviceArea = new Models.ServiceArea();
            }

            if (oldObject.Service_Area_Id <= 0)
                return;

            serviceArea.Id = oldObject.Service_Area_Id;
            serviceArea.MinistryServiceAreaID = oldObject.Service_Area_Id;
            serviceArea.DistrictId = oldObject.District_Area_Id;
            serviceArea.Name = oldObject.Service_Area_Desc.Trim();
            serviceArea.AreaNumber = oldObject.Service_Area_Cd;

            District district = dbContext.Districts.FirstOrDefault(x => x.MinistryDistrictID == oldObject.District_Area_Id);

            if (district == null)   
            {
                // this means that the District is not in the database 
                // (this happens when the production data does not include district Other than "Lower Mainland" or all the districts)
                return;
            }

            serviceArea.District = district;

            try
            {
                serviceArea.StartDate = DateTime.ParseExact(oldObject.FiscalStart.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                // do nothing
            }

            if (isNew)
            {
                serviceArea.AppCreateUserid = systemId;
                serviceArea.AppCreateTimestamp = DateTime.UtcNow;
                dbContext.ServiceAreas.Add(serviceArea);
            }
            else
            {
                serviceArea.AppLastUpdateUserid = systemId;
                serviceArea.AppLastUpdateTimestamp = DateTime.UtcNow;
                dbContext.ServiceAreas.Update(serviceArea);
            }

            try
            {
                dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Service Area ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

