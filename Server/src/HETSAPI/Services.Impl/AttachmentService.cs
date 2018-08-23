using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HetsApi.Services.Impl
{
    /// <summary>
    /// Attachment Service
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Attachment Service Constructor
        /// </summary>
        public AttachmentService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Attachment created</response>
        public virtual IActionResult AttachmentsBulkPostAsync(Attachment[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Attachment item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.Attachments.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }

            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Delete attachment
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdDeletePostAsync(int id)
        {
            bool exists = _context.Attachments.Any(a => a.Id == id);

            if (exists)
            {
                var item = _context.Attachments.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Attachments.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Returns the binary file component of an attachment
        /// </summary>
        /// <param name="id">Attachment Id</param>
        /// <response code="200">OK</response>
        public virtual IActionResult AttachmentsIdDownloadGetAsync(int id)
        {
            bool exists = _context.Attachments.Any(a => a.Id == id);

            if (exists)
            {
                Attachment attachment = _context.Attachments.First(a => a.Id == id);

                // MOTI has requested that files be stored in the database.                            
                FileContentResult result =
                    new FileContentResult(attachment.FileContents, "application/octet-stream")
                    {
                        FileDownloadName = attachment.FileName
                    };

                return result;
            }

            return new StatusCodeResult(404);
        }
    }
}
