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

        private void AdjustRecord (District item)
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

        #region District Owners

        /// <summary>
        /// Get all owners for a district
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictOwnersGetAsync(int id)
        {
            bool exists = _context.Districts.Any(a => a.Id == id);

            if (exists)
            {
                List<Owner> result = _context.Owners.AsNoTracking()
                    .Where(x => x.LocalArea.ServiceArea.District.Id == id)
                    .OrderBy(x => x.OrganizationName)
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region District Local Areas

        /// <summary>
        /// Get all local areas for a district
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictLocalAreasGetAsync(int id)
        {
            bool exists = _context.Districts.Any(a => a.Id == id);

            if (exists)
            {
                List<LocalArea> result = _context.LocalAreas.AsNoTracking()
                    .Where(x => x.ServiceArea.District.Id == id)
                    .OrderBy(x => x.Name)
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion
    }
}
