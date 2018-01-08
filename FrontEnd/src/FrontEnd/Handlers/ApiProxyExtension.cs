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
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiProxyServer(this IApplicationBuilder app, string apiUrl)
        {
            return app.Map(apiUrl, ProxyRequest);
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
