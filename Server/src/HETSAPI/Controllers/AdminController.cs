using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Administration Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        /// <summary>
        /// Administration Controller Constructor
        /// </summary>
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Start the import process
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <param name="districts">comma seperated list of district IDs to process.</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found in system</response>
        [HttpGet]
        [Route("/api/admin/import")]
        [SwaggerOperation("AdminImportGet")]
        public virtual IActionResult AdminImportGet([FromQuery]string path, [FromQuery]string districts)
        {
            return _service.AdminImportGetAsync(path, districts);
        }
    }
}
