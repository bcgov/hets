using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// District Equipment Type Controller
    /// </summary>
    [Route("/api/districtEquipmentTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DistrictEquipmentTypeController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public DistrictEquipmentTypeController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;    
            
            // set context data
            HetUser user = UserHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all district equipment types
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("DistrictEquipmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<HetDistrictEquipmentType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictEquipmentTypesGet()
        {
            // get current users district id
            int? districtId = UserHelper.GetUsersDistrictId(_context, HttpContext);

            // not found
            if (districtId == null) return new ObjectResult(new List<HetDistrictEquipmentType>());

            List<HetDistrictEquipmentType> equipmentTypes = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .Where(x => x.District.DistrictId == districtId)
                .OrderBy(x => x.DistrictEquipmentName)
                .ToList();            

            return new ObjectResult(equipmentTypes);
        }

        /// <summary>
        /// Delete district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("DistrictEquipmentTypesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult DistrictEquipmentTypesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetDistrictEquipmentType.Any(a => a.DistrictEquipmentTypeId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetDistrictEquipmentType item = _context.HetDistrictEquipmentType.First(a => a.DistrictEquipmentTypeId == id);

            _context.HetDistrictEquipmentType.Remove(item);            

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Get district equipment type by id
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdGet")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult DistrictEquipmentTypesIdGet([FromRoute]int id)
        {
            HetDistrictEquipmentType equipmentType = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .FirstOrDefault(a => a.DistrictEquipmentTypeId == id);
                        
            return new ObjectResult(new HetsResponse(equipmentType));
        }

        /// <summary>
        /// Create or update district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update (0 to create)</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdPost")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult DistrictEquipmentTypesIdPost([FromRoute]int id, [FromBody]HetDistrictEquipmentType item)
        {
            if (id != item.Id)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update equipment type            
            if (item.Id > 0)
            {
                bool exists = _context.HetDistrictEquipmentType.Any(a => a.DistrictEquipmentTypeId == id);

                if (!exists)
                {
                    // not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // get record
                HetDistrictEquipmentType equipment = _context.HetDistrictEquipmentType.First(x => x.DistrictEquipmentTypeId == id);

                equipment.DistrictEquipmentName = item.DistrictEquipmentName;
                equipment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                equipment.DistrictId = item.DistrictId;
                equipment.EquipmentTypeId = item.EquipmentTypeId;
            }
            else
            {
                HetDistrictEquipmentType equipment = new HetDistrictEquipmentType
                {
                    DistrictEquipmentName = item.DistrictEquipmentName,
                    DistrictId = item.District != null ? item.DistrictId : null,
                    EquipmentTypeId = item.EquipmentTypeId
                };

                _context.HetDistrictEquipmentType.Add(equipment);
            }

            // save the changes
            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.Id;

            // return the updated equipment type record
            HetDistrictEquipmentType equipmentType = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .FirstOrDefault(a => a.DistrictEquipmentTypeId == id);

            return new ObjectResult(new HetsResponse(equipmentType));
        }        
    }
}
