using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDistrictEquipmentTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        IActionResult DistrictEquipmentTypesBulkPostAsync(DistrictEquipmentType[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult DistrictEquipmentTypesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        IActionResult DistrictEquipmentTypesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        IActionResult DistrictEquipmentTypesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update (0 to create)</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        IActionResult DistrictEquipmentTypesIdPostAsync(int id, DistrictEquipmentType item);        
    }
}
