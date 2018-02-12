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
        [RequiresPermission(Permission.ADMIN)]
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
        public virtual IActionResult EquipmentTypesGet()
        {
            return _service.EquipmentTypesGetAsync();
        }

        /// <summary>
        /// Delete equipment type
        /// </summary>
        /// <param name="id">id of EquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        [HttpPost]
        [Route("/api/equipmentTypes/{id}/delete")]
        [SwaggerOperation("EquipmentTypesIdDeletePost")]
        public virtual IActionResult EquipmentTypesIdDeletePost([FromRoute]int id)
        {
            return _service.EquipmentTypesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get equipment type by id
        /// </summary>
        /// <param name="id">id of EquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        [HttpGet]
        [Route("/api/equipmentTypes/{id}")]
        [SwaggerOperation("EquipmentTypesIdGet")]
        [SwaggerResponse(200, type: typeof(EquipmentType))]
        public virtual IActionResult EquipmentTypesIdGet([FromRoute]int id)
        {
            return _service.EquipmentTypesIdGetAsync(id);
        }

        /// <summary>
        /// Update equipment type
        /// </summary>
        /// <param name="id">id of EquipmentType to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        [HttpPut]
        [Route("/api/equipmentTypes/{id}")]
        [SwaggerOperation("EquipmentTypesIdPut")]
        [SwaggerResponse(200, type: typeof(EquipmentType))]
        public virtual IActionResult EquipmentTypesIdPut([FromRoute]int id, [FromBody]EquipmentType item)
        {
            return _service.EquipmentTypesIdPutAsync(id, item);
        }

        /// <summary>
        /// Create equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentType created</response>
        [HttpPost]
        [Route("/api/equipmentTypes")]
        [SwaggerOperation("EquipmentTypesPost")]
        [SwaggerResponse(200, type: typeof(EquipmentType))]
        public virtual IActionResult EquipmentTypesPost([FromBody]EquipmentType item)
        {
            return _service.EquipmentTypesPostAsync(item);
        }
    }
}
