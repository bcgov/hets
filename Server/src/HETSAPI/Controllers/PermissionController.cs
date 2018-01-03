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
    /// Permission Controller
    /// </summary>
    public class PermissionController : Controller
    {
        private readonly IPermissionService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public PermissionController(IPermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permission created</response>
        [HttpPost]
        [Route("/api/permissions/bulk")]
        [SwaggerOperation("PermissionsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult PermissionsBulkPost([FromBody]Permission[] items)
        {
            return this._service.PermissionsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/permissions")]
        [SwaggerOperation("PermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        public virtual IActionResult PermissionsGet()
        {
            return this._service.PermissionsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Permission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        [HttpPost]
        [Route("/api/permissions/{id}/delete")]
        [SwaggerOperation("PermissionsIdDeletePost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult PermissionsIdDeletePost([FromRoute]int id)
        {
            return this._service.PermissionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
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
            return this._service.PermissionsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Permission to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        [HttpPut]
        [Route("/api/permissions/{id}")]
        [SwaggerOperation("PermissionsIdPut")]
        [SwaggerResponse(200, type: typeof(PermissionViewModel))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult PermissionsIdPut([FromRoute]int id, [FromBody]PermissionViewModel item)
        {
            return this._service.PermissionsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Permission created</response>
        [HttpPost]
        [Route("/api/permissions")]
        [SwaggerOperation("PermissionsPost")]
        [SwaggerResponse(200, type: typeof(PermissionViewModel))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult PermissionsPost([FromBody]PermissionViewModel item)
        {
            return this._service.PermissionsPostAsync(item);
        }
    }
}
