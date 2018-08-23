using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Equipment Attachment Service
    /// </summary>
    public interface IEquipmentAttachmentService
    {
        /// <summary>
        /// Create bulk equipment attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">EquipmentAttachment created</response>
        IActionResult EquipmentAttachmentsBulkPostAsync(EquipmentAttachment[] items);

        /// <summary>
        /// Delete equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to delete</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentAttachmentsIdDeletePostAsync(int id);

        /// <summary>
        /// Update equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult EquipmentAttachmentsIdPutAsync(int id, EquipmentAttachment item);

        /// <summary>
        /// Create equipment attachment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">EquipmentAttachment created</response>
        IActionResult EquipmentAttachmentsPostAsync(EquipmentAttachment item);
    }
}
