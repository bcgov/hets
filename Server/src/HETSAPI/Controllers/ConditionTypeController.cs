using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Condition Type Controller
    /// </summary>    
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ConditionTypeController : Controller
    {
        private readonly IConditionTypeService _service;

        /// <summary>
        /// City Controller Constructor
        /// </summary>
        public ConditionTypeController(IConditionTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk condition type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Condition Types created</response>
        [HttpPost]
        [Route("/api/conditiontypes/bulk")]
        [SwaggerOperation("ConditionTypesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult ConditionTypesBulkPost([FromBody]ConditionType[] items)
        {
            return _service.ConditionTypesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all condition types
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/conditiontypes")]
        [SwaggerOperation("ConditionTypesGet")]
        [SwaggerResponse(200, type: typeof(List<ConditionType>))]
        public virtual IActionResult ConditionTypesGet()
        {
            return _service.ConditionTypesGetAsync();
        }        
    }
}
