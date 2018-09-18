using System;
using System.Linq;
using HetsData.Helpers;
using Microsoft.AspNetCore.Http;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;

namespace HetsApi.Helpers
{
    public static class UserAccountHelper
    {
        /// <summary>
        /// Get user id from http context
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetUserId(HttpContext httpContext)
        {
            string userId = httpContext.User.Identity.Name;
            return userId;
        }        

        /// <summary>
        /// Get user's district id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static int? GetUsersDistrictId(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            int? districtId = context.HetUser.FirstOrDefault(x => x.SmUserId == userId)?.DistrictId;
            return districtId;
        }

        /// <summary>
        /// Get user record using the user id from the http context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static HetUser GetUser(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            HetUser user = context.HetUser.FirstOrDefault(x => x.SmUserId == userId);
            return user;
        }

        /// <summary>
        /// Get user record
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static HetUser GetUser(DbAppContext context, string userId, string guid = null)
        {
            HetUser user = null;

            if (!string.IsNullOrEmpty(guid))
            {
                user = GetUserByGuid(guid, context);
            }

            if (user == null)
            {
                user = GetUserBySmUserId(userId, context);
            }

            if (user == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(guid) && string.IsNullOrEmpty(user.Guid))
            {
                // self register (write the users Guid to the db)
                user.Guid = guid;
                user.AppLastUpdateUserDirectory = user.SmAuthorizationDirectory;
                user.AppLastUpdateUserGuid = guid;
                user.AppLastUpdateUserid = userId;
                user.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }
            else if (!string.IsNullOrEmpty(user.Guid) &&
                     !string.IsNullOrEmpty(guid) &&
                     !user.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase))
            {
                // invalid account - guid doesn't match user credential
                return null;
            }

            return user;
        }

        /// <summary>
        /// Returns a user based on the guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetUser GetUserByGuid(string guid, DbAppContext context)
        {
            HetUser user = context.HetUser
                .Where(x => x.Guid != null &&
                            x.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase))
                .Include(u => u.HetUserRole)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermission)
                            .ThenInclude(p => p.Permission)
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Returns a user based on the account name
        /// </summary>
        /// <param name="smUserId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetUser GetUserBySmUserId(string smUserId, DbAppContext context)
        {
            HetUser user = context.HetUser
                .Where(x => x.SmUserId != null &&
                            x.SmUserId.Equals(smUserId, StringComparison.OrdinalIgnoreCase))
                .Include(u => u.HetUserRole)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermission)
                            .ThenInclude(p => p.Permission)
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get business user record
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="businessGuid"></param>
        /// <param name="guid"></param>        
        /// <returns></returns>
        public static HetBusinessUser GetBusinessUser(DbAppContext context, string userId, string businessGuid, string guid = null)
        {
            // find the business
            HetBusiness business = context.HetBusiness.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid == businessGuid);

            if (business == null)
            {
                return null;
            }

            // ok - now find the user
            HetBusinessUser user = context.HetBusinessUser
                .FirstOrDefault(x => x.BusinessId == business.BusinessId &&
                                     x.BceidUserId == userId);

            if (user == null)
            {
                // auto register the user
                user = new HetBusinessUser
                {
                    BceidUserId = userId,
                    BusinessId = business.BusinessId,
                    BceidGuid = guid,
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = userId,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = userId,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                // to do - add web service call to retrieve remaining user data

                // add the "Business Logon" role
                HetBusinessUserRole userRole = new HetBusinessUserRole
                {
                    RoleId = StatusHelper.GetRoleId("Business BCeID User", context),
                    EffectiveDate = DateTime.UtcNow.AddMinutes(-10),
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = userId,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = userId,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                user.HetBusinessUserRole.Add(userRole);

                // save record
                context.HetBusinessUser.Add(user);
                context.SaveChanges();
            }

            // get complete user record (with roles) and return
            user = context.HetBusinessUser
                .Where(x => x.BusinessId == business.BusinessId &&
                                     x.BceidUserId == userId)
                .Include(u => u.HetBusinessUserRole)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermission)
                            .ThenInclude(p => p.Permission)
                .FirstOrDefault();

            return user;
        }
    }
}
