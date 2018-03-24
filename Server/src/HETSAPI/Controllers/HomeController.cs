using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using HETSAPI.ViewModels;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Default home controller for the HETSAPI
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiExplorerSettings(IgnoreApi = true)]
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
            HomeViewModel home = new HomeViewModel
            {
                UserId = HttpContext.User.Identity.Name,
                DevelopmentEnvironment = _env.IsDevelopment()
            };

            return View(home);
        }
    }
}
