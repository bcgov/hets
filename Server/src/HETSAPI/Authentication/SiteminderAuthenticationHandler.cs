using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Authentication
{
    #region SiteMinder Authentication Options
    /// <summary>
    /// Options required for setting up SiteMidner Authentication
    /// </summary>
    public class SiteMinderAuthOptions : AuthenticationSchemeOptions
    {
        private const string Dev_UserId = "TMcTesterson";
        private const string Dev_Guid = "2cbf7cb8d6b445f087fb82ad75566a9c";

        private const string SiteMinder_User_Guid_Key = "smgov_userguid";
        private const string SiteMinder_Universal_Id_Key = "sm_universalid";
        private const string SiteMinder_User_Name_Key = "sm_user";
        private const string SiteMinder_User_Display_Name_Key = "smgov_userdisplayname";

        /// <summary>
        /// DEfault Constructor
        /// </summary>
        public SiteMinderAuthOptions()
        {
            SiteMinderUserGuidKey = SiteMinder_User_Guid_Key;
            SiteMinderUniversalIdKey = SiteMinder_Universal_Id_Key;
            SiteMinderUserNameKey = SiteMinder_User_Name_Key;
            SiteMinderUserDisplayNameKey = SiteMinder_User_Display_Name_Key;
        }

        /// <summary>
        /// Default Scheme Name
        /// </summary>
        public static string AuthenticationSchemeName
        {
            get { return "site-minder-auth"; }
        }

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
        /// Environment
        /// </summary>
        public IHostingEnvironment Env { get; set; }

        /// <summary>
        /// Database connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// HETS Test/Dev User Name
        /// </summary>
        public string DeveloperUserId
        {
            get { return Dev_UserId;  }
        }

        /// <summary>
        /// HETS Test/Dev User Guid
        /// </summary>
        public string DeveloperGuid
        {
            get { return Dev_Guid; }
        }
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

            string userId = "";
            string siteMinderGuid = "";

            if (Options.Env.IsDevelopment())
            {
                userId = Options.DeveloperUserId;
                siteMinderGuid = Options.DeveloperGuid;

            }
            else
            {
                userId = Request.Headers[Options.SiteMinderUserNameKey];
                if (string.IsNullOrEmpty(userId))
                {
                    userId = Request.Headers[Options.SiteMinderUniversalIdKey];
                }

                siteMinderGuid = Request.Headers[Options.SiteMinderUserGuidKey];
            }

            // validate credentials
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning($"Missing SiteMinder UserId");
                return Task.FromResult(AuthenticateResult.Fail("Missing SiteMinder UserId"));
            }

            if (string.IsNullOrEmpty(siteMinderGuid))
            {
                _logger.LogWarning($"Missing SiteMinder Guid");
                return Task.FromResult(AuthenticateResult.Fail("Missing SiteMinder Guid"));
            }

            // validate credential against database        
            IDbAppContext context = CreateDbAppContextFactory(Options.ConnectionString);
            var user = context.LoadUser(userId, siteMinderGuid);
            if (user == null)
            {
                _logger.LogWarning($"Could not find user {userId} in the HETS database");
                return Task.FromResult(AuthenticateResult.Fail("Invalid HETS Credential"));
            }

            if (user.Active == false)
            {
                _logger.LogWarning($"Inactive HETS Credential attempting to login: {userId}");
                return Task.FromResult(AuthenticateResult.Fail("Inactive HETS Credential"));
            }
                
            // create authenticated user
            _logger.LogInformation($"Setting identity");
            ClaimsPrincipal principal = user.ToClaimsPrincipal(Options.Scheme);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, null, Options.Scheme);

            // done!
            return Task.FromResult(AuthenticateResult.Success(ticket));         
        }

        private IDbAppContext CreateDbAppContextFactory(string connectionString)
        {
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionString);
            DbAppContextFactory dbAppContextFactory = new DbAppContextFactory(null, options.Options);
            return dbAppContextFactory.Create();
        }
    }
}
