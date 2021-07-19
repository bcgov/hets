using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HetsApi.Model;
using HetsData.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Service Areas Controller
    /// </summary>
    [Route("api/serviceAreas")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ServiceAreaController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;

        public ServiceAreaController(DbAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all service areas
        /// </summary>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<List<ServiceAreaDto>> ServiceAreasGet()
        {
            List<HetServiceArea> serviceAreas = _context.HetServiceAreas
                .Include(x => x.District.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ServiceAreaDto>>(serviceAreas)));
        }
    }
}
