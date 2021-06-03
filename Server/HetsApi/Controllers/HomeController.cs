using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using HetsApi.Model;
using Microsoft.Extensions.Hosting;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Default home controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Authentication Controller Constructor
        /// </summary>
        /// <param name="env"></param>
        public HomeController(IWebHostEnvironment env)
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
                UserId = HttpContext.User.Identity.Name,
                DevelopmentEnvironment = _env.IsDevelopment()
            };

            return View(home);
        }
    }
}
