using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// City Controller
    /// </summary>    
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProvincialRateTypeController : Controller
    {
        private readonly IProvincialRateTypeService _service;

        /// <summary>
        /// Provincial Rate Type Controller Constructor
        /// </summary>
        public ProvincialRateTypeController(IProvincialRateTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk provincial rate type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Provincial Rate Types created</response>
        [HttpPost]
        [Route("/api/provincialratetypes/bulk")]
        [SwaggerOperation("ProvincialRateTypesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult ProvincialRateTypesBulkPost([FromBody]ProvincialRateType[] items)
        {
            return _service.ProvincialRateTypesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all condition types
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/provincialratetypes")]
        [SwaggerOperation("ProvincialRateTypesGet")]
        [SwaggerResponse(200, type: typeof(List<ProvincialRateType>))]
        public virtual IActionResult ProvincialRateTypesGet()
        {
            return _service.ProvincialRateTypesGetAsync();
        }        
    }
}
