using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Condition Type Controller
    /// </summary>
    [Route("api/conditionTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ConditionTypeController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ConditionTypeController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all condition types (filtered by user's District)
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ConditionTypeDto>> ConditionTypesGet()
        {
            // get current users district id
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // not found
            if (districtId == null) return new ObjectResult(new List<HetConditionType>());

            // get condition types for this district
            List<HetConditionType> conditionTypes = _context.HetConditionTypes.AsNoTracking()
                .Include(x => x.District)
                .Where(x => x.Active &&
                            x.District.DistrictId == districtId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ConditionTypeDto>>(conditionTypes)));
        }

        /// <summary>
        /// Delete condition type
        /// </summary>
        /// <param name="id">id of Condition Type to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<ConditionTypeDto> ConditionTypesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetConditionTypes.Any(a => a.ConditionTypeId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetConditionType item = _context.HetConditionTypes.First(a => a.ConditionTypeId == id);

            _context.HetConditionTypes.Remove(item);

            // save changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<ConditionTypeDto>(item)));
        }

        /// <summary>
        /// Get a specific condition record
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual ActionResult<ConditionTypeDto> ConditionTypesIdGet([FromRoute]int id)
        {
            // get condition type
            HetConditionType conditionType = _context.HetConditionTypes.AsNoTracking()
                .Include(x => x.District)
                .FirstOrDefault(x => x.ConditionTypeId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<ConditionTypeDto>(conditionType)));
        }

        /// <summary>
        /// Create or update a Condition Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}")]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<ConditionTypeDto> ConditionTypesIdPost([FromRoute]int id, [FromBody]ConditionTypeDto item)
        {
            if (id != item.ConditionTypeId)
            {
                // record not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update contact
            if (item.ConditionTypeId > 0)
            {
                bool exists = _context.HetConditionTypes.Any(a => a.ConditionTypeId == id);

                if (!exists)
                {
                    // record not found
                    return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // get record
                HetConditionType condition = _context.HetConditionTypes.First(x => x.ConditionTypeId == id);

                condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                condition.ConditionTypeCode = item.ConditionTypeCode;
                condition.Description = item.Description;
                condition.Active = item.Active;
                condition.DistrictId = item.District.DistrictId;
            }
            else
            {
                HetConditionType condition = new HetConditionType
                {
                    ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                    ConditionTypeCode = item.ConditionTypeCode,
                    Description = item.Description,
                    Active = item.Active,
                    DistrictId = item.District.DistrictId
                };

                _context.HetConditionTypes.Add(condition);
            }

            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.ConditionTypeId;

            // return the updated condition type record
            HetConditionType conditionType = _context.HetConditionTypes.AsNoTracking()
                .Include(x => x.District)
                .FirstOrDefault(x => x.ConditionTypeId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<ConditionTypeDto>(conditionType)));
        }
    }
}
