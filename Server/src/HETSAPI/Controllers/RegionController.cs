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
    public partial class RegionController : Controller
    {
        private readonly IRegionService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RegionController(IRegionService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Region created</response>
        [HttpPost]
        [Route("/api/regions/bulk")]
        [SwaggerOperation("RegionsBulkPost")]
        public virtual IActionResult RegionsBulkPost([FromBody]Region[] items)
        {
            return this._service.RegionsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/regions")]
        [SwaggerOperation("RegionsGet")]
        [SwaggerResponse(200, type: typeof(List<Region>))]
        public virtual IActionResult RegionsGet()
        {
            return this._service.RegionsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Region to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpPost]
        [Route("/api/regions/{id}/delete")]
        [SwaggerOperation("RegionsIdDeletePost")]
        public virtual IActionResult RegionsIdDeletePost([FromRoute]int id)
        {
            return this._service.RegionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the districts for a specific region</remarks>
        /// <param name="id">id of Region for which to fetch the Districts</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/regions/{id}/districts")]
        [SwaggerOperation("RegionsIdDistrictsGet")]
        [SwaggerResponse(200, type: typeof(List<District>))]
        public virtual IActionResult RegionsIdDistrictsGet([FromRoute]int id)
        {
            return this._service.RegionsIdDistrictsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Region to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpGet]
        [Route("/api/regions/{id}")]
        [SwaggerOperation("RegionsIdGet")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsIdGet([FromRoute]int id)
        {
            return this._service.RegionsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Region to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        [HttpPut]
        [Route("/api/regions/{id}")]
        [SwaggerOperation("RegionsIdPut")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsIdPut([FromRoute]int id, [FromBody]Region item)
        {
            return this._service.RegionsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Region created</response>
        [HttpPost]
        [Route("/api/regions")]
        [SwaggerOperation("RegionsPost")]
        [SwaggerResponse(200, type: typeof(Region))]
        public virtual IActionResult RegionsPost([FromBody]Region item)
        {
            return this._service.RegionsPostAsync(item);
        }
    }
}
