using System;
using System.Collections.Generic;
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
using HetsCommon;

namespace HetsImport.Import
{
    /// <summary>
    /// Import Service Area Records
    /// </summary>
    public static class ImportServiceArea
    {
        public const string OldTable = "Service_Area";
        public const string NewTable = "HET_SERVICE_AREA";
        public const string XmlFileName = "Service_Area.xml";

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
                performContext.WriteLine("*** Resetting HET_SERVICE_AREA database sequence after import ***");
                Debug.WriteLine("Resetting HET_SERVICE_AREA database sequence after import");

                if (dbContext.HetServiceArea.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetServiceArea.Max(x => x.ServiceAreaId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_SERVICE_AREA_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();                        
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_SERVICE_AREA database sequence after import ***");
                Debug.WriteLine("Resetting HET_SERVICE_AREA database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Create the district status records
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="systemId"></param>
        public static void ResetDistrictStatus(PerformContext performContext, DbAppContext dbContext, string systemId)
        {
            try
            {
                performContext.WriteLine("*** Creating HET_DISTRICT_STATUS records after import ***");
                Debug.WriteLine("Creating HET_DISTRICT_STATUS records after import");

                // determine the current fiscal year
                DateTime fiscalStart;
                DateTime now = DateTime.Now;
                if (now.Month == 1 || now.Month == 2 || now.Month == 3)
                {
                    fiscalStart = DateUtils.ConvertPacificToUtcTime(
                        new DateTime(now.AddYears(-1).Year, 4, 1, 0, 0, 0));
                }
                else
                {
                    fiscalStart = DateUtils.ConvertPacificToUtcTime(
                        new DateTime(now.Year, 4, 1, 0, 0, 0));
                }

                if (dbContext.HetDistrict.Any())
                {
                    List<HetDistrict> districts = dbContext.HetDistrict.AsNoTracking().ToList();

                    foreach (HetDistrict district in districts)
                    {
                        int id = district.DistrictId;

                        // check if the record already exists
                        bool exists = dbContext.HetDistrictStatus.Any(x => x.DistrictId == id);
                        if (exists) continue;                        

                        // add new status record
                        HetDistrictStatus status = new HetDistrictStatus
                        {
                            DistrictId = id,
                            CurrentFiscalYear = fiscalStart.Year,
                            NextFiscalYear = fiscalStart.Year + 1,
                            DisplayRolloverMessage = false,
                            AppCreateUserid = systemId,
                            AppCreateTimestamp = DateTime.UtcNow,
                            AppLastUpdateUserid = systemId,
                            AppLastUpdateTimestamp = DateTime.UtcNow
                        };

                        dbContext.HetDistrictStatus.Add(status);
                        dbContext.SaveChanges();
                    }
                }

                performContext.WriteLine("*** Done creating HET_DISTRICT_STATUS records after import ***");
                Debug.WriteLine("Creating HET_DISTRICT_STATUS records after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Import Service Areas
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

            try
            {
                string rootAttr = "ArrayOf" + OldTable;
                performContext.WriteLine("Processing Service Areas");
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.ServiceArea[]), new XmlRootAttribute(rootAttr));
                ser.UnknownAttribute += ImportUtility.UnknownAttribute;
                ser.UnknownElement += ImportUtility.UnknownElement;
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.ServiceArea[] legacyItems = (ImportModels.ServiceArea[])ser.Deserialize(memoryStream);

                Debug.WriteLine("Importing ServiceArea Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.ServiceArea item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Service_Area_Id.ToString());                    

                    // new entry
                    if (importMap == null && item.Service_Area_Cd != "000")
                    {
                        HetServiceArea serviceArea = null;
                        CopyToInstance(dbContext, item, ref serviceArea, systemId);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Service_Area_Id.ToString(), NewTable, serviceArea.ServiceAreaId);
                    }
                }

                performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, NewTable);
                dbContext.SaveChangesForImport();
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
        /// <param name="serviceArea"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.ServiceArea oldObject, 
            ref HetServiceArea serviceArea, string systemId)
        {
            try
            {
                if (serviceArea == null)
                {
                    serviceArea = new HetServiceArea();
                }

                if (oldObject.Service_Area_Id <= 0)
                    return;

                serviceArea.ServiceAreaId = oldObject.Service_Area_Id;
                serviceArea.MinistryServiceAreaId = oldObject.Service_Area_Id;            
                serviceArea.Name = oldObject.Service_Area_Desc.Trim();

                // remove " CA" from Service Area Names
                if (serviceArea.Name.EndsWith(" CA"))
                {
                    serviceArea.Name = serviceArea.Name.Replace(" CA", "");
                }

                // service area number
                if (oldObject.Service_Area_Cd != null)
                {
                    serviceArea.AreaNumber = int.Parse(oldObject.Service_Area_Cd);
                }

                // get the district for this service area
                int tempServiceAreaId = GetServiceAreaId(serviceArea.Name);

                if (tempServiceAreaId > 0)
                {                
                    HetDistrict district = dbContext.HetDistrict.AsNoTracking()
                        .FirstOrDefault(x => x.MinistryDistrictId == tempServiceAreaId);

                    if (district != null)
                    {
                        serviceArea.DistrictId = district.DistrictId;
                    }
                }

                if (oldObject.FiscalStart != null)
                {
                    serviceArea.FiscalStartDate = DateTime.ParseExact(oldObject.FiscalStart.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.FiscalEnd != null)
                {
                    serviceArea.FiscalEndDate = DateTime.ParseExact(oldObject.FiscalEnd.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                // Address, Phone, etc.
                serviceArea.Address = oldObject.Address.Trim();

                if (serviceArea.Address.Length > 255)
                {
                    serviceArea.Address = serviceArea.Address.Substring(1, 255);
                }

                serviceArea.Phone = oldObject.Phone.Trim();

                if (serviceArea.Phone.Length > 50)
                {
                    serviceArea.Phone = serviceArea.Phone.Substring(1, 50);
                }

                serviceArea.Phone = ImportUtility.FormatPhone(serviceArea.Phone);

                serviceArea.Fax = oldObject.Fax.Trim();

                if (serviceArea.Fax.Length > 50)
                {
                    serviceArea.Fax = serviceArea.Fax.Substring(1, 50);
                }

                serviceArea.Fax = ImportUtility.FormatPhone(serviceArea.Fax);

                serviceArea.SupportingDocuments = oldObject.Sup_Docs.Trim();

                if (serviceArea.SupportingDocuments.Length > 500)
                {
                    serviceArea.SupportingDocuments = serviceArea.SupportingDocuments.Substring(1, 500);
                }

                serviceArea.AppCreateUserid = systemId;
                serviceArea.AppCreateTimestamp = DateTime.UtcNow;
                serviceArea.AppLastUpdateUserid = systemId;
                serviceArea.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.HetServiceArea.Add(serviceArea);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Service Area Id: " + oldObject.Service_Area_Id);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.ServiceArea[]), new XmlRootAttribute(rootAttr));
                ser.UnknownAttribute += ImportUtility.UnknownAttribute;
                ser.UnknownElement += ImportUtility.UnknownElement;
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.ServiceArea[] legacyItems = (ImportModels.ServiceArea[])ser.Deserialize(memoryStream);

                foreach (ImportModels.ServiceArea item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Unfortunately this has to me manually mapped
        /// However, the data is static and won't change
        /// </summary>
        /// <param name="serviceAreaName"></param>
        /// <returns></returns>
        private static int GetServiceAreaId(string serviceAreaName)
        {
            switch (serviceAreaName.Trim().ToLower())
            {
                case "fraser valley":
                case "lower mainland":
                case "sunshine coast":
                case "howe sound":
                    return 1;

                case "south island":
                case "central island":
                case "north island":
                    return 2;

                case "selkirk":
                case "east kootenay":
                    return 3;

                case "central kootenay":
                case "kootenay boundary":
                    return 4;

                case "okanagan shuswap":
                case "south okanagan":
                    return 5;

                case "thompson":
                case "nicola":
                    return 6;

                case "north cariboo":
                case "central cariboo":
                case "south cariboo":
                    return 7;

                case "north peace":
                case "south peace":
                    return 8;

                case "nechako":
                case "robson":
                case "fort george":
                    return 9;

                case "stikine":
                case "bulkley nass":
                case "lakes":
                    return 10;

                case "north coast":
                case "skeena":
                    return 11;

                case "non-bc":
                    return 12;

                case "unknown":
                    return 12;
            }

            return 0;
        }
    }
}
