using HetsApi.Extensions;
using HetsApi.Helpers;
using HetsData.Model;
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

namespace HetsApi.Authentication
{
    public class HetsJwtBearerEvents : JwtBearerEvents
    {
        private DbAppContext _dbContext;
        private ILogger<HetsJwtBearerEvents> _logger;

        public HetsJwtBearerEvents(IWebHostEnvironment env,
            DbAppContext dbContext,

            ILogger<HetsJwtBearerEvents> logger) : base()
        {
            _dbContext = dbContext;
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

            if (directory != "IDIR")
            {
                context.Fail($"BCeID({directory}) user is not suppored yet!");
                return;
            }
            else
            {
                var idirUserResult = LoadIdirUser(context, username, userGuid, userSettings);
                if (!idirUserResult.success)
                {
                    context.Fail(idirUserResult.message);
                    return;
                }
            }

            var addClaimResult = AddClaimsFromUserInfo(userSettings);
            if (!addClaimResult.success)
            {
                context.Fail(addClaimResult.message);
                return;
            }

            await Task.CompletedTask;
        }

        private (bool success, string message) LoadIdirUser(TokenValidatedContext context, string username, string userGuid, UserSettings userSettings)
        {
            try
            {
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
                    HetUserDistrict userDistrict = _dbContext.HetUserDistrict.AsNoTracking()
                        .Include(x => x.User)
                        .Include(x => x.District)
                        .FirstOrDefault(x => x.User.UserId == userSettings.HetsUser.UserId &&
                                             x.IsPrimary);

                    // if we don't find a primary - look for the first one in the list
                    if (userDistrict == null)
                    {
                        userDistrict = _dbContext.HetUserDistrict.AsNoTracking()
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

                            HetUser user = _dbContext.HetUser.First(x => x.UserId == updUserId);
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

        private (bool success, string message) AddClaimsFromUserInfo(UserSettings userSettings)
        {
            if (userSettings.BusinessUser)
            {
                var userPrincipal = userSettings.HetsBusinessUser.ToClaimsPrincipal(JwtBearerDefaults.AuthenticationScheme);

                if (!userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin))
                {
                    return (false, $"{Constants.MissingDbUserIdError} {userSettings.UserId}");
                }
            }
            else
            {
                var userPrincipal = userSettings.HetsUser.ToClaimsPrincipal(JwtBearerDefaults.AuthenticationScheme);

                if (!userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.Login) &&
                    !userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin))
                {
                    return (false, $"{Constants.MissingDbUserIdError} {userSettings.UserId}");
                }
            }

            return (true, "");
        }
    }
}
