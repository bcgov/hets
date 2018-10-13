using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Console.Progress;
using HetsData.Model;

namespace HetsImport.Import
{
    /// <summary>
    /// Import User Records
    /// </summary>
    public static class ImportUser
    {
        public const string OldTable = "User_HETS";
        public const string NewTable = "HET_USER";
        public const string XmlFileName = "User_HETS.xml";

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
                // Users
                // **************************************
                performContext.WriteLine("*** Resetting HET_USER database sequence after import ***");
                Debug.WriteLine("Resetting HET_USER database sequence after import");

                if (dbContext.HetUser.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetUser.Max(x => x.UserId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_USER_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_USER database sequence after import ***");
                Debug.WriteLine("Resetting HET_USER database sequence after import - Done!");

                // **************************************
                // Roles
                // **************************************
                performContext.WriteLine("*** Resetting HET_ROLE database sequence after import ***");
                Debug.WriteLine("Resetting HET_ROLE database sequence after import");

                if (dbContext.HetRole.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetRole.Max(x => x.RoleId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_ROLE_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_ROLE database sequence after import ***");
                Debug.WriteLine("Resetting HET_ROLE database sequence after import - Done!");

                // **************************************
                // Users - Roles
                // **************************************
                performContext.WriteLine("*** Resetting HET_USER_ROLE database sequence after import ***");
                Debug.WriteLine("Resetting HET_USER_ROLE database sequence after import");

                if (dbContext.HetUserRole.Any())
                {
                    // get max key
                    int maxKey = dbContext.HetUserRole.Max(x => x.UserRoleId);
                    maxKey = maxKey + 1;

                    using (DbCommand command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        // check if this code already exists
                        command.CommandText = string.Format(@"SELECT SETVAL('public.""HET_USER_ROLE_ID_seq""', {0});", maxKey);

                        dbContext.Database.OpenConnection();
                        command.ExecuteNonQuery();
                        dbContext.Database.CloseConnection();
                    }
                }

                performContext.WriteLine("*** Done resetting HET_USER_ROLE database sequence after import ***");
                Debug.WriteLine("Resetting HET_USER_ROLE database sequence after import - Done!");
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
                throw;
            }
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
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BcBidImport.SigId, NewTable);

            if (startPoint == BcBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            // manage the id value for new user records
            int maxUserIndex = 0;

            if (dbContext.HetUser.Any())
            {
                maxUserIndex = dbContext.HetUser.Max(x => x.UserId);
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                // create progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.UserHets[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                ImportModels.UserHets[] legacyItems = (ImportModels.UserHets[])ser.Deserialize(memoryStream);

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

                foreach (ImportModels.UserHets item in legacyItems.WithProgress(progress))
                {
                    string name = item.Created_By;
                    GetNameParts(name, ref firstNames, ref lastNames);

                    name = item.Modified_By;
                    GetNameParts(name, ref firstNames, ref lastNames);
                }

                // import the data
                performContext.WriteLine("Importing User Data");
                progress.SetValue(0);

                foreach (ImportModels.UserHets item in legacyItems.WithProgress(progress))
                {
                    string tempId = item.Popt_Id + "-" + item.Service_Area_Id;

                    HetImportMap importMap = dbContext.HetImportMap.AsNoTracking()
                        .FirstOrDefault(x => x.OldTable == OldTable && 
                                             x.OldKey == tempId);

                    if (importMap == null)
                    {
                        string username = NormalizeUserCode(item.User_Cd).ToUpper();
                        string firstName = GetNamePart(username, firstNames);
                        string lastName = GetNamePart(username, lastNames);

                        HetUser user = null;
                        username = username.ToLower();

                        CopyToInstance(dbContext, item, ref user, systemId, username, firstName, lastName, ref maxUserIndex);

                        if (user != null)
                        {
                            ImportUtility.AddImportMap(dbContext, OldTable, tempId, NewTable, user.UserId);
                            dbContext.SaveChangesForImport();
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
                    string temp = string.Format("Error saving data (UserIndex: {0}): {1}", maxUserIndex, e.Message);
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
        private static void CopyToInstance(DbAppContext dbContext, ImportModels.UserHets oldObject, 
            ref HetUser user, string systemId, string smUserId, string firstName, string lastName, 
            ref int maxUserIndex)
        {
            try
            {
                // file contains multiple records per user (1 for each district they can access)
                // we are currently importing 1 only -> where Default_Service_Area = Y
                if (oldObject.Default_Service_Area != "Y")
                {
                    return;
                }                

                // check if this user already exists in the db
                bool userExists = dbContext.HetUser.Any(x => x.SmUserId == smUserId);

                if (userExists)
                {
                    user = dbContext.HetUser.First(x => x.SmUserId == smUserId);

                    // *******************************************************************
                    // check if this is a different district
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

                    HetServiceArea serviceArea = dbContext.HetServiceArea.AsNoTracking()
                        .FirstOrDefault(x => x.MinistryServiceAreaId == serviceAreaId);

                    if (serviceArea == null)
                    {
                        // not mapped correctly
                        throw new ArgumentException(string.Format("User Error - Invalid Service Area (userId: {0}", user.SmUserId));
                    }

                    int tempUserId = user.UserId;
                    int? tempDistrictId = serviceArea.DistrictId;

                    if (tempDistrictId != null)
                    {
                        HetUserDistrict userDistrict = dbContext.HetUserDistrict.AsNoTracking()
                            .FirstOrDefault(x => x.User.UserId == tempUserId &&
                                                 x.District.DistrictId == tempDistrictId);

                        // ***********************************************
                        // create user district record
                        // ***********************************************      
                        if (userDistrict == null)
                        {
                            userDistrict = new HetUserDistrict
                            {
                                UserId = tempUserId,
                                DistrictId = tempDistrictId,
                                AppCreateTimestamp = DateTime.UtcNow,
                                AppCreateUserid = systemId,
                                AppLastUpdateUserid = systemId,
                                AppLastUpdateTimestamp = DateTime.UtcNow
                            };

                            dbContext.HetUserDistrict.Add(userDistrict);
                            dbContext.SaveChangesForImport();
                        }
                    }
                }
                else                                                    
                {                
                    user = new HetUser
                    {
                        UserId = ++maxUserIndex,
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

                    HetServiceArea serviceArea = dbContext.HetServiceArea.AsNoTracking()
                        .FirstOrDefault(x => x.MinistryServiceAreaId == serviceAreaId);

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
                    HetUserRole userRole = new HetUserRole();

                    HetRole role = dbContext.HetRole.FirstOrDefault(x => x.Name == "HETS District User");

                    int roleId = -1;

                    if (role != null)
                    {
                        roleId = role.RoleId;
                    }

                    // ***********************************************
                    // create user
                    // ***********************************************                        
                    user.AppCreateTimestamp = DateTime.UtcNow;
                    user.AppCreateUserid = systemId;
                    user.AppLastUpdateUserid = systemId;
                    user.AppLastUpdateTimestamp = DateTime.UtcNow;

                    userRole.Role = dbContext.HetRole.First(x => x.RoleId == roleId);
                    userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);

                    userRole.AppCreateTimestamp = DateTime.UtcNow;
                    userRole.AppCreateUserid = systemId;
                    userRole.AppLastUpdateUserid = systemId;
                    userRole.AppLastUpdateTimestamp = DateTime.UtcNow;

                    user.HetUserRole = new List<HetUserRole> { userRole };
                    dbContext.HetUser.Add(user);
                    dbContext.SaveChangesForImport();

                    // ***********************************************
                    // create user district record
                    // ***********************************************      
                    HetUserDistrict userDistrict = new HetUserDistrict
                    {
                        UserId = user.UserId,
                        DistrictId = user.DistrictId,
                        IsPrimary = true,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppCreateUserid = systemId,
                        AppLastUpdateUserid = systemId,
                        AppLastUpdateTimestamp = DateTime.UtcNow
                    };

                    dbContext.HetUserDistrict.Add(userDistrict);
                    dbContext.SaveChangesForImport();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("***Error*** - Employee Id: " + oldObject.Popt_Id);
                Debug.WriteLine("***Error*** - Master User Index: " + maxUserIndex);
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
                XmlSerializer ser = new XmlSerializer(typeof(ImportModels.UserHets[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, sourceLocation, rootAttr);
                ImportModels.UserHets[] legacyItems = (ImportModels.UserHets[])ser.Deserialize(memoryStream);

                foreach (ImportModels.UserHets item in legacyItems.WithProgress(progress))
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

