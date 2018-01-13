using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// Region Service
    /// </summary>
    public class RegionService : IRegionService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Region Service Constructor
        /// </summary>
        public RegionService (DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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


        /// <summary>
        /// Delete region
        /// </summary>
        /// <remarks>Deletes a region</remarks>
        /// <param name="id">id of Region to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        public virtual IActionResult RegionsIdDeletePostAsync(int id)
        {
            bool exists = _context.Regions.Any(a => a.Id == id);

            if (exists)
            {
                var item = _context.Regions.First(a => a.Id == id);
                _context.Regions.Remove(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get region by id
        /// </summary>
        /// <remarks>Returns a specific region</remarks>
        /// <param name="id">id of Regions to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RegionsIdGetAsync (int id)        
        {
            bool exists = _context.Regions.Any(a => a.Id == id);

            if (exists)
            {
                Region result = _context.Regions.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get districts associated with a region
        /// </summary>
        /// <remarks>Returns a list of LocalAreas for a given region</remarks>
        /// <param name="id">id of Region to fetch SchoolDistricts for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RegionsIdDistrictsGetAsync(int id)        
        {
            bool exists = _context.Regions.Any(a => a.Id == id);

            if (exists)
            {
                var result = _context.Districts.Where(a => a.Region.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update region
        /// </summary>
        /// <remarks>Updates a region</remarks>
        /// <param name="id">id of Region to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Region not found</response>
        public virtual IActionResult RegionsIdPutAsync(int id, Region item)
        {
            bool exists = _context.Regions.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {                
                _context.Entry(item).State = EntityState.Modified;

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create region
        /// </summary>
        /// <remarks>Adds a region</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult RegionsPostAsync (Region item)        
        {
            bool exists = _context.Regions.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Regions.Update(item);
            }
            else
            {
                // record not found
                _context.Regions.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
