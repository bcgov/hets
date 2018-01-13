using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// District Equipment Type Service
    /// </summary>
    public class DistrictEquipmentTypeService : IDistrictEquipmentTypeService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// District Equipment Type Service Constructor
        /// </summary>
        public DistrictEquipmentTypeService(DbAppContext context, IConfiguration configuration)
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
            var result = _context.DistrictEquipmentTypes
                .Include(x => x.District.Region)
                .Include(x => x.EquipmentType)
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
                DistrictEquipmentType result = _context.DistrictEquipmentTypes
                    .Include(x => x.District)
                    .Include(x => x.EquipmentType)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdPutAsync(int id, DistrictEquipmentType item)
        {
            AdjustRecord(item);

            bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.DistrictEquipmentTypes.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create district equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult DistrictEquipmentTypesPostAsync(DistrictEquipmentType item)
        {
            AdjustRecord(item);

            bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.DistrictEquipmentTypes.Update(item);
            }
            else
            {
                // record not found
                _context.DistrictEquipmentTypes.Add(item);
            }
            
            // Save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
