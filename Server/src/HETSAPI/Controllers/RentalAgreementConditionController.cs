using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Rental Agreement Condition Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementConditionController : Controller
    {
        private readonly IRentalAgreementConditionService _service;

        /// <summary>
        /// Rental Agreement Condition Controller Constructor
        /// </summary>
        public RentalAgreementConditionController(IRentalAgreementConditionService service)
        {
            _service = service;
        }

        /// <summary>
        ///  Create bulk rental agreement condition records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/bulk")]
        [SwaggerOperation("RentalagreementconditionsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalagreementconditionsBulkPost([FromBody]RentalAgreementCondition[] items)
        {
            return _service.RentalagreementconditionsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all rental agreement conditions
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreementconditions")]
        [SwaggerOperation("RentalagreementconditionsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreementCondition>))]
        public virtual IActionResult RentalagreementconditionsGet()
        {
            return _service.RentalagreementconditionsGetAsync();
        }

        /// <summary>
        /// Delete rental agreement condition
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/{id}/delete")]
        [SwaggerOperation("RentalagreementconditionsIdDeletePost")]
        public virtual IActionResult RentalagreementconditionsIdDeletePost([FromRoute]int id)
        {
            return _service.RentalagreementconditionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get rental agreement condition by id
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
            return _service.RentalagreementconditionsIdGetAsync(id);
        }

        /// <summary>
        /// Update rental agreement condition by id
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
            return _service.RentalagreementconditionsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create rentsl agreement condition
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions")]
        [SwaggerOperation("RentalagreementconditionsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        public virtual IActionResult RentalagreementconditionsPost([FromBody]RentalAgreementCondition item)
        {
            return _service.RentalagreementconditionsPostAsync(item);
        }
    }
}
