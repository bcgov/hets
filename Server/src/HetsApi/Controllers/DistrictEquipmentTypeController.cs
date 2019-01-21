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
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace HetsApi.Controllers
{
    /// <summary>
    /// District Equipment Type Controller
    /// </summary>
    [Route("api/districtEquipmentTypes")]
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
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
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
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, HttpContext);

            // not found
            if (districtId == null) return new ObjectResult(new List<HetDistrictEquipmentType>());

            List<HetDistrictEquipmentType> equipmentTypes = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.EquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.LocalArea)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.EquipmentStatusType)
                .Where(x => x.District.DistrictId == districtId &&
                            !x.Deleted)
                .OrderBy(x => x.DistrictEquipmentName)
                .ToList();

            foreach (HetDistrictEquipmentType equipmentType in equipmentTypes)
            {
                IEnumerable<HetEquipment> approvedEquipment = equipmentType.HetEquipment
                    .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved);

                List<HetEquipment> hetEquipments = approvedEquipment.ToList();
                equipmentType.EquipmentCount = hetEquipments.Count;

                foreach(HetEquipment equipment in hetEquipments)
                {                    
                    LocalAreaEquipment localAreaEquipment = equipmentType.LocalAreas
                        .FirstOrDefault(x => x.Id == equipment.LocalAreaId);

                    if (localAreaEquipment == null)
                    {
                        localAreaEquipment = new LocalAreaEquipment
                        {
                            Id = equipment.LocalArea.LocalAreaId,
                            Name = equipment.LocalArea.Name,
                            EquipmentCount = 1
                        };

                        equipmentType.LocalAreas.Add(localAreaEquipment);
                    }
                    else
                    {
                        localAreaEquipment.EquipmentCount = localAreaEquipment.EquipmentCount + 1;
                    }
                }

                // remove unnecessary data
                equipmentType.HetEquipment = null;
            }

            return new ObjectResult(new HetsResponse(equipmentTypes));
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

            // HETS-978 - Give a clear error message when deleting equipment type fails
            int? archiveStatus = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (archiveStatus == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            int? pendingStatus = StatusHelper.GetStatusId(HetEquipment.StatusPending, "equipmentStatus", _context);
            if (pendingStatus == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId &&
                                     x.EquipmentStatusTypeId != archiveStatus &&
                                     x.EquipmentStatusTypeId != pendingStatus);

            if (equipment != null)
            {
                return new ObjectResult(new HetsResponse("HETS-37", ErrorViewModel.GetDescription("HETS-37", _configuration)));
            }

            // else "SOFT" delete record
            item.Deleted = true;
            _context.SaveChanges();

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
            if (id != item.DistrictEquipmentTypeId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update equipment type            
            if (item.DistrictEquipmentTypeId > 0)
            {
                bool exists = _context.HetDistrictEquipmentType.Any(a => a.DistrictEquipmentTypeId == id);

                // not found
                if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));                

                // get record
                HetDistrictEquipmentType equipment = _context.HetDistrictEquipmentType.First(x => x.DistrictEquipmentTypeId == id);

                equipment.DistrictEquipmentName = item.DistrictEquipmentName;
                equipment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                equipment.DistrictId = item.District.DistrictId;
                equipment.EquipmentTypeId = item.EquipmentType.EquipmentTypeId;
            }
            else
            {
                HetDistrictEquipmentType equipment = new HetDistrictEquipmentType
                {
                    DistrictEquipmentName = item.DistrictEquipmentName,
                    DistrictId = item.District.DistrictId,
                    EquipmentTypeId = item.EquipmentType.EquipmentTypeId
                };

                _context.HetDistrictEquipmentType.Add(equipment);
            }

            // save the changes
            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.DistrictEquipmentTypeId;

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
