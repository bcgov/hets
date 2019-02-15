using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HetsApi.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using HetsData.Model;

namespace HetsApi.Authentication
{    
    #region SiteMinder Authentication Options

    /// <summary>
    /// Options required for setting up SiteMinder Authentication
    /// </summary>
    public class SiteMinderAuthOptions : AuthenticationSchemeOptions
    {
        private const string ConstDevAuthenticationTokenKey = "DEV-USER";
        private const string ConstDevBusinessTokenKey = "BUS-USER";
        private const string ConstDevDefaultUserId = "TMcTesterson";       
        private const string ConstSiteMinderUserGuidKey = "smgov_userguid";
        private const string ConstSiteMinderUniversalIdKey = "sm_universalid";
        private const string ConstSiteMinderUserNameKey = "sm_user";
        private const string ConstSiteMinderUserDisplayNameKey = "smgov_userdisplayname";

        // Business BCeID Headers
        private const string ConstSiteMinderBusinessGuidKey = "smgov_businessguid";

        private const string ConstMissingSiteMinderUserIdError = "Missing SiteMinder UserId";
        private const string ConstMissingSiteMinderGuidError = "Missing SiteMinder Guid";
        private const string ConstMissingDbUserIdError = "Could not find UserId in the HETS database";
        private const string ConstInactiveDbUserIdError = "HETS database UserId is inactive";
        private const string ConstInvalidPermissions = "HETS UserId does not have valid permissions";
        private const string ConstMissingBusinessIdError = "Invalid Business Record";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SiteMinderAuthOptions()
        {
            SiteMinderUserGuidKey = ConstSiteMinderUserGuidKey;
            SiteMinderUniversalIdKey = ConstSiteMinderUniversalIdKey;
            SiteMinderUserNameKey = ConstSiteMinderUserNameKey;
            SiteMinderUserDisplayNameKey = ConstSiteMinderUserDisplayNameKey;

            // Business BCeID Keys
            SiteMinderBusinessGuidKey = ConstSiteMinderBusinessGuidKey;

            MissingSiteMinderUserIdError = ConstMissingSiteMinderUserIdError;
            MissingSiteMinderGuidError = ConstMissingSiteMinderGuidError;
            MissingDbUserIdError = ConstMissingDbUserIdError;
            InactiveDbUserIdError = ConstInactiveDbUserIdError;
            MissingBusinessIdError = ConstMissingBusinessIdError;

            InvalidPermissions = ConstInvalidPermissions;
            DevAuthenticationTokenKey = ConstDevAuthenticationTokenKey;
            DevBusinessTokenKey = ConstDevBusinessTokenKey;
            DevDefaultUserId = ConstDevDefaultUserId;
        }        

        /// <summary>
        /// Default Scheme Name
        /// </summary>
        public static string AuthenticationSchemeName => "site-minder-auth";

        /// <summary>
        /// SiteMinder Authentication Scheme Name
        /// </summary>
        public string Scheme => AuthenticationSchemeName;

