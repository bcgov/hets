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
    public partial class EquipmentAttachmentController : Controller
    {
        private readonly IEquipmentAttachmentService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public EquipmentAttachmentController(IEquipmentAttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentAttachment created</response>
        [HttpPost]
        [Route("/api/equipmentAttachments/bulk")]
        [SwaggerOperation("EquipmentAttachmentsBulkPost")]
        public virtual IActionResult EquipmentAttachmentsBulkPost([FromBody]EquipmentAttachment[] items)
        {
            return this._service.EquipmentAttachmentsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipmentAttachments")]
        [SwaggerOperation("EquipmentAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAttachment>))]
        public virtual IActionResult EquipmentAttachmentsGet()
        {
            return this._service.EquipmentAttachmentsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        [HttpPost]
        [Route("/api/equipmentAttachments/{id}/delete")]
        [SwaggerOperation("EquipmentAttachmentsIdDeletePost")]
        public virtual IActionResult EquipmentAttachmentsIdDeletePost([FromRoute]int id)
        {
            return this._service.EquipmentAttachmentsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        [HttpGet]
        [Route("/api/equipmentAttachments/{id}")]
        [SwaggerOperation("EquipmentAttachmentsIdGet")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        public virtual IActionResult EquipmentAttachmentsIdGet([FromRoute]int id)
        {
            return this._service.EquipmentAttachmentsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        [HttpPut]
        [Route("/api/equipmentAttachments/{id}")]
        [SwaggerOperation("EquipmentAttachmentsIdPut")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        public virtual IActionResult EquipmentAttachmentsIdPut([FromRoute]int id, [FromBody]EquipmentAttachment item)
        {
            return this._service.EquipmentAttachmentsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentAttachment created</response>
        [HttpPost]
        [Route("/api/equipmentAttachments")]
        [SwaggerOperation("EquipmentAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        public virtual IActionResult EquipmentAttachmentsPost([FromBody]EquipmentAttachment item)
        {
            return this._service.EquipmentAttachmentsPostAsync(item);
        }
    }
}
