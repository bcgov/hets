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
    public partial class DumpTruckController : Controller
    {
        private readonly IDumpTruckService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public DumpTruckController(IDumpTruckService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
        [HttpPost]
        [Route("/api/dumptrucks/bulk")]
        [SwaggerOperation("DumptrucksBulkPost")]
        public virtual IActionResult DumptrucksBulkPost([FromBody]DumpTruck[] items)
        {
            return this._service.DumptrucksBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/dumptrucks")]
        [SwaggerOperation("DumptrucksGet")]
        [SwaggerResponse(200, type: typeof(List<DumpTruck>))]
        public virtual IActionResult DumptrucksGet()
        {
            return this._service.DumptrucksGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpPost]
        [Route("/api/dumptrucks/{id}/delete")]
        [SwaggerOperation("DumptrucksIdDeletePost")]
        public virtual IActionResult DumptrucksIdDeletePost([FromRoute]int id)
        {
            return this._service.DumptrucksIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpGet]
        [Route("/api/dumptrucks/{id}")]
        [SwaggerOperation("DumptrucksIdGet")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksIdGet([FromRoute]int id)
        {
            return this._service.DumptrucksIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        [HttpPut]
        [Route("/api/dumptrucks/{id}")]
        [SwaggerOperation("DumptrucksIdPut")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksIdPut([FromRoute]int id, [FromBody]DumpTruck item)
        {
            return this._service.DumptrucksIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        [HttpPost]
        [Route("/api/dumptrucks")]
        [SwaggerOperation("DumptrucksPost")]
        [SwaggerResponse(200, type: typeof(DumpTruck))]
        public virtual IActionResult DumptrucksPost([FromBody]DumpTruck item)
        {
            return this._service.DumptrucksPostAsync(item);
        }
    }
}
