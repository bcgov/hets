using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using HETSAPI.Authorization;
using HETSAPI.Helpers;
using HETSAPI.Models;
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
        private readonly HttpContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Import Controller Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public ImportController(IHttpContextAccessor httpContextAccessor, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = httpContextAccessor.HttpContext;
            _env = env;
            _configuration = configuration;
        }

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        [Route("/api/Import")]
        [RequiresPermission(Permission.ImportData)]
        public IActionResult Index()
        {
            string path = GetServerPath(_context);            
            path = path.Replace("import", "");            

            HomeViewModel home = new HomeViewModel
            {
                UserId = HttpContext.User.Identity.Name,
                DevelopmentEnvironment = _env.IsDevelopment(),
                Action = path + "UploadPost"
            };

            return View(home);
        }

        /// <summary>
        /// Receives uploaded files
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>      
        [Route("/api/UploadPost")]
        [RequiresPermission(Permission.ImportData)]
        public IActionResult UploadPost(IList<IFormFile> files)
        {
            string path = GetServerPath(_context);            
            path = path.Replace("uploadpost", "");
            
            // get the upload path from the app configuration
            string uploadPath = _configuration["UploadPath"];

            HomeViewModel home = UploadHelper.UploadFiles(files, uploadPath);

            home.UserId = HttpContext.User.Identity.Name;
            home.DevelopmentEnvironment = _env.IsDevelopment();
            home.Action = path + "UploadPost";

            return View("Index", home);
        }

        /// <summary>
        /// Returns Uri (as string)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetServerPath(HttpContext context)
        {
            int? port = context.Request.Host.Port;

            UriBuilder uriBuilder = port != null ? 
                new UriBuilder(context.Request.Scheme, context.Request.Host.Host, (int)port) : 
                new UriBuilder(context.Request.Scheme, context.Request.Host.Host);

            string tempUri = uriBuilder.Uri.ToString().ToLower();

            if(tempUri.EndsWith(@"/"))
            {
                tempUri = tempUri.Substring(0, tempUri.Length - 1);
            }

            tempUri = tempUri + context.Request.Path.ToString().ToLower();            

            return tempUri;
        }
    }
}
