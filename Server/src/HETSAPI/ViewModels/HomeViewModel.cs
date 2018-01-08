namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Home Page Model
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// User Id (current user)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Development Environment
        /// </summary>
        public bool DevelopmentEnvironment { get; set; }

        /// <summary>
        /// Current Context Request Id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Message (error)
        /// </summary>
        public string Message { get; set; }
    }
}
