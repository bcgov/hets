using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Role Permission Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RolePermissionController : Controller
    {
        private readonly IRolePermissionService _service;

        /// <summary>
        /// Role Permission Controller Constructor
        /// </summary>
        public RolePermissionController(IRolePermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk role permission records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RolePermission created</response>
        [HttpPost]
        [Route("/api/rolepermissions/bulk")]
        [SwaggerOperation("RolepermissionsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult RolepermissionsBulkPost([FromBody]RolePermission[] items)
        {
            return _service.RolepermissionsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all role permissions
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rolepermissions")]
        [SwaggerOperation("RolepermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<RolePermission>))]
        public virtual IActionResult RolepermissionsGet()
        {
            return _service.RolepermissionsGetAsync();
        }

        /// <summary>
        /// Delete role permission
        /// </summary>
        /// <param name="id">id of RolePermission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        [HttpPost]
        [Route("/api/rolepermissions/{id}/delete")]
        [SwaggerOperation("RolepermissionsIdDeletePost")]
        public virtual IActionResult RolepermissionsIdDeletePost([FromRoute]int id)
        {
            return _service.RolepermissionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get role permission by id
        /// </summary>
        /// <param name="id">id of RolePermission to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        [HttpGet]
        [Route("/api/rolepermissions/{id}")]
        [SwaggerOperation("RolepermissionsIdGet")]
        [SwaggerResponse(200, type: typeof(RolePermission))]
        public virtual IActionResult RolepermissionsIdGet([FromRoute]int id)
        {
            return _service.RolepermissionsIdGetAsync(id);
        }

        /// <summary>
        /// Update role permission
        /// </summary>
        /// <param name="id">id of RolePermission to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        [HttpPut]
        [Route("/api/rolepermissions/{id}")]
        [SwaggerOperation("RolepermissionsIdPut")]
        [SwaggerResponse(200, type: typeof(RolePermission))]
        public virtual IActionResult RolepermissionsIdPut([FromRoute]int id, [FromBody]RolePermission item)
        {
            return _service.RolepermissionsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create role permission
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RolePermission created</response>
        [HttpPost]
        [Route("/api/rolepermissions")]
        [SwaggerOperation("RolepermissionsPost")]
        [SwaggerResponse(200, type: typeof(RolePermission))]
        public virtual IActionResult RolepermissionsPost([FromBody]RolePermission item)
        {
            return _service.RolepermissionsPostAsync(item);
        }
    }
}
