using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HetsApi.Services.Impl
{
    /// <summary>
    /// Equipment Type Service
    /// </summary>
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Equipment TYpe Service Constructor
        /// </summary>
        public EquipmentTypeService(DbAppContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Create bulk equipment type records
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
        /// Get all equipment types
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentTypesGetAsync()
        {
            var result = _context.EquipmentTypes.ToList();
            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
