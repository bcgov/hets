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
    public partial class LocalAreaController : Controller
    {
        private readonly ILocalAreaService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public LocalAreaController(ILocalAreaService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalArea created</response>
        [HttpPost]
        [Route("/api/localAreas/bulk")]
        [SwaggerOperation("LocalAreasBulkPost")]
        public virtual IActionResult LocalAreasBulkPost([FromBody]LocalArea[] items)
        {
            return this._service.LocalAreasBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/localAreas")]
        [SwaggerOperation("LocalAreasGet")]
        [SwaggerResponse(200, type: typeof(List<LocalArea>))]
        public virtual IActionResult LocalAreasGet()
        {
            return this._service.LocalAreasGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpPost]
        [Route("/api/localAreas/{id}/delete")]
        [SwaggerOperation("LocalAreasIdDeletePost")]
        public virtual IActionResult LocalAreasIdDeletePost([FromRoute]int id)
        {
            return this._service.LocalAreasIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpGet]
        [Route("/api/localAreas/{id}")]
        [SwaggerOperation("LocalAreasIdGet")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasIdGet([FromRoute]int id)
        {
            return this._service.LocalAreasIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        [HttpPut]
        [Route("/api/localAreas/{id}")]
        [SwaggerOperation("LocalAreasIdPut")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasIdPut([FromRoute]int id, [FromBody]LocalArea item)
        {
            return this._service.LocalAreasIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalArea created</response>
        [HttpPost]
        [Route("/api/localAreas")]
        [SwaggerOperation("LocalAreasPost")]
        [SwaggerResponse(200, type: typeof(LocalArea))]
        public virtual IActionResult LocalAreasPost([FromBody]LocalArea item)
        {
            return this._service.LocalAreasPostAsync(item);
        }
    }
}
