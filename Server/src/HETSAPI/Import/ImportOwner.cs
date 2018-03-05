using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
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
        public static string OldTable => "Owner";
        public static string NewTable => "HET_OWNER";
        public static string XmlFileName => "Owner.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";


        /// <summary>
        /// Get the list of mapped records.  
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public static List<ImportMapRecord> GetImportMap(DbAppContext dbContext, string fileLocation)
        {
            List<ImportMapRecord> result = new List<ImportMapRecord>();
            string rootAttr = "ArrayOf" + OldTable;
            XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Owner[]), new XmlRootAttribute(rootAttr));
            ser.UnknownAttribute += ImportUtility.UnknownAttribute;
            ser.UnknownElement += ImportUtility.UnknownElement;


            MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
            XmlReader reader = new XmlTextReader(memoryStream);
            if (ser.CanDeserialize(reader))
            {


                ImportModels.Owner[] legacyItems = (ImportModels.Owner[])ser.Deserialize(reader);
                List<string> keys = new List<string>();
                Dictionary<string, string> givenNames = new Dictionary<string, string>();
                Dictionary<string, string> surnames = new Dictionary<string, string>();
                Dictionary<string, string> orgNames = new Dictionary<string, string>();
                Dictionary<string, int> oldkeys = new Dictionary<string, int>();
                foreach (ImportModels.Owner item in legacyItems)
                {
                    string givenName = item.Owner_First_Name;
                    string surname = item.Owner_Last_Name;
                    int oldKey = item.Popt_Id;
                    string organizationName = "" + item.CGL_Company;
                    string key = organizationName + " " + givenName + " " + surname;
                    if (!keys.Contains(key))
                    {
                        keys.Add(key);
                        givenNames.Add(key, givenName);
                        surnames.Add(key, surname);
                        orgNames.Add(key, organizationName);
                        oldkeys.Add(key, oldKey);
                    }
                }

                keys.Sort();
                int currentOwner = 0;
                foreach (string key in keys)
                {

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord();

                    importMapRecordOrganization.TableName = NewTable;
                    importMapRecordOrganization.MappedColumn = "OrganizationName";
                    importMapRecordOrganization.OriginalValue = orgNames[key];
                    importMapRecordOrganization.NewValue = "OwnerFirst" + currentOwner;
                    result.Add(importMapRecordOrganization);

                    ImportMapRecord importMapRecordFirstName = new ImportMapRecord();

                    importMapRecordFirstName.TableName = NewTable;
                    importMapRecordFirstName.MappedColumn = "Owner_First_Name";
                    importMapRecordFirstName.OriginalValue = givenNames[key];
                    importMapRecordFirstName.NewValue = "OwnerFirst" + currentOwner;
                    result.Add(importMapRecordFirstName);

                    ImportMapRecord importMapRecordLastName = new ImportMapRecord();

                    importMapRecordLastName.TableName = NewTable;
                    importMapRecordLastName.MappedColumn = "Owner_Last_Name";
                    importMapRecordLastName.OriginalValue = givenNames[key];
                    importMapRecordLastName.NewValue = "OwnerLast" + currentOwner;
                    result.Add(importMapRecordLastName);

                    // now update the owner record.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldkeys[key].ToString());

                    if (importMap != null)
                    {
                        Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner != null)
                        {
                            owner.GivenName = "OwnerFirst" + currentOwner;
                            owner.Surname = "OwnerLast" + currentOwner;                           

                            owner.RegisteredCompanyNumber = ImportUtility.ScrambleString(owner.RegisteredCompanyNumber);
                            owner.WorkSafeBCPolicyNumber = ImportUtility.ScrambleString(owner.WorkSafeBCPolicyNumber);
                            owner.OrganizationName = "Company " + currentOwner;

                            dbContext.Owners.Update(owner);
                            dbContext.SaveChangesForImport();
                        }
                    }

                    currentOwner++;

                }
            }

            return result;
        }

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
            int maxOwnerIndex = 0;

            if (dbContext.Owners.Count() > 0)
            {
                maxOwnerIndex = dbContext.Owners.Max(x => x.Id);
            }
            int maxContactIndex = 0;

            if (dbContext.Contacts.Count() > 0)
            {
                maxContactIndex = dbContext.Contacts.Max(x => x.Id);
            }


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
                            importMap.AppLastUpdateTimestamp = DateTime.UtcNow;
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
                if (oldObject.CGL_End_Dt != null)
                {
                    owner.CGLEndDate =
                    DateTime.ParseExact(oldObject.CGL_End_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                
            }
            catch
            {
                // do nothing
            }

            try
            {
                if (oldObject.WCB_Expiry_Dt != null)
                {
                    owner.WorkSafeBCExpiryDate =
                    DateTime.ParseExact(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                
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
                if (oldObject.CGL_Company != null)
                {
                    owner.OrganizationName = oldObject.CGL_Company.Trim();
                }
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
                    con.Role = "Owner";
                    owner.Surname = oldObject.Owner_Last_Name.Trim();
                    owner.GivenName = oldObject.Owner_First_Name.Trim();
                    owner.OwnerCode = con.GivenName.Substring(0, 1) + con.Surname.Substring(0, 1);
                }
                catch
                {
                    // do nothing
                }

                con.FaxPhoneNumber = "";
                con.Province = "BC";
                
                try
                {
                    if (con.Notes != null)
                    {
                        con.Notes = new string(oldObject.Comment.Take(511).ToArray());
                    }                    
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
                owner.AppCreateUserid = createdBy.SmUserId;

                try
                {
                    owner.AppCreateTimestamp = 
                        DateTime.ParseExact(oldObject.Created_Dt.Trim().Substring(0, 10), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    owner.AppCreateTimestamp = DateTime.UtcNow;
                }
                if (createdBy != null)
                {
                    con.AppCreateUserid = createdBy.SmUserId;
                }
                
                owner.PrimaryContact = con;

                // adjust status and archive date fields.
                if (owner.ArchiveCode != null && owner.ArchiveCode.Trim().ToUpper().Equals("Y"))
                {
                    owner.Status = "Archived";
                    owner.ArchiveDate = DateTime.UtcNow;
                }
                else
                {
                    owner.ArchiveDate = null;
                    if (owner.Status != null && owner.ArchiveCode != null && owner.ArchiveCode.Trim().ToUpper().Equals("N"))
                    {
                        if (owner.Status.Trim().ToUpper().Equals("U"))
                        {                            
                            owner.Status = "Unapproved";
                        }
                        else if (owner.Status.Trim().ToUpper().Equals("A"))
                        {                         
                            owner.Status = "Approved";
                        }
                    }
                }

                dbContext.Owners.Add(owner);
            }
            else  // the owner existed in the database
            {
                try
                {
                    owner.AppLastUpdateUserid = systemId;
                    owner.AppLastUpdateTimestamp = DateTime.UtcNow;
                    con.AppLastUpdateTimestamp = DateTime.UtcNow;
                    if (modifiedBy != null)
                    {
                        con.AppLastUpdateUserid = modifiedBy.SmUserId;
                    }
                    owner.PrimaryContact = con;
                }
                catch
                {
                    // do nothing
                }

                dbContext.Owners.Update(owner);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Owner[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Owner[] legacyItems = (ImportModels.Owner[])ser.Deserialize(memoryStream);

                List<string> usernames = new List<string>();
                performContext.WriteLine("Obfuscating owner data");
                progress.SetValue(0);
                int currentOwner = 0;

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (ImportModels.Owner item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                    if (item.Modified_By != null)
                    {
                        item.Modified_By = systemId;
                    }

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord();

                    importMapRecordOrganization.TableName = NewTable;
                    importMapRecordOrganization.MappedColumn = "OrganizationName";
                    importMapRecordOrganization.OriginalValue = item.CGL_Company;
                    importMapRecordOrganization.NewValue = "Company " + currentOwner;
                    importMapRecords.Add(importMapRecordOrganization);

                    ImportMapRecord importMapRecordFirstName = new ImportMapRecord();

                    importMapRecordFirstName.TableName = NewTable;
                    importMapRecordFirstName.MappedColumn = "Owner_First_Name";
                    importMapRecordFirstName.OriginalValue = item.Owner_First_Name;
                    importMapRecordFirstName.NewValue = "OwnerFirst" + currentOwner;
                    importMapRecords.Add(importMapRecordFirstName);

                    ImportMapRecord importMapRecordLastName = new ImportMapRecord();

                    importMapRecordLastName.TableName = NewTable;
                    importMapRecordLastName.MappedColumn = "Owner_Last_Name";
                    importMapRecordLastName.OriginalValue = item.Owner_Last_Name;
                    importMapRecordLastName.NewValue = "OwnerLast" + currentOwner;
                    importMapRecords.Add(importMapRecordLastName);

                    ImportMapRecord importMapRecordOwnerCode = new ImportMapRecord();

                    importMapRecordOwnerCode.TableName = NewTable;
                    importMapRecordOwnerCode.MappedColumn = "Owner_Cd";
                    importMapRecordOwnerCode.OriginalValue = item.Owner_Cd;
                    importMapRecordOwnerCode.NewValue = "OO" + currentOwner;
                    importMapRecords.Add(importMapRecordOwnerCode);

                    item.Owner_Cd = "OO" + currentOwner;
                    item.Owner_First_Name = "OwnerFirst" + currentOwner;
                    item.Owner_Last_Name = "OwnerLast" + currentOwner;
                    item.Contact_Person = ImportUtility.ScrambleString(item.Contact_Person);
                    item.Comment = ImportUtility.ScrambleString(item.Comment);
                    item.WCB_Num = ImportUtility.ScrambleString(item.WCB_Num);
                    item.CGL_Company = ImportUtility.ScrambleString(item.CGL_Company);
                    item.CGL_Policy = ImportUtility.ScrambleString(item.CGL_Policy);

                    currentOwner++;
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


