using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Owner Records
    /// </summary>
    public static class ImportOwner
    {
        public static string OldTable = "Owner";
        public static string NewTable = "HET_OWNER";
        public static string XmlFileName = "Owner.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Owner Records
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            List<Owner> data = new List<Owner>();
            int maxOwnerIndex = dbContext.Owners.Max(x => x.Id);
            int maxContactIndex = dbContext.Contacts.Max(x => x.Id);

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Owner[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.Owner[] legacyItems = (ImportModels.Owner[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (ImportModels.Owner item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        Owner owner = null;
                        CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                        data.Add(owner);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Popt_Id.ToString(), NewTable, owner.Id);
                    }
                    else // update
                    {
                        Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner == null) // record was deleted
                        {
                            CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);

                            // update the import map
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                            
                            // touch the import map
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0) 
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
        /// <param name="owner"></param>
        /// <param name="systemId"></param>
        /// <param name="maxOwnerIndex"></param>
        /// <param name="maxContactIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Owner oldObject, ref Owner owner,
          string systemId, ref int maxOwnerIndex, ref int maxContactIndex)
        {
            bool isNew = false;

            if (owner == null)
            {
                isNew = true;
                owner = new Owner {Id = ++maxOwnerIndex};
            }

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);
            
            try
            {
                owner.IsMaintenanceContractor = (oldObject.Maintenance_Contractor.Trim() == "Y");
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.LocalArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.CGLEndDate =  
                    DateTime.ParseExact(oldObject.CGL_End_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.WorkSafeBCExpiryDate =  
                    DateTime.ParseExact(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.WorkSafeBCPolicyNumber = oldObject.WCB_Num.Trim();
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.OrganizationName = oldObject.CGL_Company.Trim();
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.ArchiveCode = oldObject.Archive_Cd;
            }
            catch
            {
                // do nothing
            }

            try
            {
                owner.Status = oldObject.Status_Cd.Trim();
            }
            catch
            {
                // do nothing
            }

            Contact con = dbContext.Contacts.FirstOrDefault(x => x.GivenName.ToUpper() == oldObject.Owner_First_Name.Trim().ToUpper() && x.Surname.ToUpper() == oldObject.Owner_Last_Name.Trim().ToUpper());

            if (con == null)
            {
                con = new Contact(++maxContactIndex);

                try
                {
                    con.Surname = oldObject.Owner_Last_Name.Trim();
                    con.GivenName = oldObject.Owner_First_Name.Trim();
                    owner.OwnerEquipmentCodePrefix = con.GivenName.Substring(0, 1) + con.Surname.Substring(0, 1);
                }
                catch
                {
                    // do nothing
                }

                con.FaxPhoneNumber = "";
                con.Province = "BC";
                
                try
                {
                    con.Notes = new string(oldObject.Comment.Take(511).ToArray());
                }
                catch
                {
                    // do nothing
                }

                dbContext.Contacts.Add(con);
            }

            // TODO finish mapping here 
            if (isNew)
            {
                owner.CreateUserid = createdBy.SmUserId;

                try
                {
                    owner.CreateTimestamp = 
                        DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    owner.CreateTimestamp = DateTime.UtcNow;
                }

                con.CreateUserid = createdBy.SmUserId;
                owner.PrimaryContact = con;
                dbContext.Owners.Add(owner);
            }
            else  // the owner existed in the database
            {
                try
                {
                    owner.LastUpdateUserid = systemId;
                    owner.LastUpdateTimestamp = DateTime.UtcNow;
                    con.LastUpdateTimestamp = DateTime.UtcNow;
                    con.LastUpdateUserid = modifiedBy.SmUserId;
                    owner.PrimaryContact = con;
                }
                catch
                {
                    // do nothing
                }

                dbContext.Owners.Update(owner);
            }
        }       
    }
}


