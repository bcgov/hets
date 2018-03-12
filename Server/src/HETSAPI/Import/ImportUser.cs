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
            XmlSerializer ser = new XmlSerializer(typeof(UserHets[]), new XmlRootAttribute(rootAttr));
            ser.UnknownAttribute += ImportUtility.UnknownAttribute;
            ser.UnknownElement += ImportUtility.UnknownElement;

            MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
            XmlReader reader = new XmlTextReader(memoryStream);
            if (ser.CanDeserialize(reader)  )
            {                
                UserHets[] legacyItems = (UserHets[])ser.Deserialize(reader);
                List<string> usernames = new List<string>();
                foreach (UserHets item in legacyItems)
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
                int idirPos = result.IndexOf(idir_token, StringComparison.Ordinal);

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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            // manage the id value for new user records
            int maxUserIndex = 0;

            if (dbContext.Users.Any())
            {
                maxUserIndex = dbContext.Users.Max(x => x.Id);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(UserHets[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                UserHets[] legacyItems = (UserHets[])ser.Deserialize(memoryStream);

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                // create an array of names using the created by and modified by values in the data
                performContext.WriteLine("Extracting first and last names");
                progress.SetValue(0);

                Dictionary<string, string> firstNames = new Dictionary<string, string>();
                Dictionary<string, string> lastNames = new Dictionary<string, string>();

                foreach (UserHets item in legacyItems.WithProgress(progress))
                {
                    string name = item.Created_By;
                    GetNameParts(name, ref firstNames, ref lastNames);

                    name = item.Modified_By;
                    GetNameParts(name, ref firstNames, ref lastNames);
                }

                // import the data
                performContext.WriteLine("Importing User Data");
                progress.SetValue(0);

                foreach (UserHets item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    string username = NormalizeUserCode(item.User_Cd);
                    string firstName = GetNamePart(username, firstNames);
                    string lastName = GetNamePart(username, lastNames);

                    username = username.ToLower();

                    User instance = dbContext.Users.FirstOrDefault(x => x.SmUserId == username);

                    // if the user exists - move to the next record
                    if (instance != null) continue;

                    CopyToInstance(dbContext, item, ref instance, systemId, username, firstName, lastName, ref maxUserIndex);

                    // new entry
                    if (importMap == null && instance != null)
                    {
                        ImportUtility.AddImportMap(dbContext, OldTable, item.Popt_Id, NewTable, instance.Id);
                    }

                    if (++ii % 500 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, item.Popt_Id, maxUserIndex);
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            performContext.WriteLine("Error saving data " + e.Message);
                            throw;
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
                    throw;
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }            
        }

        private static string GetNamePart(string username, Dictionary<string, string> names)
        {
            if (username != null &&
                names.ContainsKey(username))
            {
                string temp = names[username];
                return temp;
            }

            return "";
        }        

        private static void GetNameParts(string name, ref Dictionary<string, string> firstNames, ref Dictionary<string, string> lastNames)
        {
            if (name != null)
            {
                int firstPos = name.IndexOf(", ", StringComparison.Ordinal);
                string bracketIdirToken = "(IDIR\\";
                int secondPos = name.IndexOf(bracketIdirToken, StringComparison.Ordinal);

                if (firstPos > -1 && secondPos > -1)
                {
                    // extract first and last name from string
                    string lastName = name.Substring(0, firstPos);
                    firstPos = firstPos + 2; // comma and space
                    string firstName = name.Substring(firstPos, secondPos - firstPos).Trim();

                    // extract user id
                    string username = NormalizeUserCode(name.Substring(secondPos + bracketIdirToken.Length, name.Length - (secondPos + bracketIdirToken.Length + 1)));

                    // see if we have this username
                    if (!firstNames.ContainsKey(username))
                    {
                        // update the firstname and lastname data.
                        firstNames[username] = firstName;
                        lastNames[username] = lastName;
                    }
                }
            }
        }

        /// <summary>
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="user"></param>
        /// <param name="systemId"></param>
        /// <param name="smUserId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="maxUserIndex"></param>
        private static void CopyToInstance(DbAppContext dbContext, UserHets oldObject, ref User user, 
            string systemId, string smUserId, string firstName, string lastName, ref int maxUserIndex)
        {                        
            // File contains multiple records per user (1 for each dsitrict they can access)
            // We are currently importing 1 only -> where Default_Service_Area = Y
            if (oldObject.Default_Service_Area != "Y")
            {
                return;
            }

            if (user != null)
            {
                return; // only add new users
            }

            user = new User
            {
                Id = ++maxUserIndex,
                Active = true,
                SmUserId = smUserId,
                SmAuthorizationDirectory = "IDIR"
            };

            if (!string.IsNullOrEmpty(firstName))
            {
                user.GivenName = firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                user.Surname = lastName;
            }

            // *******************************************************************
            // create initials
            // *******************************************************************
            string temp = "";
            if (!string.IsNullOrEmpty(lastName) && lastName.Length > 0)
            {
                temp = lastName.Substring(0, 1).ToUpper();
            }

            if (!string.IsNullOrEmpty(firstName) && firstName.Length > 0)
            {
                temp = temp + firstName.Substring(0, 1).ToUpper();
            }

            if (!string.IsNullOrEmpty(temp))
            {
                user.Initials = temp;
            }
            
            // *******************************************************************
            // map user to the correct service area
            // *******************************************************************
            int serviceAreaId;
            
            try
            {
                serviceAreaId = int.Parse(oldObject.Service_Area_Id);
            }
            catch
            {
                // not mapped correctly
                throw new ArgumentException(string.Format("User Error - Invalid Service Area (userId: {0}", user.SmUserId));
            }

            ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == serviceAreaId);

            if (serviceArea == null)
            {
                // not mapped correctly
                throw new ArgumentException(string.Format("User Error - Invalid Service Area (userId: {0}", user.SmUserId));
            }

            user.DistrictId = serviceArea.DistrictId;

            // *******************************************************************
            // set the user's role
            // ** all new users will be added with basic access only
            // *******************************************************************
            UserRole userRole = new UserRole();

            Role role = dbContext.Roles.FirstOrDefault(x => x.Name == "HETS Clerk");

            int roleId = -1;

            if (role != null)
            {
                roleId = role.Id;
            }

            // ***********************************************
            // create user
            // ***********************************************                        
            user.AppCreateTimestamp = DateTime.UtcNow;
            user.AppCreateUserid = systemId;
            user.AppLastUpdateUserid = systemId;
            user.AppLastUpdateTimestamp = DateTime.UtcNow;

            userRole.Role = dbContext.Roles.First(x => x.Id == roleId);
            userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);

            userRole.AppCreateTimestamp = DateTime.UtcNow;
            userRole.AppCreateUserid = systemId;
            userRole.AppLastUpdateUserid = systemId;
            userRole.AppLastUpdateTimestamp = DateTime.UtcNow;

            user.UserRoles = new List<UserRole> {userRole};
            dbContext.Users.Add(user);
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
                XmlSerializer ser = new XmlSerializer(typeof(UserHets[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                UserHets[] legacyItems = (UserHets[])ser.Deserialize(memoryStream);

                foreach (UserHets item in legacyItems.WithProgress(progress))
                {
                    item.Created_By = systemId;
                }

                // write out the array
                FileStream fs = ImportUtility.GetObfuscationDestination(XmlFileName, destinationLocation);
                ser.Serialize(fs, legacyItems);
                fs.Close();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}

