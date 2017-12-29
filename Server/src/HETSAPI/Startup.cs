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
using System.IO;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
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
        private const string ConstHangfireUrl = "/hangfire";
        private const string ConstErrorUrl = "/error";

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

            var builder = new ConfigurationBuilder()
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
            
            // setup authorization
            services.AddAuthorization();
            services.RegisterPermissionHandler();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDbAppContextFactory, DbAppContextFactory>(CreateDbAppContextFactory);
            services.AddSingleton<IConfiguration>(Configuration);

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

            // allow for large files to be uploaded
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1073741824; // 1 GB
            });          

            // add compresssion
            services.AddResponseCompression();            

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

            PostgreSqlStorage storage = new PostgreSqlStorage(connectionString, postgreSqlStorageOptions);
            services.AddHangfire(config => 
            {
                config.UseStorage(storage);
                config.UseConsole();
            });

            // Configure Swagger - only required in the Development Environment
            if (_hostingEnv.IsDevelopment())
            {
                services.AddSwaggerGen();
                services.ConfigureSwaggerGen(options =>
                {
                    options.SingleApiVersion(new Info
                    {
                        Version = "v1",
                        Title = "HETS REST API",
                        Description = "Hired Equipment Tracking System"
                    });

                    options.DescribeAllEnumsAsStrings();

                    XPathDocument comments = new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    options.OperationFilter<XmlCommentsOperationFilter>(comments);
                    options.ModelFilter<XmlCommentsModelFilter>(comments);
                });
            }

            // Add application services.
            services.RegisterApplicationServices();

            services.AddDistributedMemoryCache(); // Adds a default in-memory cache
            services.AddSession();
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
            app.UseExceptionHandler(ConstErrorUrl);

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            // authenticate users
            app.UseAuthentication();

            // update database environment            
            TryMigrateDatabase(app, loggerFactory);

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
                DashboardOptions dashboardOptions = new DashboardOptions()
                {
                    AppPath = null
                };

                // enable the /hangfire action
                app.UseHangfireDashboard(ConstHangfireUrl, dashboardOptions);
            }

            // enable response compression
            app.UseResponseCompression();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            if (_hostingEnv.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUi();
            }
            
            if (startHangfire)
            {
                HangfireTools.ClearHangfire();

                // this should be set as an environment variable.  
                // Only enable when doing a new PROD deploy to populate CCW data and link it to the bus data.
                if (!string.IsNullOrEmpty(Configuration["ENABLE_ANNUAL_ROLLOVER"]))
                {
                    CreateHangfireAnnualRolloverJob(loggerFactory);
                }
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

        /// <summary>
        /// Create Db Context Factory
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private DbAppContextFactory CreateDbAppContextFactory(IServiceProvider serviceProvider)
        {
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(GetConnectionString());
            DbAppContextFactory dbAppContextFactory = new DbAppContextFactory(null, options.Options);
            return dbAppContextFactory;
        }       

        /// <summary>
        /// Create Hangfire Jobs
        /// </summary>
        /// <param name="loggerFactory"></param>
        private void CreateHangfireAnnualRolloverJob(ILoggerFactory loggerFactory)
        {
            // HETS has one job that runs at the end of each year.            
            ILogger log = loggerFactory.CreateLogger(typeof(Startup));

            // first check to see if Hangfire already has the job.
            log.LogInformation("Attempting setup of Hangfire Annual rollover job ...");
            
            try
            {
                string connectionString = GetConnectionString();

                log.LogInformation("Creating Hangfire job for Annual rollover ...");
                // every 5 minutes we see if a CCW record needs to be updated.  We only update one CCW record at a time.
                // since the server is on UTC, we want UTC-7 for PDT midnight.                
                RecurringJob.AddOrUpdate(() => SeniorityListExtensions.AnnualRolloverJob(null, connectionString), Cron.Yearly(3, 31, 17));                            
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Failed to setup Hangfire job.");

                log.LogCritical(new EventId(-1, "Hangfire job setup failed"), e, msg.ToString());
            }            
        }
    }    
}
