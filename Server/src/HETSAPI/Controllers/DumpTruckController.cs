using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Dump Truck Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DumpTruckController : Controller
    {
        private readonly IDumpTruckService _service;

        /// <summary>
        /// Dump Truck Controller Constructor
        /// </summary>
        public DumpTruckController(IDumpTruckService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk dump truck records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
        [HttpPost]
        [Route("/api/dumptrucks/bulk")]
        [SwaggerOperation("DumptrucksBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult DumptrucksBulkPost([FromBody]DumpTruck[] items)
        {
            return _service.DumptrucksBulkPostAsync(items);
        }

        /// <summary>
        /// Get all dump trucks
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/dumptrucks")]
        [SwaggerOperation("DumptrucksGet")]
        [SwaggerResponse(200, type: typeof(List<DumpTruck>))]
        public virtual IActionResult DumptrucksGet()
        {
            return _service.DumptrucksGetAsync();
        }

        /// <summary>
        /// Delete dump truck
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpPost]
        [Route("/api/dumptrucks/{id}/delete")]
        [SwaggerOperation("DumptrucksIdDeletePost")]
        public virtual IActionResult DumptrucksIdDeletePost([FromRoute]int id)
        {
            return _service.DumptrucksIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get dump truck by id
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpGet]
        [Route("/api/dumptrucks/{id}")]
        [SwaggerOperation("DumptrucksIdGet")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksIdGet([FromRoute]int id)
        {
            return _service.DumptrucksIdGetAsync(id);
        }

        /// <summary>
        /// Update dump truck
        /// </summary>
        /// <param name="id">id of DumpTruck to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpPut]
        [Route("/api/dumptrucks/{id}")]
        [SwaggerOperation("DumptrucksIdPut")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksIdPut([FromRoute]int id, [FromBody]DumpTruck item)
        {
            return _service.DumptrucksIdPutAsync(id, item);
        }

        /// <summary>
        /// Create dump truck
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        [HttpPost]
        [Route("/api/dumptrucks")]
        [SwaggerOperation("DumptrucksPost")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksPost([FromBody]DumpTruck item)
        {
            return _service.DumptrucksPostAsync(item);
        }
    }
}
