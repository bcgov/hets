using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using Microsoft.EntityFrameworkCore;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Equip(ment) Attach(ment) Records
    /// </summary>
    public static class ImportEquipAttach
    {
        public const string OldTable = "Equip_Attach";
        public const string NewTable = "HET_EQUIPMENT_ATTACHMENT";
        public const string XmlFileName = "Equip_Attach.xml";

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
                performContext.WriteLine("*** Resetting HET_EQUIPMENT_ATTACHMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_EQUIPMENT_ATTACHMENT database sequence after import");

                if (dbContext.EquipmentAttachments.Any())
                {
                    // get max key
                    int maxKey = dbContext.EquipmentAttachments.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_EQUIPMENT_ATTACHMENT_EQUIPMENT_ATTACHMENT_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_EQUIPMENT_ATTACHMENT database sequence after import ***");
                Debug.WriteLine("Resetting HET_EQUIPMENT_ATTACHMENT database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxEquipAttachIndex = 0;

            if (dbContext.EquipmentAttachments.Any())
            {
                maxEquipAttachIndex = dbContext.EquipmentAttachments.Max(x => x.Id);
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
                
                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Equipment Attachment Data. Total Records: " + legacyItems.Length);

                foreach (EquipAttach item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already. We used old combine because item.Equip_Id is not unique
                    string oldKeyCombined = (item.Equip_Id ?? 0 * 100 + item.Attach_Seq_Num ?? 0).ToString();
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldKeyCombined);

                    // new entry
                    if (importMap == null && item.Equip_Id > 0)
                    {
                        EquipmentAttachment instance = null;
                        CopyToInstance(dbContext, item, ref instance, systemId, ref maxEquipAttachIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, oldKeyCombined, NewTable, instance.Id);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 1000 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId, NewTable);
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
                    string temp = string.Format("Error saving data (EquipmentAttachmentIndex: {0}): {1}", maxEquipAttachIndex, e.Message);
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
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="equipAttach"></param>
        /// <param name="systemId"></param>
        /// <param name="maxEquipAttachIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, EquipAttach oldObject, ref EquipmentAttachment equipAttach,
            string systemId, ref int maxEquipAttachIndex)
        {
            try
            {
                if (oldObject.Equip_Id <= 0)
                {
                    return;
                }

                equipAttach = new EquipmentAttachment { Id = ++maxEquipAttachIndex };

                // ************************************************
                // get the imported equipment record map
                // ************************************************
                string tempId = oldObject.Equip_Id.ToString();

                ImportMap map = dbContext.ImportMaps.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempId &&
                                         x.OldTable == ImportEquip.OldTable &&
                                         x.NewTable == ImportEquip.NewTable);

                if (map == null)
                {
                    return; // ignore and move to the next record
                }

                // ************************************************
                // get the equipment record
                // ************************************************
                Equipment equipment = dbContext.Equipments.AsNoTracking()
                    .FirstOrDefault(x => x.Id == map.NewKey);

                if (equipment == null)
                {
                    throw new ArgumentException(string.Format("Cannot locate Equipment record (EquipAttach Equip Id: {0}", tempId));
                }

                // ************************************************
                // set the equipment attachment attributes
                // ************************************************
                int tempEquipmentId = equipment.Id;
                equipAttach.EquipmentId = tempEquipmentId;

                string tempDescription = ImportUtility.CleanString(oldObject.Attach_Desc);

                if (string.IsNullOrEmpty(tempDescription)) return; // don't add blank attachments

                tempDescription = ImportUtility.GetCapitalCase(tempDescription);

                // populate Name and Description with the same value
                equipAttach.Description = tempDescription;
                equipAttach.TypeName = tempDescription;               

                // ***********************************************
                // create equipment attachment
                // ***********************************************                            
                equipAttach.AppCreateUserid = systemId;
                equipAttach.AppCreateTimestamp = DateTime.UtcNow;
                equipAttach.AppLastUpdateUserid = systemId;
                equipAttach.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.EquipmentAttachments.Add(equipAttach);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Equipment Attachment: " + equipAttach.Description);
                Debug.WriteLine("***Error*** - Master Equipment Attachment Index: " + maxEquipAttachIndex);
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BcBidImport.SigId, NewTable);

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
                XmlSerializer ser = new XmlSerializer(typeof(EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                EquipAttach[] legacyItems = (EquipAttach[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating EquipAttach data");
                progress.SetValue(0);

                foreach (EquipAttach item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // no excel for EquipAttach
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

