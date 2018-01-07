using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Group Service Interface
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Create bulk group records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Group created</response>
        IActionResult GroupsBulkPostAsync(Group[] items);

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <response code="200">OK</response>
        JsonResult GroupsGetAsync();

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="id">id of Group to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        IActionResult GroupsIdDeletePostAsync(int id);

        /// <summary>
        /// Get group by id
        /// </summary>
        /// <param name="id">id of Group to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        IActionResult GroupsIdGetAsync(int id);

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="id">id of Group to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        IActionResult GroupsIdPutAsync(int id, Group item);

        /// <summary>
        /// Get users associated with a group
        /// </summary>
        /// <remarks>Used to get users in a given Group</remarks>
        /// <param name="id">id of Group to fetch Users for</param>
        /// <response code="200">OK</response>
        IActionResult GroupsIdUsersGetAsync(int id);

        /// <summary>
        /// Create group
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Group created</response>
        IActionResult GroupsPostAsync(Group item);
    }
}
