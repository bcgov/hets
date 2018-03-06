using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Equip(ment) Attach(ment) Records
    /// </summary>
    public static class ImportEquipAttach
    {
        const string OldTable = "Equip_Attach";
        const string NewTable = "HET_EQUIPMENT_ATTACHMENT";
        const string XmlFileName = "Equip_Attach.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Equipment Attachments
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // This means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                EquipAttach[] legacyItems = (EquipAttach[])ser.Deserialize(memoryStream);

                // use this list to save a trip to query database in each iteration
                List<Equipment> equips = dbContext.Equipments
                        .Include(x => x.DumpTruck)
                        .Include(x => x.DistrictEquipmentType)
                        .ToList();

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (EquipAttach item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already. We used old combine because item.Equip_Id is not unique
                    string oldKeyCombined = (item.Equip_Id ?? 0 * 100 + item.Attach_Seq_Num ?? 0).ToString();
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldKeyCombined);

                    // new entry
                    if (importMap == null) 
                    {
                        if (item.Equip_Id > 0)
                        {
                            EquipmentAttachment instance = null;
                            CopyToInstance(dbContext, item, ref instance, equips, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, oldKeyCombined, NewTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        EquipmentAttachment instance = dbContext.EquipmentAttachments.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null)
                        {
                            CopyToInstance(dbContext, item, ref instance, equips, systemId);

                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref instance, equips, systemId);

                            // touch the import map
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 1000 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BCBidImport.SigId);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BCBidImport.SigId.ToString(), BCBidImport.SigId);
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
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="equips"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipAttach oldObject, ref EquipmentAttachment instance,
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

                // get the new ID.
                var importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldKey == oldObject.Equip_Id.ToString() && x.OldTable == "Equip");
                
                Equipment equipment = null;
                if (importMap != null)
                {
                    equipment = equips.FirstOrDefault(x => x.Id == importMap.NewKey);
                }

                
                if (equipment != null)
                {
                    instance.Equipment = equipment;
                    instance.EquipmentId = equipment.Id;
                }

                instance.Description = oldObject.Attach_Desc == null ? "" : oldObject.Attach_Desc;
                instance.TypeName = (oldObject.Attach_Seq_Num ?? -1).ToString();

                if (oldObject.Created_Dt != null && oldObject.Created_Dt.Trim().Length>=10)
                {
                    instance.AppCreateTimestamp =
                        DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                instance.AppCreateUserid = createdBy.SmUserId;

                dbContext.EquipmentAttachments.Add(instance);
            }
            else
            {
                instance = dbContext.EquipmentAttachments
                    .First(x => x.EquipmentId == oldObject.Equip_Id && x.TypeName == (oldObject.Attach_Seq_Num?? -2).ToString());
                instance.AppLastUpdateUserid = systemId;
                instance.AppLastUpdateTimestamp = DateTime.UtcNow;
                dbContext.EquipmentAttachments.Update(instance);
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.EquipAttach[] legacyItems = (ImportModels.EquipAttach[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipAttach data");
                progress.SetValue(0);

                foreach (ImportModels.EquipAttach item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;

                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);
                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
                // no excel for EquipAttach.

            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

