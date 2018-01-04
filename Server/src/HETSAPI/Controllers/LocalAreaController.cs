using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
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
        /// <response code="201">LocalArea created</response>
        [HttpPost]
        [Route("/api/localAreas/bulk")]
        [SwaggerOperation("LocalAreasBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
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
        public virtual IActionResult LocalAreasGet()
        {
            return _service.LocalAreasGetAsync();
        }

        /// <summary>
        /// Delete local area
        /// </summary>
        /// <param name="id">id of LocalArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpPost]
        [Route("/api/localAreas/{id}/delete")]
        [SwaggerOperation("LocalAreasIdDeletePost")]
        public virtual IActionResult LocalAreasIdDeletePost([FromRoute]int id)
        {
            return _service.LocalAreasIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get local area by id
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpGet]
        [Route("/api/localAreas/{id}")]
        [SwaggerOperation("LocalAreasIdGet")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasIdGet([FromRoute]int id)
        {
            return _service.LocalAreasIdGetAsync(id);
        }

        /// <summary>
        /// Update local area
        /// </summary>
        /// <param name="id">id of LocalArea to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpPut]
        [Route("/api/localAreas/{id}")]
        [SwaggerOperation("LocalAreasIdPut")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasIdPut([FromRoute]int id, [FromBody]LocalArea item)
        {
            return _service.LocalAreasIdPutAsync(id, item);
        }

        /// <summary>
        /// Create local area
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalArea created</response>
        [HttpPost]
        [Route("/api/localAreas")]
        [SwaggerOperation("LocalAreasPost")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasPost([FromBody]LocalArea item)
        {
            return _service.LocalAreasPostAsync(item);
        }
    }
}
