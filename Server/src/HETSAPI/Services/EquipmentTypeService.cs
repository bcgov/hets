using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentType created</response>
        IActionResult EquipmentTypesBulkPostAsync(EquipmentType[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult EquipmentTypesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        IActionResult EquipmentTypesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        IActionResult EquipmentTypesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentType not found</response>
        IActionResult EquipmentTypesIdPutAsync(int id, EquipmentType item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentType created</response>
        IActionResult EquipmentTypesPostAsync(EquipmentType item);
    }
}
