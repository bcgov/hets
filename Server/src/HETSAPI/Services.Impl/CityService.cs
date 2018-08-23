using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HetsApi.Services.Impl
{
    /// <summary>
    /// City Service
    /// </summary>
    public class CityService : ICityService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// City Service Constructor
        /// </summary>
        public CityService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bulk create city records
        /// </summary>
        /// <remarks>Adds a number of cities</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult CitiesBulkPostAsync(City[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (City item in items)
            {
                bool exists = _context.Cities.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Cities.Update(item);
                }
                else
                {
                    _context.Cities.Add(item);
                }                
            }

            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <remarks>Returns a list of cities</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult CitiesGetAsync()
        {
            var result = _context.Cities.ToList();
            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
