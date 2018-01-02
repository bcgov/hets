using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DistrictEquipmentTypeService : IDistrictEquipmentTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public DistrictEquipmentTypeService(DbAppContext context)
        {
            _context = context;
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
        /// 
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
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictEquipmentTypesGetAsync()
        {
            var result = _context.DistrictEquipmentTypes
                .Include(x => x.District.Region)
                .Include(x => x.EquipmentType)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdDeletePostAsync(int id)
        {
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.DistrictEquipmentTypes.First(a => a.Id == id);
                if (item != null)
                {
                    _context.DistrictEquipmentTypes.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdGetAsync(int id)
        {
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.DistrictEquipmentTypes
                    .Include(x => x.District)
                    .Include(x => x.EquipmentType)
                    .First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdPutAsync(int id, DistrictEquipmentType item)
        {
            AdjustRecord(item);
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.DistrictEquipmentTypes.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult DistrictEquipmentTypesPostAsync(DistrictEquipmentType item)
        {
            AdjustRecord(item);
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == item.Id);
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
            return new ObjectResult(item);
        }
    }
}
