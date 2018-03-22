using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HETSAPI.Import;

namespace HETSAPI.Models
{
    /// <summary>
    /// Database Content Extensions (i.e. non standard CRUD operations)
    /// </summary>
    public static class DbAppContextExtensions
    {
        private const string SystemId = "SYSTEM_HETS";

        /// <summary>
        /// Returns a district for a given Ministry Id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">The Ministry Id</param>
        /// <returns>District</returns>
        public static District GetDistrictByMinistryDistrictId(this IDbAppContext context, int id)
        {
            District district = context.Districts.Where(x => x.MinistryDistrictID == id)
                    .Include(x => x.Region)
                    .FirstOrDefault();

            return district;
        }

        /// <summary>
        /// Returns a region for a given Ministry Id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">The Ministry Id</param>
        /// <returns>Region</returns>
        public static Region GetRegionByMinistryRegionId(this IDbAppContext context, int id)
        {
            Region region = context.Regions.FirstOrDefault(x => x.MinistryRegionID == id);

            return region;
        }

        /// <summary>
        /// Returns a service area for a given Ministry Id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">The Ministry Id</param>
        /// <returns>Region</returns>
        public static ServiceArea GetServiceAreaByMinistryServiceAreaId(this IDbAppContext context, int id)
        {
            ServiceArea serviceArea = context.ServiceAreas.Where(x => x.MinistryServiceAreaID == id)
                    .Include(x => x.District.Region)
                    .FirstOrDefault();

            return serviceArea;
        }

        /// <summary>
        /// Returns a district id for a given user id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId">A user id</param>
        /// <returns>Region</returns>
        public static IQueryable<int?> GetDistrictIdByUserId(this IDbAppContext context, int? userId)
        {            
            return context.Users.Where(x => x.Id.Equals(userId)).Select(x => x.DistrictId);
        }

        /// <summary>
        /// Returns a role based on the role name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Role GetRole(this IDbAppContext context, string name)
        {
            Role role = context.Roles.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    .Include(r => r.RolePermissions).ThenInclude(p => p.Permission)
                    .FirstOrDefault();

            return role;
        }        

