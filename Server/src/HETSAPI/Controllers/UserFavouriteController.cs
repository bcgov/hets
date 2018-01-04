using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// User Favourite Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserFavouriteController : Controller
    {
        private readonly IUserFavouriteService _service;

        /// <summary>
        /// User Favourite Controller Constructor
        /// </summary>
        public UserFavouriteController(IUserFavouriteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk user favourite records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPost]
        [Route("/api/userfavourites/bulk")]
        [SwaggerOperation("UserfavouritesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserfavouritesBulkPost([FromBody]UserFavourite[] items)
        {
            return _service.UserfavouritesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all user favourites
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/userfavourites")]
        [SwaggerOperation("UserfavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavourite>))]
        public virtual IActionResult UserfavouritesGet()
        {
            return _service.UserfavouritesGetAsync();
        }

        /// <summary>
        /// Delete user favourite
        /// </summary>
        /// <param name="id">id of UserFavourite to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpPost]
        [Route("/api/userfavourites/{id}/delete")]
        [SwaggerOperation("UserfavouritesIdDeletePost")]
        public virtual IActionResult UserfavouritesIdDeletePost([FromRoute]int id)
        {
            return _service.UserfavouritesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get user favourites by id
        /// </summary>
        /// <param name="id">id of UserFavourite to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpGet]
        [Route("/api/userfavourites/{id}")]
        [SwaggerOperation("UserfavouritesIdGet")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesIdGet([FromRoute]int id)
        {
            return _service.UserfavouritesIdGetAsync(id);
        }

        /// <summary>
        /// Update user favourite
        /// </summary>
        /// <param name="id">id of UserFavourite to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpPut]
        [Route("/api/userfavourites/{id}")]
        [SwaggerOperation("UserfavouritesIdPut")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesIdPut([FromRoute]int id, [FromBody]UserFavourite item)
        {
            return _service.UserfavouritesIdPutAsync(id, item);
        }

        /// <summary>
        /// Create user favourite
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPost]
        [Route("/api/userfavourites")]
        [SwaggerOperation("UserfavouritesPost")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesPost([FromBody]UserFavourite item)
        {
            return _service.UserfavouritesPostAsync(item);
        }
    }
}
