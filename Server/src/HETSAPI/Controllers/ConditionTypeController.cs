using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
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
        /// <response code="200">Condition Types created</response>
        [HttpPost]
        [Route("/api/conditiontypes/bulk")]
        [SwaggerOperation("ConditionTypesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult ConditionTypesBulkPost([FromBody]ConditionType[] items)
        {
            return _service.ConditionTypesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all condition types - filtered by user's District
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/conditiontypes")]
        [SwaggerOperation("ConditionTypesGet")]
        [RequiresPermission(Permission.Login)]
        [SwaggerResponse(200, type: typeof(List<ConditionType>))]
        public virtual IActionResult ConditionTypesGet()
        {
            return _service.ConditionTypesGetAsync();
        }

        /// <summary>
        /// Get a specific contition record
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/conditiontypes/{id}")]
        [SwaggerOperation("ConditionTypesIdGet")]
        [SwaggerResponse(200, type: typeof(ConditionType))]
        [RequiresPermission(Permission.DistrictCodeTableManagement)]
        public virtual IActionResult ConditionTypesIdGet([FromRoute]int id)
        {
            return _service.ConditionTypesIdGetAsync(id);
        }

        /// <summary>
        /// Create or update a Condition Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <response code="200">Condition Type created or updated</response>
        [HttpPost]
        [Route("/api/conditiontypes/{id}")]
        [SwaggerOperation("ConditionTypesIdPost")]
        [SwaggerResponse(200, type: typeof(ConditionType))]
        [RequiresPermission(Permission.DistrictCodeTableManagement)]
        public virtual IActionResult ConditionTypesIdPost([FromRoute]int id, [FromBody]ConditionType item)
        {
            return _service.ConditionTypesIdPostAsync(id, item);
        }
    }
}
