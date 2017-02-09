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
    public partial class OwnerController : Controller
    {
        private readonly IOwnerService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public OwnerController(IOwnerService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        [HttpPost]
        [Route("/api/owners/bulk")]
        [SwaggerOperation("OwnersBulkPost")]
        public virtual IActionResult OwnersBulkPost([FromBody]Owner[] items)
        {
            return this._service.OwnersBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners")]
        [SwaggerOperation("OwnersGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        public virtual IActionResult OwnersGet()
        {
            return this._service.OwnersGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners/{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsGet")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult OwnersIdContactsGet([FromRoute]int id)
        {
            return this._service.OwnersIdContactsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Replcaes an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="item">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        [HttpPut]
        [Route("/api/owners/{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsPut")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult OwnersIdContactsPut([FromRoute]int id, [FromBody]Contact[] item)
        {
            return this._service.OwnersIdContactsPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpPost]
        [Route("/api/owners/{id}/delete")]
        [SwaggerOperation("OwnersIdDeletePost")]
        public virtual IActionResult OwnersIdDeletePost([FromRoute]int id)
        {
            return this._service.OwnersIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpGet]
        [Route("/api/owners/{id}")]
        [SwaggerOperation("OwnersIdGet")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersIdGet([FromRoute]int id)
        {
            return this._service.OwnersIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpPut]
        [Route("/api/owners/{id}")]
        [SwaggerOperation("OwnersIdPut")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersIdPut([FromRoute]int id, [FromBody]Owner item)
        {
            return this._service.OwnersIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Owner created</response>
        [HttpPost]
        [Route("/api/owners")]
        [SwaggerOperation("OwnersPost")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersPost([FromBody]Owner item)
        {
            return this._service.OwnersPostAsync(item);
        }
    }
}
