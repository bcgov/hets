using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Equipment Type Service
    /// </summary>
    public interface IEquipmentTypeService
    {
        /// <summary>
        /// Create buk equipment type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">EquipmentType created</response>
        IActionResult EquipmentTypesBulkPostAsync(EquipmentType[] items);

        /// <summary>
        /// Get all equipment types
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult EquipmentTypesGetAsync();        
    }
}
