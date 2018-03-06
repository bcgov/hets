using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;
using ServiceArea = HETSAPI.Models.ServiceArea;
using System.Xml;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import User Records
    /// </summary>
    public static class ImportUser
    {
        const string OldTable = "User_HETS";
        const string NewTable = "HET_USER";
        const string XmlFileName = "User_HETS.xml";

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
            XmlSerializer ser = new XmlSerializer(typeof(UserHETS[]), new XmlRootAttribute(rootAttr));
            ser.UnknownAttribute += ImportUtility.UnknownAttribute;
            ser.UnknownElement += ImportUtility.UnknownElement;

            MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
            XmlReader reader = new XmlTextReader(memoryStream);
            if (ser.CanDeserialize(reader)  )
            {

                
                UserHETS[] legacyItems = (UserHETS[])ser.Deserialize(reader);
                List<string> usernames = new List<string>();
                foreach (UserHETS item in legacyItems)
                {
                    string username = NormalizeUserCode(item.User_Cd);
                    if (!usernames.Contains(username))
                    {
                        usernames.Add(username);
                    }
                }

                usernames.Sort();
                int currentUser = 0; 
                foreach (string username in usernames)
                {
                    ImportMapRecord importMapRecord = new ImportMapRecord
                    {
                        TableName = NewTable,
                        MappedColumn = "User_cd",
                        OriginalValue = username,
                        NewValue = "User" + currentUser
                    };

                    currentUser++;
                    result.Add(importMapRecord);
                }                   
            }
            
            return result;
        }

        // normalize a user code from the legacy database.
        public static string NormalizeUserCode(string userCode)
        {
            string result = "";
            if (userCode != null)
            {
                result = userCode.ToUpper().Trim();
                string idir_token = "IDIR\\";
                int idirPos = result.IndexOf(idir_token);
                if (idirPos > -1)
                {
                    result = result.Substring(idirPos + idir_token.Length);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Import Users
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
                XmlSerializer ser = new XmlSerializer(typeof(UserHETS[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                UserHETS[] legacyItems = (UserHETS[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                // the HETS raw data has multiple rows per user, it needs to be normalized.

                List<string> usernames = new List<string>();
                List<string> allNames = new List<string>();
                performContext.WriteLine("Normalizing User Data - Pass 1 ");
                progress.SetValue(0);
                foreach (UserHETS item in legacyItems.WithProgress(progress))
                {
                    string username = NormalizeUserCode(item.User_Cd); 
                    
                    if (! usernames.Contains (username))
                    {
                        usernames.Add(username);
                    }
                    if (! allNames.Contains (item.Modified_By))
                    {
                        allNames.Add(item.Modified_By);
                    }
                    if (! allNames.Contains(item.Created_By))
                    {
                        allNames.Add(item.Created_By);
                    }
                }
                performContext.WriteLine("Normalizing User Data - Pass 2");
                progress.SetValue(0);
                Dictionary<string, string> firstNames = new Dictionary<string, string>();
                Dictionary<string, string> lastNames = new Dictionary<string, string>();

                foreach (string name in allNames.WithProgress(progress))
                {
                    if (name != null)
                    {
                        string token = ", ";
                        int firstPos = name.IndexOf(", ");
                        string bracketIdirToken = "(IDIR\\";
                        int secondPos = name.IndexOf(bracketIdirToken);

                        if (firstPos > -1 && secondPos > -1)
                        {
                            string lastName = name.Substring(0, firstPos);
                            firstPos = firstPos + 2; // comma and space
                            string firstName = name.Substring(firstPos, secondPos - firstPos).Trim();
                            string username = NormalizeUserCode(name.Substring(secondPos + bracketIdirToken.Length, name.Length - (secondPos + bracketIdirToken.Length + 1)));

                            // see if we have the username.
                            if (usernames.Contains(username))
                            {
                                // update the firstname and lastname data.
                                firstNames[username] = firstName;
                                lastNames[username] = lastName;
                            }
                        }
                    }
                }

                performContext.WriteLine("Normalizing User Data - Pass 3");
                progress.SetValue(0);
                foreach (UserHETS item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    string username = NormalizeUserCode(item.User_Cd);
                    string firstName = "";
                    string lastName = "";

                    if (firstNames.ContainsKey(username))
                    {
                        firstName = firstNames[username];
                    }

                    if (lastNames.ContainsKey(username))
                    {
                        lastName = lastNames[username];
                    }

                    User instance = dbContext.Users.FirstOrDefault(x => x.SmUserId == username);

                    if (instance == null)
                    {
                        CopyToInstance(dbContext, item, ref instance, systemId, username, firstName, lastName);

                        // new entry
                        if (importMap == null && instance != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, item.Popt_Id.ToString(), NewTable, instance.Id);
                        }

                        ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, (++ii).ToString(), BCBidImport.SigId);
                        dbContext.SaveChangesForImport();
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
        /// <param name="user"></param>
        /// <param name="systemId"></param>
        private static void CopyToInstance(DbAppContext dbContext, UserHETS oldObject, ref User user, string systemId, string smUserId, string firstName, string lastName)
        {
            int serviceAreaId;

            int startPos = oldObject.User_Cd.IndexOf(@"\", StringComparison.Ordinal) + 1;

            try
            {
                serviceAreaId = int.Parse (oldObject.Service_Area_Id);
            }
            catch
            {
                return;
            }



            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
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

            if (createdBy != null && createdBy.SmUserId != null && createdBy.SmUserId == smUserId  )
            {
                user = createdBy;

                return;
            }

            if (  modifiedBy != null && modifiedBy.SmUserId != null && modifiedBy.SmUserId == smUserId)
            {
                user = modifiedBy;
                return;
            }

            UserRole userRole = new UserRole();

            string authority;

            try
            {
                authority = oldObject.Authority.Trim();
            }
            catch
            {
                // regular User
                authority = ""; 
            }


            int roleId = ImportUtility.GetRoleIdFromAuthority(authority);

            User user1 = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);

            ServiceArea serArea = dbContext.ServiceAreas
                .Include(x=>x.District)
                .FirstOrDefault(x => x.MinistryServiceAreaID == serviceAreaId);

            if (user1 == null)
            {
                if (user == null)
                {
                    user = new User();
                }

                try
                {
                    user.SmUserId = smUserId;
                    user.GivenName = firstName;
                    user.Surname = lastName;

                    if (serArea != null)
                    {
                        user.District = serArea.District;
                        user.DistrictId = serArea.DistrictId;
                    }
                }
                catch
                {
                    // do nothing
                }

                user.AppCreateTimestamp = DateTime.UtcNow;
                user.AppCreateUserid = createdBy.SmUserId;                

                //a dd user Role - Role Id is limited to 1, or 2
                if (roleId > 2)
                {
                    roleId = 1;
                }

                userRole.Role = dbContext.Roles.First(x => x.Id == roleId);
                userRole.AppCreateTimestamp = DateTime.UtcNow;
                userRole.ExpiryDate = DateTime.UtcNow.AddMonths(12);
                userRole.AppCreateUserid = createdBy.SmUserId;
                userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);

                user.UserRoles = new List<UserRole> {userRole};
                dbContext.Users.Add(user);
            }
            else
            {
                user = dbContext.Users
                    .Include(x => x.UserRoles)
                    .First(x => x.SmUserId == smUserId);

                // if the user does not have the user role, add the user role
                if (user.UserRoles == null)
                {
                    user.UserRoles = new List<UserRole>();
                }

                // if the role does not exist for the user, add the user role for the user
                if (user.UserRoles.FirstOrDefault(x => x.RoleId == roleId) == null)
                {
                    userRole.Role = dbContext.Roles.First(x => x.Id == roleId);
                    userRole.AppCreateTimestamp = DateTime.UtcNow;
                    userRole.ExpiryDate = DateTime.UtcNow.AddMonths(12);
                    userRole.AppCreateUserid = createdBy.SmUserId;
                    userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);
                    user.UserRoles.Add(userRole);
                }

                user.AppLastUpdateUserid = createdBy.SmUserId;
                user.AppCreateTimestamp = DateTime.UtcNow;
                user.Active = true;
                dbContext.Users.Update(user);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.UserHETS[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.UserHETS[] legacyItems = (ImportModels.UserHETS[])ser.Deserialize(memoryStream);

                List<string> usernames = new List<string>();                
                performContext.WriteLine("Normalizing User Data - Pass 1 ");
                progress.SetValue(0);
                foreach (UserHETS item in legacyItems.WithProgress(progress))
                {
                    string username = NormalizeUserCode(item.User_Cd);

                    if (!usernames.Contains(username))
                    {
                        usernames.Add(username);

                    }
                }
                // now create the mapping of old to new usernames.
                Dictionary<string, int> userMap = new Dictionary<string, int>();

                usernames.Sort();

                int currentUser = 0;

                List<ImportMapRecord> importMapRecords = new List<ImportMapRecord>();

                foreach (string username in usernames)
                {
                    userMap.Add(username, currentUser);
                    ImportMapRecord importMapRecord = new ImportMapRecord();
                    importMapRecord.TableName = NewTable;
                    importMapRecord.MappedColumn = "User_cd";
                    importMapRecord.OriginalValue = username;
                    importMapRecord.NewValue = "TESTER" + currentUser;
                    importMapRecords.Add(importMapRecord);
                    currentUser++;
                }


                performContext.WriteLine("Normalizing User Data - Pass 2");
                progress.SetValue(0);
                foreach (UserHETS item in legacyItems.WithProgress(progress))
                {
                    if (item.Modified_By != null)
                    {
                        item.Modified_By = systemId;
                    }

                    string oldCode = NormalizeUserCode(item.User_Cd);
                    item.User_Cd = "TESTER" + userMap[oldCode].ToString();
                    // special case for the user table - as the old data does not have first name / last name.
                    item.Created_By = userMap[oldCode].ToString() + ", Tester (IDIR\\TESTER" + userMap[oldCode].ToString() + ")";
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

