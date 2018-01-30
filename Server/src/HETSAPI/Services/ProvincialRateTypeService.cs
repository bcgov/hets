using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Provinial Rate Type Service
    /// </summary>
    public interface IProvincialRateTypeService
    {
        /// <summary>
        /// Create bulk provincial rate type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Provincial rate type created</response>
        IActionResult ProvincialRateTypesBulkPostAsync(ProvincialRateType[] items);

        /// <summary>
        /// Get all provincial rate types
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ProvincialRateTypesGetAsync();        
    }
}
