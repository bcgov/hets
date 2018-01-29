using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalAreaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalArea created</response>
        IActionResult LocalAreasBulkPostAsync(LocalArea[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult LocalAreasGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        IActionResult LocalAreasIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        IActionResult LocalAreasIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalArea to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalArea not found</response>
        IActionResult LocalAreasIdPutAsync(int id, LocalArea item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalArea created</response>
        IActionResult LocalAreasPostAsync(LocalArea item);
    }
}
