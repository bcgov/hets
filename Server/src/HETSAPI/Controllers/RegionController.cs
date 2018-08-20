using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Region Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RegionController : Controller
    {
        private readonly IRegionService _service;

        /// <summary>
        /// Region Controller Constructor
        /// </summary>
        public RegionController(IRegionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk region records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Region created</response>
        [HttpPost]
        [Route("/api/regions/bulk")]
        [SwaggerOperation("RegionsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult RegionsBulkPost([FromBody]Region[] items)
        {
            return _service.RegionsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/regions")]
        [SwaggerOperation("RegionsGet")]
        [SwaggerResponse(200, type: typeof(List<Region>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RegionsGet()
        {
            return _service.RegionsGetAsync();
        }        
    }
}
