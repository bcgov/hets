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

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentAttachmentTypeController : Controller
    {
        private readonly IEquipmentAttachmentTypeService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public EquipmentAttachmentTypeController(IEquipmentAttachmentTypeService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentAttachmentType created</response>
        [HttpPost]
        [Route("/api/equipmentAttachmentTypes/bulk")]
        [SwaggerOperation("EquipmentAttachmentTypesBulkPost")]
        public virtual IActionResult EquipmentAttachmentTypesBulkPost([FromBody]EquipmentAttachmentType[] items)
        {
            return this._service.EquipmentAttachmentTypesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipmentAttachmentTypes")]
        [SwaggerOperation("EquipmentAttachmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAttachmentType>))]
        public virtual IActionResult EquipmentAttachmentTypesGet()
        {
            return this._service.EquipmentAttachmentTypesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        [HttpPost]
        [Route("/api/equipmentAttachmentTypes/{id}/delete")]
        [SwaggerOperation("EquipmentAttachmentTypesIdDeletePost")]
        public virtual IActionResult EquipmentAttachmentTypesIdDeletePost([FromRoute]int id)
        {
            return this._service.EquipmentAttachmentTypesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        [HttpGet]
        [Route("/api/equipmentAttachmentTypes/{id}")]
        [SwaggerOperation("EquipmentAttachmentTypesIdGet")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachmentType))]
        public virtual IActionResult EquipmentAttachmentTypesIdGet([FromRoute]int id)
        {
            return this._service.EquipmentAttachmentTypesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        [HttpPut]
        [Route("/api/equipmentAttachmentTypes/{id}")]
        [SwaggerOperation("EquipmentAttachmentTypesIdPut")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachmentType))]
        public virtual IActionResult EquipmentAttachmentTypesIdPut([FromRoute]int id, [FromBody]EquipmentAttachmentType item)
        {
            return this._service.EquipmentAttachmentTypesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentAttachmentType created</response>
        [HttpPost]
        [Route("/api/equipmentAttachmentTypes")]
        [SwaggerOperation("EquipmentAttachmentTypesPost")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachmentType))]
        public virtual IActionResult EquipmentAttachmentTypesPost([FromBody]EquipmentAttachmentType item)
        {
            return this._service.EquipmentAttachmentTypesPostAsync(item);
        }
    }
}
