using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RolePermissionController : Controller
    {
        private readonly IRolePermissionService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RolePermissionController(IRolePermissionService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RolePermission created</response>
        [HttpPost]
        [Route("/api/rolepermissions/bulk")]
        [SwaggerOperation("RolepermissionsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RolepermissionsBulkPost([FromBody]RolePermission[] items)
        {
            return this._service.RolepermissionsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rolepermissions")]
        [SwaggerOperation("RolepermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<RolePermission>))]
        public virtual IActionResult RolepermissionsGet()
        {
            return this._service.RolepermissionsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RolePermission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        [HttpPost]
        [Route("/api/rolepermissions/{id}/delete")]
        [SwaggerOperation("RolepermissionsIdDeletePost")]
        public virtual IActionResult RolepermissionsIdDeletePost([FromRoute]int id)
        {
            return this._service.RolepermissionsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
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
            return this._service.RolepermissionsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RolePermission to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        [HttpPut]
        [Route("/api/rolepermissions/{id}")]
        [SwaggerOperation("RolepermissionsIdPut")]
        [SwaggerResponse(200, type: typeof(RolePermission))]
        public virtual IActionResult RolepermissionsIdPut([FromRoute]int id, [FromBody]RolePermission item)
        {
            return this._service.RolepermissionsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RolePermission created</response>
        [HttpPost]
        [Route("/api/rolepermissions")]
        [SwaggerOperation("RolepermissionsPost")]
        [SwaggerResponse(200, type: typeof(RolePermission))]
        public virtual IActionResult RolepermissionsPost([FromBody]RolePermission item)
        {
            return this._service.RolepermissionsPostAsync(item);
        }
    }
}
