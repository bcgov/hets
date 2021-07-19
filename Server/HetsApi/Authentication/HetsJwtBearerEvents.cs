using HetsApi.Extensions;
using HetsApi.Helpers;
using HetsData.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using HetsData;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HetsBceid;

namespace HetsApi.Authentication
{
    public class HetsJwtBearerEvents : JwtBearerEvents
    {
        private DbAppContext _dbContext;
        private IBceidApi _bceid;
        private ILogger<HetsJwtBearerEvents> _logger;

        public HetsJwtBearerEvents(DbAppContext dbContext, IBceidApi bceid,
            ILogger<HetsJwtBearerEvents> logger) : base()
        {
            _dbContext = dbContext;
            _bceid = bceid;
            _logger = logger;
        }

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var problem = new ValidationProblemDetails()
            {
                Type = "https://Hets.bc.gov.ca/exception",
                Title = "Access denied",
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Authentication failed.",
                Instance = context.Request.Path
            };

            problem.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            await context.Response.WriteJsonAsync(problem, "application/problem+json");
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var userSettings = new UserSettings();

            var (username, userGuid, directory) = context.Principal.GetUserInfo();

            if (directory == Constants.BCEIDBIZ)
            {
                await AuthenticateBusinessUser(context, username, userGuid, userSettings);
            }
            else if (directory == Constants.IDIR)
            {
                var idirUserResult = AuthenticateIdirUser(context, username, userGuid, userSettings);
                if (!idirUserResult.success)
                {
                    context.Fail(idirUserResult.message);
                    return;
                }
            }
            else
            {
                context.Fail(Constants.InvalidDirectory);
                return;
            }

            var addClaimResult = AddClaimsFromUserInfo(context.Principal, userSettings);
            if (!addClaimResult.success)
            {
                context.Fail(addClaimResult.message);
                return;
            }

            _dbContext.SmUserId = username;
            _dbContext.DirectoryName = directory;
            _dbContext.SmUserGuid = userGuid;
            _dbContext.SmBusinessGuid = userSettings.SiteMinderBusinessGuid;
        }

        private (bool success, string message) AuthenticateIdirUser(TokenValidatedContext context, string username, string userGuid, UserSettings userSettings)
        {
            try
            {
                userSettings.UserId = username;
                userSettings.HetsUser = UserAccountHelper.GetUser(_dbContext, username, userGuid);

                if (userSettings.HetsUser == null)
                {
                    return (false, $"{Constants.MissingDbUserIdError} {username}");
                }

                if (!userSettings.HetsUser.Active)
                {
                    return (false, $"{Constants.InactiveDbUserIdError} {username}");
                }

                // **************************************************
                // update the user back to their default district
                // **************************************************
                string tempSwitch = context.Request.Cookies["HETSDistrict"];

                if (string.IsNullOrEmpty(tempSwitch))
                {
                    HetUserDistrict userDistrict = _dbContext.HetUserDistricts.AsNoTracking()
                        .Include(x => x.User)
                        .Include(x => x.District)
                        .FirstOrDefault(x => x.User.UserId == userSettings.HetsUser.UserId &&
                                             x.IsPrimary);

                    // if we don't find a primary - look for the first one in the list
                    if (userDistrict == null)
                    {
                        userDistrict = _dbContext.HetUserDistricts.AsNoTracking()
                            .Include(x => x.User)
                            .Include(x => x.District)
                            .FirstOrDefault(x => x.User.UserId == userSettings.HetsUser.UserId);
                    }

                    // update the current district for the user
                    if (userDistrict != null &&
                        userSettings.HetsUser.DistrictId != userDistrict.District.DistrictId)
                    {
                        int districtId = userDistrict.District.DistrictId;
                        int updUserId = userSettings.HetsUser.UserId;

                        _logger.LogInformation("Resetting users district back to primary ({0})", userSettings.HetsUser.SmUserId);

                        userSettings.HetsUser.DistrictId = districtId;

                        using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
                        {
                            // lock the table during this transaction
                            _dbContext.Database.ExecuteSqlRaw(@"LOCK TABLE ""HET_USER"" IN EXCLUSIVE MODE;");

                            HetUser user = _dbContext.HetUsers.First(x => x.UserId == updUserId);
                            user.DistrictId = districtId;
                            _dbContext.SaveChanges();
                            transaction.Commit();
                        }
                    }
                }

                userSettings.SiteMinderGuid = userGuid;
                userSettings.UserAuthenticated = true;
                userSettings.BusinessUser = false;

                return (true, "");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        private async Task AuthenticateBusinessUser(TokenValidatedContext context, string username, string userGuid, UserSettings userSettings)
        {
            var guid = new Guid(userGuid);
            var userType = "BUSINESS";

            var (error, account) = await _bceid.GetBceidAccountCachedAsync(guid, username, userType, guid, userType);

            if (error.IsNotEmpty())
            {
                throw new Exception($"Unable to retrieve User [{guid.ToString("N")} ({userType})] from BCeID Service.");
            }

            var businessGuid = ((Guid)account.BusinessGuid).ToString("N");

            userSettings.HetsBusinessUser = UserAccountHelper.GetBusinessUser(_dbContext, account, username, businessGuid, userGuid);

            userSettings.SiteMinderBusinessGuid = ((Guid)account.BusinessGuid).ToString("N");
            userSettings.SiteMinderGuid = userGuid;
            userSettings.UserAuthenticated = true;
            userSettings.BusinessUser = true;
        }

        private (bool success, string message) AddClaimsFromUserInfo(ClaimsPrincipal principal, UserSettings userSettings)
        {
            if (userSettings.BusinessUser)
            {
                principal.AddIdentity(userSettings.HetsBusinessUser.ToClaimsIdentity());

                if (!principal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin))
                {
                    return (false, $"{Constants.MissingDbUserIdError} {userSettings.UserId}");
                }
            }
            else
            {
                principal.AddIdentity(userSettings.HetsUser.ToClaimsIdentity());

                if (!principal.HasClaim(HetUser.PermissionClaim, HetPermission.Login) &&
                    !principal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin))
                {
                    return (false, $"{Constants.MissingDbUserIdError} {userSettings.UserId}");
                }
            }

            return (true, "");
        }
    }
}
