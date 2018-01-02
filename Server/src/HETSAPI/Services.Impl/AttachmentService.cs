using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public AttachmentService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult AttachmentsGetAsync()
        {
            var result = _context.Attachments.ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdDeletePostAsync(int id)
        {
            var exists = _context.Attachments.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Attachments.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Attachments.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// Returns the binary file component of an attachment
        /// </summary>
        /// <param name="id">Attachment Id</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found in system</response>
        public virtual IActionResult AttachmentsIdDownloadGetAsync(int id)
        {
            var exists = _context.Attachments.Any(a => a.Id == id);
            if (exists)
            {
                var attachment = _context.Attachments.First(a => a.Id == id);
                // 
                // MOTI has requested that files be stored in the database.
                //                                           
                var result = new FileContentResult(attachment.FileContents, "application/octet-stream");
                result.FileDownloadName = attachment.FileName;

                return result;
            }
            else
            {
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdGetAsync(int id)
        {
            var exists = _context.Attachments.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Attachments.First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Attachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found</response>
        public virtual IActionResult AttachmentsIdPutAsync(int id, Attachment item)
        {
            var exists = _context.Attachments.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Attachments.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Attachment created</response>
        public virtual IActionResult AttachmentsPostAsync(Attachment item)
        {
            var exists = _context.Attachments.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.Attachments.Update(item);
            }
            else
            {
                // record not found
                _context.Attachments.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
