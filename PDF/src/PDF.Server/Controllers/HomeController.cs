using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PDF.Server.Models;

namespace PDF.Server.Controllers
{
    /// <summary>
    /// Default home controller for the HETSAPI
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