        /// <summary>
        /// Returns a user based on the guid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static User GetUserByGuid(this IDbAppContext context, string guid)
        {
            User user = context.Users.Where(x => x.Guid != null && x.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    .Include(u => u.UserRoles).ThenInclude(r => r.Role).ThenInclude(rp => rp.RolePermissions).ThenInclude(p => p.Permission)
                    .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Returns a user based on the account name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="smUserId"></param>
        /// <returns></returns>
        public static User GetUserBySmUserId(this IDbAppContext context, string smUserId)
        {
            User user = context.Users.Where(x => x.SmUserId != null && x.SmUserId.Equals(smUserId, StringComparison.OrdinalIgnoreCase))
                    .Include(u => u.UserRoles).ThenInclude(r => r.Role).ThenInclude(rp => rp.RolePermissions).ThenInclude(p => p.Permission)
                    .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Create users from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userJsonPath"></param>
        public static void AddInitialUsersFromFile(this IDbAppContext context, string userJsonPath)
        {
            if (!string.IsNullOrEmpty(userJsonPath) && File.Exists(userJsonPath))
            {
                string userJson = File.ReadAllText(userJsonPath);
                context.AddInitialUsers(userJson);
            }
        }

        private static void AddInitialUsers(this IDbAppContext context, string userJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(userJson);

            if(users != null)
            {
                context.AddInitialUsers(users);
            }
        }

        private static void AddInitialUsers(this IDbAppContext context, List<User> users)
        {
            users.ForEach(context.AddInitialUser);
        }
       
        private static void AddInitialUser(this IDbAppContext context, User initialUser)
        {
            User user = context.GetUserBySmUserId(initialUser.SmUserId);
            if (user != null)
            {
                return;
            }

            user = new User
            {
                Active = true,
                Email = initialUser.Email,
                GivenName = initialUser.GivenName,
                Initials = initialUser.Initials,
                SmAuthorizationDirectory = initialUser.SmAuthorizationDirectory,
                SmUserId = initialUser.SmUserId,
                Surname = initialUser.Surname,
                AppCreateUserid = SystemId,
                AppCreateTimestamp = DateTime.UtcNow,
                AppLastUpdateUserid = SystemId,
                AppLastUpdateTimestamp = DateTime.UtcNow
            };

            District district = null;

            if (initialUser.District != null)
            {
                district = context.GetDistrictByMinistryDistrictId(initialUser.District.MinistryDistrictID);
            }

            user.District = district;            

            string[] userRoles = initialUser.UserRoles.Select(x => x.Role.Name).ToArray();

            if (user.UserRoles == null)
                user.UserRoles = new List<UserRole>();

            foreach (string userRole in userRoles)
            {
                Role role = context.GetRole(userRole);

                if (role != null)
                {
                    user.UserRoles.Add(
                        new UserRole
                        {
                            EffectiveDate = DateTime.UtcNow,
                            Role = role,
                            AppCreateUserid = SystemId,
                            AppCreateTimestamp = DateTime.UtcNow,
                            AppLastUpdateUserid = SystemId,
                            AppLastUpdateTimestamp = DateTime.UtcNow
                        });
                }
            }

            context.Users.Add(user);
        }

        /// <summary>
        /// Create regions from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="regionJsonPath"></param>
        public static void AddInitialRegionsFromFile(this IDbAppContext context, string regionJsonPath)
        {
            if (!string.IsNullOrEmpty(regionJsonPath) && File.Exists(regionJsonPath))
            {
                string regionJson = File.ReadAllText(regionJsonPath);
                context.AddInitialRegions(regionJson);
            }
        }

        private static void AddInitialRegions(this IDbAppContext context, string regionJson)
        {
            List<Region> regions = JsonConvert.DeserializeObject<List<Region>>(regionJson);

            if (regions != null)
            {
                context.AddInitialRegions(regions);
            }
        }

        private static void AddInitialRegions(this IDbAppContext context, List<Region> regions)
        {
            regions.ForEach(context.AddInitialRegion);
        }

        /// <summary>
        /// Adds a region to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialRegion(this IDbAppContext context, Region initialRegion)
        {
            Region region = context.GetRegionByMinistryRegionId(initialRegion.MinistryRegionID);
            if (region != null)
            {
                return;
            }

            region = new Region
            {
                MinistryRegionID = initialRegion.MinistryRegionID,
                Name = initialRegion.Name,
                StartDate = initialRegion.StartDate,
                AppCreateUserid = SystemId,
                AppCreateTimestamp = DateTime.UtcNow,
                AppLastUpdateUserid = SystemId,
                AppLastUpdateTimestamp = DateTime.UtcNow
            };

            context.Regions.Add(region);
        }

        /// <summary>
        /// Adds initial districts from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="districtJsonPath"></param>
        public static void AddInitialDistrictsFromFile(this IDbAppContext context, string districtJsonPath)
        {
            if (!string.IsNullOrEmpty(districtJsonPath) && File.Exists(districtJsonPath))
            {
                string districtJson = File.ReadAllText(districtJsonPath);
                context.AddInitialDistricts(districtJson);
            }
        }

        private static void AddInitialDistricts(this IDbAppContext context, string districtJson)
        {
            List<District> districts = JsonConvert.DeserializeObject<List<District>>(districtJson);
            if (districts != null)
            {
                context.AddInitialDistricts(districts);
            }
        }

        private static void AddInitialDistricts(this IDbAppContext context, List<District> districts)
        {
            districts.ForEach(context.AddInitialDistrict);
        }

        /// <summary>
        /// Adds a district to the system, only if it does not exist.
        /// </summary>
        private static void AddInitialDistrict(this IDbAppContext context, District initialDistrict)
        {
            District district = context.GetDistrictByMinistryDistrictId(initialDistrict.MinistryDistrictID);
            if (district != null)
            {
                return;
            }

            district = new District
            {
                MinistryDistrictID = initialDistrict.MinistryDistrictID,
                Name = initialDistrict.Name,
                StartDate = initialDistrict.StartDate,
                AppCreateUserid = SystemId,
                AppCreateTimestamp = DateTime.UtcNow,
                AppLastUpdateUserid = SystemId,
                AppLastUpdateTimestamp = DateTime.UtcNow
            };

            if (initialDistrict.Region != null)
            {
                Region region = context.GetRegionByMinistryRegionId(initialDistrict.Region.MinistryRegionID);
                district.Region = region;
            }
            else
            {
                district.Region = null;
            }

            context.Districts.Add(district);
        }

        /// <summary>
        /// Adds initial Conditions from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="conditionsJsonPath"></param>
        public static void AddInitialConditionsFromFile(this IDbAppContext context, string conditionsJsonPath)
        {
            if (!string.IsNullOrEmpty(conditionsJsonPath) && File.Exists(conditionsJsonPath))
            {
                string conditionsJson = File.ReadAllText(conditionsJsonPath);
                context.AddInitialConditions(conditionsJson);
            }
        }

        private static void AddInitialConditions(this IDbAppContext context, string conditionsJson)
        {
            List<ConditionType> conditions = JsonConvert.DeserializeObject<List<ConditionType>>(conditionsJson);
            if (conditions != null)
            {
                context.AddInitialConditions(conditions);
            }
        }

        private static void AddInitialConditions(this IDbAppContext context, List<ConditionType> conditions)
        {
            conditions.ForEach(context.AddInitialCondition);
        }

        /// <summary>
        /// Adds a condition to the system, only if it does not exist.
        /// </summary>
        public static void AddInitialCondition(this IDbAppContext context, ConditionType initialCondition)
        {
            int conditionCount = context.ConditionTypes.Count();

            if (conditionCount > 0)
            {
                return;
            }

            // ignore id from the file - use the sequence to insert
            ConditionType condition = new ConditionType
            {
                Active = true,
                ConditionTypeCode = initialCondition.ConditionTypeCode,
                Description = initialCondition.Description,
                AppCreateUserid = SystemId,
                AppCreateTimestamp = DateTime.UtcNow,
                AppLastUpdateUserid = SystemId,
                AppLastUpdateTimestamp = DateTime.UtcNow                
            };

            if (initialCondition.District != null)
            {
                District district = context.Districts.First(x => x.Id == initialCondition.District.Id);
                condition.DistrictId = district.Id;
            }
            else
            {
                condition.District = null;
            }

            context.ConditionTypes.Add(condition);
        }

        /// <summary>
        /// Adds initial EquipmentTypes from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="equipmentTypesJsonPath"></param>
        public static void AddInitialEquipmentTypesFromFile(this IDbAppContext context, string equipmentTypesJsonPath)
        {
            if (!string.IsNullOrEmpty(equipmentTypesJsonPath) && File.Exists(equipmentTypesJsonPath))
            {
                string equipmentTypesJson = File.ReadAllText(equipmentTypesJsonPath);
                context.AddInitialEquipmentTypes(equipmentTypesJson);
            }
        }

        private static void AddInitialEquipmentTypes(this IDbAppContext context, string equipmentTypesJson)
        {
            List<EquipmentType> equipmentTypes = JsonConvert.DeserializeObject<List<EquipmentType>>(equipmentTypesJson);
            if (equipmentTypes != null)
            {
                context.AddInitialEquipmentTypes(equipmentTypes);
            }
        }

        private static void AddInitialEquipmentTypes(this IDbAppContext context, List<EquipmentType> equipmentTypes)
        {
            equipmentTypes.ForEach(context.AddInitialEquipmentType);
        }

        /// <summary>
        /// Adds a equipment type to the system, only if it does not exist.
        /// </summary>
        public static void AddInitialEquipmentType(this IDbAppContext context, EquipmentType initialEquipmentType)
        {
            // Blue Book Rate Number equates to the Section in the Blue Book
            // e.g. 16.1 == "Dump Truck"
            // The Blue Book Sections are the master list for all Equipment Types
            EquipmentType equipType = context.EquipmentTypes.FirstOrDefault(x => x.BlueBookRateNumber == initialEquipmentType.BlueBookRateNumber);

            if (equipType != null)
            {
                // update the record
                equipType.BlueBookRateNumber = initialEquipmentType.BlueBookRateNumber;
                equipType.BlueBookSection = initialEquipmentType.BlueBookSection;
                equipType.ExtendHours = initialEquipmentType.ExtendHours;
                equipType.IsDumpTruck = initialEquipmentType.IsDumpTruck;
                equipType.MaxHoursSub = initialEquipmentType.MaxHoursSub;
                equipType.MaximumHours = initialEquipmentType.MaximumHours;
                equipType.Name = initialEquipmentType.Name;
                equipType.NumberOfBlocks = initialEquipmentType.NumberOfBlocks;
                equipType.AppLastUpdateUserid = SystemId;
                equipType.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.EquipmentTypes.Update(equipType);
            }
            else
            {
                // create new equipment type
                equipType = new EquipmentType
                {
                    BlueBookRateNumber = initialEquipmentType.BlueBookRateNumber,
                    BlueBookSection = initialEquipmentType.BlueBookSection,
                    ExtendHours = initialEquipmentType.ExtendHours,
                    IsDumpTruck = initialEquipmentType.IsDumpTruck,
                    MaxHoursSub = initialEquipmentType.MaxHoursSub,
                    MaximumHours = initialEquipmentType.MaximumHours,
                    Name = initialEquipmentType.Name,
                    NumberOfBlocks = initialEquipmentType.NumberOfBlocks,
                    AppCreateUserid = SystemId,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserid = SystemId,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                context.EquipmentTypes.Add(equipType);
            }            
        }

        /// <summary>
        /// Create servive areas from a (json) file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="districtJsonPath"></param>
        public static void AddInitialServiceAreasFromFile(this IDbAppContext context, string districtJsonPath)
        {
            if (!string.IsNullOrEmpty(districtJsonPath) && File.Exists(districtJsonPath))
            {
                string serviceAreaJson = File.ReadAllText(districtJsonPath);
                context.AddInitialServiceAreas(serviceAreaJson);
            }
        }

        private static void AddInitialServiceAreas(this IDbAppContext context, string serviceAreaJson)
        {
            List<ServiceArea> serviceAreas = JsonConvert.DeserializeObject<List<ServiceArea>>(serviceAreaJson);

            if (serviceAreas != null)
            {
                context.AddInitialServiceAreas(serviceAreas);
            }
        }

        private static void AddInitialServiceAreas(this IDbAppContext context, List<ServiceArea> serviceAreas)
        {
            serviceAreas.ForEach(context.AddInitialServiceArea);
        }
     
        private static void AddInitialServiceArea(this IDbAppContext context, ServiceArea initialServiceArea)
        {
            ServiceArea serviceArea = context.GetServiceAreaByMinistryServiceAreaId(initialServiceArea.MinistryServiceAreaID);

            if (serviceArea != null)
            {
                return;
            }

            serviceArea = new ServiceArea
            {
                MinistryServiceAreaID = initialServiceArea.MinistryServiceAreaID,
                Name = initialServiceArea.Name,
                StartDate = initialServiceArea.StartDate,
                AppCreateUserid = SystemId,
                AppCreateTimestamp = DateTime.UtcNow,
                AppLastUpdateUserid = SystemId,
                AppLastUpdateTimestamp = DateTime.UtcNow
            };

            if (initialServiceArea.District != null)
            {
                District district = context.GetDistrictByMinistryDistrictId(initialServiceArea.District.MinistryDistrictID);
                serviceArea.District = district;
            }
            else
            {
                serviceArea.District = null;
            }

            context.ServiceAreas.Add(serviceArea);
        }

        /// <summary>
        /// Update district
        /// </summary>
        /// <param name="context"></param>
        /// <param name="districtInfo"></param>
        public static void UpdateSeedDistrictInfo(this DbAppContext context, District districtInfo)
        {
            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(context, SystemId);

            // Adjust the region.
            int ministryRegionId = districtInfo.Region.MinistryRegionID;

            var exists = context.Regions.Any(a => a.MinistryRegionID == ministryRegionId);
            if (exists)
            {
                Region region = context.Regions.First(a => a.MinistryRegionID == ministryRegionId);
                districtInfo.Region = region;
            }
            else
            {
                districtInfo.Region = null;
            }

            District district = context.GetDistrictByMinistryDistrictId(districtInfo.MinistryDistrictID);
            if (district == null)
            {
                districtInfo.AppCreateUserid = SystemId;
                districtInfo.AppCreateTimestamp = DateTime.UtcNow;
                districtInfo.AppLastUpdateUserid = SystemId;
                districtInfo.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.Districts.Add(districtInfo);
            }
            else
            {
                district.Name = districtInfo.Name;
                district.Region = districtInfo.Region;
                district.StartDate = districtInfo.StartDate;
            }
        }
        
        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="context"></param>
        /// <param name="regionInfo"></param>
        public static void UpdateSeedRegionInfo(this DbAppContext context, Region regionInfo)
        {
            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(context, SystemId);

            Region region = context.GetRegionByMinistryRegionId(regionInfo.MinistryRegionID);

            if (region == null)
            {
                regionInfo.AppCreateUserid = SystemId;
                regionInfo.AppCreateTimestamp = DateTime.UtcNow;
                regionInfo.AppLastUpdateUserid = SystemId;
                regionInfo.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.Regions.Add(regionInfo);
            }
            else
            {
                region.Name = regionInfo.Name;
                region.StartDate = regionInfo.StartDate;
            }
        }

        /// <summary>
        /// Update service area
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceAreaInfo"></param>
        public static void UpdateSeedServiceAreaInfo(this DbAppContext context, ServiceArea serviceAreaInfo)
        {
            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(context, SystemId);

            // Adjust the district.
            int ministryDistrictId = serviceAreaInfo.District.MinistryDistrictID;
            var exists = context.Districts.Any(a => a.MinistryDistrictID == ministryDistrictId);

            if (exists)
            {
                District district = context.Districts.First(a => a.MinistryDistrictID == ministryDistrictId);
                serviceAreaInfo.District = district;
            }
            else
            {
                serviceAreaInfo.District = null;
            }

            ServiceArea serviceArea = context.GetServiceAreaByMinistryServiceAreaId(serviceAreaInfo.MinistryServiceAreaID);
            if (serviceArea == null)
            {
                serviceAreaInfo.AppCreateUserid = SystemId;
                serviceAreaInfo.AppCreateTimestamp = DateTime.UtcNow;
                serviceAreaInfo.AppLastUpdateUserid = SystemId;
                serviceAreaInfo.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.ServiceAreas.Add(serviceAreaInfo);
            }
            else
            {
                serviceArea.Name = serviceAreaInfo.Name;
                serviceArea.StartDate = serviceAreaInfo.StartDate;
                serviceArea.District = serviceAreaInfo.District;
            }
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userInfo"></param>
        public static void UpdateSeedUserInfo(this DbAppContext context, User userInfo)
        {
            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(context, SystemId);

            User user = context.GetUserByGuid(userInfo.Guid);

            if (user == null)
            {
                userInfo.AppCreateUserid = SystemId;
                userInfo.AppCreateTimestamp = DateTime.UtcNow;
                userInfo.AppLastUpdateUserid = SystemId;
                userInfo.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.Users.Add(userInfo);
            }
            else
            {
                user.Active = userInfo.Active;
                user.Email = userInfo.Email;
                user.GivenName = userInfo.GivenName;
                user.Initials = userInfo.Initials;
                user.SmAuthorizationDirectory = userInfo.SmAuthorizationDirectory;
                user.SmUserId = userInfo.SmUserId;
                user.Surname = userInfo.Surname;
                user.District = userInfo.District;

                // Sync Roles
                if (user.UserRoles != null)
                {
                    foreach (UserRole item in user.UserRoles)
                    {
                        context.Entry(item).State = EntityState.Deleted;
                    }

                    foreach (UserRole item in userInfo.UserRoles)
                    {
                        item.AppCreateUserid = SystemId;
                        item.AppCreateTimestamp = DateTime.UtcNow;
                        item.AppLastUpdateUserid = SystemId;
                        item.AppLastUpdateTimestamp = DateTime.UtcNow;

                        user.UserRoles.Add(item);
                    }
                }                
            }
        }
    }
}
