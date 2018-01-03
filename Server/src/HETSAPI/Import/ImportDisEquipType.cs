using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class ImportDisEquipType
    {

        const string oldTable = "EquipType";
        const string newTable = "HET_EQUIPMMENT_TYPE";
        const string xmlFileName = "Equip_Type.xml";
        const int sigId = 150000;
        const float defaultBlueBoxSection = (float)0.1;
        const float errowAllowed = (float)0.01;
        const int nullEquipTypeId = 200000;
        const string delim = "::";
        const string delim0 = " :- ";

        public static string oldTable_Progress = oldTable + "_Progress";


        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // Check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, oldTable_Progress, BCBidImport.sigId);
            if (startPoint == BCBidImport.sigId)    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + xmlFileName + " is complete from the former process ***");
                return;
            }
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.EquipType[] legacyItems = (HETSAPI.Import.EquipType[])ser.Deserialize(memoryStream);

                int ii = startPoint;
                if (startPoint > 0)    // Skip the portion already processed
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    string serviceAreaName = "";
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    float equip_Rental_rate_No;
                    try
                    {
                        equip_Rental_rate_No = (float)Decimal.Parse(item.Equip_Rental_Rate_No, System.Globalization.NumberStyles.Any);
                    }
                    catch
                    {
                        equip_Rental_rate_No = (float)0.1;
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

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            Models.DistrictEquipmentType instance = null;
                            serviceAreaName = CopyToInstance(performContext, dbContext, item, ref instance, systemId, equip_Rental_rate_No, description);
                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equip_Rental_rate_No, description, serviceAreaName, true);
                            }
                        }
                    }
                    else // update
                    {
                        Models.DistrictEquipmentType instance = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            serviceAreaName = CopyToInstance(performContext, dbContext, item, ref instance, systemId, equip_Rental_rate_No, description);
                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equip_Rental_rate_No, description, serviceAreaName, false);
                                // update the import map.
                                importMap.NewKey = instance.Id;
                                dbContext.ImportMaps.Update(importMap);
                            }
                        }
                        else // ordinary update.
                        {
                            serviceAreaName = CopyToInstance(performContext, dbContext, item, ref instance, systemId, equip_Rental_rate_No, description);
                            if (serviceAreaName != "ERROR")
                            {
                                AddingDistrictEquipmentTypeInstance(dbContext, item, instance, equip_Rental_rate_No, description, serviceAreaName, false);
                                // touch the import map.
                                importMap.LastUpdateTimestamp = DateTime.UtcNow;
                                dbContext.ImportMaps.Update(importMap);
                            }
                        }
                    }
                    if (++ii % 250 == 0)        // Save change to database once a while to avoid frequent writing to the database.
                    {
                        try
                        {
                            ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, ii.ToString(), BCBidImport.sigId);
                            int iResult = dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            string iStr = e.ToString();
                        }
                    }
                }
                try
                {
                    performContext.WriteLine("*** Importing " + xmlFileName + " is Done ***");
                    ImportUtility.AddImportMap_For_Progress(dbContext, oldTable_Progress, BCBidImport.sigId.ToString(), BCBidImport.sigId);
                    int iResult = dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string iStr = e.ToString();
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
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        /// <param name="equip_Rental_rate_No"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        static private string CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipType oldObject, 
            ref Models.DistrictEquipmentType instance, string systemId, float equip_Rental_rate_No, string description)
        {
            string serviceAreaName = "";

            if (oldObject.Equip_Type_Id <= 0)
                return serviceAreaName;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new Models.DistrictEquipmentType();
                instance.Id = oldObject.Equip_Type_Id;
                string typeCode = "";
                try
                {
                    typeCode = oldObject.Equip_Type_Cd.Length >= 20 ? oldObject.Equip_Type_Cd.Substring(0, 20) : oldObject.Equip_Type_Cd;
                }
                catch (Exception e)
                {
                    string ll = e.ToString();
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
                    else    // This means that the District Id is not in the database.  
                    {       //This happens when the production data does not include district Other than "Lower Mainland" or all the districts                   
                        return "ERROR";
                    }
                }

                //    instance.EquipmentType = 
                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;
                Models.DistrictEquipmentType dt = new DistrictEquipmentType();
                if (oldObject.Equip_Type_Cd != null)
                {
                    EquipmentType eType = dbContext.EquipmentTypes.FirstOrDefault(x => (Math.Abs((x.BlueBookSection??0.1) - equip_Rental_rate_No))<=errowAllowed);
                    if (eType == null)
                    {
                        eType = dbContext.EquipmentTypes.FirstOrDefault(x => (Math.Abs((x.BlueBookSection ?? 0.1) - defaultBlueBoxSection)) <= errowAllowed);
                    }
                    //else    //Just in case we need to update the table of EQUIPMENT_TYPE
                    //{
                    //    if (eType.BlueBookRateNumber == 0.0)  // Update etype with BLUE_BOOK_RATE_NUMBER, MAXIMUN
                    //    {
                    //        try
                    //        {
                    //            eType.MaximumHours = (float)Decimal.Parse(oldObject.Max_Hours, System.Globalization.NumberStyles.Any);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            string ii = e.ToString();
                    //        }
                    //        try
                    //        {
                    //            eType.MaxHoursSub = (float)Decimal.Parse(oldObject.Max_Hours_Sub, System.Globalization.NumberStyles.Any);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            string ii = e.ToString();
                    //        }
                    //        try
                    //        {
                    //            eType.ExtendHours = (float)Decimal.Parse(oldObject.Extend_Hours, System.Globalization.NumberStyles.Any);
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            string ii = e.ToString();
                    //        }
                    //        eType.LastUpdateTimestamp = DateTime.ParseExact(oldObject.Created_Dt == null ? "1900-01-01" : oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    //        eType.LastUpdateUserid = createdBy.SmUserId;
                    //        //Update some content of the Equipment Type
                    //        dbContext.EquipmentTypes.Update(eType);
                    //    }
                    //}
                    instance.EquipmentTypeId = eType.Id;
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
        /// <param name="equip_Rental_rate_No"></param>
        /// <param name="description"></param>
        /// <param name="serviceAreaName"></param>
        /// <param name="addImportMaps"></param>
        static private void AddingDistrictEquipmentTypeInstance(DbAppContext dbContext, HETSAPI.Import.EquipType oldObject,
             Models.DistrictEquipmentType instance, float equip_Rental_rate_No, string description, string serviceAreaName, bool addImportMaps)
        {
            // Add the instance according to the rule of HETS-365
            var disEquipTypelist = dbContext.DistrictEquipmentTypes
                .Where(x => x.DistrictId == instance.DistrictId)
                .Where(x => x.DistrictEquipmentName.Substring(0, Math.Max(0,x.DistrictEquipmentName.IndexOf(delim))).IndexOf(instance.DistrictEquipmentName)>=0)
                .Include(x => x.EquipmentType)
                .ToList();
            if (disEquipTypelist.Count == 0)  //HETS-365 Step 1.
            {
                instance.DistrictEquipmentName += delim + description;
                dbContext.DistrictEquipmentTypes.Add(instance);
                if (addImportMaps)
                {
                    ImportUtility.AddImportMap(dbContext, oldTable, oldObject.Equip_Type_Id.ToString(), newTable, instance.Id);
                }
            }
            else               //HETS-365 Step 2.
            {

                var list1 = disEquipTypelist
                    .FindAll(x => Math.Abs((x.EquipmentType.BlueBookSection ?? 0.1) - equip_Rental_rate_No) <= errowAllowed);
                //     .OrderBy(x => x.Id);
                if (list1.Count > 0 && addImportMaps)  //HETS-365 Step 2.1
                {
                    ImportUtility.AddImportMap(dbContext, oldTable, oldObject.Equip_Type_Id.ToString(), newTable, list1.OrderBy(x => x.Id).FirstOrDefault().Id);
                }

                //Check if XML.Description matches any of the HETS.Descriptions
                var list2 = disEquipTypelist.FindAll(x => x.DistrictEquipmentName.Substring(x.DistrictEquipmentName.IndexOf(delim)+delim.Length).IndexOf(description)>0);
                if (list2.Count > 0 && addImportMaps)  //HETS-365 Step 2.1
                {
                    ImportUtility.AddImportMap(dbContext, oldTable, oldObject.Equip_Type_Id.ToString(), newTable, list2.OrderBy(x => x.Id).FirstOrDefault().Id);
                }
                if (list1.Count == 0 && list2.Count == 0)  //HETS-365 Step 3
                {
                    instance.DistrictEquipmentName += delim0 + serviceAreaName + delim + description;
                    dbContext.DistrictEquipmentTypes.Add(instance);
                    if (addImportMaps)
                    {
                        ImportUtility.AddImportMap(dbContext, oldTable, oldObject.Equip_Type_Id.ToString(), newTable, instance.Id);
                    }
                }
            }

        }
    }
}


