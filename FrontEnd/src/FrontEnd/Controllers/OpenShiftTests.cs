using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    public class OpenShiftTestsController : Controller
    {
        private const string Result = "1";

        private readonly ILogger _logger;

        /// <summary>
        /// Test Controller Constructor
        /// </summary>
        public OpenShiftTestsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(OpenShiftTestsController));
        }
	
        /// <summary>
        /// Check for proxy readiness
        /// </summary>
        /// <remarks>Returns a list of regions for a given province</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/readinessProbe")]
        public virtual IActionResult DoReadinessProbe()
        {
            _logger.LogInformation("[DoReadinessProbe] Result: " + Result);
            return new ObjectResult(Result);
        }

        /// <summary>
        /// Check for proxy "live"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/livenessProbe")]
        public virtual IActionResult DoLivenessProbe()
        {
            _logger.LogInformation("[DoLivenessProbe] Result: " + Result);
            return new ObjectResult(Result);
        }
    }
}
