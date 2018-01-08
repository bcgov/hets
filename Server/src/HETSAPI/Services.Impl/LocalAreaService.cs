using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalAreaService : ILocalAreaService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public LocalAreaService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(LocalArea item)
        {
            if (item != null && item.ServiceArea != null)
                item.ServiceArea = _context.ServiceAreas.FirstOrDefault(a => a.Id == item.ServiceArea.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalArea created</response>
        public virtual IActionResult LocalAreasBulkPostAsync(LocalArea[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (LocalArea item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update            
                bool exists = _context.LocalAreas.Any(a => a.Id == item.Id);
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
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult LocalAreasGetAsync()
        {
            var result = _context.LocalAreas
        .Include(x => x.ServiceArea.District.Region)
        .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdDeletePostAsync(int id)
        {
            var exists = _context.LocalAreas.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.LocalAreas.First(a => a.Id == id);
                if (item != null)
                {
                    _context.LocalAreas.Remove(item);
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
        /// <param name="id">id of LocalArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdGetAsync(int id)
        {
            var exists = _context.LocalAreas.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.LocalAreas
                    .Include(x => x.ServiceArea.District.Region)
                    .First(a => a.Id == id);
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
        /// <param name="id">id of LocalArea to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdPutAsync(int id, LocalArea item)
        {
            AdjustRecord(item);
            var exists = _context.LocalAreas.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.LocalAreas.Update(item);
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
        /// <param name="item"></param>
        /// <response code="201">LocalArea created</response>
        public virtual IActionResult LocalAreasPostAsync(LocalArea item)
        {
            AdjustRecord(item);
            var exists = _context.LocalAreas.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.LocalAreas.Update(item);
            }
            else
            {
                // record not found
                _context.LocalAreas.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
