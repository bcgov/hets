using Microsoft.AspNetCore.Mvc;
using HETSCommon;
using FrontEnd.Model;

namespace FrontEnd.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    [Route("test")]
    public class TestController : Controller
    {
        /// <summary>
        /// Echo all request headers
        /// </summary>
        /// <returns>
        /// The request headers formatted as html
        /// </returns>
        [HttpGet]
        public virtual IActionResult Index()
        {
            TestModel home = new TestModel
            {
                Headers = Request.Headers.ToHtml()
            };

            return View(home);
        }
    }
}
