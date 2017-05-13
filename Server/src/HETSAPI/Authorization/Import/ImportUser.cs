using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace HETSAPI.Import

{
    public class ImportUser
    {
        const string oldTable = "User_HETS";
        const string newTable = "HET_USER";
        const string xmlFileName = "User_HETS.xml";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(User_HETS[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.User_HETS[] legacyItems = (HETSAPI.Import.User_HETS[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id.ToString());

                    if (importMap == null) // new entry
                    {
                        Models.User instance = null;
                        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.Popt_Id.ToString(), newTable, instance.Id);
                    }
                    //else // update
                    //{
                    //    Models.User instance = dbContext.Users.FirstOrDefault(x => x.Id == importMap.NewKey);
                    //    if (instance == null) // record was deleted
                    //    {
                    //        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                    //        // update the import map.
                    //        importMap.NewKey = instance.Id;
                    //        dbContext.ImportMaps.Update(importMap);
                    //      //DI  dbContext.SaveChanges();
                    //    }
                    //    else // ordinary update.
                    //    {
                    //        CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                    //        // touch the import map.
                    //        importMap.LastUpdateTimestamp = DateTime.UtcNow;
                    //        dbContext.ImportMaps.Update(importMap);
                    //        //DI   dbContext.SaveChanges();
                    //    }
                    //}

                    if (ii++ % 500 == 0)
                    {
                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch
                        {

                        }
                    }
                }
                try
                {
                    dbContext.SaveChanges();
                }
                catch
                {

                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy user instance
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="user"></param>
        /// <param name="systemId"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.User_HETS oldObject, ref Models.User user, string systemId)
        {
            bool isNew = false;

            string smUserId;
            int service_Area_Id;

            int startPos = oldObject.User_Cd.IndexOf(@"\") + 1;
            try
            {
                service_Area_Id = oldObject.Service_Area_Id;
                smUserId = oldObject.User_Cd.Substring(startPos).Trim();
            }
            catch
            {
                return;
            }

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);
            Models.UserRole userRole = new UserRole();

            string authority;
            try
            {
                authority = oldObject.Authority.Trim();
            }
            catch
            {
                authority = ""; // Regular User
            }


            int roleId = ImportUtility.GetRoleIdFromAuthority(authority);

            Models.User user1 = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);

            if (user == null && user1 != null)
                user = user1;

            ServiceArea serArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == service_Area_Id);
            if (user == null)
            {
                isNew = true;
                user = new Models.User(smUserId, serArea.District);
                user.CreateTimestamp = DateTime.UtcNow;
                user.CreateUserid = createdBy.SmUserId;

                // The followings are the data mapping
                // user.Email
                // user.GivenName
                // user.Surname

                //Add user Role
                userRole.Role = dbContext.Roles.First(x => x.Id == roleId);
                userRole.CreateTimestamp = DateTime.UtcNow;
                userRole.ExpiryDate = DateTime.UtcNow.AddMonths(12);
                userRole.CreateUserid = createdBy.SmUserId;
                userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);

                user.UserRoles = new List<UserRole>();
                user.UserRoles.Add(userRole);
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
                // If the role does not exist for the user, add the user role for the user
                if (user.UserRoles.FirstOrDefault(x => x.RoleId == roleId) == null)
                {
                    userRole.Role = dbContext.Roles.First(x => x.Id == roleId);
                    userRole.CreateTimestamp = DateTime.UtcNow;
                    userRole.ExpiryDate = DateTime.UtcNow.AddMonths(12);
                    userRole.CreateUserid = createdBy.SmUserId;
                    userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);
                    user.UserRoles.Add(userRole);
                }
                user.LastUpdateUserid = createdBy.SmUserId;
                user.CreateTimestamp = DateTime.UtcNow;
                user.Active = true;
                dbContext.Users.Update(user);
            }
            try
            {
                //DI dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update user ***");
                performContext.WriteLine(e.ToString());
            }
        }

    }
}

