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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
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
        private Object thisLock = new Object();

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
