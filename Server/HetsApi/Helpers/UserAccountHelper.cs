using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore.Storage;

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
        /// Get user's district id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int? GetUsersDistrictId(DbAppContext context)
        {
            string userId = context.SmUserId;
            int? districtId = context.HetUsers.FirstOrDefault(x => x.SmUserId.ToUpper() == userId)?.DistrictId;
            return districtId;
        }

        /// <summary>
        /// Get user record using the user id from the http context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static CurrentUserDto GetUser(DbAppContext context, HttpContext httpContext)
        {
            CurrentUserDto user = new CurrentUserDto();

            // is this a business?
            bool isBusinessUser = IsBusiness(httpContext);
            string userId = context.SmUserId;

            if (!isBusinessUser)
            {
                HetUser tmpUser = context.HetUsers.AsNoTracking()
                    .FirstOrDefault(x => x.SmUserId.ToUpper() == userId);

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
                HetBusinessUser tmpUser = context.HetBusinessUsers.AsNoTracking()
                    .FirstOrDefault(x => x.BceidUserId.ToUpper() == userId);

                if (tmpUser != null)
                {
                    // get business
                    HetBusiness business = context.HetBusinesses.AsNoTracking()
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
        /// <param name="username"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static HetUser GetUser(DbAppContext context, string username, string guid = null)
        {
            HetUser user = null;

            if (!string.IsNullOrEmpty(guid))
            {
                user = GetUserByGuid(guid, context);
            }

            if (user == null)
            {
                user = GetUserBySmUserId(username, context);
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
                    context.Database.ExecuteSqlRaw(@"LOCK TABLE ""HET_USER"" IN EXCLUSIVE MODE;");

                    HetUser updUser = context.HetUsers.First(x => x.UserId == updUserId);

                    updUser.Guid = guid;
                    updUser.AppLastUpdateUserDirectory = user.SmAuthorizationDirectory;
                    updUser.AppLastUpdateUserGuid = guid;
                    updUser.AppLastUpdateUserid = username;
                    updUser.AppLastUpdateTimestamp = DateTime.UtcNow;

                    context.HetUsers.Update(updUser);

                    // update user record
                    context.SaveChanges();

                    // commit
                    transaction.Commit();
                }

                // update the user object for the current session
                user.Guid = guid;
                user.AppLastUpdateUserDirectory = user.SmAuthorizationDirectory;
                user.AppLastUpdateUserGuid = guid;
                user.AppLastUpdateUserid = username;
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
            HetUser user = context.HetUsers.AsNoTracking()
                .Where(x => x.Guid != null &&
                            x.Guid.Equals(guid))
                .Include(u => u.HetUserRoles)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermissions)
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
            HetUser user = context.HetUsers.AsNoTracking()
                .Where(x => x.SmUserId != null &&
                            x.SmUserId.ToUpper() == smUserId)
                .Include(u => u.HetUserRoles)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermissions)
                            .ThenInclude(p => p.Permission)
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// Get business user record
        /// </summary>
        /// <param name="context"></param>
        /// <param name="account"></param>
        /// <param name="username"></param>
        /// <param name="bizGuid"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static HetBusinessUser GetBusinessUser(DbAppContext context, string username, string bizGuid, string bizName, string email, string guid)
        {
            // find the business
            HetBusiness business = context.HetBusinesses.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == bizGuid.ToLower().Trim());

            // setup the business
            if (business == null)
            {
                business = new HetBusiness
                {
                    BceidBusinessGuid = bizGuid.ToLower().Trim(),
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = username,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = username,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                // get additional business data
                string legalName = bizName;

                if (!string.IsNullOrEmpty(legalName))
                {
                    business.BceidLegalName = legalName;
                }

                // save record
                context.HetBusinesses.Add(business);
                context.SaveChanges();
            }
            else
            {
                // update business information
                string legalName = bizName;

                if (!string.IsNullOrEmpty(legalName))
                {
                    business.BceidLegalName = legalName;
                }

                business.AppLastUpdateUserDirectory = "BCeID";
                business.AppLastUpdateUserGuid = guid;
                business.AppLastUpdateUserid = username;
                business.AppLastUpdateTimestamp = DateTime.UtcNow;

                context.SaveChanges();
            }

            // ok - now find the user
            HetBusinessUser user = context.HetBusinessUsers
                .FirstOrDefault(x => x.BceidGuid.ToLower() == guid.ToLower());

            if (user == null)
            {
                // auto register the user
                user = new HetBusinessUser
                {
                    BceidUserId = username,
                    BceidGuid = guid,
                    BusinessId = business.BusinessId,
                    AppCreateUserDirectory = "BCeID",
                    AppCreateUserGuid = guid,
                    AppCreateUserid = username,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = username,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

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
                    AppCreateUserid = username,
                    AppCreateTimestamp = DateTime.UtcNow,
                    AppLastUpdateUserDirectory = "BCeID",
                    AppLastUpdateUserGuid = guid,
                    AppLastUpdateUserid = username,
                    AppLastUpdateTimestamp = DateTime.UtcNow
                };

                user.HetBusinessUserRoles.Add(userRole);

                // save record
                context.HetBusinessUsers.Add(user);
                context.SaveChanges();
            }
            else
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user.BceidEmail = email;
                }

                context.SaveChanges();
            }

            // get complete user record (with roles) and return
            user = context.HetBusinessUsers.AsNoTracking()
                .Where(x => x.BusinessId == business.BusinessId &&
                            x.BceidUserId.ToUpper() == username)
                .Include(u => u.HetBusinessUserRoles)
                    .ThenInclude(r => r.Role)
                        .ThenInclude(rp => rp.HetRolePermissions)
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
