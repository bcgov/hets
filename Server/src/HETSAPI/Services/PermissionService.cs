using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permission created</response>
        IActionResult PermissionsBulkPostAsync(Permission[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult PermissionsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Permission to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        IActionResult PermissionsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Permission to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        IActionResult PermissionsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Permission to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Permission not found</response>
        IActionResult PermissionsIdPutAsync(int id, PermissionViewModel item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Permission created</response>
        IActionResult PermissionsPostAsync(PermissionViewModel item);
    }
}
