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
    public partial class RentalRequestController : Controller
    {
        private readonly IRentalRequestService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalRequestController(IRentalRequestService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequest created</response>
        [HttpPost]
        [Route("/api/rentalrequests/bulk")]
        [SwaggerOperation("RentalrequestsBulkPost")]
        public virtual IActionResult RentalrequestsBulkPost([FromBody]RentalRequest[] items)
        {
            return this._service.RentalrequestsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests")]
        [SwaggerOperation("RentalrequestsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequest>))]
        public virtual IActionResult RentalrequestsGet()
        {
            return this._service.RentalrequestsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpPost]
        [Route("/api/rentalrequests/{id}/delete")]
        [SwaggerOperation("RentalrequestsIdDeletePost")]
        public virtual IActionResult RentalrequestsIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalrequestsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpGet]
        [Route("/api/rentalrequests/{id}")]
        [SwaggerOperation("RentalrequestsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsIdGet([FromRoute]int id)
        {
            return this._service.RentalrequestsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests/{id}/history")]
        [SwaggerOperation("RentalrequestsIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult RentalrequestsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return this._service.RentalrequestsIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add a History record to the RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/rentalrequests/{id}/history")]
        [SwaggerOperation("RentalrequestsIdHistoryPost")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult RentalrequestsIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return this._service.RentalrequestsIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpPut]
        [Route("/api/rentalrequests/{id}")]
        [SwaggerOperation("RentalrequestsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsIdPut([FromRoute]int id, [FromBody]RentalRequest item)
        {
            return this._service.RentalrequestsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequest created</response>
        [HttpPost]
        [Route("/api/rentalrequests")]
        [SwaggerOperation("RentalrequestsPost")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsPost([FromBody]RentalRequest item)
        {
            return this._service.RentalrequestsPostAsync(item);
        }

        /// <summary>
        /// Searches RentalRequests
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="project">Searches equipmentAttachment type</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">Inspection start date</param>
        /// <param name="endDate">Inspection end date</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests/search")]
        [SwaggerOperation("RentalrequestsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestSearchResultViewModel>))]
        public virtual IActionResult RentalrequestsSearchGet([FromQuery]int?[] localareas, [FromQuery]string project, [FromQuery]string status, [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate)
        {
            return this._service.RentalrequestsSearchGetAsync(localareas, project, status, startDate, endDate);
        }
    }
}
