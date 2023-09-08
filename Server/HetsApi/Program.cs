using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using AutoMapper;
using HetsApi.Authentication;
using HetsData.Entities;
using HetsData.Hangfire;
using HetsData.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using HetsApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.DependencyInjection;
using HetsData.Mappings;
using Serilog.Ui.Web;
using Serilog.Ui.PostgreSqlProvider;
using Microsoft.EntityFrameworkCore.Diagnostics;
using HetsApi.Authorization;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;
using System.Net.Mime;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Register services here
    builder.Host.UseSerilog();
    builder.Configuration.AddEnvironmentVariables();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    IdentityModelEventSource.ShowPII = true;
    string connectionString = GetConnectionString(builder.Configuration);

    // add http context accessor
    builder.Services.AddHttpContextAccessor();

    // add auto mapper
    var mappingConfig = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new EntityToDtoProfile());
        cfg.AddProfile(new DtoToEntityProfile());
        cfg.AddProfile(new EntityToEntityProfile());
    });

    var mapper = mappingConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);
    builder.Services.AddSerilogUi(options => options.UseNpgSql(connectionString, "het_log"));

    // add database context
    builder.Services.AddDbContext<DbAppContext>(options =>
    {
        options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        options.ConfigureWarnings(o => o.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
    });

    builder.Services.AddDbContext<DbAppMonitorContext>(options =>
    {
        options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        options.ConfigureWarnings(o => o.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning));
    });

    builder.Services.AddScoped<IAnnualRollover, AnnualRollover>();

    builder.Services
        .AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        })
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("JWT:Authority");
        options.Audience = builder.Configuration.GetValue<string>("JWT:Audience");
        options.IncludeErrorDetails = true;
        options.EventsType = typeof(HetsJwtBearerEvents);
    });

    // setup authorization
    builder.Services.AddAuthorization();
    builder.Services.RegisterPermissionHandler();
    builder.Services.AddScoped<HetsJwtBearerEvents>();

    // repository
    builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
    builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
    builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
    builder.Services.AddScoped<IRentalAgreementRepository, RentalAgreementRepository>();
    builder.Services.AddScoped<IRentalRequestRepository, RentalRequestRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // allow for large files to be uploaded
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = 1073741824; // 1 GB
    });

    //enable Hangfire
    builder.Services.AddHangfire(configuration =>
        configuration
            .UseSerilogLogProvider()
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(connectionString)
    );

    builder.Services.AddHangfireServer(options =>
    {
        options.WorkerCount = 1;
    });

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "HETS REST API",
            Description = "Hired Equipment Tracking System"
        });
    });

    builder.Services.AddHealthChecks().AddNpgSql(
        connectionString, 
        name: "HETS-DB-Check", 
        failureStatus: HealthStatus.Degraded, 
        tags: new string[] { "postgresql", "db" });

    var app = builder.Build();

    // Use services here
    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseMiddleware<ExceptionMiddleware>();

    var healthCheckOptions = new HealthCheckOptions
    {
        ResponseWriter = async (c, r) =>
        {
            c.Response.ContentType = MediaTypeNames.Application.Json;
            var result = JsonSerializer.Serialize(
               new
               {
                   checks = r.Entries.Select(e =>
                      new {
                          description = e.Key,
                          status = e.Value.Status.ToString(),
                          tags = e.Value.Tags,
                          responseTime = e.Value.Duration.TotalMilliseconds
                      }),
                   totalResponseTime = r.TotalDuration.TotalMilliseconds
               });
            await c.Response.WriteAsync(result);
        }
    };

    app.UseHealthChecks("/healthz", healthCheckOptions);
    app.UseHangfireDashboard();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSerilogUi();
    app.MapControllers();
    app.UseSwagger();
    string swaggerApi = builder.Configuration.GetSection("Constants:SwaggerApiUrl").Value;
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(swaggerApi, "HETS REST API v1");
        options.DocExpansion(DocExpansion.None);
    });

    app.Run();
} 
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
} 
finally
{
    Log.CloseAndFlush();
}

// Retrieve database connection string
string GetConnectionString(IConfiguration configuration)
{
    string connectionString;
    string host = configuration["DATABASE_SERVICE_NAME"];
    string username = configuration["POSTGRESQL_USER"];
    string password = configuration["POSTGRESQL_PASSWORD"];
    string database = configuration["POSTGRESQL_DATABASE"];

    if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(database))
    {
        connectionString = configuration.GetConnectionString("HETS");
    }
    else
    {
        // environment variables override all other settings (OpenShift)
        connectionString = $"Host={host};Username={username};Password={password};Database={database}";
    }

    connectionString += ";Timeout=600;CommandTimeout=0;";

    return connectionString;
}
