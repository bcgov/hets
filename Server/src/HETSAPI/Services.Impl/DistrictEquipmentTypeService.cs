using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HetsApi.Services.Impl
{
    /// <summary>
    /// District Equipment Type Service
    /// </summary>
    public class DistrictEquipmentTypeService : ServiceBase, IDistrictEquipmentTypeService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// District Equipment Type Service Constructor
        /// </summary>
        public DistrictEquipmentTypeService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(DistrictEquipmentType item)
        {  
            if (item != null)
            {
                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);                    
                }
                
                if (item.EquipmentType != null)
                {
                    item.EquipmentType = _context.EquipmentTypes.FirstOrDefault(a => a.Id == item.EquipmentType.Id);
                }
            }                     
        }

        /// <summary>
        /// Create bulk district equipment type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult DistrictEquipmentTypesBulkPostAsync(DistrictEquipmentType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (DistrictEquipmentType item in items)
            {
                AdjustRecord(item);
                
                // determine if this is an insert or an update            
                bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }
            
            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get district equipment types
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictEquipmentTypesGetAsync()
        {
            // return for the current users district only
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            List<DistrictEquipmentType> result = _context.DistrictEquipmentTypes.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .Where(x => x.District.Id == districtId)
                .OrderBy(x => x.DistrictEquipmentName)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdDeletePostAsync(int id)
        {
            bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);

            if (exists)
            {
                DistrictEquipmentType item = _context.DistrictEquipmentTypes.First(a => a.Id == id);

                if (item != null)
                {
                    _context.DistrictEquipmentTypes.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get district equipment type by id
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdGetAsync(int id)
        {
            bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);

            if (exists)
            {
                DistrictEquipmentType result = _context.DistrictEquipmentTypes.AsNoTracking()
                    .Include(x => x.District)
                        .ThenInclude(y => y.Region)
                    .Include(x => x.EquipmentType)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create or update district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update (o to create)</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdPostAsync(int id, DistrictEquipmentType item)
        {            
            if (id != item.Id)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update contact            
            if (item.Id > 0)
            {
                bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);

                if (!exists)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // get record
                DistrictEquipmentType equipment = _context.DistrictEquipmentTypes.First(x => x.Id == id);

                equipment.DistrictEquipmentName = item.DistrictEquipmentName;
                
                if (item.District != null)
                {
                    equipment.DistrictId = item.District.Id;
                }
                else
                {
                    equipment.District = null;
                }

                if (item.EquipmentType != null)
                {
                    equipment.EquipmentTypeId = item.EquipmentType.Id;
                }
                else
                {
                    equipment.EquipmentType = null;
                }
            }
            else
            {
                // get child records
                AdjustRecord(item);

                _context.DistrictEquipmentTypes.Add(item);
            }

            // Save the changes
            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.Id;

            // return the updated record
            DistrictEquipmentType result = _context.DistrictEquipmentTypes.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .First(a => a.Id == id);

            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
