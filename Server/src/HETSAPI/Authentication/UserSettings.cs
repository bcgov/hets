using HETSAPI.Models;

namespace HETSAPI.Authentication
{
    /// <summary>
    /// Object to track and manage the authenticated user session
    /// </summary>
    public class UserSettings
    {        
        /// <summary>
        /// True if user is authenticated
        /// </summary>
        public bool UserAuthenticated { get; set; }

        /// <summary>
        /// HETS/SiteMinder User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// SiteMinder Guid
        /// </summary>
        public string SiteMinderGuid { get; set; }

        /// <summary>
        /// HETS User Model
        /// </summary>
        public User HetsUser { get; set; }        
    }
}

