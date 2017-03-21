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
    public partial class LookupListsController : Controller
    {
        private readonly ILookupListsService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public LookupListsController(ILookupListsService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LookupLists created</response>
        [HttpPost]
        [Route("/api/lookupLists/bulk")]
        [SwaggerOperation("LookupListsBulkPost")]
        public virtual IActionResult LookupListsBulkPost([FromBody]LookupLists[] items)
        {
            return this._service.LookupListsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/lookupLists")]
        [SwaggerOperation("LookupListsGet")]
        [SwaggerResponse(200, type: typeof(List<LookupLists>))]
        public virtual IActionResult LookupListsGet()
        {
            return this._service.LookupListsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupLists to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupLists not found</response>
        [HttpPost]
        [Route("/api/lookupLists/{id}/delete")]
        [SwaggerOperation("LookupListsIdDeletePost")]
        public virtual IActionResult LookupListsIdDeletePost([FromRoute]int id)
        {
            return this._service.LookupListsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupLists to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupLists not found</response>
        [HttpGet]
        [Route("/api/lookupLists/{id}")]
        [SwaggerOperation("LookupListsIdGet")]
        [SwaggerResponse(200, type: typeof(LookupLists))]
        public virtual IActionResult LookupListsIdGet([FromRoute]int id)
        {
            return this._service.LookupListsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupLists to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupLists not found</response>
        [HttpPut]
        [Route("/api/lookupLists/{id}")]
        [SwaggerOperation("LookupListsIdPut")]
        [SwaggerResponse(200, type: typeof(LookupLists))]
        public virtual IActionResult LookupListsIdPut([FromRoute]int id, [FromBody]LookupLists item)
        {
            return this._service.LookupListsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LookupLists created</response>
        [HttpPost]
        [Route("/api/lookupLists")]
        [SwaggerOperation("LookupListsPost")]
        [SwaggerResponse(200, type: typeof(LookupLists))]
        public virtual IActionResult LookupListsPost([FromBody]LookupLists item)
        {
            return this._service.LookupListsPostAsync(item);
        }
    }
}
