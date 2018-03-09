using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.    // This means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            List<Owner> data = new List<Owner>();
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

                foreach (ImportModels.Owner item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    // new entry
                    if (importMap == null)
                    {
                        Owner owner = null;
                        CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex);
                        data.Add(owner);
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Popt_Id.ToString(), NewTable, owner.Id);
                    }
                    else // update
                    {
                        Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);

                        if (owner == null) // record was deleted
                        {
                            CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex);

                            // update the import map
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref owner, systemId, ref maxOwnerIndex);
                            
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
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BcBidImport.SigId);
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
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BcBidImport.SigId.ToString(), BcBidImport.SigId);
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
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.Owner oldObject, ref Owner owner,
          string systemId, ref int maxOwnerIndex)
        {
            try
            {
                bool isNew = false;

                if (owner == null)
                {
                    isNew = true;
                    owner = new Owner {Id = ++maxOwnerIndex};
                }

                // ***********************************************
                // set owner code
                // ***********************************************
                owner.OwnerCode = oldObject.Owner_Cd.Trim().ToUpper();

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
                var localArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);

                if (localArea != null)
                {
                    owner.LocalAreaId = localArea.Id;
                }

                // ***********************************************
                // set other attributes
                // ***********************************************
                if (oldObject.CGL_End_Dt != null && oldObject.CGL_End_Dt != "1900-01-01T00:00:00")
                {
                    owner.CGLEndDate =
                        DateTime.ParseExact(oldObject.CGL_End_Dt.Trim().Substring(0, 10), "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                }

                if (oldObject.WCB_Expiry_Dt != null && oldObject.WCB_Expiry_Dt != "1900-01-01T00:00:00")
                {
                    owner.WorkSafeBCExpiryDate =
                        DateTime.ParseExact(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10), "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                }

                if (owner.WorkSafeBCPolicyNumber != null)
                {
                    owner.WorkSafeBCPolicyNumber = oldObject.WCB_Num.Trim();
                }

                if (oldObject.CGL_Policy != null)
                {
                    owner.CglPolicyNumber = oldObject.CGL_Policy.Trim();
                }

                // ***********************************************
                // manage archive and owner status
                // ***********************************************
                string tempArchive = oldObject.Archive_Cd;
                string tempStatus = oldObject.Status_Cd.Trim();

                if (tempArchive == "Y")
                {
                    // arvhived!
                    owner.ArchiveCode = "Y";
                    owner.ArchiveDate = DateTime.UtcNow;
                    owner.ArchiveReason = "Imported from BC Bid";

                    if (oldObject.Archive_Reason != null && oldObject.Archive_Reason.Trim().Length >= 1)
                    {
                        owner.ArchiveReason = oldObject.Archive_Reason.Trim();
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
                // manage contacts & owner information
                // ***********************************************
                string tempOwnerFirstName = "";
                string tempOwnerLastName = "";

                if (oldObject.Owner_First_Name != null && !oldObject.Owner_First_Name.Trim().Equals("#x20;"))
                {
                    tempOwnerFirstName = ImportUtility.GetCapitalCase(oldObject.Owner_First_Name.Trim());
                }

                if (oldObject.Owner_Last_Name != null && !oldObject.Owner_Last_Name.Trim().Equals("#x20;"))
                {
                    tempOwnerLastName = ImportUtility.GetCapitalCase(oldObject.Owner_Last_Name.Trim());
                }

                owner.Surname = tempOwnerLastName;
                owner.GivenName = tempOwnerFirstName;

                // no company name (yet) will create one for now
                owner.OrganizationName =
                    string.Format("{0} - {1} {2}", owner.OwnerCode, tempOwnerFirstName, tempOwnerLastName);

                // contact
                string tempContactPerson = "";

                if (oldObject.Contact_Person != null && !oldObject.Contact_Person.Trim().Equals("#x20;"))
                {
                    tempContactPerson = ImportUtility.GetCapitalCase(oldObject.Contact_Person.Trim());
                }

                // add the owner as a contact (default to primary)
                int tempId = owner.Id;

                Contact contact = dbContext.Contacts
                    .FirstOrDefault(x =>
                        String.Equals(x.GivenName, tempOwnerLastName, StringComparison.InvariantCultureIgnoreCase) &&
                        String.Equals(x.Surname, tempOwnerLastName, StringComparison.InvariantCultureIgnoreCase));

                // only add if they don't already exist
                if (contact == null)
                {
                    contact = new Contact
                    {
                        Surname = tempOwnerLastName,
                        GivenName = tempOwnerFirstName,
                        Role = "Owner",
                        FaxPhoneNumber = "",
                        Province = "BC",
                        Notes = "",
                        AppCreateUserid = systemId,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    if (owner.Contacts == null)
                    {
                        owner.Contacts = new List<Contact>();
                    }

                    dbContext.Contacts.Add(contact);
                    owner.PrimaryContactId = contact.Id;
                }

                // is the contact the same as the owner?
                if (!string.IsNullOrEmpty(tempContactPerson))
                {
                    // split the name
                    string tempContactFirstName = ImportUtility.GetNamePart(tempContactPerson, 1);
                    string tempContactLastName = ImportUtility.GetNamePart(tempContactPerson, 2);

                    // check if the name is unique
                    if ((!String.Equals(tempContactFirstName, tempOwnerFirstName,
                             StringComparison.InvariantCultureIgnoreCase) &&
                         !String.Equals(tempContactLastName, tempOwnerLastName,
                             StringComparison.InvariantCultureIgnoreCase)) ||
                        (!String.Equals(tempContactFirstName, tempOwnerFirstName,
                             StringComparison.InvariantCultureIgnoreCase) &&
                         !String.Equals(tempContactFirstName, tempOwnerLastName,
                             StringComparison.InvariantCultureIgnoreCase) &&
                         !string.IsNullOrEmpty(tempContactLastName)))
                    {
                        Contact contact2 = dbContext.Contacts
                            .FirstOrDefault(x =>
                                String.Equals(x.GivenName, tempContactFirstName,
                                    StringComparison.InvariantCultureIgnoreCase) &&
                                String.Equals(x.Surname, tempContactLastName,
                                    StringComparison.InvariantCultureIgnoreCase));

                        if (contact2 == null)
                        {
                            string tempComment = "";

                            if (oldObject.Comment != null && !oldObject.Comment.Trim().Equals("#x20;"))
                            {
                                tempComment = oldObject.Comment.Trim().Replace("#x0D;", " ");
                            }

                            contact2 = new Contact
                            {
                                Surname = tempOwnerLastName,
                                GivenName = tempOwnerFirstName,
                                Role = "Owner",
                                FaxPhoneNumber = "",
                                Province = "BC",
                                Notes = tempComment,
                                AppCreateUserid = systemId,
                                AppCreateTimestamp = DateTime.UtcNow,
                                AppLastUpdateUserid = systemId,
                                AppLastUpdateTimestamp = DateTime.UtcNow
                            };

                            if (owner.Contacts == null)
                            {
                                owner.Contacts = new List<Contact>();
                            }

                            dbContext.Contacts.Add(contact2);
                            owner.PrimaryContact = contact2; // since this was the default in the file - make it primary
                        }
                    }
                }

                // ***********************************************
                // create or update owner
                // ***********************************************            
                if (isNew)
                {
                    owner.AppCreateUserid = systemId;
                    owner.AppCreateTimestamp = DateTime.UtcNow;
                    owner.AppLastUpdateUserid = systemId;
                    owner.AppLastUpdateTimestamp = DateTime.UtcNow;

                    dbContext.Owners.Add(owner);
                }
                else // the owner existed in the database
                {
                    owner.AppLastUpdateUserid = systemId;
                    owner.AppLastUpdateTimestamp = DateTime.UtcNow;

                    dbContext.Owners.Update(owner);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("***Error*** - Owner Code: " + owner.OwnerCode);
                Debug.Print(ex.Message);
                throw;
            }
        }

        public static void Obfuscate(PerformContext performContext, DbAppContext dbContext, string sourceLocation, string destinationLocation, string systemId)
        {
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, "Obfuscate_" + OldTableProgress, BcBidImport.SigId);

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


