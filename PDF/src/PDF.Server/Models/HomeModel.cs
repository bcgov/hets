namespace PDF.Server.Models
{
    /// <summary>
    /// Home Page Model
    /// </summary>
    public class HomeModel
    {        
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
