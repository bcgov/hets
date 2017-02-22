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
    public partial class RentalAgreementConditionController : Controller
    {
        private readonly IRentalAgreementConditionService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalAgreementConditionController(IRentalAgreementConditionService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/bulk")]
        [SwaggerOperation("RentalagreementconditionsBulkPost")]
        public virtual IActionResult RentalagreementconditionsBulkPost([FromBody]RentalAgreementCondition[] items)
        {
            return this._service.RentalagreementconditionsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreementconditions")]
        [SwaggerOperation("RentalagreementconditionsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreementCondition>))]
        public virtual IActionResult RentalagreementconditionsGet()
        {
            return this._service.RentalagreementconditionsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/{id}/delete")]
        [SwaggerOperation("RentalagreementconditionsIdDeletePost")]
        public virtual IActionResult RentalagreementconditionsIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalagreementconditionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        [HttpGet]
        [Route("/api/rentalagreementconditions/{id}")]
        [SwaggerOperation("RentalagreementconditionsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalagreementconditionsIdGet([FromRoute]int id)
        {
            return this._service.RentalagreementconditionsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        [HttpPut]
        [Route("/api/rentalagreementconditions/{id}")]
        [SwaggerOperation("RentalagreementconditionsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalagreementconditionsIdPut([FromRoute]int id, [FromBody]RentalAgreementCondition item)
        {
            return this._service.RentalagreementconditionsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions")]
        [SwaggerOperation("RentalagreementconditionsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalagreementconditionsPost([FromBody]RentalAgreementCondition item)
        {
            return this._service.RentalagreementconditionsPostAsync(item);
        }
    }
}
