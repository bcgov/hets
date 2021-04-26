namespace FrontEnd
{
    /// <summary>
    /// Proxy Server Options
    /// </summary>
    public class ApiProxyServerOptions
    {
        /// <summary>
        /// Proxy Server Options Constructor
        /// </summary>
        public ApiProxyServerOptions()
        {
            Scheme = "http";
            Host = "localhost";
            Port = "80";
        }

        /// <summary>
        /// Scheme (http/https)
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public string Port { get; set; }
    }
}
