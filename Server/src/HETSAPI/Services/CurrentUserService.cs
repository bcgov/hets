using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a users favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouritetype">type of favourite to return</param>
        /// <response code="200">OK</response>
        IActionResult UsersCurrentFavouritesFavouritetypeGetAsync(string favouritetype);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        /// <response code="200">OK</response>
        IActionResult UsersCurrentFavouritesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        /// <response code="200">UserFavourite created</response>
        IActionResult UsersCurrentFavouritesPostAsync(UserFavourite item);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a favourite</remarks>
        /// <param name="item"></param>
        /// <response code="200">UserFavourite created</response>
        IActionResult UsersCurrentFavouritesPutAsync(UserFavourite item);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        IActionResult UsersCurrentGetAsync();

        /// <summary>
        /// Logoff user - switch to default district
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UsersCurrentLogoffPostAsync();
    }
}
