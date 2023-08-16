using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using HetsData.Helpers;
using HetsData.Model;
using HetsCommon;

namespace HetsImport.Import
{
    /// <summary>
    /// Import Rotation Doc Records (reference only)
    /// </summary>
    public static class ImportRotationDoc
    {
        public const string OldTable = "RotationDoc";
        public const string NewTable = "BCBID_ROTATION_DOC";
        public const string XmlFileName = "Rotation_Doc.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Rotation Doc Records Records
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxIndex = startPoint;            

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.RotationDoc[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.RotationDoc[] legacyItems = (ImportModels.RotationDoc[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                Debug.WriteLine("Importing Rotation Doc Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.RotationDoc item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == item.Note_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        BcbidRotationDoc rotationDoc = null;
                        CopyToInstance(dbContext, item, ref rotationDoc, systemId, ref maxIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Note_Id.ToString(), NewTable, rotationDoc.NoteId);
                    }

                    // save change to database
                    if (++ii % 2000 == 0)
                    {                        
                        ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId, NewTable);
                        dbContext.SaveChangesForImport();
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
                    string temp = string.Format("Error saving data (Index: {0}): {1}", maxIndex, e.Message);
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
        /// <param name="rotationDoc"></param>
        /// <param name="systemId"></param>
        /// <param name="maxIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.RotationDoc oldObject, 
            ref BcbidRotationDoc rotationDoc, string systemId, ref int maxIndex)
        {
            try
            {
                if (rotationDoc != null)
                {
                    return;
                }

                rotationDoc = new BcbidRotationDoc { NoteId = oldObject.Note_Id};
                ++maxIndex;

                // ***********************************************
                // we only need records from the current fiscal
                // so ignore all others
                // ***********************************************
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

                // ***********************************************
                // set rotation data
                // ***********************************************
                string noteType = oldObject.Note_Type;

                if (string.IsNullOrEmpty(noteType))
                {
                    return;
                }

                rotationDoc.NoteType = noteType.Trim();

                // reason
                string reason = oldObject.Reason;

                if (!string.IsNullOrEmpty(reason))
                {
                    rotationDoc.Reason = reason;
                }

                // asked date
                DateTime? createdDate = ImportUtility.CleanDate(oldObject.Created_Dt);

                if (createdDate == null ||
                    createdDate < fiscalStart)
                {
                    return; // move to next
                }
                    
                rotationDoc.AskedDate = (DateTime)createdDate;
               

                // was asked -- ForceHire
                if (noteType.ToUpper() == "FORCEHIRE")
                {
                    rotationDoc.WasAsked = false;
                    rotationDoc.IsForceHire = true;
                }
                else
                {
                    rotationDoc.WasAsked = true;
                    rotationDoc.IsForceHire = false;
                }

                // setup the reason
                string tempResponse = "";

                if (noteType.ToUpper() == "FORCEHIRE")
                {
                    tempResponse = "Force Hire";
                }
                else if (noteType.ToUpper() == "NOHIRE")
                {
                    tempResponse = "No Hire";
                }
                else
                {
                    switch (noteType.ToUpper())
                    {
                        case "0":
                            tempResponse = "Owner didn't call back/no answer";
                            break;
                        case "1":
                            tempResponse = "Equipment not suitable";
                            break;
                        case "2":
                            tempResponse = "Working elsewhere";
                            break;
                        case "3":
                            tempResponse = "No agreement on rates";
                            break;
                        case "4":
                            tempResponse = "Equipment under repairs";
                            break;
                        case "5":
                            tempResponse = "Work limit reached";
                            break;
                        case "6":
                            tempResponse = "No WCB/WCB in arrears";
                            break;
                        case "7":
                            tempResponse = "No insurance/inadequate insurance";
                            break;
                        case "8":
                            tempResponse = "Not interested/turned job down";
                            break;
                        case "9":
                            tempResponse = "Equipment not available";
                            break;
                        case "10":
                            tempResponse = "Other";
                            break;
                    }
                }

                if (string.IsNullOrEmpty(tempResponse))
                {
                    tempResponse = noteType;
                }

                rotationDoc.OfferRefusalReason = tempResponse;

                // ************************************************
                // get the imported equipment record map
                // ************************************************
                string tempId = oldObject.Equip_Id.ToString();

                HetImportMap mapEquip = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempId &&
                                         x.OldTable == ImportEquip.OldTable &&
                                         x.NewTable == ImportEquip.NewTable);

                if (mapEquip == null)
                {
                    return; // ignore and move to the next record
                }

                // ***********************************************
                // find the equipment record
                // ***********************************************
                HetEquipment equipment = dbContext.HetEquipment.AsNoTracking()
                    .Include(x => x.LocalArea)
                        .ThenInclude(y => y.ServiceArea)
                            .ThenInclude(z => z.District)
                    .FirstOrDefault(x => x.EquipmentId == mapEquip.NewKey);

                if (equipment == null)
                {
                    return; // ignore and move to the next record
                }

                int tempNewEquipmentId = equipment.EquipmentId; 
                rotationDoc.EquipmentId = tempNewEquipmentId;

                // ************************************************
                // get the imported project record map
                // ************************************************
                string tempProjectId = oldObject.Project_Id.ToString();

                HetImportMap mapProject = dbContext.HetImportMap.AsNoTracking()
                    .FirstOrDefault(x => x.OldKey == tempProjectId &&
                                         x.OldTable == ImportProject.OldTable &&
                                         x.NewTable == ImportProject.NewTable);

                // ***********************************************
                // find the project record
                // ***********************************************
                HetProject project;

                if (mapProject != null)
                {
                    project = dbContext.HetProject.AsNoTracking()
                        .FirstOrDefault(x => x.ProjectId == mapProject.NewKey);

                    if (project == null)
                    {
                        throw new ArgumentException(string.Format("Cannot locate Project record (Rotation Doc Id: {0}", tempId));
                    }

                    int tempNewProjectId = project.ProjectId;
                    rotationDoc.ProjectId = tempNewProjectId;
                }
                else
                {
                    int districtId = equipment.LocalArea.ServiceArea.District.DistrictId;

                    int? statusId = StatusHelper.GetStatusId(HetProject.StatusComplete, "projectStatus", dbContext);
                    if (statusId == null) throw new DataException(string.Format("Status Id cannot be null (Time Sheet Equip Id: {0}", tempId));

                    // create new project
                    project = new HetProject
                    {
                        DistrictId = districtId,
                        Information = "Created to support Rotation Doc import from BCBid",
                        ProjectStatusTypeId = (int)statusId,
                        Name = "Legacy BCBid Project",
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    // save now so we can access it for other time records
                    dbContext.HetProject.Add(project);
                    dbContext.SaveChangesForImport();

                    // add mapping record
                    ImportUtility.AddImportMapForProgress(dbContext, ImportProject.OldTable, tempProjectId, project.ProjectId, ImportProject.NewTable);
                    dbContext.SaveChangesForImport();
                }

                // ***********************************************
                // create rotationDoc
                // ***********************************************                            
                rotationDoc.AppCreateUserid = systemId;
                rotationDoc.AppCreateTimestamp = DateTime.UtcNow;
                rotationDoc.AppLastUpdateUserid = systemId;
                rotationDoc.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.BcbidRotationDoc.Add(rotationDoc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Master Rotation Doc Index: " + maxIndex);
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

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.RotationDoc[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.RotationDoc[] legacyItems = (ImportModels.RotationDoc[])ser.Deserialize(memoryStream);

                performContext.WriteLine("Obfuscating rotation docs data");
                progress.SetValue(0);

                foreach (ImportModels.RotationDoc item in legacyItems.WithProgress(progress))
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
                throw;
            }
        }
    }
}


