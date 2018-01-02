using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using Hangfire;
using HETSAPI.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminService : ServiceBase, IAdminService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration Configuration;
        private readonly Object thisLock = new Object();

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public AdminService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult AdminImportGetAsync(string path, string districts)
        {
            string uploadPath = Configuration["UploadPath"];
            string connectionString = _context.Database.GetDbConnection().ConnectionString;
            var result = "Created Job: ";
            lock (thisLock)
            {
                if (districts != null && districts == "388888")
                {
                    //Not using Hangfire
                    BCBidImport.ImportJob(null, connectionString, uploadPath + path);
                }
                else
                {
                    //Use Hangfire
                    var jobId = BackgroundJob.Enqueue(() => BCBidImport.ImportJob(null, connectionString, uploadPath + path));
                    result += jobId;
                }
            }
            return new ObjectResult(result);
        }        
    }
}
