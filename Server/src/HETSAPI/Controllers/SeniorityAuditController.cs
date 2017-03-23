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
    public partial class SeniorityAuditController : Controller
    {
        private readonly ISeniorityAuditService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public SeniorityAuditController(ISeniorityAuditService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">SeniorityAudit created</response>
        [HttpPost]
        [Route("/api/seniorityaudits/bulk")]
        [SwaggerOperation("SeniorityauditsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult SeniorityauditsBulkPost([FromBody]SeniorityAudit[] items)
        {
            return this._service.SeniorityauditsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/seniorityaudits")]
        [SwaggerOperation("SeniorityauditsGet")]
        [SwaggerResponse(200, type: typeof(List<SeniorityAudit>))]
        public virtual IActionResult SeniorityauditsGet()
        {
            return this._service.SeniorityauditsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpPost]
        [Route("/api/seniorityaudits/{id}/delete")]
        [SwaggerOperation("SeniorityauditsIdDeletePost")]
        public virtual IActionResult SeniorityauditsIdDeletePost([FromRoute]int id)
        {
            return this._service.SeniorityauditsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpGet]
        [Route("/api/seniorityaudits/{id}")]
        [SwaggerOperation("SeniorityauditsIdGet")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsIdGet([FromRoute]int id)
        {
            return this._service.SeniorityauditsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpPut]
        [Route("/api/seniorityaudits/{id}")]
        [SwaggerOperation("SeniorityauditsIdPut")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsIdPut([FromRoute]int id, [FromBody]SeniorityAudit item)
        {
            return this._service.SeniorityauditsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">SeniorityAudit created</response>
        [HttpPost]
        [Route("/api/seniorityaudits")]
        [SwaggerOperation("SeniorityauditsPost")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsPost([FromBody]SeniorityAudit item)
        {
            return this._service.SeniorityauditsPostAsync(item);
        }
    }
}
