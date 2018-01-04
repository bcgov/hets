using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;

        /// <summary>
        /// Role Controller Constructor
        /// </summary>
        public RoleController(IRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk role records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Role created</response>
        [HttpPost]
        [Route("/api/roles/bulk")]
        [SwaggerOperation("RolesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesBulkPost([FromBody]Role[] items)
        {
            return _service.RolesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/roles")]
        [SwaggerOperation("RolesGet")]
        [SwaggerResponse(200, type: typeof(List<RoleViewModel>))]
        public virtual IActionResult RolesGet()
        {
            return _service.RolesGetAsync();
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpPost]
        [Route("/api/roles/{id}/delete")]
        [SwaggerOperation("RolesIdDeletePost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesIdDeletePost([FromRoute]int id)
        {
            return _service.RolesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpGet]
        [Route("/api/roles/{id}")]
        [SwaggerOperation("RolesIdGet")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        public virtual IActionResult RolesIdGet([FromRoute]int id)
        {
            return _service.RolesIdGetAsync(id);
        }

        /// <summary>
        /// Get permissions associated with a role
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/roles/{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        public virtual IActionResult RolesIdPermissionsGet([FromRoute]int id)
        {
            return _service.RolesIdPermissionsGetAsync(id);
        }

        /// <summary>
        /// Add permission to a role
        /// </summary>
        /// <remarks>Adds a permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpPost]
        [Route("/api/roles/{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPost")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesIdPermissionsPost([FromRoute]int id, [FromBody]PermissionViewModel item)
        {
            return _service.RolesIdPermissionsPostAsync(id, item);
        }

        /// <summary>
        /// Update permissions for a role
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpPut]
        [Route("/api/roles/{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPut")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesIdPermissionsPut([FromRoute]int id, [FromBody]PermissionViewModel[] items)
        {
            return _service.RolesIdPermissionsPutAsync(id, items);
        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpPut]
        [Route("/api/roles/{id}")]
        [SwaggerOperation("RolesIdPut")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesIdPut([FromRoute]int id, [FromBody]RoleViewModel item)
        {
            return _service.RolesIdPutAsync(id, item);
        }

        /// <summary>
        /// Get users associated with a role
        /// </summary>
        /// <remarks>Gets all the users for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/roles/{id}/users")]
        [SwaggerOperation("RolesIdUsersGet")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult RolesIdUsersGet([FromRoute]int id)
        {
            return _service.RolesIdUsersGetAsync(id);
        }

        /// <summary>
        /// Update users with a role
        /// </summary>
        /// <remarks>Updates the users for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Role not found</response>
        [HttpPut]
        [Route("/api/roles/{id}/users")]
        [SwaggerOperation("RolesIdUsersPut")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesIdUsersPut([FromRoute]int id, [FromBody]UserRoleViewModel[] items)
        {
            return _service.RolesIdUsersPutAsync(id, items);
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Role created</response>
        [HttpPost]
        [Route("/api/roles")]
        [SwaggerOperation("RolesPost")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolesPost([FromBody]RoleViewModel item)
        {
            return _service.RolesPostAsync(item);
        }
    }
}
