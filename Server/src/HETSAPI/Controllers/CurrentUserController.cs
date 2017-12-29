using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CurrentUserController : Controller
    {
        private readonly ICurrentUserService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public CurrentUserController(ICurrentUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a users favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouritetype">type of favourite to return</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/current/favourites/{favouritetype}")]
        [SwaggerOperation("UsersCurrentFavouritesFavouritetypeGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavourite>))]
        public virtual IActionResult UsersCurrentFavouritesFavouritetypeGet([FromRoute]string favouritetype)
        {
            return this._service.UsersCurrentFavouritesFavouritetypeGetAsync(favouritetype);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/users/current/favourites/{id}/delete")]
        [SwaggerOperation("UsersCurrentFavouritesIdDeletePost")]
        public virtual IActionResult UsersCurrentFavouritesIdDeletePost([FromRoute]int id)
        {
            return this._service.UsersCurrentFavouritesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPost]
        [Route("/api/users/current/favourites")]
        [SwaggerOperation("UsersCurrentFavouritesPost")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UsersCurrentFavouritesPost([FromBody]UserFavourite item)
        {
            return this._service.UsersCurrentFavouritesPostAsync(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a favourite</remarks>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPut]
        [Route("/api/users/current/favourites")]
        [SwaggerOperation("UsersCurrentFavouritesPut")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UsersCurrentFavouritesPut([FromBody]UserFavourite item)
        {
            return this._service.UsersCurrentFavouritesPutAsync(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/current")]
        [SwaggerOperation("UsersCurrentGet")]
        [SwaggerResponse(200, type: typeof(CurrentUserViewModel))]
        public virtual IActionResult UsersCurrentGet()
        {
            return this._service.UsersCurrentGetAsync();
        }
    }
}
