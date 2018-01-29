using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Dump Truck Service
    /// </summary>
    public class DumpTruckService : IDumpTruckService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Dump Truck Service Constructor
        /// </summary>
        public DumpTruckService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk dumptruck records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
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

        /// <summary>
        /// Get all dump trucks
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult DumptrucksGetAsync()
        {
            List<DumpTruck> result = _context.DumpTrucks.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete dump truck
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult DumptrucksIdDeletePostAsync(int id)
        {
            bool exists = _context.DumpTrucks.Any(a => a.Id == id);

            if (exists)
            {
                DumpTruck item = _context.DumpTrucks.First(a => a.Id == id);

                if (item != null)
                {
                    _context.DumpTrucks.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get dump truck by id
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult DumptrucksIdGetAsync(int id)
        {
            bool exists = _context.DumpTrucks.Any(a => a.Id == id);

            if (exists)
            {
                DumpTruck result = _context.DumpTrucks.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update dump truck
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult DumptrucksIdPutAsync(int id, DumpTruck item)
        {
            bool exists = _context.DumpTrucks.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.DumpTrucks.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create dump truck
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult DumptrucksPostAsync(DumpTruck item)
        {
            bool exists = _context.DumpTrucks.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.DumpTrucks.Update(item);
            }
            else
            {
                // record not found
                _context.DumpTrucks.Add(item);
            }

            // Save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
