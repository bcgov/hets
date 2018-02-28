using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
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

        /// <summary>
        /// Get all local areas
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/localAreas")]
        [SwaggerOperation("LocalAreasGet")]
        [SwaggerResponse(200, type: typeof(List<LocalArea>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult LocalAreasGet()
        {
            return _service.LocalAreasGetAsync();
        }        
    }
}
