using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using ServiceArea = HETSAPI.Models.ServiceArea;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Dis(trict) Equip(ment) Type Records
    /// </summary>
    public static class ImportDisEquipType
    {        
        const string OldTable = "Equip_Type";
        const string NewTable = "HET_EQUIPMMENT_TYPE";
        const string XmlFileName = "Equip_Type.xml";
        const float DefaultBlueBoxSection = (float)0.1;
        const float ErrowAllowed = (float)0.01;
        const string Delim = "::";
        const string Delim0 = " :- ";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Dis(trict) Equip(ment) Type 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        { 
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
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

                if (startPoint > 0)    // skip the portion already processed
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    string serviceAreaName;

                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    float equipRentalRateNo;
                    try
                    {
                        equipRentalRateNo = (float)Decimal.Parse(item.Equip_Rental_Rate_No, System.Globalization.NumberStyles.Any);
                    }
                    catch
                    {
                        equipRentalRateNo = (float)0.1;
                    }

                    string description;
                    try
                    {
                        description = item.Equip_Type_Desc.Length >= 225 ? item.Equip_Type_Desc.Substring(0, 225) : item.Equip_Type_Desc;
                    }
                    catch
                    {
                        description = "";
                    }

                    // new entry
                    if (importMap == null) 
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            DistrictEquipmentType instance = null;

                            serviceAreaName = CopyToInstance(dbContext, item, ref instance, systemId, equipRentalRateNo);

                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equipRentalRateNo, description, serviceAreaName, true);
                            }
                        }
                    }
                    else // update
                    {
                        DistrictEquipmentType instance = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);

                        if (instance == null) // record was deleted
                        {
                            serviceAreaName = CopyToInstance(dbContext, item, ref instance, systemId, equipRentalRateNo);

                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equipRentalRateNo, description, serviceAreaName, false);

                                // update the import map
                                importMap.NewKey = instance.Id;
                                dbContext.ImportMaps.Update(importMap);
                            }
                        }
                        else // ordinary update.
                        {
                            serviceAreaName = CopyToInstance(dbContext, item, ref instance, systemId, equipRentalRateNo);

                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equipRentalRateNo, description, serviceAreaName, false);

                                // touch the import map
                                importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                                dbContext.ImportMaps.Update(importMap);
                            }
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 250 == 0)  
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    performContext.WriteLine("Error saving data " + e.Message);
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy xml item to instance (table entries)
        /// Output is ServiceArea name
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        /// <param name="equipRentalRateNo"></param>
        /// <returns></returns>
        private static string CopyToInstance(DbAppContext dbContext, EquipType oldObject, ref DistrictEquipmentType instance, string systemId, float equipRentalRateNo)
        {
            string serviceAreaName = "";

            if (oldObject.Equip_Type_Id <= 0)
                return serviceAreaName;

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new DistrictEquipmentType();

                string typeCode = "";

                try
                {
                    typeCode = oldObject.Equip_Type_Cd.Length >= 20 ? oldObject.Equip_Type_Cd.Substring(0, 20) : oldObject.Equip_Type_Cd;
                }
                catch
                {
                    // do nothing
                }

                ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);

                if (serviceArea != null)
                {
                    serviceAreaName = serviceArea.Name;
                    instance.DistrictEquipmentName = typeCode;

                    int districtId = serviceArea.DistrictId ?? 0;
                    District dis = dbContext.Districts.FirstOrDefault(x => x.RegionId == districtId);

                    if (dis != null)
                    {
                        instance.DistrictId = districtId;
                        instance.District = dis;
                    }
                    else    
                    {
                        // the District Id is not in the database
                        // (happens when the production data does not include district Other than "Lower Mainland" or all the districts)                   
                        return "ERROR";
                    }
                }

                instance.AppCreateTimestamp = DateTime.UtcNow;
                instance.AppCreateUserid = createdBy.SmUserId;

                if (oldObject.Equip_Type_Cd != null)
                {
                    EquipmentType eType = dbContext.EquipmentTypes.FirstOrDefault(x => (Math.Abs((x.BlueBookSection??0.1) - equipRentalRateNo))<=ErrowAllowed);

                    if (eType == null)
                    {
                        eType = dbContext.EquipmentTypes.FirstOrDefault(x => (Math.Abs((x.BlueBookSection ?? 0.1) - DefaultBlueBoxSection)) <= ErrowAllowed);
                    }

                    if (eType != null) instance.EquipmentTypeId = eType.Id;
                }               
            }

            return serviceAreaName;
        }

        /// <summary>
        ///  Insert District_Equipment_Type or not according to the rule laid out by HETS-365  
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="equipRentalRateNo"></param>
        /// <param name="description"></param>
        /// <param name="serviceAreaName"></param>
        /// <param name="addImportMaps"></param>
        private static void AddingDistrictEquipmentTypeInstance(DbAppContext dbContext, EquipType oldObject,
             DistrictEquipmentType instance, float equipRentalRateNo, string description, string serviceAreaName, bool addImportMaps)
        {
            // check to see if the DistrictEquipmentType record has data.
            if (instance != null && instance.DistrictId != null && instance.DistrictEquipmentName != null)
            {
                // add the instance (according to the rule of HETS-365)
                List<DistrictEquipmentType> disEquipTypelist = dbContext.DistrictEquipmentTypes
                    .Where(x => x.DistrictId == instance.DistrictId)
                    .Where(x => x.DistrictEquipmentName.Substring(0, Math.Max(0, x.DistrictEquipmentName.IndexOf(Delim, StringComparison.Ordinal)))
                                    .IndexOf(instance.DistrictEquipmentName, StringComparison.Ordinal) >= 0)
                    .Include(x => x.EquipmentType)
                    .ToList();

                // HETS-365 Step 1
                if (disEquipTypelist.Count == 0)
                {
                    instance.DistrictEquipmentName += Delim + description;
                    dbContext.DistrictEquipmentTypes.Add(instance);
                    dbContext.SaveChanges();
                    if (addImportMaps)
                    {
                        ImportUtility.AddImportMap(dbContext, OldTable, oldObject.Equip_Type_Id.ToString(), NewTable, instance.Id);
                    }
                }
                else // HETS-365 Step 2
                {

                    List<DistrictEquipmentType> list1 = disEquipTypelist
                        .FindAll(x => Math.Abs((x.EquipmentType.BlueBookSection ?? 0.1) - equipRentalRateNo) <= ErrowAllowed);

                    // HETS-365 Step 2.1
                    if (list1.Count > 0 && addImportMaps)
                    {
                        DistrictEquipmentType temp = list1.OrderBy(x => x.Id).FirstOrDefault();

                        if (temp != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldObject.Equip_Type_Id.ToString(), NewTable, temp.Id);
                        }
                    }

                    // check if XML.Description matches any of the HETS.Descriptions
                    List<DistrictEquipmentType> list2 = disEquipTypelist
                        .FindAll(x => x.DistrictEquipmentName.Substring(x.DistrictEquipmentName.IndexOf(Delim, StringComparison.Ordinal) + Delim.Length)
                                    .IndexOf(description, StringComparison.Ordinal) >= 0);

                    // HETS-365 Step 2.1
                    if (list2.Count > 0 && addImportMaps)
                    {
                        DistrictEquipmentType temp = list2.OrderBy(x => x.Id).FirstOrDefault();

                        if (temp != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldObject.Equip_Type_Id.ToString(), NewTable, temp.Id);
                        }                        
                    }

                    // HETS-365 Step 3
                    if (list1.Count == 0 && list2.Count == 0)
                    {
                        instance.DistrictEquipmentName += Delim0 + serviceAreaName + Delim + description;
                        dbContext.DistrictEquipmentTypes.Add(instance);

                        if (addImportMaps)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, oldObject.Equip_Type_Id.ToString(), NewTable, instance.Id);
                        }
                    }
                }
            }            
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Obfuscating " + XmlFileName + " is complete from the former process ***");
                return;
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
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipType data");
                progress.SetValue(0);

                foreach (EquipType item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;

                    if (item.Modified_By != null)
                    {
                        item.Modified_By = systemId;
                    }
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
                
                // no excel for EquipType
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}


