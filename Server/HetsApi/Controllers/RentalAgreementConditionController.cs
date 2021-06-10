using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Condition Controller
    /// </summary>
    [Route("/api/rentalAgreementConditions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementConditionController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public RentalAgreementConditionController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Delete rental agreement condition
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("RentalAgreementConditionsIdDeletePost")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult RentalAgreementConditionsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetRentalAgreementCondition.Any(a => a.RentalAgreementConditionId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementCondition condition = _context.HetRentalAgreementCondition.AsNoTracking()
                .First(a => a.RentalAgreementConditionId == id);

            _context.HetRentalAgreementCondition.Remove(condition);

            // save changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(condition));
        }

        /// <summary>
        /// Update rental agreement condition by id
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("RentalAgreementConditionsIdPut")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreementCondition))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult RentalAgreementConditionsIdPut([FromRoute]int id, [FromBody]HetRentalAgreementCondition item)
        {
            bool exists = _context.HetRentalAgreementCondition.Any(a => a.RentalAgreementConditionId == id);

            // not found
            if (!exists || id != item.RentalAgreementConditionId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreementCondition condition = _context.HetRentalAgreementCondition
                .First(a => a.RentalAgreementConditionId == id);

            condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            condition.Comment = item.Comment;
            condition.ConditionName = item.ConditionName;

            // save changes
            _context.SaveChanges();

            // return the updated condition record
            condition = _context.HetRentalAgreementCondition.AsNoTracking()
                .First(a => a.RentalAgreementConditionId == id);

            return new ObjectResult(new HetsResponse(condition));
        }
    }
}
