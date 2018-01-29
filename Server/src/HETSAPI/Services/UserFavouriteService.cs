using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserFavouriteService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserFavourite created</response>
        IActionResult UserfavouritesBulkPostAsync(UserFavourite[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UserfavouritesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        IActionResult UserfavouritesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        IActionResult UserfavouritesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        IActionResult UserfavouritesIdPutAsync(int id, UserFavourite item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        IActionResult UserfavouritesPostAsync(UserFavourite item);
    }
}
