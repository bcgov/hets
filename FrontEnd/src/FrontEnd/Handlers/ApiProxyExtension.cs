using Microsoft.AspNetCore.Builder;

namespace FrontEnd.Handlers
{
    /// <summary>
    /// Proxy Extension
    /// </summary>
    public static class ApiProxyExtension
    {
        /// <summary>
        /// Use Proxy Server
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiProxyServer(this IApplicationBuilder app)
        {
            string apiPathKey = "/api";
            return app.Map(apiPathKey, ProxyRequest);
        }
       
        /// <summary>
        /// Proxy Request
        /// </summary>
        /// <param name="app"></param>
        private static void ProxyRequest(IApplicationBuilder app)
        {
            app.UseMiddleware<ApiProxyMiddleware>();
        }
    }
}
