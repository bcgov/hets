using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceAreaService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">ServiceArea created</response>
        IActionResult ServiceareasBulkPostAsync(ServiceArea[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ServiceareasGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ServiceArea to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        IActionResult ServiceareasIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ServiceArea to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        IActionResult ServiceareasIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of ServiceArea to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">ServiceArea not found</response>
        IActionResult ServiceareasIdPutAsync(int id, ServiceArea item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">ServiceArea created</response>
        IActionResult ServiceareasPostAsync(ServiceArea item);
    }
}
