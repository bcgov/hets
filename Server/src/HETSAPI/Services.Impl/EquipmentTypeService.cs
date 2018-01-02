using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public EquipmentTypeService(DbAppContext context)
        {
            _context = context;
        }


        private void AdjustRecord(EquipmentType item)
        {
            // Equipment Type no longer has any child objects.
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult EquipmentTypesBulkPostAsync(EquipmentType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (EquipmentType item in items)
            {
                AdjustRecord(item);
                
                // determine if this is an insert or an update            
                bool exists = _context.EquipmentTypes.Any(a => a.Id == item.Id);
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
        public virtual IActionResult EquipmentTypesGetAsync()
        {
            var result = _context.EquipmentTypes
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult EquipmentTypesIdDeletePostAsync(int id)
        {
            var exists = _context.EquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.EquipmentTypes.First(a => a.Id == id);
                if (item != null)
                {
                    _context.EquipmentTypes.Remove(item);
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
        public virtual IActionResult EquipmentTypesIdGetAsync(int id)
        {
            var exists = _context.EquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.EquipmentTypes.First(a => a.Id == id);
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
        public virtual IActionResult EquipmentTypesIdPutAsync(int id, EquipmentType item)
        {
            var exists = _context.EquipmentTypes.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.EquipmentTypes.Update(item);
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
        public virtual IActionResult EquipmentTypesPostAsync(EquipmentType item)
        {
            var exists = _context.EquipmentTypes.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.EquipmentTypes.Update(item);
            }
            else
            {
                // record not found
                _context.EquipmentTypes.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
