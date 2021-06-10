using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;
using Microsoft.Extensions.Configuration;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Provincial Rate Types Controller
    /// </summary>
    [Route("api/provincialRateTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProvincialRateTypeController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public ProvincialRateTypeController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Get all provincial rate types
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("ProvincialRateTypesGet")]
        [SwaggerResponse(200, type: typeof(List<HetProvincialRateType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProvincialRateTypesGet()
        {
            List<HetProvincialRateType> rates = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Active)
                .ToList();

            int pseudoId = 0;

            foreach (HetProvincialRateType rateType in rates)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(rates));
        }

        /// <summary>
        /// Get all overtime provincial rate types
        /// </summary>
        [HttpGet]
        [Route("overtime")]
        [SwaggerOperation("ProvincialRateTypesOvertimeGet")]
        [SwaggerResponse(200, type: typeof(List<HetProvincialRateType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProvincialRateTypesOvertimeGet()
        {
            List<HetProvincialRateType> rates = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Active &&
                            x.Overtime)
                .ToList();

            int pseudoId = 0;

            foreach (HetProvincialRateType rateType in rates)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(rates));
        }

        /// <summary>
        /// Update provincial rate type
        /// </summary>
        /// <param name="id">id of provincial rate type to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("ProvincialRatesIdPut")]
        [SwaggerResponse(200, type: typeof(HetProvincialRateType))]
        [RequiresPermission(HetPermission.CodeTableManagement, HetPermission.WriteAccess)]
        public virtual IActionResult ProvincialRatesIdPut([FromRoute]int id, [FromBody]HetProvincialRateType item)
        {
            bool exists = _context.HetProvincialRateType.Any(a => a.RateType == item.RateType);

            // not found
            if (!exists || id != item.Id) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetProvincialRateType rate = _context.HetProvincialRateType.First(a => a.RateType == item.RateType);

            rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            rate.Description = item.Description;
            rate.Rate = item.Rate;
            rate.Active = item.Active;
            rate.IsIncludedInTotal = item.IsIncludedInTotal;
            rate.IsInTotalEditable = item.IsInTotalEditable;
            rate.IsRateEditable = item.IsRateEditable;
            rate.IsPercentRate = item.IsPercentRate;
            rate.PeriodType = item.PeriodType;

            // save the changes
            _context.SaveChanges();

            // get the updated record and return
            rate = _context.HetProvincialRateType.First(a => a.RateType == item.RateType);
            rate.Id = id;

            return new ObjectResult(new HetsResponse(rate));
        }
    }
}
