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
        const string oldRegionTable = "HETS_Region";
        const string newRegionTable = "HETS_Region";
        const string regionXMLFile = "Region.xml";

        const string oldDistrictTable = "HETS_District";
        const string newDistrictTable = "HETS_District";
        const string districtXMLFile = "District.xml";

        const string oldCityTable = "HETS_City";
        const string newCityTable = "HET_City";
        const string cityXMLFile = "HETS_City.xml";

        const string oldServiceAreaTable = "Service_Area";
        const string newServiceAreaTable = "ServiceArea";
        const string serviceAreaXMLFile = "Service_Area.xml";

        const string oldLocalAreaTable = "Area";
        const string newLocalAreaTable = "LocalArea";
        const string localAreaXMLFile = "Area.xml";

        const string oldOwnerTable = "Owner";
        const string newOwnerTable = "HET_OWNER";
        const string ownerXMLFile = "Owner.xml";

        const string oldUserTable = "User_HETS";
        const string newUserTable = "HET_USER";
        const string userXMLFile = "User_HETS.xml";

        const string oldEquipTypeTable = "EquipType";
        const string newEquipTypeTable = "HET_EQUIPMMENT_TYPE";
        const string equipTypeXMLFile = "Equip_Type.xml";

        const string oldEquipTable = "Equip";
        const string newEquipTable = "HET_EQUIPMMENT";
        const string equipXMLFile = "Equip.xml";


        static private void CopyToRegion(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_Region oldObject, ref Models.Region reg, string systemId)
        {
            bool isNew = false;
            if (reg == null)
            {
                isNew = true;
                reg = new Models.Region();
            }

            if (dbContext.Regions.Where(x => x.Name.ToUpper() == oldObject.Name.ToUpper()).Count() == 0)
            {
                isNew = true;
                reg.Name = oldObject.Name.Trim();
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
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Region ***");
                performContext.WriteLine(e.ToString());
            }
        }

        static private void CopyToDistrict(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_District oldObject, ref Models.District dis, string systemId)
        {
            bool isNew = false;
            if (dis == null)
            {
                isNew = true;
                dis = new Models.District();
            }

            if (dbContext.Districts.Where(x => x.Name.ToUpper() == oldObject.Name.Trim().ToUpper()).Count() == 0)
            {
                isNew = true;
                dis.Name = oldObject.Name.Trim();
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
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update District ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy from legacy to new record For the table of HET_City
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="city"></param>
        static private void CopyToCity(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.HETS_City oldObject, ref Models.City city, string systemId)
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
                city.Name = oldObject.Name.Trim();
                city.Id = dbContext.Cities.Max(x => x.Id) + 1;   //oldObject.Seq_Num;  
                city.CreateTimestamp = DateTime.UtcNow;
                city.CreateUserid = systemId;
            }

            if (isNew)
            {
                dbContext.Cities.Add(city);   //Adding the city to the database table of HET_CITY
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update City ***");
                performContext.WriteLine(e.ToString());
            }
        }


        /// <summary>
        /// Copy from legacy to new record
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="serviceArea"></param>
        static private void CopyToServiceArea(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Service_Area oldObject, ref ServiceArea serviceArea, string systemId)
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
            serviceArea.Name = oldObject.Service_Area_Desc.Trim();
            serviceArea.AreaNumber = oldObject.Service_Area_Cd;

            District district = dbContext.Districts.FirstOrDefault(x => x.MinistryDistrictID == oldObject.District_Area_Id);
            serviceArea.District = district;

            try
            {
                serviceArea.StartDate = DateTime.Parse(oldObject.FiscalStart.Trim().Substring(0, 10));
            }
            catch
            {
 
            }

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
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Service Area ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="localArea"></param>
        static private void CopyToLocalArea(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Area oldObject, ref LocalArea localArea, string systemId)
        {
            bool isNew = false;
            if (localArea == null)
            {
                isNew = true;
                localArea = new LocalArea(oldObject.Area_Id);
                localArea.Id = oldObject.Area_Id;
            }
            if (oldObject.Area_Id <= 0)
                return;
            localArea.Name = oldObject.Area_Desc.Trim();

            try
            {
                ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.MinistryServiceAreaID == oldObject.Service_Area_Id);
                localArea.ServiceArea = serviceArea;
            }
            catch
            {
            }


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
        }


        static private void CopyToOwner(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Owner oldObject, ref Models.Owner owner, 
            string systemId, ref int maxOwnerIndex, ref int maxContactIndex)
        {
            bool isNew = false;
            if (owner == null)
            {
                isNew = true;
                owner = new Models.Owner(++maxOwnerIndex);
            }

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = AddUserFromString(dbContext, oldObject.Created_By, systemId);

            // The followings are the data mapping
            //  owner.LocalAreaId = oldObject.Area_Id;
            try
            {
                owner.IsMaintenanceContractor = (oldObject.Maintenance_Contractor.Trim() == "Y") ? true : false;
            }
            catch
            {

            }
            try
            {
               // owner.LocalAreaId = oldObject.Area_Id;
                owner.LocalArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);
            }
            catch
            {

            }
            try
            {
                owner.CGLEndDate = DateTime.Parse(oldObject.CGL_End_Dt.Trim().Substring(0, 10));
            }
            catch
            {
 
            }
            try
            {
                owner.WorkSafeBCExpiryDate = DateTime.Parse(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10));
            }
            catch
            {
            }
            try
            {
                owner.WorkSafeBCPolicyNumber = oldObject.WCB_Num.Trim();
            }
            catch
            {
            }
            try
            {
                owner.OrganizationName = oldObject.CGL_Company.Trim();
            }
            catch
            {
            }
            try
            {
                owner.ArchiveCode = oldObject.Archive_Cd;
            }
            catch
            {
            }

            try
            {
                owner.Status = oldObject.Status_Cd.Trim();
            }
            catch
            {
            }


            Contact con = dbContext.Contacts.FirstOrDefault(x => x.GivenName.ToUpper() == oldObject.Owner_First_Name.Trim().ToUpper() && x.Surname.ToUpper() == oldObject.Owner_Last_Name.Trim().ToUpper());
            if (con == null)
            {
                con = new Contact(++maxContactIndex);
                try
                {
                    con.Surname = oldObject.Owner_Last_Name.Trim();
                    con.GivenName = oldObject.Owner_First_Name.Trim();
                }
                catch
                {
                }


                con.FaxPhoneNumber = "";
                con.Province = "BC";
                //    owner.PrimaryContact.PostalCode = oldObject.
                try
                {
                    con.Notes = new string(oldObject.Comment.Take(511).ToArray());
                }
                catch
                {
                }
                dbContext.Contacts.Add(con);
            }


            // TODO finish mapping here 
            if (isNew)
            {
                owner.CreateUserid = createdBy.SmUserId;
                try
                {
                    owner.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10)); // DateTime.UtcNow;
                }
                catch
                {
                }
                con.CreateTimestamp = DateTime.UtcNow;
                con.CreateUserid = createdBy.SmUserId;
                owner.PrimaryContact = con;
                dbContext.Owners.Add(owner);
            }
            else  // The owner existed in the database
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

                }
                dbContext.Owners.Update(owner);
            }
        }

        /// <summary>
        /// Copy user instance
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="user"></param>
        /// <param name="systemId"></param>
        static private void CopyToUser(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.User_HETS oldObject, ref Models.User user, string systemId)
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
            Models.User modifiedBy = AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = AddUserFromString(dbContext, oldObject.Created_By, systemId);
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


            int roleId = GetRoleIdFromAuthority(authority);

            Models.User user1 = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);

            if (user == null && user1 != null)
                user = user1;

                ServiceArea serArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == service_Area_Id);
            if (user == null  )
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

        /// <summary>
        /// Convert Authority to userRole id
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        static private int GetRoleIdFromAuthority(string authority)
        {
            int roleId;
            switch (authority)
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
                    roleId = 1; // Unknown as regular user
                    break;
            }
            return roleId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="etype"></param>
        /// <param name="systemId"></param>
        static private void CopyToEquipType(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipType oldObject, ref Models.EquipmentType etype, string systemId)
        {
            bool isNew = false;

            if (oldObject.Equip_Type_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (etype == null)
            {
                isNew = true;
                etype = new Models.EquipmentType();
                etype.Id = oldObject.Equip_Type_Id;
                etype.IsDumpTruck = false;   // Where this is coming from?   !!!!!!
                try
                {
                    etype.ExtendHours = float.Parse(oldObject.Extend_Hours.Trim());
                    etype.MaximumHours = float.Parse(oldObject.Max_Hours.Trim());
                    etype.MaxHoursSub = float.Parse(oldObject.Max_Hours_Sub.Trim());
                }
                catch
                {

                }
                try
                {
                    etype.Name = oldObject.Equip_Type_Cd.Trim();
                }
                catch
                {

                }
               
                etype.CreateTimestamp = DateTime.UtcNow;
                etype.CreateUserid = createdBy.SmUserId;
                dbContext.EquipmentTypes.Add(etype);
            }
            else
            {
                etype = dbContext.EquipmentTypes
                    .First(x => x.Id == oldObject.Equip_Type_Id);
                etype.LastUpdateUserid = modifiedBy.SmUserId;
                try
                {
                    etype.LastUpdateTimestamp = DateTime.Parse(oldObject.Modified_Dt.Trim().Substring(0, 10));
                }
                catch
                {

                }
                dbContext.EquipmentTypes.Update(etype);
            }
            try
            {
                // Di dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Equipment type ***");
                performContext.WriteLine(e.ToString());
            }
        }


        static private void CopyToEquip(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Equip oldObject, ref Models.Equipment instance, string systemId)
        {
            bool isNew = false;

            if (oldObject.Equip_Type_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.Equipment();
                instance.Id = oldObject.Equip_Id;
                try
                {
                    // instance.DumpTruckId = oldObject.Reg_Dump_Trk;
                    try
                    {
                        instance.ArchiveCode = oldObject.Archive_Cd;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.ArchiveReason = new string(oldObject.Archive_Reason.Take(2048).ToArray());  
                        instance.Notes = new List<Note>();
                        instance.Notes.Add(new Note(oldObject.Comment, true));
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    // instance.ArchiveDate = oldObject. 
                    try
                    {
                        instance.LicencePlate = oldObject.Licence;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        LocalArea area = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);
                        DistrictEquipmentType equipType = dbContext.DistrictEquipmentTypes.FirstOrDefault(x => x.EquipmentTypeId == oldObject.Equip_Type_Id);
                        instance.DistrictEquipmentType = equipType;
                        instance.LocalArea = area;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    try
                    {
                        instance.EquipmentCode = oldObject.Equip_Cd;
                        instance.Model = oldObject.Model;
                        instance.Make = oldObject.Make;
                        instance.Year = oldObject.Year;
                        instance.Operator = oldObject.Operator;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    // instance.RefuseRate = float.Parse(oldObject.Refuse_Rate.Trim());
                    try
                    {
                        instance.PayRate = float.Parse(oldObject.Pay_Rate.Trim());
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        // Find the owner which is referenced in the equipment of the xml file entry
                        int newkey = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldOwnerTable && x.OldKey == oldObject.Owner_Popt_Id).Id;
                        Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == newkey);
                       // instance.OwnerId = owner.Id;
                        instance.Owner = owner;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.SerialNumber = oldObject.Serial_Num;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.Seniority = float.Parse(oldObject.Seniority.Trim());
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.Status = oldObject.Status_Cd;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    try
                    {
                        instance.YearsOfService = float.Parse(oldObject.Num_Years.Trim());
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }

                    try
                    {
                        instance.ServiceHoursLastYear = oldObject.YTD1 != null ? float.Parse(oldObject.YTD1.Trim()) : 0;
                        instance.ServiceHoursTwoYearsAgo = oldObject.YTD2 != null ? float.Parse(oldObject.YTD2.Trim()) : 0;
                        instance.ServiceHoursThreeYearsAgo = oldObject.YTD3 != null ? float.Parse(oldObject.YTD3.Trim()) : 0;
                    }
                    catch (Exception e)
                    {
                        string i = e.ToString();
                    }
                    
                }
                catch (Exception e)
                {
                    string i = e.ToString();
                }
                try
                {
                    instance.ReceivedDate= DateTime.Parse(oldObject.Received_Dt.Trim().Substring(0, 10));
                }
                catch
                {

                }

                instance.CreateTimestamp = DateTime.UtcNow;
                instance.CreateUserid = createdBy.SmUserId;
                dbContext.Equipments.Add(instance);
            }
            else
            {
                instance = dbContext.Equipments
                    .First(x => x.Id == oldObject.Equip_Id);
                instance.LastUpdateUserid = modifiedBy.SmUserId;
                try
                {
                    instance.LastUpdateUserid = modifiedBy.SmUserId;
                    instance.LastUpdateTimestamp = DateTime.Parse(oldObject.Modified_Dt.Trim().Substring(0, 10));
                }
                catch
                {

                }
                dbContext.Equipments.Update(instance);
            }
            try
            {
                //Di dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Equipment ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        static private void SaveChangesToDatabase(PerformContext performContext, DbAppContext dbContext)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
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
                        CopyToRegion(performContext, dbContext, item, ref reg, systemId);
                        AddImportMap(dbContext, oldTable, item.Region_Id, newTable, reg.Id);
                    }
                    else // update
                    {
                        Models.Region reg = dbContext.Regions.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (reg == null) // record was deleted
                        {
                            CopyToRegion(performContext, dbContext, item, ref reg, systemId);
                            // update the import map.
                            importMap.NewKey = reg.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToRegion(performContext, dbContext, item, ref reg, systemId);
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
                        CopyToDistrict(performContext, dbContext, item, ref dis, systemId);
                        AddImportMap(dbContext, oldTable, item.District_Id, newTable, dis.Id);
                    }
                    else // update
                    {
                        Models.District dis = dbContext.Districts.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (dis == null) // record was deleted
                        {
                            CopyToDistrict(performContext, dbContext, item, ref dis, systemId);
                            // update the import map.
                            importMap.NewKey = dis.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToDistrict(performContext, dbContext, item, ref dis, systemId);
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
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.City_Id);

                    if (importMap == null) // new entry
                    {
                        Models.City city = null;
                        CopyToCity(performContext, dbContext, item, ref city, systemId);
                        AddImportMap(dbContext, oldTable, item.City_Id, newTable, city.Id);
                    }
                    else // update
                    {
                        Models.City city = dbContext.Cities.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (city == null) // record was deleted
                        {
                            CopyToCity(performContext, dbContext, item, ref city, systemId);
                            // update the import map.
                            importMap.NewKey = city.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToCity(performContext, dbContext, item, ref city, systemId);
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
                        if (item.Service_Area_Cd > 0)
                        {
                            ServiceArea serviceArea = null;
                            CopyToServiceArea(performContext, dbContext, item, ref serviceArea, systemId);
                            AddImportMap(dbContext, oldTable, item.Service_Area_Id, newTable, serviceArea.Id);
                        }
                    }
                    else // update
                    {
                        ServiceArea serviceArea = dbContext.ServiceAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (serviceArea == null) // record was deleted
                        {
                            CopyToServiceArea(performContext, dbContext, item, ref serviceArea, systemId);
                            // update the import map.
                            importMap.NewKey = serviceArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                            dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToServiceArea(performContext, dbContext, item, ref serviceArea, systemId);
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
                        if (item.Area_Id > 0)
                        {
                            LocalArea localArea = null;
                            CopyToLocalArea(performContext, dbContext, item, ref localArea, systemId);
                            AddImportMap(dbContext, oldTable, item.Area_Id, newTable, localArea.Id);
                        }
                    }
                    else // update
                    {
                        LocalArea localArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (localArea == null) // record was deleted
                        {
                            CopyToLocalArea(performContext, dbContext, item, ref localArea, systemId);
                            // update the import map.
                            importMap.NewKey = localArea.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToLocalArea(performContext, dbContext, item, ref localArea, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
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

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR With add or update Local Area ***");
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
            string oldTable, string newTable, string xmlFileName, string systemId, ref int maxOwnerIndex, ref int maxContactIndex)
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
                int ii = 1;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id);

                    if (importMap == null) // new entry
                    {
                        Models.Owner owner = null;
                        CopyToOwner(performContext, dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                        AddImportMap(dbContext, oldTable, item.Popt_Id, newTable, owner.Id);
                    }
                    else // update
                    {
                        Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner == null) // record was deleted
                        {
                            CopyToOwner(performContext, dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                            // update the import map.
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                           // dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToOwner(performContext, dbContext, item, ref owner, systemId, ref  maxOwnerIndex, ref maxContactIndex);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                           // dbContext.SaveChanges();
                        }
                    }
                    if (ii % 500 == 0)
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
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch
            {
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
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id);

                    if (importMap == null) // new entry
                    {
                        Models.User instance = null;
                        CopyToUser(performContext, dbContext, item, ref instance, systemId);
                        AddImportMap(dbContext, oldTable, item.Popt_Id, newTable, instance.Id);
                    }
                    //else // update
                    //{
                    //    Models.User instance = dbContext.Users.FirstOrDefault(x => x.Id == importMap.NewKey);
                    //    if (instance == null) // record was deleted
                    //    {
                    //        CopyToUser(performContext, dbContext, item, ref instance, systemId);
                    //        // update the import map.
                    //        importMap.NewKey = instance.Id;
                    //        dbContext.ImportMaps.Update(importMap);
                    //      //DI  dbContext.SaveChanges();
                    //    }
                    //    else // ordinary update.
                    //    {
                    //        CopyToUser(performContext, dbContext, item, ref instance, systemId);
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
        /// Import Qquipment Type
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="oldTable"></param>
        /// <param name="newTable"></param>
        /// <param name="xmlFileName"></param>
        /// <param name="systemId"></param>
        static private void ImportEquipType(PerformContext performContext, DbAppContext dbContext, string fileLocation,
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
                XmlSerializer ser = new XmlSerializer(typeof(EquipType[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.EquipType[] legacyItems = (HETSAPI.Import.EquipType[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Type_Id);

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Type_Id > 0)
                        {
                            Models.EquipmentType instance = null;
                            CopyToEquipType(performContext, dbContext, item, ref instance, systemId);
                            AddImportMap(dbContext, oldTable, item.Equip_Type_Id, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.EquipmentType instance = dbContext.EquipmentTypes.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToEquipType(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                            //Di  dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToEquipType(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                           //Di dbContext.SaveChanges();
                        }
                    }
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
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch
            {

            }
        }


        static private void ImportEquip(PerformContext performContext, DbAppContext dbContext, string fileLocation,
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
                XmlSerializer ser = new XmlSerializer(typeof(Equip[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Equip[] legacyItems = (HETSAPI.Import.Equip[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Equip_Id);

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Id > 0)
                        {
                            Models.Equipment instance = null;
                            CopyToEquip(performContext, dbContext, item, ref instance, systemId);
                            AddImportMap(dbContext, oldTable, item.Equip_Id, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.Equipment instance = dbContext.Equipments.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToEquip(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                          //  dbContext.SaveChanges();
                        }
                        else // ordinary update.
                        {
                            CopyToEquip(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                           // dbContext.SaveChanges();
                        }
                    }

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
                performContext.WriteLine("*** Done ***");
                try
                {
                    dbContext.SaveChanges();
                }
                catch
                {

                }
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// /// <summary>
        /// Given a userString like: "Espey, Carol  (IDIR\cespey)"  Format the user and Add the user if no in the database
        /// Return the user or a default system user called "SYSTEM_HETS" as smSystemId
        /// </summary>
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userString"></param>
        /// <param name="smSystemId"></param>
        /// <returns></returns>
        static public Models.User AddUserFromString(DbAppContext dbContext, string userString, string smSystemId)
        {
            // Find out the smUserId for the <Modified_By>
            int index = -1;
            try
            {
                index = userString.IndexOf(@"(IDIR\");
            }
            catch
            {
                return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
            }
            if (index > 0)
            { 
                try
                {
                    int commaPos = userString.IndexOf(@",");
                    int leftBreakPos = userString.IndexOf(@"(");
                    int startPos = userString.IndexOf(@"\");
                    int rightBreakPos = userString.IndexOf(@")");
                    string surName = userString.Substring(0, commaPos);
                    string givenName = userString.Substring(commaPos + 2, leftBreakPos - commaPos - 2);
                    string smUserId = userString.Substring(startPos + 1, rightBreakPos - startPos - 1);
                    Models.User user = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);
                    if (user == null)
                    {
                        user = new User();
                        user.Surname = surName.Trim();
                        user.GivenName = givenName.Trim();
                        user.SmUserId = smUserId.Trim();
                        dbContext.Users.Add(user);
                        dbContext.SaveChanges();
                    }
                    return  user;
                }
                catch (Exception e)
                {
                    return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
                }
            }
            else
            {
                return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
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

            string systemId = "SYSTEM_HETS"; // dbContext.Users.FirstOrDefault(x => x.SmUserId.ToUpper() == "SYSTEM_HETS").Id.ToString();

            // start by importing Region from Region.xml. THis goes to table HETS_REGION
            ImportRegions(context, dbContext, fileLocation, oldRegionTable, newRegionTable, regionXMLFile, systemId);
            
            // start by importing districts from District.xml. THis goes to table HETS_DISTRICT
            dbContext = new DbAppContext(null, options.Options);
            ImportDistricts(context, dbContext, fileLocation, oldDistrictTable, newDistrictTable, districtXMLFile, systemId);
            
            // start by importing Cities from HETS_City.xml to HET_CITY
            dbContext = new DbAppContext(null, options.Options);
            ImportCities(context, dbContext, fileLocation, oldCityTable, newCityTable, cityXMLFile, systemId);
            
            // Service Areas: from the file of Service_Area.xml to the table of HET_SERVICE_AREA
            dbContext = new DbAppContext(null, options.Options);
            ImportServiceAreas(context, dbContext, fileLocation, oldServiceAreaTable, newServiceAreaTable, serviceAreaXMLFile, systemId);
            
            // Importing the Local Areas from the file of Area.xml to the table of HET_LOCAL_AREA
            dbContext = new DbAppContext(null, options.Options);
            ImportLocalAreas(context, dbContext, fileLocation, oldLocalAreaTable, newLocalAreaTable, localAreaXMLFile, systemId);

            // Owners: This has effects on Table HETS_OWNER and HETS_Contact
            dbContext = new DbAppContext(null, options.Options);
            int maxOwnerIndex  = dbContext.Owners.Max(x => x.Id);
            int maxContactIndex = dbContext.Contacts.Max(x => x.Id);
            ImportOwners(context, dbContext, fileLocation, oldOwnerTable, newOwnerTable, ownerXMLFile, systemId, ref maxOwnerIndex, ref maxContactIndex);

            // Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE  
            dbContext = new DbAppContext(null, options.Options);
            ImportUsers(context, dbContext, fileLocation, oldUserTable, newUserTable, userXMLFile, systemId);

            //Import Equiptment type from EquipType.xml This has effects on Table HET_USER and HET_EQUIPMENT_TYPE  
           //  dbContext = new DbAppContext(null, options.Options);
           //  ImportEquipType(context, dbContext, fileLocation, oldEquipTypeTable, newEquipTypeTable, equipTypeXMLFile, systemId);

            //Import Equiptment  from Equip.xml  This has effects on Table HET_USER and HET_EQUIP
          //  dbContext = new DbAppContext(null, options.Options);
           // ImportEquip(context, dbContext, fileLocation, oldEquipTable, newEquipTable, equipXMLFile, systemId);
        }
    }
}
