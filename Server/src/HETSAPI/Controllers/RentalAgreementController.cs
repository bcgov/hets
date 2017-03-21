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
    public partial class RentalAgreementController : Controller
    {
        private readonly IRentalAgreementService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalAgreementController(IRentalAgreementService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements/bulk")]
        [SwaggerOperation("RentalagreementsBulkPost")]
        public virtual IActionResult RentalagreementsBulkPost([FromBody]RentalAgreement[] items)
        {
            return this._service.RentalagreementsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreement>))]
        public virtual IActionResult RentalagreementsGet()
        {
            return this._service.RentalagreementsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreement to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/delete")]
        [SwaggerOperation("RentalagreementsIdDeletePost")]
        public virtual IActionResult RentalagreementsIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalagreementsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}")]
        [SwaggerOperation("RentalagreementsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdGet([FromRoute]int id)
        {
            return this._service.RentalagreementsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPut]
        [Route("/api/rentalagreements/{id}")]
        [SwaggerOperation("RentalagreementsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdPut([FromRoute]int id, [FromBody]RentalAgreement item)
        {
            return this._service.RentalagreementsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsPost([FromBody]RentalAgreement item)
        {
            return this._service.RentalagreementsPostAsync(item);
        }
    }
}
