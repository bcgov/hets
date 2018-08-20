using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// District Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DistrictController : Controller
    {
        private readonly IDistrictService _service;

        /// <summary>
        /// District Controller Constructor
        /// </summary>
        public DistrictController(IDistrictService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk district records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">District created</response>
        [HttpPost]
        [Route("/api/districts/bulk")]
        [SwaggerOperation("DistrictsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult DistrictsBulkPost([FromBody]District[] items)
        {
            return _service.DistrictsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all districts
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/districts")]
        [SwaggerOperation("DistrictsGet")]
        [SwaggerResponse(200, type: typeof(List<District>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult DistrictsGet()
        {
            return _service.DistrictsGetAsync();
        }        

        /// <summary>
        /// Get all owners by distict (minimal data returned) - lookup
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/district/{id}/owners")]
        [SwaggerOperation("DistrictOwnersGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult DistrictOwnersGet([FromRoute]int id)
        {
            return _service.DistrictOwnersGetAsync(id);
        }

        /// <summary>
        /// Get all local areas by distict (minimal data returned) - lookup
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/district/{id}/localAreas")]
        [SwaggerOperation("DistrictLocalAreasGet")]
        [SwaggerResponse(200, type: typeof(List<LocalArea>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult DistrictLocalAreasGet([FromRoute]int id)
        {
            return _service.DistrictLocalAreasGetAsync(id);
        }
    }
}
