using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Dump Truck Service
    /// </summary>
    public interface IDumpTruckService
    {
        /// <summary>
        /// Create bulk dump truck records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">DumpTruck created</response>
        IActionResult DumptrucksBulkPostAsync(DumpTruck[] items);        
    }
}
