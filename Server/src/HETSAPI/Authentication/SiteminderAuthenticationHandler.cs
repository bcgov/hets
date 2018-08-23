using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HetsApi.Authentication
{    
    #region SiteMinder Authentication Options
    /// <summary>
    /// Options required for setting up SiteMidner Authentication
    /// </summary>
    public class SiteMinderAuthOptions : AuthenticationSchemeOptions
    {
        private const string ConstDevAuthenticationTokenKey = "DEV-USER";
        private const string ConstDevDefaultUserId = "TMcTesterson";       
        private const string ConstSiteMinderUserGuidKey = "smgov_userguid";
        private const string ConstSiteMinderUniversalIdKey = "sm_universalid";
        private const string ConstSiteMinderUserNameKey = "sm_user";
        private const string ConstSiteMinderUserDisplayNameKey = "smgov_userdisplayname";

        private const string ConstMissingSiteMinderUserIdError = "Missing SiteMinder UserId";
        private const string ConstMissingSiteMinderGuidError = "Missing SiteMinder Guid";
        private const string ConstMissingDbUserIdError = "Could not find UserId in the HETS database";
        private const string ConstInactivegDbUserIdError = "HETS database UserId is inactive";
        private const string ConstInvalidPermissions = "HETS UserId does not have valid permissions";

        /// <summary>
        /// DEfault Constructor
        /// </summary>
        public SiteMinderAuthOptions()
        {
            SiteMinderUserGuidKey = ConstSiteMinderUserGuidKey;
            SiteMinderUniversalIdKey = ConstSiteMinderUniversalIdKey;
            SiteMinderUserNameKey = ConstSiteMinderUserNameKey;
            SiteMinderUserDisplayNameKey = ConstSiteMinderUserDisplayNameKey;
            MissingSiteMinderUserIdError = ConstMissingSiteMinderUserIdError;
            MissingSiteMinderGuidError = ConstMissingSiteMinderGuidError;
            MissingDbUserIdError = ConstMissingDbUserIdError;
            InactivegDbUserIdError = ConstInactivegDbUserIdError;
            InvalidPermissions = ConstInvalidPermissions;
            DevAuthenticationTokenKey = ConstDevAuthenticationTokenKey;
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
        /// Inactive Database UserId Error
        /// </summary>
        public string InactivegDbUserIdError { get; set; }

        /// <summary>
        /// User does not jave active / valid permissions
        /// </summary>
        public string InvalidPermissions { get; set; }

        /// <summary>
        /// Development Environment Authentication Key
        /// </summary>
        public string DevAuthenticationTokenKey { get; set; }

        /// <summary>
        /// Development Environment efault UserId
        /// </summary>
        public string DevDefaultUserId { get; set; }
    }
    #endregion    

    /// <summary>
    /// Setup Siteminder Authentication Handler
    /// </summary>
    public static class SiteminderAuthenticationExtensions
    {
        /// <summary>
        /// Add Authentication Handler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSiteminderAuth(this AuthenticationBuilder builder, Action<SiteMinderAuthOptions> configureOptions)
        {
            return builder.AddScheme<SiteMinderAuthOptions, SiteminderAuthenticationHandler>(SiteMinderAuthOptions.AuthenticationSchemeName, configureOptions);
        }
    }

    /// <summary>
    /// Siteminder Authentication Handler
    /// </summary>
    public class SiteminderAuthenticationHandler : AuthenticationHandler<SiteMinderAuthOptions>
    {
        private readonly ILogger _logger;        

        /// <summary>
        /// Siteminder Authentication Constructir
        /// </summary>
        /// <param name="configureOptions"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public SiteminderAuthenticationHandler(IOptionsMonitor<SiteMinderAuthOptions> configureOptions, ILoggerFactory loggerFactory, UrlEncoder encoder, ISystemClock clock)
            : base(configureOptions, loggerFactory, encoder, clock)
        {
            _logger = loggerFactory.CreateLogger(typeof(SiteminderAuthenticationHandler));                    
        }

        /// <summary>
        /// Process Authentication Request
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // get siteminder headers
            _logger.LogDebug("Parsing the HTTP headers for SiteMinder authentication credential");
                  
            SiteMinderAuthOptions options = new SiteMinderAuthOptions();

            try
            {
                HttpContext context = Request.HttpContext;
                IDbAppContext dbAppContext = (DbAppContext)context.RequestServices.GetService(typeof(DbAppContext));
                IHostingEnvironment hostingEnv = (IHostingEnvironment)context.RequestServices.GetService(typeof(IHostingEnvironment));
                
                UserSettings userSettings = new UserSettings();
                string userId = "";
                string siteMinderGuid = "";

                // **************************************************
                // If this is an Error or Authentiation API - Ignore
                // **************************************************
                string url = context.Request.GetDisplayUrl().ToLower();

                if (url.Contains("/authentication/dev") ||
                    url.Contains("/error") ||
                    url.Contains("/hangfire") ||
                    url.Contains("/swagger") ||
                    url.Contains(".map") ||
                    url.Contains(".png") ||
                    url.Contains(".css") ||
                    url.Contains(".ico") ||
                    url.Contains(".eot") ||
                    url.Contains(".woff") ||
                    url.Contains(".ttf") ||
                    url.Contains(".js"))
                {
                    _logger.LogInformation("Bypassing authentication process ({0})", url);
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                // **************************************************
                // Check if we have a Dev Environment Cookie
                // **************************************************
                if (hostingEnv.IsDevelopment())
                {
                    string temp = context.Request.Cookies[options.DevAuthenticationTokenKey];

                    if (!string.IsNullOrEmpty(temp))
                    {
                        _logger.LogInformation("Dev Authentication token found ({0})", temp);
                        userId = temp;
                    }
                }                                            

                // **************************************************
                // Authenticate based on SiteMinder Headers
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

                    // **************************************************
                    // Validate credentials
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
                // Validate credential against database              
                // **************************************************
                _logger.LogInformation("Validating credential against the HETS db");

                userSettings.HetsUser = hostingEnv.IsDevelopment()
                    ? dbAppContext.LoadUser(userId)
                    : dbAppContext.LoadUser(userId, siteMinderGuid);

                if (userSettings.HetsUser == null)
                {
                    _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                    return Task.FromResult(AuthenticateResult.Fail(options.MissingDbUserIdError));
                }

                if (!userSettings.HetsUser.Active)
                {
                    _logger.LogWarning(options.InactivegDbUserIdError + " (" + userId + ")");
                    return Task.FromResult(AuthenticateResult.Fail(options.InactivegDbUserIdError));
                }

                // **************************************************
                // Update the user back to their default district
                // **************************************************
                string tempSwitch = context.Request.Cookies["HETSDistrict"];

                if (string.IsNullOrEmpty(tempSwitch) && userSettings.HetsUser != null)
                {
                    UserDistrict userDistrict = dbAppContext.UserDistricts.AsNoTracking()
                        .Include(x => x.User)
                        .Include(x => x.District)
                        .FirstOrDefault(x => x.User.Id == userSettings.HetsUser.Id &&
                                             x.IsPrimary);

                    // if we don't find a primary - look for the first one in the list
                    if (userDistrict == null)
                    {
                        userDistrict = dbAppContext.UserDistricts.AsNoTracking()
                            .Include(x => x.User)
                            .Include(x => x.District)
                            .FirstOrDefault(x => x.User.Id == userSettings.HetsUser.Id);
                    }

                    // update the current district for the user
                    if (userDistrict != null && userSettings.HetsUser.DistrictId != userDistrict.District.Id)
                    {
                        _logger.LogInformation("Resetting users district back to primary ({0})", userSettings.HetsUser.SmUserId);

                        userSettings.HetsUser.DistrictId = userDistrict.District.Id;
                        dbAppContext.Users.Update(userSettings.HetsUser);
                        dbAppContext.SaveChanges();
                    }
                }

                // **************************************************
                // Validate / check user permissions
                // **************************************************
                _logger.LogInformation("Validating user permissions");

                ClaimsPrincipal userPrincipal = userSettings.HetsUser.ToClaimsPrincipal(options.Scheme);

                if (!userPrincipal.HasClaim(User.PermissionClaim, Permission.Login) &&
                    !userPrincipal.HasClaim(User.PermissionClaim, Permission.BusinessLogin) &&
                    !userPrincipal.HasClaim(User.PermissionClaim, Permission.ImportData))
                {
                    _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                    return Task.FromResult(AuthenticateResult.Fail(options.InvalidPermissions));
                }                

                // **************************************************
                // Create authenticated user
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
