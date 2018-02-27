using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Rental Agreement Rate Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementRateController : Controller
    {
        private readonly IRentalAgreementRateService _service;

        /// <summary>
        /// Rental Agreement Rate Controller Constructor
        /// </summary>
        public RentalAgreementRateController(IRentalAgreementRateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk rental agreement rate records 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementRate created</response>
        [HttpPost]
        [Route("/api/rentalagreementrates/bulk")]
        [SwaggerOperation("RentalagreementratesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult RentalagreementratesBulkPost([FromBody]RentalAgreementRate[] items)
        {
            return _service.RentalagreementratesBulkPostAsync(items);
        }             
    }
}
