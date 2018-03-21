using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Owner Records
    /// </summary>
    public static class ImportOwner
    {
        public const string OldTable = "Owner";
        public const string NewTable = "HET_OWNER";
        public const string XmlFileName = "Owner.xml";

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
                // **************************************
                // Owners
                // **************************************
                performContext.WriteLine("*** Resetting HET_OWNER database sequence after import ***");
                Debug.WriteLine("Resetting HET_OWNER database sequence after import");

                if (dbContext.Owners.Any())
                {
                    // get max key
                    int maxKey = dbContext.Owners.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_OWNER_OWNER_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_OWNER database sequence after import ***");
                Debug.WriteLine("Resetting HET_OWNER database sequence after import - Done!");

                // **************************************
                // Contacts
                // **************************************
                performContext.WriteLine("*** Resetting HET_CONTACT database sequence after import ***");
                Debug.WriteLine("Resetting HET_CONTACT database sequence after import");

                if (dbContext.Contacts.Any())
                {
                    // get max key
                    int maxKey = dbContext.Contacts.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_CONTACT_CONTACT_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_CONTACT database sequence after import ***");
                Debug.WriteLine("Resetting HET_CONTACT database sequence after import - Done!");

                // **************************************
                // Notes
                // **************************************
                performContext.WriteLine("*** Resetting HET_NOTE database sequence after import ***");
                Debug.WriteLine("Resetting HET_NOTE database sequence after import");

                if (dbContext.Notes.Any())
                {
                    // get max key
                    int maxKey = dbContext.Notes.Max(x => x.Id);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"ALTER SEQUENCE public.""HET_NOTE_NOTE_ID_seq"" RESTART WITH {0};", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_NOTE database sequence after import ***");
                Debug.WriteLine("Resetting HET_NOTE database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Fix the primary contact foreign keys
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        public static void FixPrimaryContacts(PerformContext performContext, DbAppContext dbContext)
        {
            try
            {
                int ii = 0;

                performContext.WriteLine("*** Resetting HET_OWNER contact foreign keys ***");
                Debug.WriteLine("Resetting HET_OWNER contact foreign keys");

                IQueryable<Owner> owners = dbContext.Owners
                    .Include(x => x.PrimaryContact)
                    .Where(x => x.PrimaryContactId != null);

                foreach (Owner owner in owners)
                {
                    int? contactId = owner.PrimaryContactId;

                    if (contactId != null)
                    {
                        // get contact and update
                        Contact contact = dbContext.Contacts.FirstOrDefault(x => x.Id == contactId);

                        if (owner.Contacts == null)
                        {
                            owner.Contacts = new List<Contact>();
                        }

                        owner.Contacts.Add(contact);
                        dbContext.Owners.Update(owner);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0)
                    {
                        dbContext.SaveChangesForImport();                        
                    }
                }

                // save last batch
                dbContext.SaveChangesForImport();
                

                performContext.WriteLine("*** Done resetting HET_OWNER contact foreign keys ***");
                Debug.WriteLine("Resetting HET_OWNER contact foreign keys - Done!");                
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
        }

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
                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "OrganizationName",
                        OriginalValue = orgNames[key],
                        NewValue = "OwnerFirst" + currentOwner
                    };

                    result.Add(importMapRecordOrganization);

                    ImportMapRecord importMapRecordFirstName = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Owner_First_Name",
                        OriginalValue = givenNames[key],
                        NewValue = "OwnerFirst" + currentOwner
                    };

                    result.Add(importMapRecordFirstName);

                    ImportMapRecord importMapRecordLastName = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Owner_Last_Name",
                        OriginalValue = givenNames[key],
                        NewValue = "OwnerLast" + currentOwner
                    };

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
            // check the start point. If startPoint == sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            int maxOwnerIndex = 0;

            if (dbContext.Owners.Any())
            {
                maxOwnerIndex = dbContext.Owners.Max(x => x.Id);
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

                Debug.WriteLine("Importing Owner Data. Total Records: " + legacyItems.Length);

                foreach (ImportModels.Owner item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        Owner owner = null;
                        CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Popt_Id.ToString(), NewTable, owner.Id);
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 500 == 0) 
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId, NewTable);
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            string temp = string.Format("Error saving data (OwnerIndex: {0}): {1}", maxOwnerIndex, e.Message);
                            performContext.WriteLine(temp);
                            throw new DataException(temp);
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
                    string temp = string.Format("Error saving data (OwnerIndex: {0}): {1}", maxOwnerIndex, e.Message);
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
        /// <param name="owner"></param>
        /// <param name="systemId"></param>
        /// <param name="maxOwnerIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Owner oldObject, 
            ref Owner owner, string systemId, ref int maxOwnerIndex)
        {
            try
            {
                if (owner != null)
                {
                    return;
                }

                owner = new Owner {Id = ++maxOwnerIndex};
                
                // ***********************************************
                // set owner code
                // ***********************************************
                string tempOwnerCode = ImportUtility.CleanString(oldObject.Owner_Cd).ToUpper();

                if (!string.IsNullOrEmpty(tempOwnerCode))
                {
                    owner.OwnerCode = tempOwnerCode;
                }

                // ***********************************************
                // maintenance contractor
                // ***********************************************
                if (oldObject.Maintenance_Contractor != null)
                {
                    owner.IsMaintenanceContractor = (oldObject.Maintenance_Contractor.Trim() == "Y");
                }

                // ***********************************************
                // set local area
                // ***********************************************                
                LocalArea localArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);

                if (localArea != null)
                {
                    owner.LocalAreaId = localArea.Id;
                }

                // ***********************************************
                // set other attributes
                // ***********************************************   
                owner.WorkSafeBCExpiryDate = ImportUtility.CleanDateTime(oldObject.CGL_End_Dt);
                owner.CGLEndDate = ImportUtility.CleanDateTime(oldObject.CGL_End_Dt);

                owner.WorkSafeBCPolicyNumber = ImportUtility.CleanString(oldObject.WCB_Num);
                if (owner.WorkSafeBCPolicyNumber == "0")
                {
                    owner.WorkSafeBCPolicyNumber = null;
                }

                owner.CglPolicyNumber = ImportUtility.CleanString(oldObject.CGL_Policy);
                if (owner.CglPolicyNumber == "")
                {
                    owner.CglPolicyNumber = null;
                }
                
                // ***********************************************
                // manage archive and owner status
                // ***********************************************
                string tempArchive = oldObject.Archive_Cd;
                string tempStatus = oldObject.Status_Cd.Trim();

                if (tempArchive == "Y")
                {
                    // archived!
                    owner.ArchiveCode = "Y";
                    owner.ArchiveDate = DateTime.UtcNow;
                    owner.ArchiveReason = "Imported from BC Bid";
                    owner.Status = "Archived";

                    string tempArchiveReason = ImportUtility.CleanString(oldObject.Archive_Reason);

                    if (!string.IsNullOrEmpty(tempArchiveReason))
                    {
                        owner.ArchiveReason = ImportUtility.GetUppercaseFirst(tempArchiveReason);
                    }
                }
                else
                {
                    owner.ArchiveCode = "N";
                    owner.ArchiveDate = null;
                    owner.ArchiveReason = null;
                    owner.Status = tempStatus == "A" ? "Approved" : "Unapproved";
                    owner.StatusComment = string.Format("Imported from BC Bid ({0})", tempStatus);
                }

                // ***********************************************
                // manage owner information
                // ***********************************************
                string tempOwnerFirstName = ImportUtility.CleanString(oldObject.Owner_First_Name);
                tempOwnerFirstName = ImportUtility.GetCapitalCase(tempOwnerFirstName);

                string tempOwnerLastName = ImportUtility.CleanString(oldObject.Owner_Last_Name);
                tempOwnerLastName = ImportUtility.GetCapitalCase(tempOwnerLastName);

                // some fields have duplicates in the name
                if (tempOwnerLastName == tempOwnerFirstName)
                {
                    tempOwnerFirstName = "";
                }

                if (!string.IsNullOrEmpty(tempOwnerLastName))
                {
                    owner.Surname = tempOwnerLastName;
                }

                if (!string.IsNullOrEmpty(tempOwnerFirstName))
                {
                    owner.GivenName = tempOwnerFirstName;
                }                

                // no company name (yet) will create one for now
                if (!string.IsNullOrEmpty(owner.OwnerCode) &&
                    string.IsNullOrEmpty(tempOwnerLastName))
                {
                    owner.OrganizationName = owner.OwnerCode;
                }
                else if (!string.IsNullOrEmpty(owner.OwnerCode))
                {
                    owner.OrganizationName = string.Format("{0} - {1} {2}", owner.OwnerCode, tempOwnerFirstName, tempOwnerLastName);
                }
                else
                {
                    owner.OrganizationName = string.Format("{0} {1}", tempOwnerFirstName, tempOwnerLastName);
                }                     

                // check if the organization name is actually a "Ltd" or other company name
                // (in BC Bid the names are somtimes used to store the org)
                if (owner.OrganizationName.IndexOf(" Ltd", StringComparison.Ordinal) > -1 ||
                    owner.OrganizationName.IndexOf(" Resort", StringComparison.Ordinal) > -1 ||
                    owner.OrganizationName.IndexOf(" Oilfield", StringComparison.Ordinal) > -1 ||
                    owner.OrganizationName.IndexOf(" Nation", StringComparison.Ordinal) > -1 ||
                    owner.OrganizationName.IndexOf(" Ventures", StringComparison.Ordinal) > -1 ||
                    owner.OrganizationName.IndexOf(" Indian Band", StringComparison.Ordinal) > -1)
                {
                    owner.Surname = null;
                    owner.GivenName = null;

                    if (!string.IsNullOrEmpty(owner.OwnerCode))
                    {
                        owner.OrganizationName = string.Format("{0} {1}", tempOwnerFirstName, tempOwnerLastName);
                    }
                }

                // ***********************************************
                // manage contacts
                // ***********************************************
                bool contactExists = false;

                if (owner.Contacts != null)
                {
                    foreach (Contact contactItem in owner.Contacts)
                    {
                        if (!String.Equals(contactItem.GivenName, tempOwnerFirstName, StringComparison.InvariantCultureIgnoreCase) &&
                            !String.Equals(contactItem.Surname, tempOwnerLastName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            contactExists = true;
                        }
                    }
                }

                // only add if they don't already exist
                if (!contactExists && !string.IsNullOrEmpty(tempOwnerLastName))
                {
                    Contact ownerContact = new Contact
                    {                        
                        Role = "Owner",
                        Province = "BC",
                        Notes = "",
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    if (!string.IsNullOrEmpty(tempOwnerLastName))
                    {
                        ownerContact.Surname = tempOwnerLastName;
                    }

                    if (!string.IsNullOrEmpty(tempOwnerFirstName))
                    {
                        ownerContact.GivenName = tempOwnerFirstName;
                    }                    

                    if (owner.Contacts == null)
                    {
                        owner.Contacts = new List<Contact>();
                    }
                    
                    owner.Contacts.Add(ownerContact);
                }

                // is the BC Bid contact the same as the owner?
                string tempContactPerson = ImportUtility.CleanString(oldObject.Contact_Person);
                tempContactPerson = ImportUtility.GetCapitalCase(tempContactPerson);

                if (!string.IsNullOrEmpty(tempContactPerson))
                {
                    // split the name
                    string tempContactFirstName;
                    string tempContactLastName;

                    if (tempContactPerson.IndexOf(" Or ", StringComparison.Ordinal) > -1)
                    {
                        tempContactFirstName = tempContactPerson;
                        tempContactLastName = "";
                    }
                    else
                    {
                        tempContactFirstName = ImportUtility.GetNamePart(tempContactPerson, 1);
                        tempContactLastName = ImportUtility.GetNamePart(tempContactPerson, 2);
                    }

                    // check if the name is unique
                    if ((!String.Equals(tempContactFirstName, tempOwnerFirstName, StringComparison.InvariantCultureIgnoreCase) &&
                         !String.Equals(tempContactLastName, tempOwnerLastName, StringComparison.InvariantCultureIgnoreCase)) ||
                        (!String.Equals(tempContactFirstName, tempOwnerFirstName, StringComparison.InvariantCultureIgnoreCase) &&
                         !String.Equals(tempContactFirstName, tempOwnerLastName, StringComparison.InvariantCultureIgnoreCase) &&
                         !string.IsNullOrEmpty(tempContactLastName)))
                    {
                        // check if the name(s) already exist
                        contactExists = false;

                        if (owner.Contacts != null)
                        {
                            foreach (Contact contactItem in owner.Contacts)
                            {
                                if (String.Equals(contactItem.GivenName, tempContactFirstName, StringComparison.InvariantCultureIgnoreCase) &&
                                    String.Equals(contactItem.Surname, tempContactLastName, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    contactExists = true;
                                }
                            }
                        }

                        if (!contactExists)
                        {                                                        
                            Contact primaryContact = new Contact
                            {
                                Role = "Primary Contact",
                                Province = "BC",                                
                                AppCreateUserid = systemId,
                                AppCreateTimestamp = DateTime.UtcNow,
                                AppLastUpdateUserid = systemId,
                                AppLastUpdateTimestamp = DateTime.UtcNow
                            };

                            if (!string.IsNullOrEmpty(tempContactLastName))
                            {
                                primaryContact.Surname = tempContactLastName;
                            }

                            if (!string.IsNullOrEmpty(tempContactFirstName))
                            {
                                primaryContact.GivenName = tempContactFirstName;
                            }

                            string tempComment = ImportUtility.CleanString(oldObject.Comment);

                            if (!string.IsNullOrEmpty(tempComment))
                            {
                                tempComment = ImportUtility.GetCapitalCase(tempComment);
                                primaryContact.Notes = tempComment;
                            }
                                                        
                            owner.PrimaryContact = primaryContact; // since this was the default in the file - make it primary
                        }
                    }
                }

                // ensure the contact is valid
                if (owner.Contacts != null)
                {
                    for (int i = owner.Contacts.Count - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(owner.Contacts[i].GivenName) &&
                            string.IsNullOrEmpty(owner.Contacts[i].Surname))
                        {
                            owner.Contacts.RemoveAt(i);
                        }
                    }

                    // update primary
                    if (owner.Contacts.Count > 0 &&
                        owner.PrimaryContact == null)
                    {
                        owner.PrimaryContact = owner.Contacts[0];
                        owner.Contacts.Remove(owner.Contacts[0]);
                    }
                }

                // ***********************************************
                // create owner
                // ***********************************************                            
                owner.AppCreateUserid = systemId;
                owner.AppCreateTimestamp = DateTime.UtcNow;
                owner.AppLastUpdateUserid = systemId;
                owner.AppLastUpdateTimestamp = DateTime.UtcNow;

                dbContext.Owners.Add(owner);                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Owner Code: " + owner.OwnerCode);
                Debug.WriteLine("***Error*** - Master Owner Index: " + maxOwnerIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.Owner[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.Owner[] legacyItems = (ImportModels.Owner[])ser.Deserialize(memoryStream);

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

                    ImportMapRecord importMapRecordOrganization = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "OrganizationName",
                        OriginalValue = item.CGL_Company,
                        NewValue = "Company " + currentOwner
                    };

                    importMapRecords.Add(importMapRecordOrganization);

                    ImportMapRecord importMapRecordFirstName = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Owner_First_Name",
                        OriginalValue = item.Owner_First_Name,
                        NewValue = "OwnerFirst" + currentOwner
                    };

                    importMapRecords.Add(importMapRecordFirstName);

                    ImportMapRecord importMapRecordLastName = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Owner_Last_Name",
                        OriginalValue = item.Owner_Last_Name,
                        NewValue = "OwnerLast" + currentOwner
                    };

                    importMapRecords.Add(importMapRecordLastName);

                    ImportMapRecord importMapRecordOwnerCode = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "Owner_Cd",
                        OriginalValue = item.Owner_Cd,
                        NewValue = "OO" + currentOwner
                    };

                    importMapRecords.Add(importMapRecordOwnerCode);

                    item.Owner_Cd = "OO" + currentOwner;
                    item.Owner_First_Name = "OwnerFirst" + currentOwner;
                    item.Owner_Last_Name = "OwnerLast" + currentOwner;
                    item.Contact_Person = ImportUtility.ScrambleString(ImportUtility.CleanString(item.Contact_Person));
                    item.Comment = ImportUtility.ScrambleString(ImportUtility.CleanString(item.Comment));
                    item.WCB_Num = ImportUtility.ScrambleString(ImportUtility.CleanString(item.WCB_Num));
                    item.CGL_Company = ImportUtility.ScrambleString(ImportUtility.CleanString(item.CGL_Company));
                    item.CGL_Policy = ImportUtility.ScrambleString(ImportUtility.CleanString(item.CGL_Policy));

                    currentOwner++;
                }
                
                performContext.WriteLine("Writing " + XmlFileName + " to " + destinationLocation);

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();

                // write out the spreadsheet of import records
                ImportUtility.WriteImportRecordsToExcel(destinationLocation, importMapRecords, OldTable);
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


