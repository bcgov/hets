using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILookupListService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LookupList created</response>
        IActionResult LookuplistsBulkPostAsync(LookupList[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult LookuplistsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        IActionResult LookuplistsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        IActionResult LookuplistsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        IActionResult LookuplistsIdPutAsync(int id, LookupList item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LookupList created</response>
        IActionResult LookuplistsPostAsync(LookupList item);
    }
}
