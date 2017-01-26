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
    public partial class CityController : Controller
    {
        private readonly ICityService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public CityController(ICityService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">City created</response>
        [HttpPost]
        [Route("/api/cities/bulk")]
        [SwaggerOperation("CitiesBulkPost")]
        public virtual IActionResult CitiesBulkPost([FromBody]City[] items)
        {
            return this._service.CitiesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/cities")]
        [SwaggerOperation("CitiesGet")]
        [SwaggerResponse(200, type: typeof(List<City>))]
        public virtual IActionResult CitiesGet()
        {
            return this._service.CitiesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpPost]
        [Route("/api/cities/{id}/delete")]
        [SwaggerOperation("CitiesIdDeletePost")]
        public virtual IActionResult CitiesIdDeletePost([FromRoute]int id)
        {
            return this._service.CitiesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpGet]
        [Route("/api/cities/{id}")]
        [SwaggerOperation("CitiesIdGet")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesIdGet([FromRoute]int id)
        {
            return this._service.CitiesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of City to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        [HttpPut]
        [Route("/api/cities/{id}")]
        [SwaggerOperation("CitiesIdPut")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesIdPut([FromRoute]int id, [FromBody]City item)
        {
            return this._service.CitiesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">City created</response>
        [HttpPost]
        [Route("/api/cities")]
        [SwaggerOperation("CitiesPost")]
        [SwaggerResponse(200, type: typeof(City))]
        public virtual IActionResult CitiesPost([FromBody]City item)
        {
            return this._service.CitiesPostAsync(item);
        }
    }
}
