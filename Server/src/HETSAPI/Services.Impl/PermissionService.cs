using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Permission Service
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Permission Service Constructor
        /// </summary>
        public PermissionService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create bulk permission records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Permissions created</response>
        public virtual IActionResult PermissionsBulkPostAsync(Permission[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Permission item in items)
            {
                bool exists = _context.Permissions.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Permissions.Update(item);
                }
                else
                {
                    _context.Permissions.Add(item);
                }                
            }

            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <remarks>Returns a collection of permissions</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult PermissionsGetAsync()
        {
            List<PermissionViewModel> result = _context.Permissions.Select(x => x.ToViewModel()).ToList();
            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
