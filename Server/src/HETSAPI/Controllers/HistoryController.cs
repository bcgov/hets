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
    public partial class HistoryController : Controller
    {
        private readonly IHistoryService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public HistoryController(IHistoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/histories/bulk")]
        [SwaggerOperation("HistoriesBulkPost")]
        public virtual IActionResult HistoriesBulkPost([FromBody]History[] items)
        {
            return this._service.HistoriesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/histories")]
        [SwaggerOperation("HistoriesGet")]
        [SwaggerResponse(200, type: typeof(List<History>))]
        public virtual IActionResult HistoriesGet()
        {
            return this._service.HistoriesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpPost]
        [Route("/api/histories/{id}/delete")]
        [SwaggerOperation("HistoriesIdDeletePost")]
        public virtual IActionResult HistoriesIdDeletePost([FromRoute]int id)
        {
            return this._service.HistoriesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpGet]
        [Route("/api/histories/{id}")]
        [SwaggerOperation("HistoriesIdGet")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesIdGet([FromRoute]int id)
        {
            return this._service.HistoriesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpPut]
        [Route("/api/histories/{id}")]
        [SwaggerOperation("HistoriesIdPut")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesIdPut([FromRoute]int id, [FromBody]History item)
        {
            return this._service.HistoriesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/histories")]
        [SwaggerOperation("HistoriesPost")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesPost([FromBody]History item)
        {
            return this._service.HistoriesPostAsync(item);
        }
    }
}
