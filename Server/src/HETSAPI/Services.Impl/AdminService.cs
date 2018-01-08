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
        private readonly IConfiguration _configuration;
        private readonly Object _thisLock = new Object();

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public AdminService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult AdminImportGetAsync(string path, string districts)
        {
            string uploadPath = _configuration["UploadPath"];
            string connectionString = _context.Database.GetDbConnection().ConnectionString;
            string result = "Created Job: ";

            lock (_thisLock)
            {
                if (districts != null && districts == "388888")
                {
                    // not using Hangfire
                    BCBidImport.ImportJob(null, connectionString, uploadPath + path);
                }
                else
                {
                    // use Hangfire
                    string jobId = BackgroundJob.Enqueue(() => BCBidImport.ImportJob(null, connectionString, uploadPath + path));
                    result += jobId;
                }
            }

            return new ObjectResult(result);
        }        
    }
}
