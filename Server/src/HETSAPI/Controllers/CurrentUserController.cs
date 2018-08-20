using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Authorization;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Current User Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CurrentUserController : Controller
    {
        private readonly ICurrentUserService _service;

        /// <summary>
        /// Current User Controller Constructor
        /// </summary>
        public CurrentUserController(ICurrentUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get faviourites for a user (by type)
        /// </summary>
        /// <remarks>Returns a users favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouritetype">type of favourite to return</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/current/favourites/{favouritetype}")]
        [SwaggerOperation("UsersCurrentFavouritesFavouriteTypeGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavourite>))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentFavouritesFavouriteTypeGet([FromRoute]string favouritetype)
        {
            return _service.UsersCurrentFavouritesFavouritetypeGetAsync(favouritetype);
        }

        /// <summary>
        /// Delete user favourite
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/users/current/favourites/{id}/delete")]
        [SwaggerOperation("UsersCurrentFavouritesIdDeletePost")]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentFavouritesIdDeletePost([FromRoute]int id)
        {
            return _service.UsersCurrentFavouritesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Create user favourite
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        /// <response code="200">UserFavourite created</response>
        [HttpPost]
        [Route("/api/users/current/favourites")]
        [SwaggerOperation("UsersCurrentFavouritesPost")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentFavouritesPost([FromBody]UserFavourite item)
        {
            return _service.UsersCurrentFavouritesPostAsync(item);
        }

        /// <summary>
        /// Update a user favourite
        /// </summary>
        /// <remarks>Updates a favourite</remarks>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPut]
        [Route("/api/users/current/favourites")]
        [SwaggerOperation("UsersCurrentFavouritesPut")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentFavouritesPut([FromBody]UserFavourite item)
        {
            return _service.UsersCurrentFavouritesPutAsync(item);
        }

        /// <summary>
        /// Get current logged on user
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/current")]
        [SwaggerOperation("UsersCurrentGet")]
        [SwaggerResponse(200, type: typeof(CurrentUserViewModel))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentGet()
        {
            return _service.UsersCurrentGetAsync();
        }

        /// <summary>
        /// Logout User - Switch back to Primary
        /// </summary>
        /// <response code="200">User District switched</response>
        [HttpGet]
        [Route("/api/users/current/logoff")]
        [SwaggerOperation("UserDistrictsIdLogoffPost")]
        [SwaggerResponse(200, type: typeof(UserDistrict))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult UsersCurrentLogoffPost()
        {
            return _service.UsersCurrentLogoffPostAsync();
        }
    }
}