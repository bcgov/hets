using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">History created</response>
        IActionResult HistoriesBulkPostAsync(History[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult HistoriesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        IActionResult HistoriesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        IActionResult HistoriesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        IActionResult HistoriesIdPutAsync(int id, History item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        IActionResult HistoriesPostAsync(History item);
    }
}
