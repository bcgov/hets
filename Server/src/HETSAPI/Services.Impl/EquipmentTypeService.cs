using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Equipment TYpe Service
    /// </summary>
    public class EquipmentTypeService : IEquipmentTypeService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Equipment TYpe Service Constructor
        /// </summary>
        public EquipmentTypeService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        private void AdjustRecord(EquipmentType item)
        {
            // Equipment Type no longer has any child objects.
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
        /// Get all equipment types
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentTypesGetAsync()
        {
            var result = _context.EquipmentTypes.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult EquipmentTypesIdDeletePostAsync(int id)
        {
            bool exists = _context.EquipmentTypes.Any(a => a.Id == id);

            if (exists)
            {
                EquipmentType item = _context.EquipmentTypes.First(a => a.Id == id);

                if (item != null)
                {
                    _context.EquipmentTypes.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get equipment type by id
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult EquipmentTypesIdGetAsync(int id)
        {
            bool exists = _context.EquipmentTypes.Any(a => a.Id == id);

            if (exists)
            {
                EquipmentType result = _context.EquipmentTypes.First(a => a.Id == id);


                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult EquipmentTypesIdPutAsync(int id, EquipmentType item)
        {
            bool exists = _context.EquipmentTypes.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.EquipmentTypes.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult EquipmentTypesPostAsync(EquipmentType item)
        {
            bool exists = _context.EquipmentTypes.Any(a => a.Id == item.Id);

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

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
