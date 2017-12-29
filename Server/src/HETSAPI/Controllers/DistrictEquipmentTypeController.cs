using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DistrictEquipmentTypeController : Controller
    {
        private readonly IDistrictEquipmentTypeService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public DistrictEquipmentTypeController(IDistrictEquipmentTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        [HttpPost]
        [Route("/api/districtEquipmentTypes/bulk")]
        [SwaggerOperation("DistrictEquipmentTypesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult DistrictEquipmentTypesBulkPost([FromBody]DistrictEquipmentType[] items)
        {
            return this._service.DistrictEquipmentTypesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/districtEquipmentTypes")]
        [SwaggerOperation("DistrictEquipmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<DistrictEquipmentType>))]
        public virtual IActionResult DistrictEquipmentTypesGet()
        {
            return this._service.DistrictEquipmentTypesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        [HttpPost]
        [Route("/api/districtEquipmentTypes/{id}/delete")]
        [SwaggerOperation("DistrictEquipmentTypesIdDeletePost")]
        public virtual IActionResult DistrictEquipmentTypesIdDeletePost([FromRoute]int id)
        {
            return this._service.DistrictEquipmentTypesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        [HttpGet]
        [Route("/api/districtEquipmentTypes/{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdGet")]
        [SwaggerResponse(200, type: typeof(DistrictEquipmentType))]
        public virtual IActionResult DistrictEquipmentTypesIdGet([FromRoute]int id)
        {
            return this._service.DistrictEquipmentTypesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        [HttpPut]
        [Route("/api/districtEquipmentTypes/{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdPut")]
        [SwaggerResponse(200, type: typeof(DistrictEquipmentType))]
        public virtual IActionResult DistrictEquipmentTypesIdPut([FromRoute]int id, [FromBody]DistrictEquipmentType item)
        {
            return this._service.DistrictEquipmentTypesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        [HttpPost]
        [Route("/api/districtEquipmentTypes")]
        [SwaggerOperation("DistrictEquipmentTypesPost")]
        [SwaggerResponse(200, type: typeof(DistrictEquipmentType))]
        public virtual IActionResult DistrictEquipmentTypesPost([FromBody]DistrictEquipmentType item)
        {
            return this._service.DistrictEquipmentTypesPostAsync(item);
        }
    }
}
