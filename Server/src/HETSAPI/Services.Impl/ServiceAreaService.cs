using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Service Area Service
    /// </summary>
    public class ServiceAreaService : IServiceAreaService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Service Area Service Constructor
        /// </summary>
        public ServiceAreaService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(ServiceArea item)
        {
            if (item != null && item.District != null)
                item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
        }

        /// <summary>
        /// Create bulk service area records
        /// </summary>
        /// <remarks>Adds a number of districts.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceAreasBulkPostAsync(ServiceArea[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (ServiceArea item in items)
            {
                AdjustRecord(item);

                bool exists = _context.ServiceAreas.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.ServiceAreas.Update(item);
                }
                else
                {
                    _context.ServiceAreas.Add(item);
                }                
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all service area
        /// </summary>
        /// <remarks>Returns a list of available service areas</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceAreasGetAsync()
        {
            List<ServiceArea> result = _context.ServiceAreas
                .Include(x => x.District.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
