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
    /// Local Area Service
    /// </summary>
    public class LocalAreaService : ILocalAreaService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Local Area Service Constructor
        /// </summary>
        public LocalAreaService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(LocalArea item)
        {
            if (item != null && item.ServiceArea != null)
                item.ServiceArea = _context.ServiceAreas.FirstOrDefault(a => a.Id == item.ServiceArea.Id);
        }

        /// <summary>
        /// Create bulk local area service records
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
        /// Get all local areas
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult LocalAreasGetAsync()
        {
            List<LocalArea> result = _context.LocalAreas
                .Include(x => x.ServiceArea.District.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete local area 
        /// </summary>
        /// <param name="id">id of LocalArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdDeletePostAsync(int id)
        {
            bool exists = _context.LocalAreas.Any(a => a.Id == id);

            if (exists)
            {
                LocalArea item = _context.LocalAreas.First(a => a.Id == id);

                if (item != null)
                {
                    _context.LocalAreas.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get local area by id
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdGetAsync(int id)
        {
            bool exists = _context.LocalAreas.Any(a => a.Id == id);

            if (exists)
            {
                LocalArea result = _context.LocalAreas
                    .Include(x => x.ServiceArea.District.Region)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update local area
        /// </summary>
        /// <param name="id">id of LocalArea to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        public virtual IActionResult LocalAreasIdPutAsync(int id, LocalArea item)
        {
            AdjustRecord(item);

            bool exists = _context.LocalAreas.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.LocalAreas.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create local area
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalArea created</response>
        public virtual IActionResult LocalAreasPostAsync(LocalArea item)
        {
            AdjustRecord(item);

            bool exists = _context.LocalAreas.Any(a => a.Id == item.Id);

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

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
