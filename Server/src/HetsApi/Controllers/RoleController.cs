using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [Route("api/roles")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RoleController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public RoleController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;

            // set context data
            HetUser user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }
        
        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("RolesGet")]
        [SwaggerResponse(200, type: typeof(List<HetRole>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RolesGet()
        {
            // get all roles
            List<HetRole> roles = _context.HetRole.AsNoTracking().ToList();

            return new ObjectResult(new HetsResponse(roles));
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>  
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("RolesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetRole))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetRole.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRole role = _context.HetRole.First(x => x.RoleId == id);

            // remove associated role permission records
            List<HetRolePermission> itemsToRemove = _context.HetRolePermission
                .Where(x => x.Role.RoleId == role.RoleId)
                .ToList();

            foreach (HetRolePermission item in itemsToRemove)
            {
                _context.HetRolePermission.Remove(item);
            }

            // remove role and save
            _context.HetRole.Remove(role);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(role));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("RolesIdGet")]
        [SwaggerResponse(200, type: typeof(HetRole))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRole.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRole role = _context.HetRole.AsNoTracking().First(x => x.RoleId == id);

            return new ObjectResult(new HetsResponse(role));
        }

        /// <summary>
        /// Get permissions associated with a role
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        [HttpGet]
        [Route("{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<HetPermission>))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdPermissionsGet([FromRoute]int id)
        {
            HetRole role = _context.HetRole.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermission)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // return role permissions
            return new ObjectResult(new HetsResponse(role.HetRolePermission.ToList()));
        }

        /// <summary>
        /// Add permission to a role
        /// </summary>
        /// <remarks>Adds a permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPost")]
        [SwaggerResponse(200, type: typeof(List<HetPermission>))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdPermissionsPost([FromRoute]int id, [FromBody]HetPermission item)
        {
            HetRole role = _context.HetRole.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermission)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));                

            List<HetPermission> allPermissions = _context.HetPermission.ToList();
            List<string> existingPermissionCodes = role.HetRolePermission.Select(x => x.Permission.Code).ToList();

            if (!existingPermissionCodes.Contains(item.Code))
            {
                HetPermission permToAdd = allPermissions.FirstOrDefault(x => x.Code == item.Code);

                // invalid permission
                if (permToAdd == null) throw new ArgumentException(string.Format("Invalid Permission Code {0}", item.Code));
                                
                // add permission
                HetRolePermission rolePermission = new HetRolePermission
                {
                    Permission = permToAdd,
                    Role = role
                };

                _context.HetRolePermission.Add(rolePermission);
            }

            _context.SaveChanges();

            // get updated permissions
            role = _context.HetRole.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermission)
                    .ThenInclude(rp => rp.Permission)
                .First();
            
            // return role permissions
            return new ObjectResult(new HetsResponse(role.HetRolePermission.ToList()));            
        }

        /// <summary>
        /// Update permissions for a role
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        [HttpPut]
        [Route("{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPut")]
        [SwaggerResponse(200, type: typeof(List<HetPermission>))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdPermissionsPut([FromRoute]int id, [FromBody]HetPermission[] items)
        {
            HetRole role = _context.HetRole.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermission)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // get permissions
            List<HetPermission> allPermissions = _context.HetPermission.ToList();
            List<int> permissionIds = items.Select(x => x.PermissionId).ToList();
            List<int> existingPermissionIds = role.HetRolePermission.Select(x => x.Permission.PermissionId).ToList();
            List<int> permissionIdsToAdd = permissionIds.Where(x => !existingPermissionIds.Contains(x)).ToList();

            // permissions to add
            foreach (int permissionId in permissionIdsToAdd)
            {
                HetPermission permToAdd = allPermissions.FirstOrDefault(x => x.PermissionId == permissionId);

                // invalid permission
                if (permToAdd == null) throw new ArgumentException("Invalid Permission Code");

                // add permission
                HetRolePermission rolePermission = new HetRolePermission
                {
                    Permission = permToAdd,
                    Role = role
                };

                _context.HetRolePermission.Add(rolePermission);
            }

            // permissions to remove
            List<HetRolePermission> permissionsToRemove = role.HetRolePermission.Where(x => x.Permission != null && !permissionIds.Contains(x.Permission.PermissionId)).ToList();

            foreach (HetRolePermission perm in permissionsToRemove)
            {
                _context.HetRolePermission.Remove(perm);
            }

            _context.SaveChanges();

            // get updated permissions
            role = _context.HetRole.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermission)
                .ThenInclude(rp => rp.Permission)
                .First();

            // return role permissions
            return new ObjectResult(new HetsResponse(role.HetRolePermission.ToList()));
        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("RolesIdPut")]
        [SwaggerResponse(200, type: typeof(HetRole))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesIdPut([FromRoute]int id, [FromBody]HetRole item)
        {
            bool exists = _context.HetRole.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // get record
            HetRole role = _context.HetRole.First(x => x.RoleId == id);

            role.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            role.Name = item.Name;
            role.Description = item.Description;

            // save changes
            _context.SaveChanges();

            // get updated role
            role = _context.HetRole.AsNoTracking()
                .First(x => x.RoleId == id);

            // return role permissions
            return new ObjectResult(new HetsResponse(role));
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Role created</response>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("RolesPost")]
        [SwaggerResponse(200, type: typeof(HetRole))]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual IActionResult RolesPost([FromBody]HetRole item)
        {
            HetRole role = new HetRole
            {
                Description = item.Description,
                Name = item.Name
            };

            // save changes
            _context.HetRole.Add(role);
            _context.SaveChanges();

            int id = role.RoleId;

            // get updated role
            role = _context.HetRole.AsNoTracking()
                .First(x => x.RoleId == id);

            // return role permissions
            return new ObjectResult(new HetsResponse(role));
        }        
    }
}
