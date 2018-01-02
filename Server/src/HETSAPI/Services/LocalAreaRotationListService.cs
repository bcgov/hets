using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalAreaRotationListService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        IActionResult LocalarearotationlistsBulkPostAsync(LocalAreaRotationList[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult LocalarearotationlistsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        IActionResult LocalarearotationlistsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        IActionResult LocalarearotationlistsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        IActionResult LocalarearotationlistsIdPutAsync(int id, LocalAreaRotationList item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        IActionResult LocalarearotationlistsPostAsync(LocalAreaRotationList item);
    }
}
