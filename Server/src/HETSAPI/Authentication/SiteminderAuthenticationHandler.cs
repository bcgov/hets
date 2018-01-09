using System;
using System.Security.Authentication;
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

namespace HETSAPI.Authentication
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
        public string SiteMinderUserGuidKey { get; private set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string SiteMinderUniversalIdKey { get; private set; }

        /// <summary>
        /// User Name
        /// </summary>
        public string SiteMinderUserNameKey { get; private set; }

        /// <summary>
        /// User's Display Name
        /// </summary>
        public string SiteMinderUserDisplayNameKey { get; private set; }

        /// <summary>
        /// Missing SiteMinder UserId Error
        /// </summary>
        public string MissingSiteMinderUserIdError { get; private set; }

        /// <summary>
        /// Missing SiteMinder Guid Error
        /// </summary>
        public string MissingSiteMinderGuidError { get; private set; }

        /// <summary>
        /// Missing Database UserId Error
        /// </summary>
        public string MissingDbUserIdError { get; private set; }

        /// <summary>
        /// Inactive Database UserId Error
        /// </summary>
        public string InactivegDbUserIdError { get; private set; }

        /// <summary>
        /// Development Environment Authentication Key
        /// </summary>
        public string DevAuthenticationTokenKey { get; private set; }

        /// <summary>
        /// Development Environment efault UserId
        /// </summary>
        public string DevDefaultUserId { get; private set; }
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
                ClaimsPrincipal principal;

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
                    url.Contains(".map") ||
                    url.Contains(".js"))
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                // **************************************************
                // Check if we have a Dev Environment Cookie
                // **************************************************
                if (hostingEnv.IsDevelopment())
                {
                    string temp = context.Request.Cookies[options.DevAuthenticationTokenKey];

                    if (!string.IsNullOrEmpty(temp))
                        userId = temp;
                }

                // **************************************************
                // Check if the user session is already created
                // **************************************************
                try
                {
                    _logger.LogInformation("Checking user session");
                    userSettings = UserSettings.ReadUserSettings(context);
                }
                catch
                {
                    //do nothing
                }

                // is user authenticated - if so we're done
                if ((userSettings.UserAuthenticated && string.IsNullOrEmpty(userId)) ||
                    (userSettings.UserAuthenticated && !string.IsNullOrEmpty(userId) &&
                     !string.IsNullOrEmpty(userSettings.UserId) && userSettings.UserId == userId))
                {
                    _logger.LogInformation("User already authenticated with active session: " + userSettings.UserId);
                    principal = userSettings.HetsUser.ToClaimsPrincipal(options.Scheme);
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, null, Options.Scheme)));
                }

                // **************************************************
                // Authenticate based on SiteMinder Headers
                // **************************************************
                _logger.LogDebug("Parsing the HTTP headers for SiteMinder authentication credential");

                if (string.IsNullOrEmpty(userId))
                {
                    userId = context.Request.Headers[options.SiteMinderUserNameKey];
                    if (string.IsNullOrEmpty(userId))
                    {
                        userId = context.Request.Headers[options.SiteMinderUniversalIdKey];
                    }

                    siteMinderGuid = context.Request.Headers[options.SiteMinderUserGuidKey];

                    // **************************************************
                    // Validate credentials
                    // **************************************************
                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogError(options.MissingSiteMinderUserIdError);
                        throw new AuthenticationException(options.MissingSiteMinderUserIdError);
                    }

                    if (string.IsNullOrEmpty(siteMinderGuid))
                    {
                        _logger.LogError(options.MissingSiteMinderGuidError);
                        throw new AuthenticationException(options.MissingSiteMinderGuidError);
                    }
                }

                // **************************************************
                // Validate credential against database              
                // **************************************************
                userSettings.HetsUser = hostingEnv.IsDevelopment()
                    ? dbAppContext.LoadUser(userId)
                    : dbAppContext.LoadUser(userId, siteMinderGuid);

                if (userSettings.HetsUser == null)
                {
                    _logger.LogWarning(options.MissingDbUserIdError + " (" + userId + ")");
                    throw new AuthenticationException(options.MissingDbUserIdError + " (" + userId + ")");
                }

                if (!userSettings.HetsUser.Active)
                {
                    _logger.LogWarning(options.InactivegDbUserIdError + " (" + userId + ")");
                    throw new AuthenticationException(options.InactivegDbUserIdError + " (" + userId + ")");
                }

                // **************************************************
                // Create authenticated user
                // **************************************************
                _logger.LogInformation("Authentication successful: " + userId);
                _logger.LogInformation("Setting identity and creating session for: " + userId);
                
                // create session info
                userSettings.UserId = userId;
                userSettings.UserAuthenticated = true;

                // update user settings
                UserSettings.SaveUserSettings(userSettings, context);

                // done!
                principal = userSettings.HetsUser.ToClaimsPrincipal(options.Scheme);
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, null, Options.Scheme)));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e);
                throw;
            }
        }        
    }
}
