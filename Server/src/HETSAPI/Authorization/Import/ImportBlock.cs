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
    public class ImportBlock
    {
        const string oldTable = "Block";
        const string newTable = "HET_LOCAL_AREA_ROTATION_LIST";
        const string xmlFileName = "Block.xml";
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
                XmlSerializer ser = new XmlSerializer(typeof(Block[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Block[] legacyItems = (HETSAPI.Import.Block[])ser.Deserialize(memoryStream);

                int ii = startPoint;
                if (startPoint > 0)    // Skip the portion already processed
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    int areaId = item.Area_Id ?? 0;
                    int equipmentTypeId = item.Equip_Type_Id ?? 0;
                    int blockNum = Convert.ToInt32(float.Parse(item.Block_Num==null?"0.0": item.Block_Num));

                    // This is for conversion record hope this is uquique:
                    string oldUniqueId = ((areaId * 10000 + equipmentTypeId) * 100 + blockNum).ToString();
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldUniqueId);

                    if (importMap == null) // new entry
                    {
                        if (areaId > 0)
                        {
                            Models.LocalAreaRotationList instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId, oldUniqueId);
                            ImportUtility.AddImportMap(dbContext, oldTable, oldUniqueId, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.LocalAreaRotationList instance = dbContext.LocalAreaRotationLists.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId, oldUniqueId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId, oldUniqueId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    if (++ii % 500 == 0)  // Save change to database once a while to avoid frequent writing to the database.
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
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
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


        /// <summary>
        /// Copy Block item of LocalAreaRotationList item
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        /// <param name="oldUniqueId"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Block oldObject, 
            ref Models.LocalAreaRotationList instance, string systemId, string oldUniqueId)
        {
            bool isNew = false;

            if (oldObject.Area_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            int areaId = oldObject.Area_Id ?? 0;
            int equipmentTypeId = oldObject.Equip_Type_Id ?? 0;
            int blockNum = Convert.ToInt32(float.Parse(oldObject.Block_Num == null ? "0.0" : oldObject.Block_Num));
            int cyclekNum = Convert.ToInt32(float.Parse(oldObject.Cycle_Num == null ? "0.0" : oldObject.Cycle_Num));
            int maxCycle = Convert.ToInt32(float.Parse(oldObject.Max_Cycle == null ? "0.0" : oldObject.Max_Cycle));
            int lastHiredEqupId = oldObject.Last_Hired_Equip_Id ?? 0;

            if (instance == null)
            {
                isNew = true;
                instance = new Models.LocalAreaRotationList();
                // instance.ProjectId = oldObject.Reg_Dump_Trk;
                DistrictEquipmentType disEquipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.Id == equipmentTypeId);
                if (disEquipType != null)
                {
                    instance.DistrictEquipmentType = disEquipType;
                    instance.DistrictEquipmentTypeId = disEquipType.Id;
                }

                // Extract AskNextBlock*Id which is the secondary key of Equip.Id
                int equipId = oldObject.Last_Hired_Equip_Id ?? 0;
                if (dbContext.Equipments.Any(x => x.Id == equipId))   
                {
                    switch (blockNum)
                    {
                        case 1:
                            instance.AskNextBlockOpenId = equipId;
                            break;
                        case 2:
                            instance.AskNextBlock1Id = equipId;
                            break;
                        case 3:
                            instance.AskNextBlock2Id = equipId;
                            break;
                        default:
                            break;
                    }
                }

                instance.CreateUserid = createdBy.SmUserId;
                if (oldObject.Created_Dt != null)
                {
                    instance.CreateTimestamp = DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                dbContext.LocalAreaRotationLists.Add(instance);
            }
            else
            {
                // Updating the existing instance - but how to locate it? There is(are) no unique key(s) for that
            }
        }
    }
}


