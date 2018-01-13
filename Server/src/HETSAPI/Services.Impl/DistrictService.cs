using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// District Service
    /// </summary>
    public class DistrictService : IDistrictService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// District Service Constructor
        /// </summary>
        public DistrictService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void AdjustRecord (District item)
        {
            if (item != null && item.Region != null)
                item.Region = _context.Regions.FirstOrDefault(x => x.Id == item.Region.Id);
        }

        /// <summary>
        /// Create bulk district records
        /// </summary>
        /// <remarks>Adds a number of districts.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsBulkPostAsync(District[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (District item in items)
            {
                AdjustRecord(item);

                bool exists = _context.Districts.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Districts.Update(item);
                }
                else
                {
                    _context.Districts.Add(item);
                }               
            }

            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all districts
        /// </summary>
        /// <remarks>Returns a list of available districts</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsGetAsync()
        {
            // eager loading of regions
            List<District> result = _context.Districts
                    .Include(x => x.Region)
                    .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete district
        /// </summary>
        /// <remarks>Deletes a district</remarks>
        /// <param name="id">id of District to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        public virtual IActionResult DistrictsIdDeletePostAsync(int id)
        {
            bool exists = _context.Districts.Any(a => a.Id == id);

            if (exists)
            {
                District item = _context.Districts.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Districts.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get district by id
        /// </summary>
        /// <remarks>Returns a specific district</remarks>
        /// <param name="id">id of Districts to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsIdGetAsync(int id)
        {
            bool exists = _context.Districts.Any(a => a.Id == id);

            if (exists)
            {
                District result = _context.Districts
                    .Where(a => a.Id == id)
                    .Include(a => a.Region)
                    .FirstOrDefault();
                    
                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update district
        /// </summary>
        /// <remarks>Updates a district</remarks>
        /// <param name="id">id of District to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        public virtual IActionResult DistrictsIdPutAsync(int id, District item)
        {
            bool exists = _context.Districts.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                AdjustRecord(item);
                _context.Districts.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get service areas associated with a district
        /// </summary>
        /// <remarks>Returns the Service Areas for a specific region</remarks>
        /// <param name="id">id of District for which to fetch the ServiceAreas</param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsIdServiceareasGetAsync(int id)
        {
            var result = "";
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Create district
        /// </summary>
        /// <remarks>Adds a district</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsPostAsync(District item)
        {            
            AdjustRecord(item);

            bool exists = _context.Districts.Any(a => a.Id == item.Id);

            if (exists )
            {
                _context.Districts.Update(item);            
            }
            else
            {
                // record not found
                _context.Districts.Add(item);                
            }

            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));      
        }
    }
}
