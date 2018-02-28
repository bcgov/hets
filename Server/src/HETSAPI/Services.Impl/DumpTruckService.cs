using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Dump Truck Service
    /// </summary>
    public class DumpTruckService : IDumpTruckService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Dump Truck Service Constructor
        /// </summary>
        public DumpTruckService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create bulk dumptruck records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">DumpTruck created</response>
        public virtual IActionResult DumptrucksBulkPostAsync(DumpTruck[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (DumpTruck item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.DumpTrucks.Any(a => a.Id == item.Id);

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
    }
}
