using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// District Equipment Type Service
    /// </summary>
    public interface IDistrictEquipmentTypeService
    {
        /// <summary>
        /// Create bulk district equipent type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">DistrictEquipmentType created</response>
        IActionResult DistrictEquipmentTypesBulkPostAsync(DistrictEquipmentType[] items);

        /// <summary>
        /// Get all district equipment type for a user's district
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult DistrictEquipmentTypesGetAsync();

        /// <summary>
        /// Delete district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        IActionResult DistrictEquipmentTypesIdDeletePostAsync(int id);

        /// <summary>
        /// Get district equipment type by id
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        IActionResult DistrictEquipmentTypesIdGetAsync(int id);

        /// <summary>
        /// Create or update district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update (0 to create)</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult DistrictEquipmentTypesIdPostAsync(int id, DistrictEquipmentType item);        
    }
}
