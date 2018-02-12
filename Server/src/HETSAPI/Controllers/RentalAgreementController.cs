using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Rental Agreement Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementController : Controller
    {
        private readonly IRentalAgreementService _service;

        /// <summary>
        /// Rental Agreement Controller Construtor
        /// </summary>
        public RentalAgreementController(IRentalAgreementService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk rental agreement records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements/bulk")]
        [SwaggerOperation("RentalagreementsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalagreementsBulkPost([FromBody]RentalAgreement[] items)
        {
            return _service.RentalagreementsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all rental agreements
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreement>))]
        public virtual IActionResult RentalagreementsGet()
        {
            return _service.RentalagreementsGetAsync();
        }

        /// <summary>
        /// Delete rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/delete")]
        [SwaggerOperation("RentalagreementsIdDeletePost")]
        public virtual IActionResult RentalagreementsIdDeletePost([FromRoute]int id)
        {
            return _service.RentalagreementsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get rental agreement by id
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
            return _service.RentalagreementsIdGetAsync(id);
        }

        /// <summary>
        /// Get a pdf version of a rental agreement
        /// </summary>
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}/pdf")]
        [SwaggerOperation("RentalagreementsIdPdfGet")]
        public virtual IActionResult RentalagreementsIdPdfGet([FromRoute]int id)
        {
            return _service.RentalagreementsIdPdfGetAsync(id);
        }

        /// <summary>
        /// Update rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPut]
        [Route("/api/rentalagreements/{id}")]
        [SwaggerOperation("RentalagreementsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdPut([FromRoute]int id, [FromBody]RentalAgreement item)
        {
            return _service.RentalagreementsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create rental agreement
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsPost([FromBody]RentalAgreement item)
        {
            return _service.RentalagreementsPostAsync(item);
        }

        /// <summary>
        /// Release (terminate) a rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to release</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/release")]
        [SwaggerOperation("RentalagreementsIdReleasePost")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdReleasePost([FromRoute]int id)
        {
            return _service.RentalagreementsIdReleasePostAsync(id);
        }

        #region Rental Agreement Time Records

        /// <summary>
        /// Get time records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Time Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Time Records for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}/timeRecords")]
        [SwaggerOperation("RentalagreementsIdTimeRecordsGet")]
        [SwaggerResponse(200, type: typeof(List<TimeRecord>))]
        public virtual IActionResult RentalagreementsIdTimeRecordsGet([FromRoute]int id)
        {
            return _service.RentalAgreementsIdTimeRecordsGetAsync(id);
        }

        /// <summary>
        /// Add a rental agreement time record
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="item">Adds to Rental Agreement Time Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/timeRecord")]
        [SwaggerOperation("RentalagreementsIdTimeRecordsPost")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult RentalagreementsIdTimeRecordsPost([FromRoute]int id, [FromBody]TimeRecord item)
        {
            return _service.RentalAgreementsIdTimeRecordsPostAsync(id, item);
        }

        /// <summary>
        /// Update or create an array of time records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="items">Array of Rental Agreement Time Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/timeRecords")]
        [SwaggerOperation("RentalagreementsIdTimeRecordsBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult RentalagreementsIdTimeRecordsBulkPostAsync([FromRoute]int id, [FromBody]TimeRecord[] items)
        {
            return _service.RentalAgreementsIdTimeRecordsBulkPostAsync(id, items);
        }

        #endregion

        #region Rental Agreement Rate Records

        /// <summary>
        /// Get rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Rate Records for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}/rateRecords")]
        [SwaggerOperation("RentalagreementsIdRentalAgreementRatesGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreementRate>))]
        public virtual IActionResult RentalagreementsIdRentalAgreementRatesGet([FromRoute]int id)
        {
            return _service.RentalAgreementsIdRentalAgreementRatesGetAsync(id);
        }

        /// <summary>
        /// Add a rental agreement rate record
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add a rate record for</param>
        /// <param name="item">Adds to Rental Agreement Rate Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/rateRecord")]
        [SwaggerOperation("RentalagreementsIdRentalAgreementRatesPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        public virtual IActionResult RentalagreementsIdRentalAgreementRatesPost([FromRoute]int id, [FromBody]RentalAgreementRate item)
        {
            return _service.RentalAgreementsIdRentalAgreementRatesPostAsync(id, item);
        }

        /// <summary>
        /// Update or create an array of rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add rate records for</param>
        /// <param name="items">Array of Rental Agreement Rate Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/rateRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementRatesBulkPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        public virtual IActionResult RentalAgreementsIdRentalAgreementRatesBulkPost([FromRoute]int id, [FromBody]RentalAgreementRate[] items)
        {
            return _service.RentalAgreementsIdRentalAgreementRatesBulkPostAsync(id, items);
        }

        #endregion

        #region Rental Agreement Condition Records

        /// <summary>
        /// Get condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Condition Records for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}/conditionRecords")]
        [SwaggerOperation("RentalagreementsIdRentalAgreementConditionsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreementCondition>))]
        public virtual IActionResult RentalagreementsIdRentalAgreementConditionsGet([FromRoute]int id)
        {
            return _service.RentalAgreementsIdRentalAgreementConditionsGetAsync(id);
        }

        /// <summary>
        /// Add a rental agreement condition record
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add a condition record for</param>
        /// <param name="item">Adds to Rental Agreement Condition Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/conditionRecord")]
        [SwaggerOperation("RentalagreementsIdRentalAgreementConditionsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalagreementsIdRentalAgreementConditionsPost([FromRoute]int id, [FromBody]RentalAgreementCondition item)
        {
            return _service.RentalAgreementsIdRentalAgreementConditionsPostAsync(id, item);
        }

        /// <summary>
        /// Update or create an array of condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add condition records for</param>
        /// <param name="items">Array of Rental Agreement Condition Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/conditionRecords")]
        [SwaggerOperation("RentalAgreementsIdRentalAgreementConditionsBulkPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalAgreementsIdRentalAgreementConditionsBulkPost([FromRoute]int id, [FromBody]RentalAgreementCondition[] items)
        {
            return _service.RentalAgreementsIdRentalAgreementConditionsBulkPostAsync(id, items);
        }

        #endregion
    }
}
