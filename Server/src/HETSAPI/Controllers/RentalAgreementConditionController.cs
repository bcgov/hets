using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
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
        /// <response code="200">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/bulk")]
        [SwaggerOperation("RentalagreementconditionsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult RentalagreementconditionsBulkPost([FromBody]RentalAgreementCondition[] items)
        {
            return _service.RentalagreementconditionsBulkPostAsync(items);
        }                

        /// <summary>	
        /// Delete rental agreement condition	
        /// </summary>	
        /// <param name="id">id of RentalAgreementCondition to delete</param>	
        /// <response code="200">OK</response>	
        [HttpPost]	
        [Route("/api/rentalagreementconditions/{id}/delete")]	
        [SwaggerOperation("RentalagreementconditionsIdDeletePost")]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RentalagreementconditionsIdDeletePost([FromRoute]int id)
        {	
            return _service.RentalagreementconditionsIdDeletePostAsync(id);
        }

        /// <summary>	
        /// Update rental agreement condition by id	
        /// </summary>	
        /// <param name="id">id of RentalAgreementCondition to fetch</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        [HttpPut]	
        [Route("/api/rentalagreementconditions/{id}")]	
        [SwaggerOperation("RentalagreementconditionsIdPut")]	
        [SwaggerResponse(200, type: typeof(RentalAgreementCondition))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RentalagreementconditionsIdPut([FromRoute]int id, [FromBody]RentalAgreementCondition item)
        {	
            return _service.RentalagreementconditionsIdPutAsync(id, item);	
        }
    }
}
