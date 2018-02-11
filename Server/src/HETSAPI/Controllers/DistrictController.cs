using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
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
        [RequiresPermission(Permission.ADMIN)]
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
        public virtual IActionResult DistrictsGet()
        {
            return _service.DistrictsGetAsync();
        }

        /// <summary>
        /// Delete district
        /// </summary>
        /// <param name="id">id of District to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        [HttpPost]
        [Route("/api/districts/{id}/delete")]
        [SwaggerOperation("DistrictsIdDeletePost")]
        public virtual IActionResult DistrictsIdDeletePost([FromRoute]int id)
        {
            return _service.DistrictsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get district by id
        /// </summary>
        /// <param name="id">id of District to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        [HttpGet]
        [Route("/api/districts/{id}")]
        [SwaggerOperation("DistrictsIdGet")]
        [SwaggerResponse(200, type: typeof(District))]
        public virtual IActionResult DistrictsIdGet([FromRoute]int id)
        {
            return _service.DistrictsIdGetAsync(id);
        }

        /// <summary>
        /// Update district
        /// </summary>
        /// <param name="id">id of District to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        [HttpPut]
        [Route("/api/districts/{id}")]
        [SwaggerOperation("DistrictsIdPut")]
        [SwaggerResponse(200, type: typeof(District))]
        public virtual IActionResult DistrictsIdPut([FromRoute]int id, [FromBody]District item)
        {
            return _service.DistrictsIdPutAsync(id, item);
        }

        /// <summary>
        /// Get service areas associated with a district
        /// </summary>
        /// <remarks>Returns the Service Areas for a specific region</remarks>
        /// <param name="id">id of District for which to fetch the ServiceAreas</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/districts/{id}/serviceareas")]
        [SwaggerOperation("DistrictsIdServiceareasGet")]
        [SwaggerResponse(200, type: typeof(List<ServiceArea>))]
        public virtual IActionResult DistrictsIdServiceareasGet([FromRoute]int id)
        {
            return _service.DistrictsIdServiceareasGetAsync(id);
        }

        /// <summary>
        /// Create district
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">District created</response>
        [HttpPost]
        [Route("/api/districts")]
        [SwaggerOperation("DistrictsPost")]
        [SwaggerResponse(200, type: typeof(District))]
        public virtual IActionResult DistrictsPost([FromBody]District item)
        {
            return _service.DistrictsPostAsync(item);
        }

        /// <summary>
        /// Get all owners by distict (minimal data returned) - lookup
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/district/{id}/owners")]
        [SwaggerOperation("DistrictOwnersGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        public virtual IActionResult DistrictOwnersGet([FromRoute]int id)
        {
            return _service.DistrictOwnersGetAsync(id);
        }
    }
}
