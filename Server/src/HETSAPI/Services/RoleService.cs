using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HetsApi.Services
{
    /// <summary>
    /// Role Service
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Create bulk role records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Role created</response>
        IActionResult RolesBulkPostAsync(Role[] items);

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RolesGetAsync();

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        /// <response code="200">OK</response>
        IActionResult RolesIdDeletePostAsync(int id);

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        IActionResult RolesIdGetAsync(int id);

        /// <summary>
        /// Get all role permissions
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        IActionResult RolesIdPermissionsGetAsync(int id);

        /// <summary>
        /// Add permissions to a role
        /// </summary>
        /// <remarks>Adds a permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult RolesIdPermissionsPostAsync(int id, PermissionViewModel item);

        /// <summary>
        /// Update permissions for a role
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        IActionResult RolesIdPermissionsPutAsync(int id, PermissionViewModel[] items);

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult RolesIdPutAsync(int id, RoleViewModel item);
        
        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Role created</response>
        IActionResult RolesPostAsync(RoleViewModel item);
    }
}
