/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using HETSAPI.Models;
using System.Text;
using HETSAPI.Authorization;
using HETSAPI.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Hangfire.PostgreSql;
using Hangfire;
using Hangfire.Console;
using System.Reflection;
using System.Runtime.Loader;
using HETSAPI.Import;

namespace HETSAPI
{
    /// <summary>
    /// The application Startup class
    /// </summary>
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        public IConfigurationRoot Configuration { get; }

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();

            services.AddAuthorization();
            services.RegisterPermissionHandler();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDbAppContextFactory, DbAppContextFactory>(CreateDbAppContextFactory);
            services.AddSingleton<IConfiguration>(Configuration);

            // Add database context
            services.AddDbContext<DbAppContext>(options => options.UseNpgsql(connectionString));

            // allow for large files to be uploaded
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1073741824; // 1 GB
            });

            services.AddResponseCompression();

            // Add framework services.
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

            // Configure Swagger
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

                var comments = new XPathDocument($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
                options.OperationFilter<XmlCommentsOperationFilter>(comments);
                options.ModelFilter<XmlCommentsModelFilter>(comments);
            });

            // Add application services.
            services.RegisterApplicationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            TryMigrateDatabase(app, loggerFactory);
            app.UseAuthentication(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            bool startHangfire = true;
#if DEBUG
            // do not start Hangfire if we are running tests.        
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
                DashboardOptions dashboardOptions = new DashboardOptions();
                dashboardOptions.AppPath = null;

                app.UseHangfireDashboard("/hangfire", dashboardOptions); // this enables the /hangfire action

            }


            app.UseResponseCompression();
            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUi();

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

        // TODO:
        // - Should database migration be done here; in Startup?
        private void TryMigrateDatabase(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            ILogger log = loggerFactory.CreateLogger(typeof(Startup));
            log.LogInformation("Attempting to migrate the database ...");

            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
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
                    seederFactory.Seed(context as DbAppContext);
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

        // ToDo:
        // - Replace the individual environment variables with one that naturally works with the configuration provider and how connection strings work.
        // -- For instance:
        // --- ConnectionStrings:Schoolbus or ConnectionStrings__Schoolbus
        // -- This way the configuration provider is performing all of the lifting and the connection string can be retrieved in a single consistent manner.
        private string GetConnectionString()
        {
            string connectionString = string.Empty;

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

        private DbAppContextFactory CreateDbAppContextFactory(IServiceProvider serviceProvider)
        {
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(GetConnectionString());
            DbAppContextFactory dbAppContextFactory = new DbAppContextFactory(null, options.Options);
            return dbAppContextFactory;
        }

        

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
