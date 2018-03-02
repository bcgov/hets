using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Equip(ment) Records
    /// </summary>
    public static class ImportEquip
    {
        const string OldTable = "Equip";
        const string NewTable = "HET_EQUIPMMENT";
        const string XmlFileName = "Equip.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Equipment
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
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
                XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Equip[] legacyItems = (Equip[]) ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (Equip item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x =>
                        x.OldTable == OldTable && x.OldKey == item.Equip_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        if (item.Equip_Id > 0)
                        {
                            Equipment instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Equip_Id.ToString(), NewTable,
                                instance.Id);
                        }
                    }
                    else // update
                    {
                        Equipment instance = dbContext.Equipments.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null && item.Equip_Id > 0)
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);

                            // update the import map
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update
                        {
                            if (item.Equip_Id > 0)
                            {
                                CopyToInstance(performContext, dbContext, item, ref instance, systemId);

                                // touch the import map
                                importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
                                dbContext.ImportMaps.Update(importMap);
                            }
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (ii++ % 1000 == 0)
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
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(PerformContext performContext, DbAppContext dbContext, Equip oldObject, ref Equipment instance, string systemId)
        {
            if (oldObject.Equip_Id <= 0)
                return;
            User modifiedBy = null;
            User createdBy = null;

            if (oldObject.Modified_By != null)
            {
                modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            }
            if (oldObject.Created_By != null)
            {
                createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);
            }
            
            if (instance == null)
            {
                instance = new Equipment
                {
                    Id = oldObject.Equip_Id,
                    ArchiveCode = oldObject.Archive_Cd == null
                        ? ""
                        : new string(oldObject.Archive_Cd.Take(50).ToArray()),
                    ArchiveReason = oldObject.Archive_Reason == null
                        ? ""
                        : new string(oldObject.Archive_Reason.Take(2048).ToArray()),
                    LicencePlate = oldObject.Licence == null ? "" : new string(oldObject.Licence.Take(20).ToArray())
                };

                if (oldObject.Approved_Dt != null)
                {
                    instance.ApprovedDate = DateTime.ParseExact(oldObject.Approved_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.Received_Dt != null)
                {
                    instance.ReceivedDate = DateTime.ParseExact(oldObject.Received_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.Comment != null)
                {
                    instance.Notes = new List<Note>();

                    Note note = new Note
                    {
                        Text = new string(oldObject.Comment.Take(2048).ToArray()),
                        IsNoLongerRelevant = true
                    };

                    instance.Notes.Add(note);
                }

                if (oldObject.Area_Id != null)
                {
                    LocalArea area = dbContext.LocalAreas.FirstOrDefault(x => x.LocalAreaNumber == oldObject.Area_Id);
                    if (area != null)
                        instance.LocalArea = area;
                }

                if (oldObject.Equip_Type_Id != null)
                {
                    // get the new ID for the Equip Type.
                    var importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldKey == oldObject.Equip_Type_Id.ToString() && x.OldTable == "EquipType");

                    //Equipment_TYPE_ID is copied to the table of HET_DISTRICT_DISTRICT_TYPE as key
                    DistrictEquipmentType equipType = null;

                    if (importMap != null)
                    {
                        equipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.EquipmentTypeId == importMap.NewKey);
                    }

                    if (equipType != null)
                    {
                        instance.DistrictEquipmentType = equipType;
                        instance.DistrictEquipmentTypeId = oldObject.Equip_Type_Id;
                    }
                }

                instance.EquipmentCode = oldObject.Equip_Cd == null ? "" : new string(oldObject.Equip_Cd.Take(25).ToArray());
                instance.Model = oldObject.Model == null ? "" : new string(oldObject.Model.Take(50).ToArray());
                instance.Make = oldObject.Make == null ? "" : new string(oldObject.Make.Take(50).ToArray());
                instance.Year = oldObject.Year == null ? "" : new string(oldObject.Year.Take(15).ToArray());
                instance.Operator = oldObject.Operator == null ? "" : new string(oldObject.Operator.Take(255).ToArray());
                instance.SerialNumber = oldObject.Serial_Num == null ? "" : new string(oldObject.Serial_Num.Take(100).ToArray());
                instance.Status = oldObject.Status_Cd == null ? "" : new string(oldObject.Status_Cd.Take(50).ToArray());
                instance.Type = oldObject.Type == null ? "" : oldObject.Type.Trim();

                if (oldObject.Pay_Rate != null)
                {
                    try
                    {
                        instance.PayRate = float.Parse(oldObject.Pay_Rate.Trim());
                    }
                    catch
                    {
                        instance.PayRate = (float)0.0;
                    }
                }

                if (instance.Seniority != null)
                {
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch
                    {
                        instance.Seniority = (float)0.0;
                    }
                }

                // find the owner which is referenced in the equipment of the xml file entry Through ImportMaps because owner_ID is not prop_ID
                ImportMap map = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == ImportOwner.OldTable && x.OldKey == oldObject.Owner_Popt_Id.ToString());

                if (map != null)
                {
                    Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == map.NewKey);
                    if (owner != null)
                    {
                        instance.Owner = owner;
                        Contact con = dbContext.Contacts.FirstOrDefault(x => x.Id == owner.PrimaryContactId);

                        // update owner contact address
                        if (con != null)            
                        {
                            try
                            {
                                con.Address1 = oldObject.Addr1;
                                con.Address2 = oldObject.Addr2;
                                con.City = oldObject.City;
                                con.PostalCode = oldObject.Postal;
                                con.Province = "BC";
                                dbContext.Contacts.Update(con);
                            }
                            catch (Exception e)
                            {
                                performContext.WriteLine("Error mapping data " + e.Message);
                            }
                        }
                    }
                }
                
                if (oldObject.Seniority != null)
                {
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch
                    {
                        instance.Seniority = (float)0.0;
                    }
                }

                if (oldObject.Num_Years != null)
                {
                    try
                    {
                        instance.YearsOfService = float.Parse(oldObject.Num_Years.Trim());
                    }
                    catch
                    {
                        instance.YearsOfService = (float)0.0;
                    }
                }

                if (oldObject.Block_Num != null)
                {
                    try
                    {
                        instance.BlockNumber = decimal.ToInt32(Decimal.Parse(oldObject.Block_Num, System.Globalization.NumberStyles.Float));
                    }
                    catch
                    {
                        // do nothing
                    }
                }

                if (oldObject.Size != null)
                {
                    try
                    {
                        if (oldObject.Size != null && !oldObject.Size.Trim().Equals("#x20;"))
                        {
                            instance.Size = oldObject.Size;
                        }
                        
                    }
                    catch
                    {
                        // do nothing
                    }
                }

                if (oldObject.YTD1 != null && oldObject.YTD2 != null && oldObject.YTD3 != null)
                {
                    try
                    {
                        instance.ServiceHoursLastYear = (float)Decimal.Parse(oldObject.YTD1, System.Globalization.NumberStyles.Any);
                    }
                    catch
                    {
                        instance.ServiceHoursLastYear = (float)0.0;
                    }
                    try
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)Decimal.Parse(oldObject.YTD2, System.Globalization.NumberStyles.Any); 
                        instance.ServiceHoursThreeYearsAgo = (float)Decimal.Parse(oldObject.YTD3, System.Globalization.NumberStyles.Any);  
                    }
                    catch
                    {
                        instance.ServiceHoursTwoYearsAgo = (float)0.0;
                        instance.ServiceHoursThreeYearsAgo = (float)0.0;
                    }
                }

                instance.AppCreateTimestamp = DateTime.UtcNow;
                if (createdBy != null)
                {
                    instance.AppCreateUserid = createdBy.SmUserId;
                }

                // adjust status and archive date fields.
                if (instance.ArchiveCode != null && instance.ArchiveCode.Trim().ToUpper().Equals("Y"))
                {
                    instance.Status = "Archived";
                    instance.ArchiveDate = DateTime.UtcNow;
                }
                else
                {
                    if (instance.Status != null && instance.ArchiveCode != null && instance.ArchiveCode.Trim().ToUpper().Equals("N"))
                    {
                        if (instance.Status.Trim().ToUpper().Equals("U"))
                        {
                            instance.ArchiveDate = null;
                            instance.Status = "Unapproved";
                        }
                        else if (instance.Status.Trim().ToUpper().Equals("A"))
                        {
                            instance.ArchiveDate = null;
                            instance.Status = "Approved";
                        }
                    }                    
                }


                dbContext.Equipments.Add(instance);
            }
            else
            {
                instance = dbContext.Equipments.First(x => x.Id == oldObject.Equip_Id);
                if (modifiedBy != null)
                {
                    instance.AppLastUpdateUserid = modifiedBy.SmUserId;
                }

                try
                {
                    if (modifiedBy != null)
                    {
                        instance.AppLastUpdateUserid = modifiedBy.SmUserId;
                    }
                    if (oldObject.Modified_Dt != null)
                    {
                        instance.AppLastUpdateTimestamp = DateTime.ParseExact(oldObject.Modified_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    
                }
                catch (Exception e)
                {
                    performContext.WriteLine("Error mapping data " + e.Message);
                }

                dbContext.Equipments.Update(instance);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Equip[] legacyItems = (ImportModels.Equip[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating Equip data");
                progress.SetValue(0);

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (ImportModels.Equip item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                    if (item.Modified_By != null)
                    {
                        item.Modified_By = systemId;
                    }

                    Random random = new Random();
                    string newSerialNum = random.Next(10000).ToString();

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord();


                    importMapRecordOrganization.TableName = NewTable;
                    importMapRecordOrganization.MappedColumn = "Serial_Num";
                    importMapRecordOrganization.OriginalValue = item.Serial_Num;
                    importMapRecordOrganization.NewValue = newSerialNum;
                    importMapRecords.Add(importMapRecordOrganization);

                    item.Serial_Num = newSerialNum;
                    item.Addr1 = ImportUtility.ScrambleString(item.Addr1);
                    item.Addr2 = ImportUtility.ScrambleString(item.Addr2);
                    item.Addr3 = ImportUtility.ScrambleString(item.Addr3);
                    item.Addr4 = ImportUtility.ScrambleString(item.Addr4);
                    item.Postal = ImportUtility.ScrambleString(item.Postal);
                    item.Licence = ImportUtility.ScrambleString(item.Licence);
                    item.Operator = ImportUtility.ScrambleString(item.Operator);                    
                }

                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);
                // write out the array.
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
                // write out the spreadsheet of import records.
                ImportUtility.WriteImportRecordsToExcel(destinationLocation, importMapRecords, OldTable);

            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}
