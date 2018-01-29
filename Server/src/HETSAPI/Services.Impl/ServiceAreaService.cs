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
    /// Service Area Service
    /// </summary>
    public class ServiceAreaService : IServiceAreaService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Service Area Service Constructor
        /// </summary>
        public ServiceAreaService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        /// <summary>
        /// Delete service area
        /// </summary>
        /// <remarks>Deletes a Service Area</remarks>
        /// <param name="id">id of Service Area to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult ServiceAreasIdDeletePostAsync(int id)
        {
            bool exists = _context.ServiceAreas.Any(a => a.Id == id);

            if (exists)
            {
                ServiceArea item = _context.ServiceAreas.First(a => a.Id == id);

                if (item != null)
                {
                    _context.ServiceAreas.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get service area by id
        /// </summary>
        /// <remarks>Returns a specific Service Area</remarks>
        /// <param name="id">id of Service Area to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceAreasIdGetAsync(int id)
        {
            bool exists = _context.ServiceAreas.Any(a => a.Id == id);

            if (exists)
            {
                ServiceArea result = _context.ServiceAreas
                    .Where(a => a.Id == id)
                    .Include(a => a.District.Region)
                    .FirstOrDefault();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update service area
        /// </summary>
        /// <remarks>Updates a Service Area</remarks>
        /// <param name="id">id of Service Area to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult ServiceAreasIdPutAsync(int id, ServiceArea item)
        {
            bool exists = _context.ServiceAreas.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                AdjustRecord(item);
                _context.ServiceAreas.Update(item);

                // save the changes
                _context.SaveChanges();
                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create service area
        /// </summary>
        /// <remarks>Adds a Service Area</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceAreasPostAsync(ServiceArea item)
        {
            AdjustRecord(item);
            bool exists = _context.ServiceAreas.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.ServiceAreas.Update(item);
            }
            else
            {
                // record not found
                _context.ServiceAreas.Add(item);
            }

            // save the changes
            _context.SaveChanges();
            return new ObjectResult(new HetsResponse(item));
        }
    }
}
