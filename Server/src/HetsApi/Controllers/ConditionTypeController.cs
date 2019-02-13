using System.Collections.Generic;
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
    /// Condition Type Controller
    /// </summary>
    [Route("api/conditionTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ConditionTypeController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public ConditionTypeController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Get all condition types (filtered by user's District)
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("ConditionTypesGet")]
        [SwaggerResponse(200, type: typeof(List<HetConditionType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ConditionTypesGet()
        {
            // get current users district id
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, HttpContext);

            // not found
            if (districtId == null) return new ObjectResult(new List<HetConditionType>());

            // get condition types for this district
            List<HetConditionType> conditionTypes = _context.HetConditionType.AsNoTracking()
                .Include(x => x.District)
                .Where(x => x.Active &&
                            x.District.DistrictId == districtId)
                .ToList();

            return new ObjectResult(new HetsResponse(conditionTypes));
        }

        /// <summary>
        /// Delete condition type
        /// </summary>
        /// <param name="id">id of Condition Type to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("ConditionTypesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetConditionType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult ConditionTypesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetConditionType.Any(a => a.ConditionTypeId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetConditionType item = _context.HetConditionType.First(a => a.ConditionTypeId == id);

            _context.HetConditionType.Remove(item);

            // save changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Get a specific condition record
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("ConditionTypesIdGet")]
        [SwaggerResponse(200, type: typeof(HetConditionType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult ConditionTypesIdGet([FromRoute]int id)
        {
            // get condition type
            HetConditionType conditionType = _context.HetConditionType.AsNoTracking()
                .Include(x => x.District)
                .FirstOrDefault(x => x.ConditionTypeId == id);

            return new ObjectResult(new HetsResponse(conditionType));
        }

        /// <summary>
        /// Create or update a Condition Type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}")]
        [SwaggerOperation("ConditionTypesIdPost")]
        [SwaggerResponse(200, type: typeof(HetConditionType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult ConditionTypesIdPost([FromRoute]int id, [FromBody]HetConditionType item)
        {
            if (id != item.ConditionTypeId)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update contact
            if (item.ConditionTypeId > 0)
            {
                bool exists = _context.HetConditionType.Any(a => a.ConditionTypeId == id);

                if (!exists)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // get record
                HetConditionType condition = _context.HetConditionType.First(x => x.ConditionTypeId == id);

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

                _context.HetConditionType.Add(condition);
            }

            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.ConditionTypeId;

            // return the updated condition type record
            HetConditionType conditionType = _context.HetConditionType.AsNoTracking()
                .Include(x => x.District)
                .FirstOrDefault(x => x.ConditionTypeId == id);

            return new ObjectResult(new HetsResponse(conditionType));
        }
    }
}
