using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
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
        /// <response code="200">Role created</response>
        [HttpPost]
        [Route("/api/roles/bulk")]
        [SwaggerOperation("RolesBulkPost")]
        [RequiresPermission(Permission.Admin)]
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
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult RolesGet()
        {
            return _service.RolesGetAsync();
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        /// <response code="200">OK</response>        
        [HttpPost]
        [Route("/api/roles/{id}/delete")]
        [SwaggerOperation("RolesIdDeletePost")]
        [RequiresPermission(Permission.RolesAndPermissions)]
        public virtual IActionResult RolesIdDeletePost([FromRoute]int id)
        {
            return _service.RolesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/roles/{id}")]
        [SwaggerOperation("RolesIdGet")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        [RequiresPermission(Permission.RolesAndPermissions)]
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
        [RequiresPermission(Permission.RolesAndPermissions)]
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
        [HttpPost]
        [Route("/api/roles/{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPost")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        [RequiresPermission(Permission.RolesAndPermissions)]
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
        [HttpPut]
        [Route("/api/roles/{id}/permissions")]
        [SwaggerOperation("RolesIdPermissionsPut")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        [RequiresPermission(Permission.RolesAndPermissions)]
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
        [HttpPut]
        [Route("/api/roles/{id}")]
        [SwaggerOperation("RolesIdPut")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        [RequiresPermission(Permission.RolesAndPermissions)]
        public virtual IActionResult RolesIdPut([FromRoute]int id, [FromBody]RoleViewModel item)
        {
            return _service.RolesIdPutAsync(id, item);
        }        

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Role created</response>
        [HttpPost]
        [Route("/api/roles")]
        [SwaggerOperation("RolesPost")]
        [SwaggerResponse(200, type: typeof(RoleViewModel))]
        [RequiresPermission(Permission.RolesAndPermissions)]
        public virtual IActionResult RolesPost([FromBody]RoleViewModel item)
        {
            return _service.RolesPostAsync(item);
        }
    }
}
