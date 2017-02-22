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
    public partial class RentalAgreementRateController : Controller
    {
        private readonly IRentalAgreementRateService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalAgreementRateController(IRentalAgreementRateService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementRate created</response>
        [HttpPost]
        [Route("/api/rentalagreementrates/bulk")]
        [SwaggerOperation("RentalagreementratesBulkPost")]
        public virtual IActionResult RentalagreementratesBulkPost([FromBody]RentalAgreementRate[] items)
        {
            return this._service.RentalagreementratesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreementrates")]
        [SwaggerOperation("RentalagreementratesGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreementRate>))]
        public virtual IActionResult RentalagreementratesGet()
        {
            return this._service.RentalagreementratesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        [HttpPost]
        [Route("/api/rentalagreementrates/{id}/delete")]
        [SwaggerOperation("RentalagreementratesIdDeletePost")]
        public virtual IActionResult RentalagreementratesIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalagreementratesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        [HttpGet]
        [Route("/api/rentalagreementrates/{id}")]
        [SwaggerOperation("RentalagreementratesIdGet")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        public virtual IActionResult RentalagreementratesIdGet([FromRoute]int id)
        {
            return this._service.RentalagreementratesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        [HttpPut]
        [Route("/api/rentalagreementrates/{id}")]
        [SwaggerOperation("RentalagreementratesIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        public virtual IActionResult RentalagreementratesIdPut([FromRoute]int id, [FromBody]RentalAgreementRate item)
        {
            return this._service.RentalagreementratesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreementRate created</response>
        [HttpPost]
        [Route("/api/rentalagreementrates")]
        [SwaggerOperation("RentalagreementratesPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        public virtual IActionResult RentalagreementratesPost([FromBody]RentalAgreementRate item)
        {
            return this._service.RentalagreementratesPostAsync(item);
        }
    }
}
