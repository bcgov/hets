using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserRole created</response>
        IActionResult UserrolesBulkPostAsync(UserRole[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UserrolesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        IActionResult UserrolesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        IActionResult UserrolesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        IActionResult UserrolesIdPutAsync(int id, UserRole item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserRole created</response>
        IActionResult UserrolesPostAsync(UserRole item);
    }
}
