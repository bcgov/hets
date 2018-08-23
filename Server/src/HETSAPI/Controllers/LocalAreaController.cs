using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Local Area Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LocalAreaController : Controller
    {
        private readonly ILocalAreaService _service;

        /// <summary>
        /// Local Area Controller Constructor
        /// </summary>
        public LocalAreaController(ILocalAreaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk local area records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">LocalArea created</response>
        [HttpPost]
        [Route("/api/localAreas/bulk")]
        [SwaggerOperation("LocalAreasBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult LocalAreasBulkPost([FromBody]LocalArea[] items)
        {
            return _service.LocalAreasBulkPostAsync(items);
        }        
    }
}
