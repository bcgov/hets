using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using ServiceArea = HETSAPI.Models.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import District Equipment Type Records
    /// </summary>
    public static class ImportDistrictEquipmentType
    {
        public const string OldTable = "Equip_Type";
        public const string NewTable = "HET_DISTRICT_EQUIPMENT_TYPE";
        public const string XmlFileName = "Equip_Type.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import District Equipment Types
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxEquipTypeIndex = 0;

            if (dbContext.DistrictEquipmentTypes.Any())
            {
                maxEquipTypeIndex = dbContext.DistrictEquipmentTypes.Max(x => x.Id);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing DistrictEquipmentType Data. Total Records: " + legacyItems.Length);

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Equip_Type_Id.ToString() &&
                                             x.NewTable == NewTable);

                    // new entry
                    if (importMap == null && item.Equip_Type_Id > 0)
                    {
                        DistrictEquipmentType equipType = null;
                        CopyToInstance(dbContext, item, ref equipType, systemId, ref maxEquipTypeIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Type_Id.ToString(), NewTable, equipType.Id);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 250 == 0)
                    {
                        try
                        {
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            performContext.WriteLine("Error saving data " + e.Message);
                        }
                    }
                }

                try
                {
                    performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId, NewTable);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string temp = string.Format("Error saving data (DistrictEquipmentTypeIndex: {0}): {1}", maxEquipTypeIndex, e.Message);
                    performContext.WriteLine(temp);
                    throw new DataException(temp);
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Copy xml item to instance (table entries)
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="equipType"></param>
        /// <param name="systemId"></param>
        /// <param name="maxEquipTypeIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipType oldObject, 
            ref DistrictEquipmentType equipType, string systemId, ref int maxEquipTypeIndex)
        {
            try
            {
                if (oldObject.Equip_Type_Id <= 0)
                {
                    return;
                }

                if (equipType != null)
                {
                    return;
                }

                // get the equipment type
                string tempEquipTypeCode = ImportUtility.CleanString(oldObject.Equip_Type_Cd).ToUpper();

                // get the parent equipment type
                EquipmentType type = dbContext.EquipmentTypes.FirstOrDefault(x => x.Name == tempEquipTypeCode);

                if (type == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Equipment Type (Equipment Type Code: {0} | Equipment Type Id: {1})", 
                            tempEquipTypeCode, oldObject.Equip_Type_Id));
                }

                // get the description
                string tempDistrictDescription = ImportUtility.CleanString(oldObject.Equip_Type_Desc);
                tempDistrictDescription = ImportUtility.GetCapitalCase(tempDistrictDescription);

                // add new district equipment type
                int tempId = type.Id;

                equipType = new DistrictEquipmentType
                {
                    Id = ++maxEquipTypeIndex,
                    EquipmentTypeId = tempId,
                    DistrictEquipmentName = tempDistrictDescription
                };
                                      
                // set the district
                ServiceArea serviceArea = dbContext.ServiceAreas
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);

                if (serviceArea == null)
                {
                    throw new ArgumentException(
                        string.Format("Cannot find Service Area (Service Area Id: {0} | Equipment Type Id: {1})",
                            oldObject.Service_Area_Id, oldObject.Equip_Type_Id));
                }

                int districtId = serviceArea.District.Id;
                equipType.DistrictId = districtId;

                // save district equipment type record
                equipType.AppCreateUserid = systemId;
                equipType.AppCreateTimestamp = DateTime.UtcNow;
                equipType.AppLastUpdateUserid = systemId;
                equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.DistrictEquipmentTypes.Add(equipType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - (Old) Equipment Code: " + oldObject.Equip_Type_Cd);
                Debug.WriteLine("***Error*** - Master District Equipment Index: " + maxEquipTypeIndex);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}


