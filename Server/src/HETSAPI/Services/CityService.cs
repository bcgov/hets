using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// City Service
    /// </summary>
    public interface ICityService
    {
        /// <summary>
        /// Create bulk city records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">City created</response>
        IActionResult CitiesBulkPostAsync(City[] items);

        /// <summary>
        /// Get all city records
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult CitiesGetAsync();        
    }
}
