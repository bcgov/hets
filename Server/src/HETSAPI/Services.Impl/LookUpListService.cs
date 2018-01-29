using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Lookup List Service
    /// </summary>
    public class LookupListService : ILookupListService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Lookup List Service Constructor
        /// </summary>
        public LookupListService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk lookup records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult LookuplistsBulkPostAsync(LookupList[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (LookupList item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.LookupLists.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all lookup lists
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult LookuplistsGetAsync()
        {
            List<LookupList> result = _context.LookupLists.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete lookup list
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdDeletePostAsync(int id)
        {
            bool exists = _context.LookupLists.Any(a => a.Id == id);

            if (exists)
            {
                LookupList item = _context.LookupLists.First(a => a.Id == id);

                if (item != null)
                {
                    _context.LookupLists.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get lookup list by id
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdGetAsync(int id)
        {
            bool exists = _context.LookupLists.Any(a => a.Id == id);

            if (exists)
            {
                LookupList result = _context.LookupLists.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update lookup list
        /// </summary>
        /// <param name="id">id of DumpTruck to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdPutAsync(int id, LookupList item)
        {
            bool exists = _context.LookupLists.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.LookupLists.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create lookup list
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult LookuplistsPostAsync(LookupList item)
        {
            bool exists = _context.LookupLists.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.LookupLists.Update(item);
            }
            else
            {
                // record not found
                _context.LookupLists.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
