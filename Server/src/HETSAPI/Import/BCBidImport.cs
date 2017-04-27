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

    public class BCBidImport
    {
        static private void CopyToRegion(DbAppContext dbContext, HETSAPI.Import.HETS_Region oldObject, ref Models.Region reg, string systemId)
        {
            bool isNew = false;
            if (reg == null)
            {
                isNew = true;
                reg = new Models.Region();
            }

            if (dbContext.Cities.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0)
            {
                isNew = true;
                reg.Name = oldObject.Name;
                reg.Id = oldObject.Region_Id; // dbContext.Regions.Max(x => x.Id) + 1;   
                reg.MinistryRegionID = oldObject.Ministry_Region_Id;
                reg.RegionNumber = oldObject.Region_Number;
                reg.CreateTimestamp = DateTime.UtcNow;
                reg.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Regions.Add(reg);   //Adding the city to the database table of HET_CITY
            }

            dbContext.SaveChanges();
        }

        static private void CopyToDistrict(DbAppContext dbContext, HETSAPI.Import.HETS_District oldObject, ref Models.District dis, string systemId)
        {
            bool isNew = false;
            if (dis == null)
            {
                isNew = true;
                dis = new Models.District();
            }

            if (dbContext.Cities.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0)
            {
                isNew = true;
                dis.Name = oldObject.Name;
                dis.Id = oldObject.District_Id; //dbContext.Districts.Max(x => x.Id) + 1;   //oldObject.Seq_Num;  
                dis.MinistryDistrictID = oldObject.Ministry_District_Id;
                dis.RegionId = oldObject.Region_ID;
                dis.CreateTimestamp = DateTime.UtcNow;
                dis.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Districts.Add(dis);   //Adding the city to the database table of HET_CITY
            }

            dbContext.SaveChanges();
        }

        /// <summary>
        /// Copy from legacy to new record For the table of HET_City
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="city"></param>
        static private void CopyToCity(DbAppContext dbContext, HETSAPI.Import.HETS_City oldObject, ref Models.City city, string systemId)
        {
            bool isNew = false;
            if (city == null)
            {
                isNew = true;
                city = new Models.City();
            }

            if( dbContext.Cities.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0 )
            {
                isNew = true;
                city.Name = oldObject.Name;
                city.Id = dbContext.Cities.Max(x => x.Id) + 1;   //oldObject.Seq_Num;  
                city.CreateTimestamp = DateTime.UtcNow;
                city.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Cities.Add(city);   //Adding the city to the database table of HET_CITY
            }

            dbContext.SaveChanges();
        }


        /// <summary>
        /// Copy from legacy to new record
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="serviceArea"></param>
        static private void CopyToServiceArea(DbAppContext dbContext, HETSAPI.Import.Service_Area oldObject, ref ServiceArea serviceArea, string systemId)
        {
            bool isNew = false;
            if (serviceArea == null)
            {
                isNew = true;
                serviceArea = new ServiceArea();
            }

            if (oldObject.Service_Area_Id <= 0)
                return;
            serviceArea.Id = oldObject.Service_Area_Id;
            serviceArea.MinistryServiceAreaID = oldObject.Service_Area_Id;
            serviceArea.DistrictId = oldObject.District_Area_Id;
            serviceArea.Name = oldObject.Service_Area_Desc;
            serviceArea.AreaNumber = oldObject.Service_Area_Cd;

            District district = dbContext.Districts.FirstOrDefault(x => x.MinistryDistrictID == oldObject.District_Area_Id);
            serviceArea.District = district;

            if (isNew)
            {
                serviceArea.CreateUserid = systemId;
                serviceArea.CreateTimestamp = DateTime.UtcNow;
                dbContext.ServiceAreas.Add(serviceArea);
            }
            else
            {
                serviceArea.LastUpdateUserid = systemId;
                serviceArea.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.ServiceAreas.Update(serviceArea);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="localArea"></param>
        static private void CopyToLocalArea(DbAppContext dbContext, HETSAPI.Import.Area oldObject, ref LocalArea localArea, string systemId)
        {
            bool isNew = false;
            if (localArea == null)
            {
                isNew = true;
                localArea = new LocalArea();
            }
            if (oldObject.Area_Id <= 0)
                return;
            localArea.Id = oldObject.Area_Id;
            localArea.Name = oldObject.Area_Desc;            
            
            ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);
            localArea.ServiceArea = serviceArea;

            if (isNew)
            {
                localArea.CreateUserid = systemId;
                localArea.CreateTimestamp = DateTime.UtcNow;
                dbContext.LocalAreas.Add(localArea);
            }
            else
            {
                localArea.LastUpdateUserid = systemId;
                localArea.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.LocalAreas.Update(localArea);
            }
            dbContext.SaveChanges();
        }


        static private void CopyToOwner(DbAppContext dbContext, HETSAPI.Import.Owner oldObject, ref Models.Owner owner, string systemId)
        {
            bool isNew = false;
            if (owner == null)
            {
                isNew = true;
                owner = new Models.Owner();
            }

            // The followings are the data mapping
            owner.LocalAreaId = oldObject.Area_Id;
            owner.CreateUserid = systemId;
            owner.IsMaintenanceContractor = (oldObject.Maintenance_Contractor == "Y") ? true : false;
            owner.WorkSafeBCExpiryDate = DateTime.Parse(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10));
            owner.WorkSafeBCPolicyNumber = oldObject.WCB_Num;
            owner.OrganizationName = oldObject.CGL_Company;
            owner.ArchiveCode = oldObject.Archive_Cd;

            Contact con = dbContext.Contacts.FirstOrDefault(x => x.GivenName.ToUpper() == oldObject.Owner_First_Name.ToUpper() && x.Surname.ToUpper() == oldObject.Owner_Last_Name);
            if (con == null)
            {
                owner.PrimaryContact = new Contact();
                owner.PrimaryContact.Surname = oldObject.Owner_Last_Name;
                owner.PrimaryContact.GivenName = oldObject.Owner_First_Name;
                owner.PrimaryContact.FaxPhoneNumber = "";
                owner.PrimaryContact.Province = "BC";
                //    owner.PrimaryContact.PostalCode = oldObject.
                owner.PrimaryContact.Notes = oldObject.Comment;
            }
            else
            {
                owner.PrimaryContact = con;
            }

            // TODO finish mapping here 
            if (isNew)
            {
                owner.CreateUserid = systemId;
                owner.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10)); // DateTime.UtcNow;
                owner.PrimaryContact.CreateTimestamp = DateTime.UtcNow;
                owner.PrimaryContact.CreateUserid = systemId;
                dbContext.Owners.Add(owner);
            }
            else
            {
                owner.LastUpdateUserid = systemId;
                owner.LastUpdateTimestamp = DateTime.UtcNow;
                owner.PrimaryContact.LastUpdateTimestamp = DateTime.UtcNow;
                owner.PrimaryContact.LastUpdateUserid = systemId;
                dbContext.Owners.Update(owner);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Copy user instance
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="user"></param>
        /// <param name="systemId"></param>
        static private void CopyToUser(DbAppContext dbContext, HETSAPI.Import.User_HETS oldObject, ref Models.User user, string systemId)
        {
            bool isNew = false;

            int startPos = oldObject.User_Cd.IndexOf(@"\") + 1;
            string smUserId = oldObject.User_Cd.Substring(startPos);
            user = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);

            if (user == null)
            {
                isNew = true;
                user = new Models.User();
            }

            // The followings are the data mapping
            ServiceArea serArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == oldObject.Service_Area_Id);
            user.DistrictId =  serArea.DistrictId;
            user.SmUserId = oldObject.User_Cd.Substring(startPos);
            // user.Email
            // user.GivenName
            // user.Surname

            int roleId =1;  //Default as regular user
            Models.UserRole userRole = new UserRole();
            switch (oldObject.Authority.Trim())
            {
                case "A":
                    roleId = 2; // Adminsitrator
                    break;
                case "N":
                    roleId = 1; // Regular User
                    break;
                case "R":
                    roleId = 3; // Special Admin
                    break;
                case "U":
                    roleId = 4; // User Management
                    break;
                default:
                    roleId = 5; // Unknown
                    break;
            }
            userRole.RoleId = roleId; // Unknown
            userRole.CreateTimestamp = DateTime.UtcNow;
            userRole.ExpiryDate = DateTime.UtcNow.AddMonths(12);
            userRole.CreateUserid = systemId;
            userRole.EffectiveDate = DateTime.UtcNow.AddDays(-1);

            if (isNew)
            {
                user.UserRoles.Add(userRole);
                user.CreateUserid = systemId;
                user.CreateTimestamp = DateTime.UtcNow;              
                user.Active = true;
                dbContext.Users.Add(user);
            }
            else
            {
                // if the user does not have the user role, add the user role
                int id = user.Id;
                UserRole uRole = user.UserRoles.FirstOrDefault(y => y.RoleId == roleId);
                if (uRole == null)
                {
                    user.UserRoles.Add(userRole);
                }
                dbContext.Users.Update(user);
            }
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Utility function to add a new ImportMap to the database
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable"></param>
        /// <param name="oldKey"></param>
        /// <param name="newTable"></param>
        /// <param name="newKey"></param>
        static private void AddImportMap(DbAppContext dbContext, string oldTable, int oldKey, string newTable, int newKey)
        {
            ImportMap importMap = new Models.ImportMap();
            importMap.OldTable = oldTable;
            importMap.OldKey = oldKey;
            importMap.NewTable = newTable;
            importMap.NewKey = newKey;
            dbContext.ImportMaps.Add(importMap);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Generates Memory stream from the input xml file
        /// </summary>
        /// <returns></returns>
        static private MemoryStream memoryStreamGenerator(string xmlFileName, string oldTable, string fileLocation, string rootAttr)
        {
           // fileLocation = @"H:\uploads\tmp";    //This is to test xml on network drive - network drive needs proper permission
            string fullPath = fileLocation + Path.DirectorySeparatorChar + xmlFileName;
            string contents = Regex.Replace(File.ReadAllText(fullPath), @"\r\n?|\n|[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "");  //Getting rid of all the new lines as well

            //string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            //return Regex.Replace(txt, r, "", RegexOptions.Compiled);


            // get the contents of the first tag.
            int startPos = contents.IndexOf('<') + 1;
            int endPos = contents.IndexOf('>');
            string tag = contents.Substring(startPos, endPos - startPos);

            contents = contents.Replace(tag, oldTable);


            string fixedXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                              + "<" + rootAttr + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n"
                              + contents
                              + "</" + rootAttr + ">";

            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXML));
            return memoryStream;
        }

        static private void ImportRegions(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETS_Region[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_Region[] legacyItems = (HETSAPI.Import.HETS_Region[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Region_Id);

                    if (importMap == null) // new entry
                    {
                        Models.Region reg = null;
                        CopyToRegion(dbContext, item, ref reg, systemId);
                        AddImportMap(dbContext, oldTable, item.Region_Id, newTable, reg.Id);
                    }
                    else // update
                    {
                        Models.Region reg = dbContext.Regions.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (reg == null) // record was deleted
                        {
                            CopyToRegion(dbContext, item, ref reg, systemId);
                            // update the import map.
                            importMap.NewKey = reg.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToRegion(dbContext, item, ref reg, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        static private void ImportDistricts(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETS_District[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_District[] legacyItems = (HETSAPI.Import.HETS_District[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.District_Id);

                    if (importMap == null) // new entry
                    {
                        Models.District dis = null;
                        CopyToDistrict(dbContext, item, ref dis, systemId);
                        AddImportMap(dbContext, oldTable, item.District_Id, newTable, dis.Id);
                    }
                    else // update
                    {
                        Models.District dis = dbContext.Districts.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (dis == null) // record was deleted
                        {
                            CopyToDistrict(dbContext, item, ref dis, systemId);
                            // update the import map.
                            importMap.NewKey = dis.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToDistrict(dbContext, item, ref dis, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        /// Import existing Cities
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportCities(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETS_City[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.HETS_City[] legacyItems = (HETSAPI.Import.HETS_City[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Seq_Num);

                    if (importMap == null) // new entry
                    {
                        Models.City city = null;
                        CopyToCity(dbContext, item, ref city, systemId);
                        AddImportMap(dbContext, oldTable, item.Seq_Num, newTable, city.Id);
                    }
                    else // update
                    {
                        Models.City city = dbContext.Cities.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (city == null) // record was deleted
                        {
                            CopyToCity(dbContext, item, ref city, systemId);
                            // update the import map.
                            importMap.NewKey = city.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToCity(dbContext, item, ref city, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        /// Import existing service areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportServiceAreas(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing Service Areas");
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETSAPI.Import.Service_Area[]), new XmlRootAttribute(rootAttr));           
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Service_Area[] legacyItems = (HETSAPI.Import.Service_Area[])ser.Deserialize(memoryStream);

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Service_Area_Id);

                    if (importMap == null) // new entry
                    {
                        ServiceArea serviceArea = null;
                        CopyToServiceArea(dbContext, item, ref serviceArea, systemId);
                        AddImportMap(dbContext, oldTable, item.Service_Area_Id, newTable, serviceArea.Id);
                    }
                    else // update
                    {
                        ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (serviceArea == null) // record was deleted
                        {
                            CopyToServiceArea(dbContext, item, ref serviceArea, systemId);
                            // update the import map.
                            importMap.NewKey = serviceArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToServiceArea(dbContext, item, ref serviceArea, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        /// Import local areas
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        static private void ImportLocalAreas(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                performContext.WriteLine("Processing Local Areas");

                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(HETSAPI.Import.Area[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Area[] legacyItems = (HETSAPI.Import.Area[])ser.Deserialize(memoryStream);
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Area_Id);

                    if (importMap == null) // new entry
                    {
                        LocalArea localArea = null;
                        CopyToLocalArea(dbContext, item, ref localArea, systemId);
                        AddImportMap(dbContext, oldTable, item.Service_Area_Id, newTable, localArea.Id);
                    }
                    else // update
                    {
                        LocalArea localArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (localArea == null) // record was deleted
                        {
                            CopyToLocalArea(dbContext, item, ref localArea, systemId);
                            // update the import map.
                            importMap.NewKey = localArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToLocalArea(dbContext, item, ref localArea, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        ///  Import Owners
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="oldTable"></param>
        /// <param name="newTable"></param>
        /// <param name="xmlFileName"></param>
        /// <param name="systemId"></param>
        static private void ImportOwners(PerformContext performContext, DbAppContext dbContext, string fileLocation,
            string oldTable, string newTable, string xmlFileName, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Owner[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Owner[] legacyItems = (HETSAPI.Import.Owner[])ser.Deserialize(memoryStream);

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id);

                    if (importMap == null) // new entry
                    {
                        Models.Owner owner = null;
                        CopyToOwner(dbContext, item, ref owner, systemId);
                        AddImportMap(dbContext, oldTable, item.Popt_Id, newTable, owner.Id);
                    }
                    else // update
                    {
                        Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner == null) // record was deleted
                        {
                            CopyToOwner(dbContext, item, ref owner, systemId);
                            // update the import map.
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToOwner(dbContext, item, ref owner, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        static private void ImportUsers(PerformContext performContext, DbAppContext dbContext, string fileLocation,
    string oldTable, string newTable, string xmlFileName, string systemId)
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
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.User_HETS[] legacyItems = (HETSAPI.Import.User_HETS[])ser.Deserialize(memoryStream);

                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id);

                    if (importMap == null) // new entry
                    {
                        Models.User instance = null;
                        CopyToUser(dbContext, item, ref instance, systemId);
                        AddImportMap(dbContext, oldTable, item.Popt_Id, newTable, instance.Id);
                    }
                    else // update
                    {
                        Models.User instance = dbContext.Users.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToUser(dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToUser(dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                    }
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
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="connectionstring"></param>
        static public void ImportJob(PerformContext context, string connectionstring, string fileLocation)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");

            string systemId = dbContext.Users.FirstOrDefault(x => x.SmUserId.ToUpper() == "SYSTEM_HETS").Id.ToString();

            // start by importing Region from Region.xml. THis goes to table HETS_REGION
            ImportRegions(context, dbContext, fileLocation, "HETS_Region", "HETS_Region", "Region.xml", systemId);
            // start by importing districts from District.xml. THis goes to table HETS_DISTRICT
            ImportDistricts(context, dbContext, fileLocation, "HETS_District", "HETS_District", "District.xml", systemId);
            // start by importing Cities from HETS_City.xml to HET_CITY
            ImportCities(context, dbContext, fileLocation, "HETS_City", "HETS_City", "HETS_City.xml", systemId);
            // Service Areas: from the file of Service_Area.xml to the table of HET_SERVICE_AREA
            ImportServiceAreas(context, dbContext, fileLocation, "Service_Area", "ServiceArea", "Service_Area.xml", systemId);
            // Importing the Local Areas from the file of Area.xml to the table of HET_LOCAL_AREA
            ImportLocalAreas(context, dbContext, fileLocation, "Area", "LocalArea", "Area.xml", systemId);
            // Owners: This has effects on Table HETS_OWNER and HETS_Contact
            ImportOwners(context, dbContext, fileLocation,  "Owner", "Owner", "Owner.xml", systemId);
            // Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE  
            ImportUsers(context, dbContext, fileLocation, "User_HETS", "User_HETS", "User_HETS.xml", systemId);
        }

    }
}
