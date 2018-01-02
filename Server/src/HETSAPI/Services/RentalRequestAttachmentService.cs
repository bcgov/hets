using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRentalRequestAttachmentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        IActionResult RentalrequestattachmentsBulkPostAsync(RentalRequestAttachment[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalrequestattachmentsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        IActionResult RentalrequestattachmentsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        IActionResult RentalrequestattachmentsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        IActionResult RentalrequestattachmentsIdPutAsync(int id, RentalRequestAttachment item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        IActionResult RentalrequestattachmentsPostAsync(RentalRequestAttachment item);
    }
}
