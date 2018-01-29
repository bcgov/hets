using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
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
        /// Get attachments
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult AttachmentsGetAsync()
        {
            var result = _context.Attachments.ToList();
            return new ObjectResult(new HetsResponse(result));
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
        /// <response code="404">Attachment not found in system</response>
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

        /// <summary>
        /// Get attachment by id
        /// </summary>
        /// <param name="id">id of Attachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdGetAsync(int id)
        {
            bool exists = _context.Attachments.Any(a => a.Id == id);

            if (exists)
            {
                Attachment result = _context.Attachments.First(a => a.Id == id);                
                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update attachment
        /// </summary>
        /// <param name="id">id of Attachment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdPutAsync(int id, Attachment item)
        {
            bool exists = _context.Attachments.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Attachments.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create attachment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Attachment created</response>
        public virtual IActionResult AttachmentsPostAsync(Attachment item)
        {
            bool exists = _context.Attachments.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Attachments.Update(item);
            }
            else
            {
                // record not found  -create
                _context.Attachments.Add(item);
            }

            // Save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
