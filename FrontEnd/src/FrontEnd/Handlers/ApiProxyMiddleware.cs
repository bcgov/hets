using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FrontEnd.Handlers
{
    /// <summary>
    /// Proxy Middleware
    /// </summary>
    public class ApiProxyMiddleware
    {
        private readonly string _apiPathKey;
        private readonly string _apiUri;
        private readonly ProxyMiddleware _proxy;
        private readonly ILogger _logger;

        /// <summary>
        /// Proxy Middleware Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="apiServerOptions"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        public ApiProxyMiddleware(RequestDelegate next, IOptions<ApiProxyServerOptions> apiServerOptions, ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            _apiUri = apiServerOptions.Value.ToUri().AbsoluteUri;
            _proxy = new ProxyMiddleware(next, apiServerOptions.Value.ToProxyOptions());
            _logger = loggerFactory.CreateLogger<ApiProxyMiddleware>();
            _apiPathKey = configuration.GetSection("Constants").GetSection("ApiPath").Value;
        }

        /// <summary>
        /// The invoke method is called by the ProxyExtension class
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                string requestPath = context.Request.Path.Value;
                int indexOfApi = requestPath.IndexOf(_apiPathKey, StringComparison.Ordinal);
                context.Request.Path = requestPath.Remove(0, indexOfApi);

                // set http security headers
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate, private";
                context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";                

                await _proxy.Invoke(context);

                // debugging only
                if (requestPath.IndexOf("/groups/", StringComparison.Ordinal) >= 0)
                {
                    _logger.LogInformation("Path: " + requestPath);
                    _logger.LogInformation("Response Body: " + context.Response.Body);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(-1, "ApiProxyMiddleware Exception"), e, $"An unexpected exception occured while forwarding a request to the API proxy; {_apiUri}.");
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"Exception encountered forwarding a request to {_apiUri}.");
            }
        }       
    }
}

