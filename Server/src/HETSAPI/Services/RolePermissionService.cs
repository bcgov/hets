using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRolePermissionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RolePermission created</response>
        IActionResult RolepermissionsBulkPostAsync(RolePermission[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RolepermissionsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RolePermission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        IActionResult RolepermissionsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RolePermission to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        IActionResult RolepermissionsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RolePermission to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RolePermission not found</response>
        IActionResult RolepermissionsIdPutAsync(int id, RolePermission item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RolePermission created</response>
        IActionResult RolepermissionsPostAsync(RolePermission item);
    }
}
