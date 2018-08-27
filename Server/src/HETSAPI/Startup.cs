using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.Console;
using HetsApi.Authorization;
using HetsApi.Authentication;
using HetsData.Model;

namespace HetsApi
{
    /// <summary>
    /// Application startup class
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        public IConfigurationRoot Configuration { get; }

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

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();

            // add database context
            services.AddDbContext<DbAppContext>(options => options.UseNpgsql(connectionString));

            // setup SiteMinder authentication (core 2.0)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                options.DefaultChallengeScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
            }).AddSiteMinderAuth(options =>
            {

            });

            // setup authorization
            services.AddAuthorization();
            services.RegisterPermissionHandler();            

            // allow for large files to be uploaded
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1073741824; // 1 GB
            });                  

            services.AddMvc(options => options.AddDefaultAuthorizationPolicyFilter())                
                .AddJsonOptions(
                    opts => {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        opts.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                        opts.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                        
                        // ReferenceLoopHandling is set to Ignore to prevent JSON parser issues with the user / roles model.
                        opts.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
                        
            // enable Hangfire            
            PostgreSqlStorageOptions postgreSqlStorageOptions = new PostgreSqlStorageOptions {
                SchemaName = "public"
            };
            if (connectionString != null)
            {
                PostgreSqlStorage storage = new PostgreSqlStorage(connectionString, postgreSqlStorageOptions);
                services.AddHangfire(config =>
                {
                    config.UseStorage(storage);
                    config.UseConsole();
                });
            }
            
            // Configure Swagger - only required in the Development Environment
            if (_hostingEnv.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "HETS REST API",
                        Description = "Hired Equipment Tracking System"
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

            // web site error handler             
            app.UseWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseExceptionHandler(Configuration.GetSection("Constants:ErrorUrl").Value);
            });
            
            // authenticate users
            app.UseAuthentication();

            // enable Hangfire
            app.UseHangfireServer();

            // disable the back to site link
            DashboardOptions dashboardOptions = new DashboardOptions
            {
                AppPath = null,                    
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };

            // enable the hangfire dashboard
            app.UseHangfireDashboard(Configuration.GetSection("Constants:HangfireUrl").Value, dashboardOptions);            
            
            // setup mvc routes
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });            
            
            if (_hostingEnv.IsDevelopment())
            {
                string swaggerApi = Configuration.GetSection("Constants:SwaggerApiUrl").Value;
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(swaggerApi, "HETS REST API v1");
                    options.DocExpansion(DocExpansion.None);
                });
            }                                
        }
        
        /// <summary>
        /// Retrieve database connection string
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string connectionString;

            string host = Configuration["DATABASE_SERVICE_NAME"];
            string username = Configuration["POSTGRESQL_USER"];
            string password = Configuration["POSTGRESQL_PASSWORD"];
            string database = Configuration["POSTGRESQL_DATABASE"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(database))
            {
                connectionString = Configuration.GetConnectionString("HETS");
            }
            else
            {
                // environment variables override all other settings (OpenShift)
                connectionString = $"Host={host};Username={username};Password={password};Database={database};";
            }

            return connectionString;
        }             
    }    
}
