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
    public partial class RentalRequestAttachmentController : Controller
    {
        private readonly IRentalRequestAttachmentService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalRequestAttachmentController(IRentalRequestAttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments/bulk")]
        [SwaggerOperation("RentalrequestattachmentsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalrequestattachmentsBulkPost([FromBody]RentalRequestAttachment[] items)
        {
            return this._service.RentalrequestattachmentsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequestattachments")]
        [SwaggerOperation("RentalrequestattachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestAttachment>))]
        public virtual IActionResult RentalrequestattachmentsGet()
        {
            return this._service.RentalrequestattachmentsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments/{id}/delete")]
        [SwaggerOperation("RentalrequestattachmentsIdDeletePost")]
        public virtual IActionResult RentalrequestattachmentsIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalrequestattachmentsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpGet]
        [Route("/api/rentalrequestattachments/{id}")]
        [SwaggerOperation("RentalrequestattachmentsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsIdGet([FromRoute]int id)
        {
            return this._service.RentalrequestattachmentsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpPut]
        [Route("/api/rentalrequestattachments/{id}")]
        [SwaggerOperation("RentalrequestattachmentsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsIdPut([FromRoute]int id, [FromBody]RentalRequestAttachment item)
        {
            return this._service.RentalrequestattachmentsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments")]
        [SwaggerOperation("RentalrequestattachmentsPost")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsPost([FromBody]RentalRequestAttachment item)
        {
            return this._service.RentalrequestattachmentsPostAsync(item);
        }
    }
}
