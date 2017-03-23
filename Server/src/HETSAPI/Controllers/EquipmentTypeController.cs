/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentTypeController : Controller
    {
        private readonly IEquipmentTypeService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public EquipmentTypeController(IEquipmentTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentType created</response>
        [HttpPost]
        [Route("/api/equipmentTypes/bulk")]
        [SwaggerOperation("EquipmentTypesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult EquipmentTypesBulkPost([FromBody]EquipmentType[] items)
        {
            return this._service.EquipmentTypesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipmentTypes")]
        [SwaggerOperation("EquipmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentType>))]
        public virtual IActionResult EquipmentTypesGet()
        {
            return this._service.EquipmentTypesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        [HttpPost]
        [Route("/api/equipmentTypes/{id}/delete")]
        [SwaggerOperation("EquipmentTypesIdDeletePost")]
        public virtual IActionResult EquipmentTypesIdDeletePost([FromRoute]int id)
        {
            return this._service.EquipmentTypesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
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
            return this._service.EquipmentTypesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        [HttpPut]
        [Route("/api/equipmentTypes/{id}")]
        [SwaggerOperation("EquipmentTypesIdPut")]
        [SwaggerResponse(200, type: typeof(EquipmentType))]
        public virtual IActionResult EquipmentTypesIdPut([FromRoute]int id, [FromBody]EquipmentType item)
        {
            return this._service.EquipmentTypesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentType created</response>
        [HttpPost]
        [Route("/api/equipmentTypes")]
        [SwaggerOperation("EquipmentTypesPost")]
        [SwaggerResponse(200, type: typeof(EquipmentType))]
        public virtual IActionResult EquipmentTypesPost([FromBody]EquipmentType item)
        {
            return this._service.EquipmentTypesPostAsync(item);
        }
    }
}
