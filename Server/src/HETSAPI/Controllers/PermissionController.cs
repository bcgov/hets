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
        /// <response code="200">Permission created</response>
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
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult PermissionsGet()
        {
            return _service.PermissionsGetAsync();
        }        
    }
}
