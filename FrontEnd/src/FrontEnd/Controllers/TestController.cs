using Microsoft.AspNetCore.Mvc;
using HETSCommon;

namespace FrontEnd.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    [Route("test")]
    [Route("hets/test")]
    public class TestController : Controller
    {
        /// <summary>
        /// Echos request headers
        /// </summary>
        /// <returns>
        /// The request headers formatted as html
        /// </returns>
        [HttpGet]
        [Route("headers")]
        [Produces("text/html")]
        public virtual IActionResult EchoHeaders()
        {
            return Ok(Request.Headers.ToHtml());
        }
    }
}
