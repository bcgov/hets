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
    public partial class ContactAddressController : Controller
    {
        private readonly IContactAddressService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public ContactAddressController(IContactAddressService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">ContactAddress created</response>
        [HttpPost]
        [Route("/api/contactaddresses/bulk")]
        [SwaggerOperation("ContactaddressesBulkPost")]
        public virtual IActionResult ContactaddressesBulkPost([FromBody]ContactAddress[] items)
        {
            return this._service.ContactaddressesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/contactaddresses")]
        [SwaggerOperation("ContactaddressesGet")]
        [SwaggerResponse(200, type: typeof(List<ContactAddress>))]
        public virtual IActionResult ContactaddressesGet()
        {
            return this._service.ContactaddressesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactAddress to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactAddress not found</response>
        [HttpPost]
        [Route("/api/contactaddresses/{id}/delete")]
        [SwaggerOperation("ContactaddressesIdDeletePost")]
        public virtual IActionResult ContactaddressesIdDeletePost([FromRoute]int id)
        {
            return this._service.ContactaddressesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactAddress to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactAddress not found</response>
        [HttpGet]
        [Route("/api/contactaddresses/{id}")]
        [SwaggerOperation("ContactaddressesIdGet")]
        [SwaggerResponse(200, type: typeof(ContactAddress))]
        public virtual IActionResult ContactaddressesIdGet([FromRoute]int id)
        {
            return this._service.ContactaddressesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ContactAddress to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">ContactAddress not found</response>
        [HttpPut]
        [Route("/api/contactaddresses/{id}")]
        [SwaggerOperation("ContactaddressesIdPut")]
        [SwaggerResponse(200, type: typeof(ContactAddress))]
        public virtual IActionResult ContactaddressesIdPut([FromRoute]int id, [FromBody]ContactAddress item)
        {
            return this._service.ContactaddressesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">ContactAddress created</response>
        [HttpPost]
        [Route("/api/contactaddresses")]
        [SwaggerOperation("ContactaddressesPost")]
        [SwaggerResponse(200, type: typeof(ContactAddress))]
        public virtual IActionResult ContactaddressesPost([FromBody]ContactAddress item)
        {
            return this._service.ContactaddressesPostAsync(item);
        }
    }
}
