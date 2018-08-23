using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// (Document) Attachment Service
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// Create bulk attachments records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Attachment created</response>
        IActionResult AttachmentsBulkPostAsync(Attachment[] items);

        /// <summary>
        /// Delete an attchment
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        /// <response code="200">OK</response>
        IActionResult AttachmentsIdDeletePostAsync(int id);

        /// <summary>
        /// Returns the binary file component of an attachment
        /// </summary>
        /// <param name="id">Attachment Id</param>
        /// <response code="200">OK</response>
        IActionResult AttachmentsIdDownloadGetAsync(int id);
    }
}
