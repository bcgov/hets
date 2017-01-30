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
    public partial class ContactPhoneController : Controller
    {
        private readonly IContactPhoneService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public ContactPhoneController(IContactPhoneService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">ContactPhone created</response>
        [HttpPost]
        [Route("/api/contactphones/bulk")]
        [SwaggerOperation("ContactphonesBulkPost")]
        public virtual IActionResult ContactphonesBulkPost([FromBody]ContactPhone[] items)
        {
            return this._service.ContactphonesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/contactphones")]
        [SwaggerOperation("ContactphonesGet")]
        [SwaggerResponse(200, type: typeof(List<ContactPhone>))]
        public virtual IActionResult ContactphonesGet()
        {
            return this._service.ContactphonesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactPhone to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactPhone not found</response>
        [HttpPost]
        [Route("/api/contactphones/{id}/delete")]
        [SwaggerOperation("ContactphonesIdDeletePost")]
        public virtual IActionResult ContactphonesIdDeletePost([FromRoute]int id)
        {
            return this._service.ContactphonesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactPhone to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactPhone not found</response>
        [HttpGet]
        [Route("/api/contactphones/{id}")]
        [SwaggerOperation("ContactphonesIdGet")]
        [SwaggerResponse(200, type: typeof(ContactPhone))]
        public virtual IActionResult ContactphonesIdGet([FromRoute]int id)
        {
            return this._service.ContactphonesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactPhone to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactPhone not found</response>
        [HttpPut]
        [Route("/api/contactphones/{id}")]
        [SwaggerOperation("ContactphonesIdPut")]
        [SwaggerResponse(200, type: typeof(ContactPhone))]
        public virtual IActionResult ContactphonesIdPut([FromRoute]int id, [FromBody]ContactPhone item)
        {
            return this._service.ContactphonesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">ContactPhone created</response>
        [HttpPost]
        [Route("/api/contactphones")]
        [SwaggerOperation("ContactphonesPost")]
        [SwaggerResponse(200, type: typeof(ContactPhone))]
        public virtual IActionResult ContactphonesPost([FromBody]ContactPhone item)
        {
            return this._service.ContactphonesPostAsync(item);
        }
    }
}
