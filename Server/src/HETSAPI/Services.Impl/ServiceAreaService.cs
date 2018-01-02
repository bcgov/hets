using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceAreaService : IServiceAreaService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public ServiceAreaService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(ServiceArea item)
        {
            if (item != null)
            {
                // Adjust district
                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of districts.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceareasBulkPostAsync(ServiceArea[] items)
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
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a list of available districts</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceareasGetAsync()
        {
            var result = _context.ServiceAreas
                .Include(x => x.District.Region)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Deletes a Service Area</remarks>
        /// <param name="id">id of Service Area to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult ServiceareasIdDeletePostAsync(int id)
        {
            var exists = _context.ServiceAreas.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.ServiceAreas.First(a => a.Id == id);
                if (item != null)
                {
                    _context.ServiceAreas.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a specific Service Area</remarks>
        /// <param name="id">id of Service Area to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceareasIdGetAsync(int id)
        {
            var exists = _context.ServiceAreas.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.ServiceAreas
                    .Where(a => a.Id == id)
                    .Include(a => a.District.Region)
                    .FirstOrDefault();
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a Service Area</remarks>
        /// <param name="id">id of Service Area to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult ServiceareasIdPutAsync(int id, ServiceArea item)
        {
            var exists = _context.ServiceAreas.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                AdjustRecord(item);
                _context.ServiceAreas.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a Service Area</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ServiceareasPostAsync(ServiceArea item)
        {
            AdjustRecord(item);
            var exists = _context.ServiceAreas.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.ServiceAreas.Update(item);
            }
            else
            {
                // record not found
                _context.ServiceAreas.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
