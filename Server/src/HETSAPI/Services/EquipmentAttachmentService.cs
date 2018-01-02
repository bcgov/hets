using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquipmentAttachmentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentAttachment created</response>
        IActionResult EquipmentAttachmentsBulkPostAsync(EquipmentAttachment[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult EquipmentAttachmentsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        IActionResult EquipmentAttachmentsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        IActionResult EquipmentAttachmentsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachment not found</response>
        IActionResult EquipmentAttachmentsIdPutAsync(int id, EquipmentAttachment item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentAttachment created</response>
        IActionResult EquipmentAttachmentsPostAsync(EquipmentAttachment item);
    }
}
