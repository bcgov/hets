using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Import Controller
    /// </summary>
    [Route("/api")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ImportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IHostingEnvironment _env;

        public ImportController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _env = env;                        
        }

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        [Route("/Import")]
        [RequiresPermission(HetPermission.ImportData)]

        public IActionResult Index()
        {
            string path = GetServerPath(_httpContext, _env);
            path = path.Replace("import", "");

            HomeModel home = new HomeModel
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
        [Route("/UploadPost")]
        [RequiresPermission(HetPermission.ImportData)]
        public IActionResult UploadPost(IList<IFormFile> files)
        {
            string path = GetServerPath(_httpContext, _env);
            path = path.Replace("UploadPost", "");

            // get the upload path from the app configuration
            string uploadPath = _configuration["UploadPath"];

            HomeModel home = UploadHelper.UploadFiles(files, uploadPath);

            home.UserId = HttpContext.User.Identity.Name;
            home.DevelopmentEnvironment = _env.IsDevelopment();
            home.Action = path + "UploadPost";

            return View("Index", home);
        }

        /// <summary>
        /// Returns Uri (as string)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        private static string GetServerPath(HttpContext context, IHostingEnvironment env)
        {
            string path = context.Request.Path.ToString().ToLower();

            if (path.EndsWith(@"/"))
            {
                path = path.Substring(0, path.Length - 1);
            }

            if (!path.StartsWith(@"/"))
            {
                path = @"/" + path;
            }

            if (env.IsProduction())
            {
                path = "/hets" + path;
            }

            return path;
        }
    }
}
