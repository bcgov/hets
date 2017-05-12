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
    public partial class LocalAreaRotationListController : Controller
    {
        private readonly ILocalAreaRotationListService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public LocalAreaRotationListController(ILocalAreaRotationListService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        [HttpPost]
        [Route("/api/localarearotationlists/bulk")]
        [SwaggerOperation("LocalarearotationlistsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult LocalarearotationlistsBulkPost([FromBody]LocalAreaRotationList[] items)
        {
            return this._service.LocalarearotationlistsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/localarearotationlists")]
        [SwaggerOperation("LocalarearotationlistsGet")]
        [SwaggerResponse(200, type: typeof(List<LocalAreaRotationList>))]
        public virtual IActionResult LocalarearotationlistsGet()
        {
            return this._service.LocalarearotationlistsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpPost]
        [Route("/api/localarearotationlists/{id}/delete")]
        [SwaggerOperation("LocalarearotationlistsIdDeletePost")]
        public virtual IActionResult LocalarearotationlistsIdDeletePost([FromRoute]int id)
        {
            return this._service.LocalarearotationlistsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpGet]
        [Route("/api/localarearotationlists/{id}")]
        [SwaggerOperation("LocalarearotationlistsIdGet")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsIdGet([FromRoute]int id)
        {
            return this._service.LocalarearotationlistsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpPut]
        [Route("/api/localarearotationlists/{id}")]
        [SwaggerOperation("LocalarearotationlistsIdPut")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsIdPut([FromRoute]int id, [FromBody]LocalAreaRotationList item)
        {
            return this._service.LocalarearotationlistsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        [HttpPost]
        [Route("/api/localarearotationlists")]
        [SwaggerOperation("LocalarearotationlistsPost")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsPost([FromBody]LocalAreaRotationList item)
        {
            return this._service.LocalarearotationlistsPostAsync(item);
        }
    }
}
