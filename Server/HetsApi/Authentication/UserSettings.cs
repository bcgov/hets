using HetsData.Entities;

namespace HetsApi.Authentication
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
        /// True if user is a business user
        /// </summary>
        public bool BusinessUser { get; set; }

        /// <summary>
        /// HETS/SiteMinder User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// SiteMinder Guid
        /// </summary>
        public string SiteMinderGuid { get; set; }

        /// <summary>
        /// SiteMinder Business Guid
        /// </summary>
        public string SiteMinderBusinessGuid { get; set; }

        /// <summary>
        /// HETS User Model
        /// </summary>
        public HetUser HetsUser { get; set; }

        /// <summary>
        /// HETS Business User Model
        /// </summary>
        public HetBusinessUser HetsBusinessUser { get; set; }
    }
}

