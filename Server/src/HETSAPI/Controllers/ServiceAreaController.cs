using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
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
        /// <response code="200">ServiceArea created</response>
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
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult ServiceareasGet()
        {
            return _service.ServiceAreasGetAsync();
        }        
    }
}
