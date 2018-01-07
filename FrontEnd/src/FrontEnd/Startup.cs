using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using FrontEnd.Handlers;
using System;
using System.IO;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

namespace FrontEnd
{
    /// <summary>
    /// Startup Fontend Proxy
    /// </summary>
    public class Startup
    {        
        /// <summary>
        /// Configuration for Frontend Proxy
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Startup Mvc
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Configure Mvc Pipeline Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // enable gzip compression
            services.AddResponseCompression();
 
            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(
                    opts => {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        
                        // referenceLoopHandling is set to Ignore to prevent JSON parser issues with the user / roles model.
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });

            // allow access to the Configuration object
            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<ApiProxyServerOptions>(ConfigureApiProxyServerOptions);
        }

        /// <summary>
        /// Configure Mvc Pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseResponseCompression();
            app.UseMvc();
            app.UseDefaultFiles();

            string webFileFolder = Directory.GetCurrentDirectory();
            webFileFolder = webFileFolder + Path.DirectorySeparatorChar + "src"+ Path.DirectorySeparatorChar + "dist";

            Console.WriteLine("Web root is " +  webFileFolder);

            // Only serve up static files if they exist.
            FileServerOptions options = new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(webFileFolder),

                StaticFileOptions =
                {
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate, private";
                        ctx.Context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                        ctx.Context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
                        ctx.Context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                        ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                    }
                }
            };

            app.UseFileServer(options);
            app.UseApiProxyServer(Configuration);
        }

        private void ConfigureApiProxyServerOptions(ApiProxyServerOptions options)
        {
            ApiProxyServerOptions defaultConfig = Configuration.GetSection("ApiProxyServer").Get<ApiProxyServerOptions>();
            if (defaultConfig != null)
            {
                options.Host = defaultConfig.Host;
                options.Port = defaultConfig.Port;
                options.Scheme = defaultConfig.Scheme;
            }

            string apiServerUri = Configuration["MIDDLEWARE_NAME"];

            if (apiServerUri != null)
            {
                string[] apiServerUriParts = apiServerUri.Split(':');
                string host = apiServerUriParts[0];
                string port = apiServerUriParts.Length > 1 ? apiServerUriParts[1] : "80";
                options.Scheme = "http";
                options.Host = host;
                options.Port = port;
            }
        }
    }
}
