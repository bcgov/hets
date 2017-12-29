using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using HETSAPI.Models.Impl;


namespace HETSAPI.Controllers
{
    /// <summary>
    /// Error Controller for HETS API Application
    /// </summary>
    public class ErrorController : Controller
    {
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// AuthenticationController Constructor
        /// </summary>
        /// <param name="env"></param>
        public ErrorController(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeModel home = new HomeModel
            {
                DevelopmentEnvironment = _env.IsDevelopment()
            };

            if (HttpContext == null) return View(home);

            home.UserId = HttpContext.User.Identity.Name;
            IExceptionHandlerFeature feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            home.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            home.Message = feature?.Error.Message;

            return View(home);
        }
    }
}