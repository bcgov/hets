using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Permission Service
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Permission Service Constructor
        /// </summary>
        public PermissionService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk permission records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permissions created</response>
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

        /// <summary>
        /// Delete permissions
        /// </summary>
        /// <param name="id">id of Permission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        public virtual IActionResult PermissionsIdDeletePostAsync(int id)
        {
            Permission permission = _context.Permissions.FirstOrDefault(x => x.Id == id);

            if (permission == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // remove any user role associations.            
            List<RolePermission> toRemove = _context.RolePermissions.Where(x => x.Permission.Id == id).ToList();

            toRemove.ForEach(x => _context.RolePermissions.Remove(x));

            _context.Permissions.Remove(permission);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(permission.ToViewModel()));
        }

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <remarks>Returns a permission</remarks>
        /// <param name="id">id of Permission to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        public virtual IActionResult PermissionsIdGetAsync(int id)
        {
            Permission permission = _context.Permissions.FirstOrDefault(x => x.Id == id);

            if (permission == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            return new ObjectResult(permission.ToViewModel());
        }

        /// <summary>
        /// Update permissions
        /// </summary>
        /// <param name="id">id of Permission to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        public virtual IActionResult PermissionsIdPutAsync(int id, PermissionViewModel item)
        {
            Permission permission = _context.Permissions.FirstOrDefault(x => x.Id == id);

            if (permission == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }
            
            permission.Code = item.Code;
            permission.Description = item.Description;
            permission.Name = item.Name;            

            // save changes
            _context.Permissions.Update(permission);
            _context.SaveChanges();

            return new ObjectResult(permission.ToViewModel());
        }

        /// <summary>
        /// Create permission
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Permission created</response>
        public virtual IActionResult PermissionsPostAsync(PermissionViewModel item)
        {
            Permission permission = new Permission
            {
                Code = item.Code,
                Description = item.Description,
                Name = item.Name
            };

            // save changes
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            return new ObjectResult(permission.ToViewModel());
        }
    }
}
