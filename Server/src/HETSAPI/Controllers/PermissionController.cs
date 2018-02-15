using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Permission Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _service;

        /// <summary>
        /// Permission Controller Constructor
        /// </summary>
        public PermissionController(IPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk permission records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permission created</response>
        [HttpPost]
        [Route("/api/permissions/bulk")]
        [SwaggerOperation("PermissionsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult PermissionsBulkPost([FromBody]Permission[] items)
        {
            return _service.PermissionsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/permissions")]
        [SwaggerOperation("PermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        public virtual IActionResult PermissionsGet()
        {
            return _service.PermissionsGetAsync();
        }

        /// <summary>
        /// Delete permission
        /// </summary>
        /// <param name="id">id of Permission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        [HttpPost]
        [Route("/api/permissions/{id}/delete")]
        [SwaggerOperation("PermissionsIdDeletePost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult PermissionsIdDeletePost([FromRoute]int id)
        {
            return _service.PermissionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get permission by id
        /// </summary>
        /// <param name="id">id of Permission to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        [HttpGet]
        [Route("/api/permissions/{id}")]
        [SwaggerOperation("PermissionsIdGet")]
        [SwaggerResponse(200, type: typeof(PermissionViewModel))]
        public virtual IActionResult PermissionsIdGet([FromRoute]int id)
        {
            return _service.PermissionsIdGetAsync(id);
        }

        /// <summary>
        /// Update permission
        /// </summary>
        /// <param name="id">id of Permission to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        [HttpPut]
        [Route("/api/permissions/{id}")]
        [SwaggerOperation("PermissionsIdPut")]
        [SwaggerResponse(200, type: typeof(PermissionViewModel))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult PermissionsIdPut([FromRoute]int id, [FromBody]PermissionViewModel item)
        {
            return _service.PermissionsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create permission
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Permission created</response>
        [HttpPost]
        [Route("/api/permissions")]
        [SwaggerOperation("PermissionsPost")]
        [SwaggerResponse(200, type: typeof(PermissionViewModel))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult PermissionsPost([FromBody]PermissionViewModel item)
        {
            return _service.PermissionsPostAsync(item);
        }
    }
}
