using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HETSAPI.Import
{
    public class ImportEquipmentType
    {

        const string oldTable = "EquipType";
        const string newTable = "HET_EQUIPMMENT_TYPE";
        const string xmlFileName = "Equip_Type.xml";
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
                EquipType[] legacyItems = (EquipType[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Type_Id.ToString());

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            EquipmentType instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, item.Equip_Type_Id.ToString(), newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        EquipmentType instance = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }
                    if (ii++ % 500 == 0)  // Save change to database once a while to avoid frequent writing to the database.
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


        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, EquipType oldObject, ref EquipmentType instance, string systemId)
        {
            if (oldObject.Equip_Type_Id <= 0)
                return;

            // Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new EquipmentType();
                instance.Id = oldObject.Equip_Type_Id;
                instance.IsDumpTruck = false;   // Where this is coming from?   !!!!!!
                try
                {
                    instance.ExtendHours = float.Parse(oldObject.Extend_Hours.Trim());
                    instance.MaximumHours = float.Parse(oldObject.Max_Hours.Trim());
                    instance.MaxHoursSub = float.Parse(oldObject.Max_Hours_Sub.Trim());
                }
                catch
                {

                }
                try
                {
                    instance.Name = oldObject.Equip_Type_Cd.Trim();
                }
                catch
                {

                }

                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;
                dbContext.EquipmentTypes.Add(instance);
            }
            else
            {
                instance = dbContext.EquipmentTypes
                    .First(x => x.Id == oldObject.Equip_Type_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                try
                {
                    instance.LastUpdateTimestamp =  DateTime.ParseExact(oldObject.Modified_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {

                }
                dbContext.EquipmentTypes.Update(instance);
            }
        }
    }
}

