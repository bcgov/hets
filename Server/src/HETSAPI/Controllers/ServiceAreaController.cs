using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Service Area Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ServiceAreaController : Controller
    {
        private readonly IServiceAreaService _service;

        /// <summary>
        /// Service Area Controller Constructor
        /// </summary>
        public ServiceAreaController(IServiceAreaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk service area records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">ServiceArea created</response>
        [HttpPost]
        [Route("/api/serviceareas/bulk")]
        [SwaggerOperation("ServiceareasBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult ServiceareasBulkPost([FromBody]ServiceArea[] items)
        {
            return _service.ServiceAreasBulkPostAsync(items);
        }

        /// <summary>
        /// Get all service areas
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/serviceareas")]
        [SwaggerOperation("ServiceareasGet")]
        [SwaggerResponse(200, type: typeof(List<ServiceArea>))]
        public virtual IActionResult ServiceareasGet()
        {
            return _service.ServiceAreasGetAsync();
        }

        /// <summary>
        /// Delete service area
        /// </summary>
        /// <param name="id">id of ServiceArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        [HttpPost]
        [Route("/api/serviceareas/{id}/delete")]
        [SwaggerOperation("ServiceareasIdDeletePost")]
        public virtual IActionResult ServiceareasIdDeletePost([FromRoute]int id)
        {
            return _service.ServiceAreasIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get service area by id
        /// </summary>
        /// <param name="id">id of ServiceArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        [HttpGet]
        [Route("/api/serviceareas/{id}")]
        [SwaggerOperation("ServiceareasIdGet")]
        [SwaggerResponse(200, type: typeof(ServiceArea))]
        public virtual IActionResult ServiceareasIdGet([FromRoute]int id)
        {
            return _service.ServiceAreasIdGetAsync(id);
        }

        /// <summary>
        /// Update service area
        /// </summary>
        /// <param name="id">id of ServiceArea to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        [HttpPut]
        [Route("/api/serviceareas/{id}")]
        [SwaggerOperation("ServiceareasIdPut")]
        [SwaggerResponse(200, type: typeof(ServiceArea))]
        public virtual IActionResult ServiceareasIdPut([FromRoute]int id, [FromBody]ServiceArea item)
        {
            return _service.ServiceAreasIdPutAsync(id, item);
        }

        /// <summary>
        /// Create service area
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">ServiceArea created</response>
        [HttpPost]
        [Route("/api/serviceareas")]
        [SwaggerOperation("ServiceareasPost")]
        [SwaggerResponse(200, type: typeof(ServiceArea))]
        public virtual IActionResult ServiceareasPost([FromBody]ServiceArea item)
        {
            return _service.ServiceAreasPostAsync(item);
        }
    }
}
