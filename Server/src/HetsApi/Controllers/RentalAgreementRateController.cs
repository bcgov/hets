using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Rate Controller
    /// </summary>
    [Route("/api/rentalAgreementRates")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementRateController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public RentalAgreementRateController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Delete rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("RentalAgreementRatesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementRate))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult RentalAgreementRatesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreementRate.Any(a => a.RentalAgreementRateId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementRate rate = _context.HetRentalAgreementRate.First(a => a.RentalAgreementRateId == id);

            _context.HetRentalAgreementRate.Remove(rate);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(rate));
        }

        /// <summary>
        /// Update rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("RentalAgreementRatesIdPut")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementRate))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult RentalAgreementRatesIdPut([FromRoute]int id, [FromBody]HetRentalAgreementRate item)
        {
            bool exists = _context.HetRentalAgreementRate.Any(a => a.RentalAgreementRateId == id);

            // not found
            if (!exists || id != item.RentalAgreementRateId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the rate period type id
            int ratePeriodTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context) ?? throw new DataException("Rate Period Id cannot be null");

            // get record
            HetRentalAgreementRate rate = _context.HetRentalAgreementRate.First(a => a.RentalAgreementRateId == id);

            rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;

            rate.Comment = item.Comment;
            rate.ComponentName = item.ComponentName;
            rate.IsIncludedInTotal = item.IsIncludedInTotal;
            rate.Rate = item.Rate;
            rate.RatePeriodTypeId = ratePeriodTypeId;
            rate.Active = true;
            rate.Overtime = false;
            rate.Set = item.Set;

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(rate));
        }
    }
}
