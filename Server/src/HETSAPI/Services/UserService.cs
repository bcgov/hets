using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HetsApi.Services
{
    /// <summary>
    /// User Service
    /// </summary>
    public interface IUserService
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">User created</response>
        IActionResult UsersBulkPostAsync(User[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UsersGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given context type</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdFavouritesGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdGetAsync(int id);        
        
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdPermissionsGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdPutAsync(int id, UserViewModel item);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdRolesGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="201">Role created for user</response>
        IActionResult UsersIdRolesPostAsync(int id, UserRoleViewModel item);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        IActionResult UsersIdRolesPutAsync(int id, UserRoleViewModel[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">User created</response>
        IActionResult UsersPostAsync(UserViewModel item);

        /// <summary>
        /// Searches Users
        /// </summary>
        /// <remarks>Used to search users.</remarks>
        /// <param name="districts">Districts (comma seperated list of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        /// <response code="200">OK</response>
        IActionResult UsersSearchGetAsync(string districts, string surname, bool? includeInactive);

        /// <summary>
        /// Get user districts
        /// </summary>
        /// <remarks>Returns a users' districts</remarks>
        /// <param name="id">id of User to fetch districts for</param>
        /// <response code="200">OK</response>        
        IActionResult UsersIdDistrictsGetAsync(int id);
    }
}
