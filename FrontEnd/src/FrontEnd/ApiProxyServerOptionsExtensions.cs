using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace FrontEnd
{
    /// <summary>
    /// Proxy Server Options Extension
    /// </summary>
    public static class ApiProxyServerOptionsExtensions
    {
        /// <summary>
        /// Returns Uri
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Uri ToUri(this ApiProxyServerOptions options)
        {
            int.TryParse(options.Port, out int portNumber);
            UriBuilder uriBuilder = new UriBuilder(options.Scheme, options.Host, portNumber);
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Returns Proxy Options
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IOptions<ProxyOptions> ToProxyOptions(this ApiProxyServerOptions options)
        {
            ProxyOptions proxyOptions = new ProxyOptions()
            {
                Host = options.Host,
                Port = options.Port,
                Scheme = options.Scheme
            };

            return Options.Create(proxyOptions);
        }
    }
}
