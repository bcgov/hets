using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDumpTruckService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DumpTruck created</response>
        IActionResult DumptrucksBulkPostAsync(DumpTruck[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult DumptrucksGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        IActionResult DumptrucksIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        IActionResult DumptrucksIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DumpTruck to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DumpTruck not found</response>
        IActionResult DumptrucksIdPutAsync(int id, DumpTruck item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">DumpTruck created</response>
        IActionResult DumptrucksPostAsync(DumpTruck item);
    }
}
