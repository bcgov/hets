using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AdminController : Controller
    {
        private readonly IAdminService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Starts the import process
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
            return this._service.AdminImportGetAsync(path, districts);
        }
    }
}
