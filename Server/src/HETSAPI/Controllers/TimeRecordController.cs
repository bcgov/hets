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
    public partial class TimeRecordController : Controller
    {
        private readonly ITimeRecordService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public TimeRecordController(ITimeRecordService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">TimeRecord created</response>
        [HttpPost]
        [Route("/api/timerecords/bulk")]
        [SwaggerOperation("TimerecordsBulkPost")]
        public virtual IActionResult TimerecordsBulkPost([FromBody]TimeRecord[] items)
        {
            return this._service.TimerecordsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/timerecords")]
        [SwaggerOperation("TimerecordsGet")]
        [SwaggerResponse(200, type: typeof(List<TimeRecord>))]
        public virtual IActionResult TimerecordsGet()
        {
            return this._service.TimerecordsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpPost]
        [Route("/api/timerecords/{id}/delete")]
        [SwaggerOperation("TimerecordsIdDeletePost")]
        public virtual IActionResult TimerecordsIdDeletePost([FromRoute]int id)
        {
            return this._service.TimerecordsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpGet]
        [Route("/api/timerecords/{id}")]
        [SwaggerOperation("TimerecordsIdGet")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsIdGet([FromRoute]int id)
        {
            return this._service.TimerecordsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpPut]
        [Route("/api/timerecords/{id}")]
        [SwaggerOperation("TimerecordsIdPut")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsIdPut([FromRoute]int id, [FromBody]TimeRecord item)
        {
            return this._service.TimerecordsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">TimeRecord created</response>
        [HttpPost]
        [Route("/api/timerecords")]
        [SwaggerOperation("TimerecordsPost")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsPost([FromBody]TimeRecord item)
        {
            return this._service.TimerecordsPostAsync(item);
        }
    }
}
