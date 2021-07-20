using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HetsApi.Model;
using HetsData.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Region Controller
    /// </summary>
    [Route("api/regions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RegionController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;

        public RegionController(DbAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<List<RegionDto>> RegionsGet()
        {
            List<HetRegion> regions = _context.HetRegions.ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RegionDto>>(regions)));
        }
    }
}
