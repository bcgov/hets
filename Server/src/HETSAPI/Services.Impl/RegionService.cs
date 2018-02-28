using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// Region Service
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Region Service Constructor
        /// </summary>
        public RegionService (DbAppContext context)
        {
            _context = context;           
        }
	
        /// <summary>
        /// Create bulk region records
        /// </summary>
        /// <remarks>Adds a number of regions.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult RegionsBulkPostAsync (Region[] items)        
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Region item in items)
            {
                bool exists = _context.Regions.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Regions.Update(item);
                }
                else
                {
                    _context.Regions.Add(item);
                }                    
            }
            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <remarks>Returns a list of available regions</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult RegionsGetAsync ()        
        {
            List<Region> result = _context.Regions.ToList();
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
