using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// User Role Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _service;

        /// <summary>
        /// User Role Controller Constructor
        /// </summary>
        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk user role records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserRole created</response>
        [HttpPost]
        [Route("/api/userroles/bulk")]
        [SwaggerOperation("UserrolesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserrolesBulkPost([FromBody]UserRole[] items)
        {
            return _service.UserrolesBulkPostAsync(items);
        }

        /// <summary>
        /// Get ll user roles
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/userroles")]
        [SwaggerOperation("UserrolesGet")]
        [SwaggerResponse(200, type: typeof(List<UserRole>))]
        public virtual IActionResult UserrolesGet()
        {
            return _service.UserrolesGetAsync();
        }

        /// <summary>
        /// Delete user role
        /// </summary>
        /// <param name="id">id of UserRole to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpPost]
        [Route("/api/userroles/{id}/delete")]
        [SwaggerOperation("UserrolesIdDeletePost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserrolesIdDeletePost([FromRoute]int id)
        {
            return _service.UserrolesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get user role by id
        /// </summary>
        /// <param name="id">id of UserRole to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpGet]
        [Route("/api/userroles/{id}")]
        [SwaggerOperation("UserrolesIdGet")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        public virtual IActionResult UserrolesIdGet([FromRoute]int id)
        {
            return _service.UserrolesIdGetAsync(id);
        }

        /// <summary>
        /// Update user role
        /// </summary>
        /// <param name="id">id of UserRole to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpPut]
        [Route("/api/userroles/{id}")]
        [SwaggerOperation("UserrolesIdPut")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserrolesIdPut([FromRoute]int id, [FromBody]UserRole item)
        {
            return _service.UserrolesIdPutAsync(id, item);
        }

        /// <summary>
        /// Create user role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserRole created</response>
        [HttpPost]
        [Route("/api/userroles")]
        [SwaggerOperation("UserrolesPost")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserrolesPost([FromBody]UserRole item)
        {
            return _service.UserrolesPostAsync(item);
        }
    }
}
