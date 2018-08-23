using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Region Service
    /// </summary>
    public interface IRegionService
    {
        /// <summary>
        /// Create bulk region records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Region created</response>
        IActionResult RegionsBulkPostAsync(Region[] items);

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RegionsGetAsync();        
    }
}
