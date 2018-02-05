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
        const string OldTable = "UserHETS";
        const string NewTable = "HET_USER";
        const string XmlFileName = "User_HETS.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        static void UnknownElement(object sender, XmlElementEventArgs e)
        {
            Console.WriteLine("Unexpected element: {0} as line {1}, column {2}",
                e.Element.Name, e.LineNumber, e.LinePosition);
        }

        static void UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            Console.WriteLine("Unexpected attribute: {0} as line {1}, column {2}",
                e.Attr.Name, e.LineNumber, e.LinePosition);
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
            XmlSerializer ser = new XmlSerializer(typeof(UserHETS[]), new XmlRootAttribute(rootAttr));
            ser.UnknownAttribute += UnknownAttribute;
            ser.UnknownElement += UnknownElement;

            MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
            //XmlReader reader = new XmlTextReader(memoryStream);
            //if (ser.CanDeserialize(reader)  )
            //{
                UserHETS[] legacyItems = (UserHETS[])ser.Deserialize(memoryStream);

                foreach (UserHETS item in legacyItems)
                {
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());

                    ImportMapRecord importMapRecord = new ImportMapRecord();

                    importMapRecord.TableName = NewTable;
                    importMapRecord.MappedColumn = "User_cd";
                    importMapRecord.OriginalValue = item.Popt_Id.ToString();
                    if (importMap != null)
                    {
                        User mappedUser = dbContext.Users.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (mappedUser != null)
                        {
                            importMapRecord.NewValue = mappedUser.SmUserId;
                        }
                    }

                }
            //}

            

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

                foreach (UserHETS item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == item.Popt_Id.ToString());
                    User instance = dbContext.Users.FirstOrDefault(x => item.User_Cd.ToUpper().IndexOf(x.SmUserId.ToUpper(), StringComparison.Ordinal) >= 0);

                    if (instance == null)
                    {
                        CopyToInstance(dbContext, item, ref instance, systemId);

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
        private static void CopyToInstance(DbAppContext dbContext, UserHETS oldObject, ref User user, string systemId)
        {
            string smUserId;
            int serviceAreaId;

            int startPos = oldObject.User_Cd.IndexOf(@"\", StringComparison.Ordinal) + 1;

            try
            {
                serviceAreaId = int.Parse (oldObject.Service_Area_Id);
                smUserId = oldObject.User_Cd.Substring(startPos).Trim();
            }
            catch
            {
                return;
            }

            // add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (createdBy.SmUserId == smUserId  )
            {
                user = createdBy;
                return;
            }

            if (  modifiedBy.SmUserId == smUserId)
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
                    .Include(x => x.GroupMemberships)
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
    }
}

