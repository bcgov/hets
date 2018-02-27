using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Role Service
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RoleService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk role permission records
        /// </summary>
        /// <remarks>Bulk load of role permissions</remarks>
        /// <param name="items"></param>
        /// <response code="201">Roles created</response>
        public IActionResult RolepermissionsBulkPostAsync(RolePermission[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (RolePermission item in items)
            {
                // adjust the role
                if (item.Role != null)
                {
                    int roleId = item.Role.Id;
                    bool roleExists = _context.Roles.Any(a => a.Id == roleId);

                    if (roleExists)
                    {
                        Role role = _context.Roles.First(a => a.Id == roleId);
                        item.Role = role;
                    }
                }

                // adjust the permission
                if (item.Permission != null)
                {
                    int permissionId = item.Permission.Id;
                    bool permissionExists = _context.Permissions.Any(a => a.Id == permissionId);

                    if (permissionExists)
                    {
                        Permission permission = _context.Permissions.First(a => a.Id == permissionId);
                        item.Permission = permission;
                    }
                }

                bool exists = _context.RolePermissions.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RolePermissions.Update(item);
                }
                else
                {
                    _context.RolePermissions.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Create bulk role records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permissions created</response>
        public IActionResult RolesBulkPostAsync(Role[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Role item in items)
            {
                bool exists = _context.Roles.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Roles.Update(item);
                }
                else
                {
                    _context.Roles.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <remarks>Returns a collection of roles</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesGetAsync()
        {
            List<RoleViewModel> result = new List<RoleViewModel>();
            IQueryable<Role> data = _context.Roles.Select(x => x);

            foreach (Role item in data)
            {
                result.Add(item.ToViewModel());
            }

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdDeletePostAsync(int id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.Id == id);

            if (role == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // remove associated role permission records
            IQueryable<RolePermission> itemsToRemove = _context.RolePermissions.Where(x => x.Role.Id == role.Id);

            foreach (RolePermission item in itemsToRemove)
            {
                _context.RolePermissions.Remove(item);
            }

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(role.ToViewModel()));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdGetAsync(int id)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.Id == id);

            if (role == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            return new ObjectResult(new HetsResponse(role.ToViewModel()));
        }

        /// <summary>
        /// Get permissions associated with a role
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesIdPermissionsGetAsync(int id)
        {
            Role role = _context.Roles
                .Where(x => x.Id == id)
                .Include(x => x.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            if (role == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            IEnumerable<Permission> dbPermissions = role.RolePermissions.Select(x => x.Permission);

            // create DTO with serializable response
            List<PermissionViewModel> result = dbPermissions.Select(x => x.ToViewModel()).ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Update permissions associated with a role
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesIdPermissionsPutAsync(int id, PermissionViewModel[] items)
        {
            using (IDbContextTransaction txn = _context.BeginTransaction())
            {
                Role role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                List<Permission> allPermissions = _context.Permissions.ToList();
                List<int?> permissionIds = items.Select(x => x.Id).ToList();
                List<int> existingPermissionIds = role.RolePermissions.Select(x => x.Permission.Id).ToList();
                List<int?> permissionIdsToAdd = permissionIds.Where(x => !existingPermissionIds.Contains((int)x)).ToList();

                // Permissions to add
                foreach (int? permissionId in permissionIdsToAdd)
                {
                    Permission permToAdd = allPermissions.FirstOrDefault(x => x.Id == permissionId);

                    if (permToAdd == null)
                    {
                        throw new ArgumentException(string.Format("Invalid Permission Code {0}", permissionId));
                    }

                    role.AddPermission(permToAdd);
                }

                // Permissions to remove
                List<RolePermission> permissionsToRemove = role.RolePermissions.Where(x => x.Permission != null && !permissionIds.Contains(x.Permission.Id)).ToList();

                foreach (RolePermission perm in permissionsToRemove)
                {
                    role.RemovePermission(perm.Permission);
                    _context.RolePermissions.Remove(perm);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                IEnumerable<Permission> dbPermissions = role.RolePermissions.Select(x => x.Permission);

                // create DTO with serializable response
                List<PermissionViewModel> result = dbPermissions.Select(x => x.ToViewModel()).ToList();

                return new ObjectResult(new HetsResponse(result));
            }
        }

        /// <summary>
        /// Add permissions to a role
        /// </summary>
        /// <remarks>Adds permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPermissionsPostAsync(int id, Permission[] items)
        {
            using (IDbContextTransaction txn = _context.BeginTransaction())
            {
                Role role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                var allPermissions = _context.Permissions.ToList();
                var permissionIds = items.Select(x => x.Id).ToList();
                var existingPermissionIds = role.RolePermissions.Select(x => x.Permission.Id).ToList();
                var permissionIdsToAdd = permissionIds.Where(x => !existingPermissionIds.Contains(x)).ToList();

                // Permissions to add
                foreach (int permissionId in permissionIdsToAdd)
                {
                    Permission permToAdd = allPermissions.FirstOrDefault(x => x.Id == permissionId);

                    if (permToAdd == null)
                    {
                        throw new ArgumentException(string.Format("Invalid Permission Code {0}", permissionId));
                    }

                    role.AddPermission(permToAdd);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                IEnumerable<Permission> dbPermissions = role.RolePermissions.Select(x => x.Permission);

                // Create DTO with serializable response
                List<PermissionViewModel> result = dbPermissions.Select(x => x.ToViewModel()).ToList();

                return new ObjectResult(new HetsResponse(result));
            }            
        }

        /// <summary>
        /// Add permission to a role
        /// </summary>
        /// <remarks>Adds permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPermissionsPostAsync(int id, PermissionViewModel item)
        {
            using (IDbContextTransaction txn = _context.BeginTransaction())
            {
                Role role = _context.Roles
                    .Where(x => x.Id == id)
                    .Include(x => x.RolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                    .FirstOrDefault();

                if (role == null)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                List<Permission> allPermissions = _context.Permissions.ToList();
                List<string> existingPermissionCodes = role.RolePermissions.Select(x => x.Permission.Code).ToList();

                if (!existingPermissionCodes.Contains(item.Code))
                {
                    Permission permToAdd = allPermissions.FirstOrDefault(x => x.Code == item.Code);

                    if (permToAdd == null)
                    {
                        throw new ArgumentException(string.Format("Invalid Permission Code {0}", item.Code));
                    }

                    role.AddPermission(permToAdd);
                }

                _context.Roles.Update(role);
                _context.SaveChanges();
                txn.Commit();

                List<RolePermission> dbPermissions = _context.RolePermissions.ToList();

                // Create DTO with serializable response
                List<RolePermissionViewModel> result = dbPermissions.Select(x => x.ToViewModel()).ToList();

                return new ObjectResult(new HetsResponse(result));
            }
        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdPutAsync(int id, RoleViewModel item)
        {
            Role role = _context.Roles.FirstOrDefault(x => x.Id == id);

            if (role == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            role.Name = item.Name;
            role.Description = item.Description;
            _context.Roles.Update(role);

            // save changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(role.ToViewModel()));
        }

        /// <summary>
        /// Get users associated with a role
        /// </summary>
        /// <remarks>Gets all the users for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RolesIdUsersGetAsync(int id)
        {
            // and the users with those UserRoles
            List<User> result = new List<User>();

            List<User> users = _context.Users
                    .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                    .ToList();

            foreach (User user in users)
            {
                bool found = false;

                if (user.UserRoles != null)
                {
                    // ef core does not support lazy loading, so we need to explicitly get data here.
                    foreach (UserRole userRole in user.UserRoles)
                    {
                        if (userRole.Role != null && userRole.Role.Id == id && userRole.EffectiveDate <= DateTime.UtcNow && (userRole.ExpiryDate == null || userRole.ExpiryDate > DateTime.UtcNow))
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found && !result.Contains(user))
                {
                    result.Add(user);
                }
            }

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Update users associated with a role
        /// </summary>
        /// <remarks>Updates the users for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        public virtual IActionResult RolesIdUsersPutAsync(int id, UserRoleViewModel[] items)
        {
            bool roleExists = _context.Roles.Any(x => x.Id == id);
            bool dataChanged = false;

            if (roleExists)
            {
                Role role = _context.Roles.First(x => x.Id == id);

                // scan through users
                IIncludableQueryable<User, Role> users = _context.Users
                        .Include(x => x.UserRoles)
                        .ThenInclude(y => y.Role);

                foreach (User user in users)
                {
                    // first see if it is one of our matches.                    
                    UserRoleViewModel foundItem = null;

                    foreach (var item in items)
                    {
                        if (item.UserId == user.Id)
                        {
                            foundItem = item;
                            break;
                        }
                    }

                    if (foundItem == null) // delete the user role if it exists
                    {
                        foreach (UserRole userRole in user.UserRoles)
                        {
                            if (userRole.Role.Id == id)
                            {
                                user.UserRoles.Remove(userRole);
                                _context.Users.Update(user);
                                dataChanged = true;
                            }
                        }
                    }
                    else // add the user role if it does not exist
                    {
                        bool found = false;

                        foreach (UserRole userRole in user.UserRoles)
                        {
                            if (userRole.Role.Id == id)
                            {
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            UserRole newUserRole = new UserRole
                            {
                                EffectiveDate = DateTime.Now,
                                Role = role
                            };

                            user.UserRoles.Add(newUserRole);
                            _context.Users.Update(user);
                            dataChanged = true;
                        }
                    }
                }
                if (dataChanged)
                {
                    _context.SaveChanges();
                }

                return new StatusCodeResult(200);
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Role created</response>
        public virtual IActionResult RolesPostAsync(RoleViewModel item)
        {
            Role role = new Role
            {
                Description = item.Description,
                Name = item.Name
            };

            // save changes
            _context.Roles.Add(role);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(role.ToViewModel()));
        }
    }
}
