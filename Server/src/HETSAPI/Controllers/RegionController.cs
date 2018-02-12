using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
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
        /// <response code="201">Region created</response>
        [HttpPost]
        [Route("/api/regions/bulk")]
        [SwaggerOperation("RegionsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
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
        public virtual IActionResult RegionsGet()
        {
            return _service.RegionsGetAsync();
        }

        /// <summary>
        /// Delete region
        /// </summary>
        /// <param name="id">id of Region to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpPost]
        [Route("/api/regions/{id}/delete")]
        [SwaggerOperation("RegionsIdDeletePost")]
        public virtual IActionResult RegionsIdDeletePost([FromRoute]int id)
        {
            return _service.RegionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get districts associated with a region
        /// </summary>
        /// <remarks>Returns the districts for a specific region</remarks>
        /// <param name="id">id of Region for which to fetch the Districts</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/regions/{id}/districts")]
        [SwaggerOperation("RegionsIdDistrictsGet")]
        [SwaggerResponse(200, type: typeof(List<District>))]
        public virtual IActionResult RegionsIdDistrictsGet([FromRoute]int id)
        {
            return _service.RegionsIdDistrictsGetAsync(id);
        }

        /// <summary>
        /// Get region by id
        /// </summary>
        /// <param name="id">id of Region to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpGet]
        [Route("/api/regions/{id}")]
        [SwaggerOperation("RegionsIdGet")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsIdGet([FromRoute]int id)
        {
            return _service.RegionsIdGetAsync(id);
        }

        /// <summary>
        /// Update region
        /// </summary>
        /// <param name="id">id of Region to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpPut]
        [Route("/api/regions/{id}")]
        [SwaggerOperation("RegionsIdPut")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsIdPut([FromRoute]int id, [FromBody]Region item)
        {
            return _service.RegionsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create region
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Region created</response>
        [HttpPost]
        [Route("/api/regions")]
        [SwaggerOperation("RegionsPost")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsPost([FromBody]Region item)
        {
            return _service.RegionsPostAsync(item);
        }
    }
}
