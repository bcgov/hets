using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Types Controller
    /// </summary>
    [Route("api/equipmentTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;

        public EquipmentTypeController(DbAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all equipment types
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<EquipmentTypeDto>> EquipmentTypesGet()
        {
            List<HetEquipmentType> equipmentTypes = _context.HetEquipmentTypes.ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<EquipmentTypeDto>>(equipmentTypes)));
        }
    }
}
