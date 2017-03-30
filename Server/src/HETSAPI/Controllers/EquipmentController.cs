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
    public partial class EquipmentController : Controller
    {
        private readonly IEquipmentService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public EquipmentController(IEquipmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
        [HttpPost]
        [Route("/api/equipment/bulk")]
        [SwaggerOperation("EquipmentBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult EquipmentBulkPost([FromBody]Equipment[] items)
        {
            return this._service.EquipmentBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment")]
        [SwaggerOperation("EquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<Equipment>))]
        public virtual IActionResult EquipmentGet()
        {
            return this._service.EquipmentGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpGet]
        [Route("/api/equipment/{id}/attachments")]
        [SwaggerOperation("EquipmentIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        public virtual IActionResult EquipmentIdAttachmentsGet([FromRoute]int id)
        {
            return this._service.EquipmentIdAttachmentsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpPost]
        [Route("/api/equipment/{id}/delete")]
        [SwaggerOperation("EquipmentIdDeletePost")]
        public virtual IActionResult EquipmentIdDeletePost([FromRoute]int id)
        {
            return this._service.EquipmentIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/equipmentattachments")]
        [SwaggerOperation("EquipmentIdEquipmentattachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAttachment>))]
        public virtual IActionResult EquipmentIdEquipmentattachmentsGet([FromRoute]int id)
        {
            return this._service.EquipmentIdEquipmentattachmentsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpGet]
        [Route("/api/equipment/{id}")]
        [SwaggerOperation("EquipmentIdGet")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentIdGet([FromRoute]int id)
        {
            return this._service.EquipmentIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult EquipmentIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return this._service.EquipmentIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/equipment/{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryPost")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult EquipmentIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return this._service.EquipmentIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpPut]
        [Route("/api/equipment/{id}")]
        [SwaggerOperation("EquipmentIdPut")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentIdPut([FromRoute]int id, [FromBody]Equipment item)
        {
            return this._service.EquipmentIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/view")]
        [SwaggerOperation("EquipmentIdViewGet")]
        [SwaggerResponse(200, type: typeof(EquipmentViewModel))]
        public virtual IActionResult EquipmentIdViewGet([FromRoute]int id)
        {
            return this._service.EquipmentIdViewGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        [HttpPost]
        [Route("/api/equipment")]
        [SwaggerOperation("EquipmentPost")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentPost([FromBody]Equipment item)
        {
            return this._service.EquipmentPostAsync(item);
        }

        /// <summary>
        /// Recalculates seniority for the database
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records.</remarks>
        /// <param name="region">Region to recalculate</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/recalcSeniority")]
        [SwaggerOperation("EquipmentRecalcSeniorityGet")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult EquipmentRecalcSeniorityGet([FromQuery]int region)
        {
            return this._service.EquipmentRecalcSeniorityGetAsync(region);
        }

        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="equipmentAttachment">Searches equipmentAttachment type</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentViewModel>))]
        public virtual IActionResult EquipmentSearchGet([FromQuery]string localareas, [FromQuery]string types, [FromQuery]string equipmentAttachment, [FromQuery]int? owner, [FromQuery]string status, [FromQuery]bool? hired, [FromQuery]DateTime? notverifiedsincedate)
        {
            return this._service.EquipmentSearchGetAsync(localareas, types, equipmentAttachment, owner, status, hired, notverifiedsincedate);
        }
    }
}
