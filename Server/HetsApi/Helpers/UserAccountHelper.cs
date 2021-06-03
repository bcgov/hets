using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace HetsApi.Helpers
{
    public static class UserAccountHelper
    {
        private const string ConstDevBusinessTokenKey = "BUS-USER";
        private const string ConstSiteMinderBusinessGuidKey = "smgov_businessguid";
        private const string ConstSiteMinderUserDisplayName = "smgov_userdisplayname";
        private const string ConstSiteMinderEmail = "smgov_email";
        private const string ConstSiteMinderBusinessLegalName = "smgov_businesslegalname";
        private const string ConstSiteMinderBusinessNumber = "smgov_businessnumber";

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
        public static string GetBusinessGuid(HttpContext httpContext, IWebHostEnvironment hostingEnv)
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

            // get the guid from the SM headers
            if (string.IsNullOrEmpty(guid))
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
                    .FirstOrDefault(x => x.SmUserId.ToLower().Equals(userId.ToLower()));

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
                    user.AgreementCity = tmpUser.AgreementCity;
                }
            }
            else
            {
                HetBusinessUser tmpUser = context.HetBusinessUser.AsNoTracking()
                    .FirstOrDefault(x => x.BceidUserId.ToLower().Equals(userId.ToLower()));

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
                int updUserId = user.UserId;

                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    // lock the table during this transaction
                    context.Database.ExecuteSqlCommand(@"LOCK TABLE ""HET_USER"" IN EXCLUSIVE MODE;");

                    HetUser updUser = context.HetUser.First(x => x.UserId == updUserId);

                    updUser.Guid = guid;
                    updUser.AppLastUpdateUserDirectory = user.SmAuthorizationDirectory;
                    updUser.AppLastUpdateUserGuid = guid;
                    updUser.AppLastUpdateUserid = userId;
                    updUser.AppLastUpdateTimestamp = DateTime.UtcNow;

                    context.HetUser.Update(updUser);

                    // update user record
                    context.SaveChanges();

                    // commit
                    transaction.Commit();
                }

                // update the user object for the current session
                user.Guid = guid;
                user.AppLastUpdateUserDirectory = user.SmAuthorizationDirectory;
                user.AppLastUpdateUserGuid = guid;
                user.AppLastUpdateUserid = userId;
                user.AppLastUpdateTimestamp = DateTime.UtcNow;
            }
            else if (!string.IsNullOrEmpty(user.Guid) &&
                     !string.IsNullOrEmpty(guid) &&
                     !user.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase))
            {
                // invalid account - guid doesn't match user credential
                return null;
            }

            // detach user and return
            context.Entry(user).State = EntityState.Detached;
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
            HetUser user = context.HetUser.AsNoTracking()
                .Where(x => x.Guid != null &&
                            x.Guid.Equals(guid))
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
            HetUser user = context.HetUser.AsNoTracking()
                .Where(x => x.SmUserId != null &&
                            x.SmUserId.ToLower().Equals(smUserId.ToLower()))
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
        /// <param name="httpContext"></param>
        /// <param name="userId"></param>
        /// <param name="businessGuid"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static HetBusinessUser GetBusinessUser(DbAppContext context, HttpContext httpContext, string userId, string businessGuid, string guid = null)
        {
            // find the business
            HetBusiness business = context.HetBusiness.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            // setup the business
            if (business == null)
            {
                business = new HetBusiness
                {
                    BceidBusinessGuid = businessGuid.ToLower().Trim(),
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = userId,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = userId,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                // get additional business data
                string legalName = httpContext.Request.Headers[ConstSiteMinderBusinessLegalName];
                string businessNumber = httpContext.Request.Headers[ConstSiteMinderBusinessNumber];

                if (!string.IsNullOrEmpty(legalName))
                {
                    business.BceidLegalName = legalName;
                }

                if (!string.IsNullOrEmpty(businessNumber))
                {
                    business.BceidBusinessNumber = businessNumber;
                }

                // save record
                context.HetBusiness.Add(business);
                context.SaveChanges();
            }
            else
            {
                // update business information
                string legalName = httpContext.Request.Headers[ConstSiteMinderBusinessLegalName];
                string businessNumber = httpContext.Request.Headers[ConstSiteMinderBusinessNumber];

                if (!string.IsNullOrEmpty(legalName))
                {
                    business.BceidLegalName = legalName;
                }

                if (!string.IsNullOrEmpty(businessNumber))
                {
                    business.BceidBusinessNumber = businessNumber;
                }

                business.AppLastUpdateUserDirectory = "BCeID";
                business.AppLastUpdateUserGuid = guid;
                business.AppLastUpdateUserid = userId;
                business.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.SaveChanges();
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

                // get additional user data
                string displayName = httpContext.Request.Headers[ConstSiteMinderUserDisplayName];
                string email = httpContext.Request.Headers[ConstSiteMinderEmail];

                if (!string.IsNullOrEmpty(displayName))
                {
                    user.BceidDisplayName = displayName;
                }

                if (!string.IsNullOrEmpty(email))
                {
                    user.BceidEmail = email;
                }

                // add the "Business Logon" role
                HetBusinessUserRole userRole = new HetBusinessUserRole
                {
                    RoleId = StatusHelper.GetRoleId("Business BCeID", context),
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
            else
            {
                // update the user
                string displayName = httpContext.Request.Headers[ConstSiteMinderUserDisplayName];
                string email = httpContext.Request.Headers[ConstSiteMinderEmail];

                if (!string.IsNullOrEmpty(displayName))
                {
                    user.BceidDisplayName = displayName;
                }

                if (!string.IsNullOrEmpty(email))
                {
                    user.BceidEmail = email;
                }

                context.SaveChanges();
            }

            // get complete user record (with roles) and return
            user = context.HetBusinessUser.AsNoTracking()
                .Where(x => x.BusinessId == business.BusinessId &&
                            x.BceidUserId == userId)
                .Include(u => u.HetBusinessUserRole)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermission)
                            .ThenInclude(p => p.Permission)
                .FirstOrDefault();

            // detach user and return
            if (user != null)
            {
                context.Entry(user).State = EntityState.Detached;
            }

            return user;
        }
    }
}
