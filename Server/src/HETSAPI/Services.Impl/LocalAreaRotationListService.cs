using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Local Area Rotation List Service
    /// </summary>
    public class LocalAreaRotationListService : ILocalAreaRotationListService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Local Area Rotation List Service Constructor
        /// </summary>
        public LocalAreaRotationListService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk area rottion list records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult LocalarearotationlistsBulkPostAsync(LocalAreaRotationList[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (LocalAreaRotationList item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.LocalAreaRotationLists.Any(a => a.Id == item.Id);

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
        /// Get all local area rotation lists
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult LocalarearotationlistsGetAsync()
        {
            List<LocalAreaRotationList> result = _context.LocalAreaRotationLists.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete local area rotation list
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LocalarearotationlistsIdDeletePostAsync(int id)
        {
            bool exists = _context.LocalAreaRotationLists.Any(a => a.Id == id);

            if (exists)
            {
                LocalAreaRotationList item = _context.LocalAreaRotationLists.First(a => a.Id == id);

                if (item != null)
                {
                    _context.LocalAreaRotationLists.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get local area rotation list by id
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LocalarearotationlistsIdGetAsync(int id)
        {
            bool exists = _context.LocalAreaRotationLists.Any(a => a.Id == id);

            if (exists)
            {
                LocalAreaRotationList result = _context.LocalAreaRotationLists.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update local area rotation list
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LocalarearotationlistsIdPutAsync(int id, LocalAreaRotationList item)
        {
            bool exists = _context.LocalAreaRotationLists.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.LocalAreaRotationLists.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create local area rotation list
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult LocalarearotationlistsPostAsync(LocalAreaRotationList item)
        {
            bool exists = _context.LocalAreaRotationLists.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.LocalAreaRotationLists.Update(item);
            }
            else
            {
                // record not found
                _context.LocalAreaRotationLists.Add(item);
            }
            // Save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
