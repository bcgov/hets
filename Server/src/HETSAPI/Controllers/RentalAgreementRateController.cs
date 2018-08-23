using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
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

        /// <summary>
        /// Delete rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to delete</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/rentalagreementrates/{id}/delete")]
        [SwaggerOperation("RentalagreementratesIdDeletePost")]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RentalagreementratesIdDeletePost([FromRoute]int id)
        {
            return _service.RentalagreementratesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Update rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        [HttpPut]
        [Route("/api/rentalagreementrates/{id}")]
        [SwaggerOperation("RentalagreementratesIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreementRate))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RentalagreementratesIdPut([FromRoute]int id, [FromBody]RentalAgreementRate item)
        {
            return _service.RentalagreementratesIdPutAsync(id, item);
        }
    }
}
