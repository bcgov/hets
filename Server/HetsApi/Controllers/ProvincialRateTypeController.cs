using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Provincial Rate Types Controller
    /// </summary>
    [Route("api/provincialRateTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProvincialRateTypeController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProvincialRateTypeController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Get all provincial rate types
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProvincialRateTypeDto>> ProvincialRateTypesGet()
        {
            List<HetProvincialRateType> rates = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Active)
                .ToList();

            int pseudoId = 0;

            foreach (HetProvincialRateType rateType in rates)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ProvincialRateTypeDto>>(rates)));
        }

        /// <summary>
        /// Get all overtime provincial rate types
        /// </summary>
        [HttpGet]
        [Route("overtime")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProvincialRateTypeDto>> ProvincialRateTypesOvertimeGet()
        {
            List<HetProvincialRateType> rates = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Active &&
                            x.Overtime)
                .ToList();

            int pseudoId = 0;

            foreach (HetProvincialRateType rateType in rates)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ProvincialRateTypeDto>>(rates)));
        }

        /// <summary>
        /// Update provincial rate type
        /// </summary>
        /// <param name="id">id of provincial rate type to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.CodeTableManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<ProvincialRateTypeDto> ProvincialRatesIdPut([FromRoute] int id, [FromBody] ProvincialRateTypeDto item)
        {
            bool exists = _context.HetProvincialRateTypes.Any(a => a.RateType == item.RateType);

            // not found
            if (!exists || id != item.Id) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetProvincialRateType rate = _context.HetProvincialRateTypes.First(a => a.RateType == item.RateType);

            rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            rate.Description = item.Description;
            rate.Rate = item.Rate;
            rate.Active = item.Active;
            rate.IsIncludedInTotal = item.IsIncludedInTotal;
            rate.IsInTotalEditable = item.IsInTotalEditable;
            rate.IsRateEditable = item.IsRateEditable;
            rate.IsPercentRate = item.IsPercentRate;

            // save the changes
            _context.SaveChanges();

            // get the updated record and return
            rate = _context.HetProvincialRateTypes.First(a => a.RateType == item.RateType);
            rate.Id = id;

            return new ObjectResult(new HetsResponse(_mapper.Map<ProvincialRateTypeDto>(rate)));
        }
    }
}
