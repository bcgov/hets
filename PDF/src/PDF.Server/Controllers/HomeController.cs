using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Pdf.Server.Models;

namespace Pdf.Server.Controllers
{
    /// <summary>
    /// Default home controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Authentication Controller Constructor
        /// </summary>
        /// <param name="env"></param>
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Default action
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            HomeModel home = new HomeModel
            {
                DevelopmentEnvironment = _env.IsDevelopment()
            };

            return View(home);
        }
    }
}
