using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Equipment Type Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeService _service;

        /// <summary>
        /// Equipment Type Controller Collection
        /// </summary>
        public EquipmentTypeController(IEquipmentTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk equipment type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentType created</response>
        [HttpPost]
        [Route("/api/equipmentTypes/bulk")]
        [SwaggerOperation("EquipmentTypesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult EquipmentTypesBulkPost([FromBody]EquipmentType[] items)
        {
            return _service.EquipmentTypesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all equipment types
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipmentTypes")]
        [SwaggerOperation("EquipmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentType>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult EquipmentTypesGet()
        {
            return _service.EquipmentTypesGetAsync();
        }        
    }
}
