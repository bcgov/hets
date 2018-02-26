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
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult DumptrucksBulkPost([FromBody]DumpTruck[] items)
        {
            return _service.DumptrucksBulkPostAsync(items);
        }        
    }
}
