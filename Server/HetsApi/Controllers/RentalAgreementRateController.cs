using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Rate Controller
    /// </summary>
    [Route("/api/rentalAgreementRates")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementRateController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public RentalAgreementRateController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementRateDto> RentalAgreementRatesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreementRates.Any(a => a.RentalAgreementRateId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementRate rate = _context.HetRentalAgreementRates.First(a => a.RentalAgreementRateId == id);

            _context.HetRentalAgreementRates.Remove(rate);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementRateDto>(rate)));
        }

        /// <summary>
        /// Update rental agreement rate
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementRateDto> RentalAgreementRatesIdPut([FromRoute]int id, 
            [FromBody]RentalAgreementRateDto item)
        {
            bool exists = _context.HetRentalAgreementRates.Any(a => a.RentalAgreementRateId == id);

            // not found
            if (!exists || id != item.RentalAgreementRateId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the rate period type id
            int ratePeriodTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context) ?? throw new DataException("Rate Period Id cannot be null");

            // get record
            HetRentalAgreementRate rate = _context.HetRentalAgreementRates.First(a => a.RentalAgreementRateId == id);

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

            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementRateDto>(rate)));
        }
    }
}
