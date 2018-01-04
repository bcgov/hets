using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class ImportEquipAttach
    {
        const string oldTable = "EquipAttach";
        const string newTable = "HET_EQUIPMENT_ATTACHMENT";
        const string xmlFileName = "Equip_Attach.xml";
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
                XmlSerializer ser = new XmlSerializer(typeof(EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                EquipAttach[] legacyItems = (EquipAttach[])ser.Deserialize(memoryStream);

                //Use this list to save a trip to query database in each iteration
                List<Equipment> equips = dbContext.Equipments
                        .Include(x => x.DumpTruck)
                        .Include(x => x.DistrictEquipmentType)
                        .ToList();

                int ii = startPoint;
                if (startPoint > 0)    // Skip the portion already processed
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already. We used old combine because item.Equip_Id is not unique.
                    string oldKeyCombined = (item.Equip_Id ?? 0 * 100 + item.Attach_Seq_Num ?? 0).ToString();
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldKeyCombined);

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Id > 0)
                        {
                            EquipmentAttachment instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, equips, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, oldKeyCombined, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        EquipmentAttachment instance = dbContext.EquipmentAttachments.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, equips, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, equips, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    if (++ii % 1000 == 0)       // Save change to database once a while to avoid frequent writing to the database.
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


        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, EquipAttach oldObject, ref EquipmentAttachment instance,
            List<Equipment> equips, string systemId)
        {
            if (oldObject.Equip_Id <= 0)
                return;

            // Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new EquipmentAttachment();
                int equipId = oldObject.Equip_Id ?? -1;

                Equipment equipment = equips.FirstOrDefault(x => x.Id == equipId);
                if (equipment != null)
                {
                    instance.Equipment = equipment;
                    instance.EquipmentId = equipment.Id;
                }

                instance.Description = oldObject.Attach_Desc == null ? "" : oldObject.Attach_Desc;
                instance.TypeName = (oldObject.Attach_Seq_Num ?? -1).ToString();
                if (oldObject.Created_Dt != null && oldObject.Created_Dt.Trim().Length>=10)
                {
                    instance.CreateTimestamp =
                        DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                instance.CreateUserid = createdBy.SmUserId;

                dbContext.EquipmentAttachments.Add(instance);
            }
            else
            {
                instance = dbContext.EquipmentAttachments
                    .First(x => x.EquipmentId == oldObject.Equip_Id && x.TypeName == (oldObject.Attach_Seq_Num?? -2).ToString());
                instance.LastUpdateUserid = systemId; // modifiedBy.SmUserId;
                instance.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.EquipmentAttachments.Update(instance);
            }
        }

    }
}

