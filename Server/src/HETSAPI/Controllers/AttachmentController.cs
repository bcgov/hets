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
    public partial class AttachmentController : Controller
    {
        private readonly IAttachmentService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public AttachmentController(IAttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Attachment created</response>
        [HttpPost]
        [Route("/api/attachment/bulk")]
        [SwaggerOperation("AttachmentBulkPost")]
        public virtual IActionResult AttachmentBulkPost([FromBody]Attachment[] items)
        {
            return this._service.AttachmentBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/attachment")]
        [SwaggerOperation("AttachmentGet")]
        [SwaggerResponse(200, type: typeof(List<Attachment>))]
        public virtual IActionResult AttachmentGet()
        {
            return this._service.AttachmentGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        [HttpPost]
        [Route("/api/attachment/{id}/delete")]
        [SwaggerOperation("AttachmentIdDeletePost")]
        public virtual IActionResult AttachmentIdDeletePost([FromRoute]int id)
        {
            return this._service.AttachmentIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        [HttpGet]
        [Route("/api/attachment/{id}")]
        [SwaggerOperation("AttachmentIdGet")]
        [SwaggerResponse(200, type: typeof(Attachment))]
        public virtual IActionResult AttachmentIdGet([FromRoute]int id)
        {
            return this._service.AttachmentIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        [HttpPut]
        [Route("/api/attachment/{id}")]
        [SwaggerOperation("AttachmentIdPut")]
        [SwaggerResponse(200, type: typeof(Attachment))]
        public virtual IActionResult AttachmentIdPut([FromRoute]int id, [FromBody]Attachment item)
        {
            return this._service.AttachmentIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Attachment created</response>
        [HttpPost]
        [Route("/api/attachment")]
        [SwaggerOperation("AttachmentPost")]
        [SwaggerResponse(200, type: typeof(Attachment))]
        public virtual IActionResult AttachmentPost([FromBody]Attachment item)
        {
            return this._service.AttachmentPostAsync(item);
        }
    }
}
