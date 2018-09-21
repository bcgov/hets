using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.AspNetCore.Hosting;

namespace HetsApi.Helpers
{
    public static class UserAccountHelper
    {
        private const string ConstDevBusinessTokenKey = "BUS-USER";
        private const string ConstSiteMinderBusinessGuidKey = "smgov_businessguid";

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
        /// Check if this is a Business User
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsBusiness(HttpContext httpContext)
        {
            return httpContext.User.Claims
                .Any(claim => claim.Type == ClaimTypes.Actor && 
                              claim.Value == "BusinessUser");
        }

        /// <summary>
        /// Get the Business Guid from the Http Headers
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="hostingEnv"></param>
        /// <returns></returns>
        public static string GetBusinessGuid(HttpContext httpContext, IHostingEnvironment hostingEnv)
        {
            string guid = "";

            // check if we have a dev token first
            if (hostingEnv.IsDevelopment())
            {
                string temp = httpContext.Request.Cookies[ConstDevBusinessTokenKey];

                if (!string.IsNullOrEmpty(temp) &&
                    temp.Contains(','))
                {
                    var credential = temp.Split(',');
                    guid = credential[1];
                }
            }
            else
            {
                guid = httpContext.Request.Headers[ConstSiteMinderBusinessGuidKey];
            }

            return guid;
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
        public static User GetUser(DbAppContext context, HttpContext httpContext)
        {
            User user = new User();

            // is this a business?
            bool isBusinessUser = IsBusiness(httpContext);
            string userId = GetUserId(httpContext);

            if (!isBusinessUser)
            {                
                HetUser tmpUser = context.HetUser.AsNoTracking()
                    .FirstOrDefault(x => x.SmUserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));

                if (tmpUser != null)
                {
                    user.Id = tmpUser.UserId;
                    user.SmUserId = tmpUser.SmUserId;
                    user.GivenName = tmpUser.GivenName;
                    user.Surname = tmpUser.Surname;
                    user.DisplayName = tmpUser.GivenName + " " + tmpUser.Surname;
                    user.UserGuid = tmpUser.Guid;
                    user.BusinessUser = false;
                    user.SmAuthorizationDirectory = tmpUser.SmAuthorizationDirectory;
                }
            }
            else
            {
                HetBusinessUser tmpUser = context.HetBusinessUser.AsNoTracking()
                    .FirstOrDefault(x => x.BceidUserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));

                if (tmpUser != null)
                {
                    // get business
                    HetBusiness business = context.HetBusiness.AsNoTracking()
                        .First(x => x.BusinessId == tmpUser.BusinessId);

                    user.Id = tmpUser.BusinessUserId;
                    user.SmUserId = tmpUser.BceidUserId;
                    user.GivenName = tmpUser.BceidFirstName;
                    user.Surname = tmpUser.BceidLastName;
                    user.DisplayName = tmpUser.BceidDisplayName;
                    user.UserGuid = tmpUser.BceidGuid;
                    user.BusinessUser = true;
                    user.BusinessId = tmpUser.BusinessId;
                    user.BusinessGuid = business.BceidBusinessGuid;
                    user.SmAuthorizationDirectory = "BCeID";
                }
            }

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

            // setup the business
            if (business == null)
            {
                business = new HetBusiness
                {
                    BceidBusinessGuid = businessGuid,
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = userId,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = userId,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                // to do - add web service call to retrieve remaining business data
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
                    BceidGuid = guid,
                    BusinessId = business.BusinessId,
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
                business.HetBusinessUser.Add(user);

                // save record
                context.HetBusiness.Add(business);
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
