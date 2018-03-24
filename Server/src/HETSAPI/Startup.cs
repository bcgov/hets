//***********************************************************************************************
// REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
//***********************************************************************************************
// The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe 
// or other piece of equipment they want to hire out to the transportation ministry for day 
// labour and  emergency projects.  The Hired Equipment Program distributes available work to 
// local equipment owners. The program is  based on seniority and is designed to deliver work 
// to registered users fairly and efficiently  through the development of local 
// area call-out lists. 
//***********************************************************************************************
// OpenAPI spec version: v1
//***********************************************************************************************
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using System.Reflection;
using Hangfire.PostgreSql;
using Hangfire;
using Hangfire.Console;
using HETSAPI.Models;
using HETSAPI.Authorization;
using HETSAPI.Authentication;

namespace HETSAPI
{
    /// <summary>
    /// The application Startup class
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        /// <summary>
        /// HETS Configuration
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Startup HETS Backend Services
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

        /// <summary>
        /// Add services to the container
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();

            // add database context
            services.AddDbContext<DbAppContext>(options => options.UseNpgsql(connectionString));

            // setup siteminder authentication (core 2.0)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
                options.DefaultChallengeScheme = SiteMinderAuthOptions.AuthenticationSchemeName;
            }).AddSiteminderAuth(options =>
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

            // Add application services.
            services.RegisterApplicationServices();
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

            // update database environment 
            if (Configuration != null)
            {
                string updateDb = Configuration.GetSection("UpdateLocalDb").Value;
                if (env.IsDevelopment() && updateDb.ToLower() != "false")
                {
                    TryMigrateDatabase(app, loggerFactory);
                }
                else if (!env.IsDevelopment())
                {
                    TryMigrateDatabase(app, loggerFactory);
                }
            }

            // do not start Hangfire if we are running tests. 
            bool startHangfire = true;

#if DEBUG                   
            foreach (var assem in Assembly.GetEntryAssembly().GetReferencedAssemblies())
            {
                if (assem.FullName.ToLowerInvariant().StartsWith("xunit"))
                {
                    startHangfire = false;
                    break;
                }                
            }        
#endif

            if (startHangfire)
            {
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
            }
            
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
        /// Database Migration
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        private void TryMigrateDatabase(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            ILogger log = loggerFactory.CreateLogger(typeof(Startup));
            log.LogInformation("Attempting to migrate the database ...");

            try
            {
                using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    log.LogInformation("Fetching the application's database context ...");
                    DbContext context = serviceScope.ServiceProvider.GetService<DbAppContext>();

                    log.LogInformation("Migrating the database ...");
                    context.Database.Migrate();
                    log.LogInformation("The database migration complete.");

                    log.LogInformation("Updating the database documentation ...");
                    DbCommentsUpdater<DbAppContext> updater = new DbCommentsUpdater<DbAppContext>((DbAppContext)context);
                    updater.UpdateDatabaseDescriptions();
                    log.LogInformation("The database documentation has been updated.");
                    
                    log.LogInformation("Adding/Updating seed data ...");
                    Seeders.SeedFactory<DbAppContext> seederFactory = new Seeders.SeedFactory<DbAppContext>(Configuration, _hostingEnv, loggerFactory);
                    seederFactory.Seed((DbAppContext) context);
                    log.LogInformation("Seeding operations are complete.");
                }

                log.LogInformation("All database migration activities are complete.");
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("The database migration failed!");
                msg.AppendLine("The database may not be available and the application will not function as expected.");
                msg.AppendLine("Please ensure a database is available and the connection string is correct.");
                msg.AppendLine("If you are running in a development environment, ensure your test database and server configuraiotn match the project's default connection string.");

                log.LogCritical(new EventId(-1, "Database Migration Failed"), e, msg.ToString());
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
                // When things get cleaned up properly, this is the only call we'll have to make.
                connectionString = Configuration.GetConnectionString("HETS");
            }
            else
            {
                // Environment variables override all other settings; same behaviour as the configuration provider when things get cleaned up. 
                connectionString = $"Host={host};Username={username};Password={password};Database={database};";
            }

            return connectionString;
        }             
    }    
}
