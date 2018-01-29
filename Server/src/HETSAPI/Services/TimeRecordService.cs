using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimeRecordService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">TimeRecord created</response>
        IActionResult TimerecordsBulkPostAsync(TimeRecord[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult TimerecordsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        IActionResult TimerecordsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        IActionResult TimerecordsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of TimeRecord to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        IActionResult TimerecordsIdPutAsync(int id, TimeRecord item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">TimeRecord created</response>
        IActionResult TimerecordsPostAsync(TimeRecord item);
    }
}
