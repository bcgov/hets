using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
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
        /// <response code="200">RentalAgreementCondition created</response>
        [HttpPost]
        [Route("/api/rentalagreementconditions/bulk")]
        [SwaggerOperation("RentalagreementconditionsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult RentalagreementconditionsBulkPost([FromBody]RentalAgreementCondition[] items)
        {
            return _service.RentalagreementconditionsBulkPostAsync(items);
        }                
    }
}
