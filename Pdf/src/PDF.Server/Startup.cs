using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Pdf.Server
{
    /// <summary>
    /// Startup Pdf Micro Service
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

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
            _hostingEnv = env;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)          
                .AddEnvironmentVariables();

            Configuration = builder.Build();            
        }        

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // load configuration data
            services.AddSingleton(provider => Configuration);

            // enable Node Services
            services.AddNodeServices();

            services.AddMvc().
                AddJsonOptions(
                    opts => {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        
                        // ReferenceLoopHandling is set to Ignore to prevent JSON parser issues with the user / roles model.
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });            

            // Configure Swagger - only required in the Development Environment
            if (_hostingEnv.IsDevelopment())
            {                
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "PDF REST API",
                        Description = "Pdf Generation Micro Service"
                    });
                    options.DescribeAllEnumsAsStrings();
                });
            }
        }

        /// <summary>
        /// Configure the HTTP request pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            app.UseDefaultFiles();

            if (_hostingEnv.IsDevelopment())
            {
                string swaggerApi = Configuration.GetSection("Constants:SwaggerApiUrl").Value;
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(swaggerApi, "PDF REST API v1");
                    options.DocExpansion(DocExpansion.None);
                });
            }
        }
    }
}
