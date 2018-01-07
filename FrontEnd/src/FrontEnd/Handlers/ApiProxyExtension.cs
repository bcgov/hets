using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiProxyServer(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            string apiPathKey = configuration.GetSection("Constants").GetSection("ApiPath").Value;
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
