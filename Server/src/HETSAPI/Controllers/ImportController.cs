using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using HETSAPI.Authorization;
using HETSAPI.Helpers;
using HETSAPI.Models;
using HETSAPI.Services.Impl;
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// This controller is used to handle importing data from the previous system (BC Bid)
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ImportController : Controller
    {       
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Import Controller Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public ImportController(IHostingEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            HomeViewModel home = new HomeViewModel
            {
                UserId = HttpContext.User.Identity.Name,
                DevelopmentEnvironment = _env.IsDevelopment()
            };

            return View(home);
        }

        /// <summary>
        /// Receives uploaded files
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>      
        [RequiresPermission(Permission.ImportData)]
        public IActionResult UploadPost(IList<IFormFile> files)
        {
            // get the upload path from the app configuration
            string uploadPath = _configuration["UploadPath"];

            HomeViewModel home = UploadHelper.UploadFiles(files, uploadPath);

            home.UserId = HttpContext.User.Identity.Name;
            home.DevelopmentEnvironment = _env.IsDevelopment();

            return View("Index", home);
        }        
    }
}

