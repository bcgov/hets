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
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="types">Equipment Types (array of id numbers)</param>
        /// <param name="attachments">Equipment Attachments (array of id numbers)</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentViewModel>))]
        public virtual IActionResult EquipmentSearchGet([FromQuery]int?[] localareas, [FromQuery]int?[] types, [FromQuery]int?[] attachments, [FromQuery]int? owner, [FromQuery]string status, [FromQuery]bool? hired, [FromQuery]DateTime? notverifiedsincedate)
        {
            return this._service.EquipmentSearchGetAsync(localareas, types, attachments, owner, status, hired, notverifiedsincedate);
        }
    }
}
