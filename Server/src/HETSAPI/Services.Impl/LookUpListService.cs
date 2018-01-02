using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class LookupListService : ILookupListService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public LookupListService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
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
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult LookuplistsGetAsync()
        {
            var result = _context.LookupLists.ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdDeletePostAsync(int id)
        {
            var exists = _context.LookupLists.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.LookupLists.First(a => a.Id == id);
                if (item != null)
                {
                    _context.LookupLists.Remove(item);
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
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdGetAsync(int id)
        {
            var exists = _context.LookupLists.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.LookupLists.First(a => a.Id == id);
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
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        public virtual IActionResult LookuplistsIdPutAsync(int id, LookupList item)
        {
            var exists = _context.LookupLists.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.LookupLists.Update(item);
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
        /// <response code="201">DumpTruck created</response>
        public virtual IActionResult LookuplistsPostAsync(LookupList item)
        {
            var exists = _context.LookupLists.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.LookupLists.Update(item);
            }
            else
            {
                // record not found
                _context.LookupLists.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
