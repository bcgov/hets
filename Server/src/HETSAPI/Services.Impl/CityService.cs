using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// City Service
    /// </summary>
    public class CityService : ICityService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// City Service Constructor
        /// </summary>
        public CityService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        /// <summary>
        /// Delete city
        /// </summary>
        /// <remarks>Deletes a City</remarks>
        /// <param name="id">id of City to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        public virtual IActionResult CitiesIdDeletePostAsync(int id)
        {
            bool exists = _context.Cities.Any(a => a.Id == id);

            if (exists)
            {
                City item = _context.Cities.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Cities.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get city by id
        /// </summary>
        /// <remarks>Returns a specific City by ID</remarks>
        /// <param name="id">id of City to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult CitiesIdGetAsync(int id)
        {
            bool exists = _context.Cities.Any(a => a.Id == id);

            if (exists)
            {
                City result = _context.Cities.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update city
        /// </summary>
        /// <remarks>Updates a City</remarks>
        /// <param name="id">id of City to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">City not found</response>
        public virtual IActionResult CitiesIdPutAsync(int id, City item)
        {
            bool exists = _context.Cities.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Cities.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create city
        /// </summary>
        /// <remarks>Adds a City</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult CitiesPostAsync(City item)
        {
            _context.Cities.Add(item);
            _context.SaveChanges();
            return new ObjectResult(new HetsResponse(item));
        }
    }
}
