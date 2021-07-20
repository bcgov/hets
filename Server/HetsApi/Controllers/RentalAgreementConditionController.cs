using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Condition Controller
    /// </summary>
    [Route("/api/rentalAgreementConditions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementConditionController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public RentalAgreementConditionController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete rental agreement condition
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementConditionDto> RentalAgreementConditionsIdDeletePost([FromRoute] int id)
        {
            bool exists = _context.HetRentalAgreementConditions.Any(a => a.RentalAgreementConditionId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementCondition condition = _context.HetRentalAgreementConditions.AsNoTracking()
                .First(a => a.RentalAgreementConditionId == id);

            _context.HetRentalAgreementConditions.Remove(condition);

            // save changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementConditionDto>(condition)));
        }

        /// <summary>
        /// Update rental agreement condition by id
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementConditionDto> RentalAgreementConditionsIdPut([FromRoute] int id,
            [FromBody] RentalAgreementConditionDto item)
        {
            bool exists = _context.HetRentalAgreementConditions.Any(a => a.RentalAgreementConditionId == id);

            // not found
            if (!exists || id != item.RentalAgreementConditionId)
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementCondition condition = _context.HetRentalAgreementConditions
                .First(a => a.RentalAgreementConditionId == id);

            condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            condition.Comment = item.Comment;
            condition.ConditionName = item.ConditionName;

            // save changes
            _context.SaveChanges();

            // return the updated condition record
            condition = _context.HetRentalAgreementConditions.AsNoTracking()
                .First(a => a.RentalAgreementConditionId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementConditionDto>(condition)));
        }
    }
}