        /// <summary>
        /// User GUID
        /// </summary>
        public string SiteMinderUserGuidKey { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string SiteMinderUniversalIdKey { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string SiteMinderUserNameKey { get; set; }

        /// <summary>
        /// User's Display Name
        /// </summary>
        public string SiteMinderUserDisplayNameKey { get; set; }

        /// <summary>
        /// Business Guid
        /// </summary>
        public string SiteMinderBusinessGuidKey { get; set; }

        /// <summary>
        /// Missing SiteMinder UserId Error
        /// </summary>
        public string MissingSiteMinderUserIdError { get; set; }

        /// <summary>
        /// Missing SiteMinder Guid Error
        /// </summary>
        public string MissingSiteMinderGuidError { get; set; }

        /// <summary>
        /// Missing Database UserId Error
        /// </summary>
        public string MissingDbUserIdError { get; set; }

        /// <summary>
        /// Missing Business Id Error
        /// </summary>
        public string MissingBusinessIdError { get; set; }

        /// <summary>
        /// Inactive Database UserId Error
        /// </summary>
        public string InactiveDbUserIdError { get; set; }

        /// <summary>
        /// User does not have active / valid permissions
        /// </summary>
        public string InvalidPermissions { get; set; }

        /// <summary>
        /// Development Environment Authentication Key
        /// </summary>
        public string DevAuthenticationTokenKey { get; set; }

        /// <summary>
        /// Development Environment Authentication Key - Business User
        /// </summary>
        public string DevBusinessTokenKey { get; set; }

        /// <summary>
        /// Development Environment default UserId
        /// </summary>
        public string DevDefaultUserId { get; set; }
    }

    #endregion    

    /// <summary>
    /// Setup SiteMinder Authentication Handler
    /// </summary>
    public static class SiteMinderAuthenticationExtensions
    {
        /// <summary>
        /// Add Authentication Handler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSiteMinderAuth(this AuthenticationBuilder builder, Action<SiteMinderAuthOptions> configureOptions)
        {
            return builder.AddScheme<SiteMinderAuthOptions, SiteMinderAuthenticationHandler>(SiteMinderAuthOptions.AuthenticationSchemeName, configureOptions);
        }
    }

    /// <summary>
    /// SiteMinder Authentication Handler
    /// </summary>
    public class SiteMinderAuthenticationHandler : AuthenticationHandler<SiteMinderAuthOptions>
    {
        private readonly ILogger _logger;        

        /// <summary>
        /// SiteMinder Authentication Constructor
        /// </summary>
        /// <param name="configureOptions"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public SiteMinderAuthenticationHandler(IOptionsMonitor<SiteMinderAuthOptions> configureOptions, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock) : base(configureOptions, loggerFactory, encoder, clock)
        {
            _logger = loggerFactory.CreateLogger(typeof(SiteMinderAuthenticationHandler));                    
        }

        /// <summary>
        /// Process Authentication Request
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // get SiteMinder headers
            _logger.LogDebug("Parsing the HTTP headers for SiteMinder authentication credential");
                  
            SiteMinderAuthOptions options = new SiteMinderAuthOptions();

            try
            {
                HttpContext context = Request.HttpContext;
                DbAppContext dbAppContext = (DbAppContext)context.RequestServices.GetService(typeof(DbAppContext));
                IHostingEnvironment hostingEnv = (IHostingEnvironment)context.RequestServices.GetService(typeof(IHostingEnvironment));
                
                UserSettings userSettings = new UserSettings();
                string userId = "";
                string siteMinderGuid = "";
                string businessGuid = "";

                string url = context.Request.GetDisplayUrl().ToLower();
                _logger.LogWarning("Timestamp: {0:dd-MM-yyyy HH:mm:ss.FFFF} | Url: {1}", DateTime.Now, url);

                // ********************************************************
                // if this is an Error or Authentication API - Ignore
                // ********************************************************                     
                if (url.Contains("/authentication/dev") ||
                    url.Contains("/error") ||
                    url.Contains("/hangfire") ||
                    url.Contains("/swagger"))                    
                {
                    _logger.LogInformation("Bypassing authentication process ({0})", url);
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                // **************************************************
                // check if we have a Dev Environment Cookie
                // **************************************************
                string tempToken = context.Request.Cookies[options.DevAuthenticationTokenKey];                

                if (hostingEnv.IsDevelopment() && !string.IsNullOrEmpty(tempToken))
                {
                    _logger.LogInformation("Dev Authentication token found ({0})", tempToken);
                    userId = tempToken;                    
                }
                else if ((context.Connection.RemoteIpAddress.ToString().StartsWith("::1") ||
                          context.Connection.RemoteIpAddress.ToString().StartsWith("127.0.0.1")) &&
                         url.StartsWith("http://localhost:8080") &&
                         !string.IsNullOrEmpty(tempToken))
                {
                    _logger.LogInformation("Local server access");
                    _logger.LogInformation("Dev Authentication token found ({0})", tempToken);
                    userId = tempToken;
                }

                // **************************************************
                // check if we have a Dev Environment Cookie
                // * Business User
                // **************************************************
                if (hostingEnv.IsDevelopment())
                {
                    string temp = context.Request.Cookies[options.DevBusinessTokenKey];

                    if (!string.IsNullOrEmpty(temp) &&
                        temp.Contains(','))
                    {                                                
                        _logger.LogInformation("Dev (Business) Authentication token found ({0})", temp);

                        var credential = temp.Split(',');
                        userId = credential[0];
                        businessGuid = credential[1];                                                
                    }
                }

                // **************************************************
                // authenticate based on SiteMinder Headers
                // **************************************************                
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("Parsing the HTTP headers for SiteMinder authentication credential");

                    userId = context.Request.Headers[options.SiteMinderUserNameKey];

                    if (string.IsNullOrEmpty(userId))
                    {
                        userId = context.Request.Headers[options.SiteMinderUniversalIdKey];
                    }

                    // if we still don't have an id - then we cannot authenticate this user
                    if (string.IsNullOrEmpty(userId))
                    {
                        return Task.FromResult(AuthenticateResult.Fail(options.MissingSiteMinderUserIdError));
                    }

                    // check if the AD name is in the credential - and remove
                    int start = userId.IndexOf("\\", StringComparison.Ordinal);

                    if (start > -1)
                    {
                        userId = userId.Substring(start + 1);
                    }

                    userId = userId.ToLower();

                    siteMinderGuid = context.Request.Headers[options.SiteMinderUserGuidKey];

                    // check if there is a business guid (Business BCeID)
                    businessGuid = context.Request.Headers[options.SiteMinderBusinessGuidKey];

                    // **************************************************
                    // validate credentials
                    // **************************************************                    
                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogError(options.MissingSiteMinderUserIdError);
                        return Task.FromResult(AuthenticateResult.Fail(options.MissingSiteMinderGuidError));
                    }

                    if (string.IsNullOrEmpty(siteMinderGuid))
                    {
                        _logger.LogError(options.MissingSiteMinderGuidError);
                        return Task.FromResult(AuthenticateResult.Fail(options.MissingSiteMinderGuidError));
                    }
                }

                // **************************************************
                // validate credential against database              
                // **************************************************
                _logger.LogInformation("Validating credential against the HETS db");
                _logger.LogInformation("User Credential: {0}", userId);
                _logger.LogInformation("SiteMinder Guid: {0}", siteMinderGuid);

                if (!string.IsNullOrEmpty(businessGuid))
                {
                    _logger.LogInformation("SiteMinder Business Guid: {0}", businessGuid);
                    
                    userSettings.HetsBusinessUser = UserAccountHelper.GetBusinessUser(dbAppContext, context, userId, businessGuid, siteMinderGuid);

                    if (userSettings.HetsBusinessUser == null)
                    {
                        return Task.FromResult(AuthenticateResult.Fail(options.MissingDbUserIdError));
                    }

                    if (userSettings.HetsBusinessUser != null)
                    {
                        userSettings.SiteMinderBusinessGuid = businessGuid;
                        userSettings.SiteMinderGuid = siteMinderGuid;
                        userSettings.UserAuthenticated = true;
                        userSettings.BusinessUser = true;
                    }                    
                }
                else
                {
                    userSettings.HetsUser = UserAccountHelper.GetUser(dbAppContext, userId, siteMinderGuid);

                    if (userSettings.HetsUser == null)
                    {
                        _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                        return Task.FromResult(AuthenticateResult.Fail(options.MissingDbUserIdError));
                    }

                    if (!userSettings.HetsUser.Active)
                    {
                        _logger.LogWarning(options.InactiveDbUserIdError + " (" + userId + ")");
                        return Task.FromResult(AuthenticateResult.Fail(options.InactiveDbUserIdError));
                    }

                    // **************************************************
                    // update the user back to their default district
                    // **************************************************
                    string tempSwitch = context.Request.Cookies["HETSDistrict"];

                    if (string.IsNullOrEmpty(tempSwitch) && userSettings.HetsUser != null)
                    {
                        HetUserDistrict userDistrict = dbAppContext.HetUserDistrict.AsNoTracking()
                            .Include(x => x.User)
                            .Include(x => x.District)
                            .FirstOrDefault(x => x.User.UserId == userSettings.HetsUser.UserId &&
                                                 x.IsPrimary);

                        // if we don't find a primary - look for the first one in the list
                        if (userDistrict == null)
                        {
                            userDistrict = dbAppContext.HetUserDistrict.AsNoTracking()
                                .Include(x => x.User)
                                .Include(x => x.District)
                                .FirstOrDefault(x => x.User.UserId == userSettings.HetsUser.UserId);
                        }

                        // update the current district for the user
                        if (userDistrict != null &&
                            userSettings.HetsUser.DistrictId != userDistrict.District.DistrictId)
                        {
                            _logger.LogInformation("Resetting users district back to primary ({0})",
                                userSettings.HetsUser.SmUserId);

                            userSettings.HetsUser.DistrictId = userDistrict.District.DistrictId;
                            dbAppContext.HetUser.Update(userSettings.HetsUser);
                            dbAppContext.SaveChanges();                            
                        }
                    }

                    userSettings.SiteMinderGuid = siteMinderGuid;
                    userSettings.UserAuthenticated = true;
                    userSettings.BusinessUser = false;
                }

                // **************************************************
                // validate / check user permissions
                // **************************************************
                _logger.LogInformation("Validating user permissions");

                ClaimsPrincipal userPrincipal;

                if (userSettings.BusinessUser && 
                    userSettings.UserAuthenticated &&
                    userSettings.HetsBusinessUser != null)
                {
                    userPrincipal = userSettings.HetsBusinessUser.ToClaimsPrincipal(options.Scheme);

                    if (!userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin))
                    {
                        _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                        return Task.FromResult(AuthenticateResult.Fail(options.InvalidPermissions));
                    }
                }
                else 
                {
                    userPrincipal = userSettings.HetsUser.ToClaimsPrincipal(options.Scheme);

                    if (!userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.Login) &&
                        !userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.BusinessLogin) &&
                        !userPrincipal.HasClaim(HetUser.PermissionClaim, HetPermission.ImportData))
                    {
                        _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                        return Task.FromResult(AuthenticateResult.Fail(options.InvalidPermissions));
                    }
                }                

                // **************************************************
                // create authenticated user
                // **************************************************
                _logger.LogInformation("Authentication successful: " + userId);
                _logger.LogInformation("Setting identity and creating session for: " + userId);                
                
                // **************************************************
                // done!
                // **************************************************
                ClaimsPrincipal principal = userPrincipal;
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, null, Options.Scheme)));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                Console.WriteLine(exception);
                throw;
            }
        }        
    }    
}
