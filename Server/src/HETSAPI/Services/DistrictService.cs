using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// District Service
    /// </summary>
    public interface IDistrictService
    {
        /// <summary>
        /// Create bulk district records 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">District created</response>
        IActionResult DistrictsBulkPostAsync(District[] items);

        /// <summary>
        /// Gat all districts
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult DistrictsGetAsync();        

        /// <summary>
        /// Get all owners by district (minimal data returned) - lookup
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult DistrictOwnersGetAsync(int id);
    }
}
