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
    /// User Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        /// <summary>
        /// User Controller Constructor
        /// </summary>
        public UserController(IUserService service)
        {
            _service = service;
        }
        
        /// <summary>
        /// Create bulk users
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/users/bulk")]
        [SwaggerOperation("UsersBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersBulkPost([FromBody]User[] items)
        {
            return _service.UsersBulkPostAsync(items);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users")]
        [SwaggerOperation("UsersGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult UsersGet()
        {
            return _service.UsersGetAsync();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">id of User to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [Route("/api/users/{id}/delete")]
        [SwaggerOperation("UsersIdDeletePost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersIdDeletePost([FromRoute]int id)
        {
            return _service.UsersIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get user favorites
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given context type</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/favourites")]
        [SwaggerOperation("UsersIdFavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavouriteViewModel>))]
        public virtual IActionResult UsersIdFavouritesGet([FromRoute]int id)
        {
            return _service.UsersIdFavouritesGetAsync(id);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdGet")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersIdGet([FromRoute]int id)
        {
            return _service.UsersIdGetAsync(id);
        }        

        /// <summary>
        /// Get permissions for a user
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/permissions")]
        [SwaggerOperation("UsersIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersIdPermissionsGet([FromRoute]int id)
        {
            return _service.UsersIdPermissionsGetAsync(id);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdPut")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersIdPut([FromRoute]int id, [FromBody]UserViewModel item)
        {
            return _service.UsersIdPutAsync(id, item);
        }

        /// <summary>
        /// Get all roles for a user
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesGet")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult UsersIdRolesGet([FromRoute]int id)
        {
            return _service.UsersIdRolesGetAsync(id);
        }

        /// <summary>
        /// Adds a role to a user
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="201">Role created for user</response>
        [HttpPost]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPost")]
        [SwaggerResponse(200, type: typeof(UserRoleViewModel))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersIdRolesPost([FromRoute]int id, [FromBody]UserRoleViewModel item)
        {
            return _service.UsersIdRolesPostAsync(id, item);
        }

        /// <summary>
        /// Add user to roles
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPut")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersIdRolesPut([FromRoute]int id, [FromBody]UserRoleViewModel[] items)
        {
            return _service.UsersIdRolesPutAsync(id, items);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/users")]
        [SwaggerOperation("UsersPost")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult UsersPost([FromBody]UserViewModel item)
        {
            return _service.UsersPostAsync(item);
        }

        /// <summary>
        /// Search for users
        /// </summary>
        /// <remarks>Used to search users.</remarks>
        /// <param name="districts">Districts (comma seperated list of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/search")]        
        [SwaggerOperation("UsersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult UsersSearchGet([FromQuery]string districts, [FromQuery]string surname, [FromQuery]bool? includeInactive)
        {
            return _service.UsersSearchGetAsync(districts, surname, includeInactive);
        }
    }
}
