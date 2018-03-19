using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using Hangfire;
using HETSAPI.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Admin Service
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

        public IActionResult AdminImportGetAsync(string path, bool realTime)
        {
            string result;

            lock (_thisLock)
            {
                try
                {                
                    string uploadPath = _configuration["UploadPath"];
                    string seniorityScoringRules = _configuration["SeniorityScoringRules"];
                    string connectionString = _context.Database.GetDbConnection().ConnectionString;                    

                    if (realTime)
                    {
                        // not using Hangfire
                        BcBidImport.ImportJob(null, seniorityScoringRules, connectionString, uploadPath + path);
                        result = "Import complete";
                    }
                    else
                    {
                        // use Hangfire
                        result = "Created Job: ";
                        string jobId = BackgroundJob.Enqueue(() => BcBidImport.ImportJob(null, seniorityScoringRules, connectionString, uploadPath + path));
                        result += jobId;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result = @"*** Import Error ***: " + e.Message;
                }
            }

            return new ObjectResult(result);
        }

        public IActionResult AdminObfuscateGetAsync(string sourcePath, string destinationPath)
        {
            string result = "Created Obfuscation Job: ";

            lock (_thisLock)
            {
                string uploadPath = _configuration["UploadPath"];
                string connectionString = _context.Database.GetDbConnection().ConnectionString;

                ImportUtility.CreateObfuscationDestination(uploadPath + destinationPath);

                // use Hangfire
                string jobId = BackgroundJob.Enqueue(() => BcBidImport.ObfuscationJob(null, connectionString, uploadPath + sourcePath, uploadPath + destinationPath));
                result += jobId;                
            }

            return new ObjectResult(result);
        }

        public IActionResult GetSpreadsheet(string path, string filename)
        {
            // create an excel spreadsheet that will show the data       
            lock (_thisLock)
            {
                if (_configuration != null)
                {
                    string uploadPath = _configuration["UploadPath"];
                    string fullPath = Path.Combine(uploadPath + path, filename);

                    MemoryStream memory = new MemoryStream();

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        stream.CopyToAsync(memory).Wait();
                    }

                    memory.Position = 0;

                    FileStreamResult fileStreamResult = new FileStreamResult(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = filename
                    };

                    return fileStreamResult;
                }
            }

            return null;
        }        
    }
}
