using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Service Area Service
    /// </summary>
    public interface IServiceAreaService
    {
        /// <summary>
        /// Create bulk service area records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">ServiceArea created</response>
        IActionResult ServiceAreasBulkPostAsync(ServiceArea[] items);

        /// <summary>
        /// Get all service areas
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ServiceAreasGetAsync();        
    }
}
