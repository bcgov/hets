using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Request Attachment Service
    /// </summary>
    public class RentalRequestAttachmentService : IRentalRequestAttachmentService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Rental Request Attachment Service Constructor
        /// </summary>
        public RentalRequestAttachmentService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk rental request attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Rental Request Attachment created</response>
        public virtual IActionResult RentalrequestattachmentsBulkPostAsync(RentalRequestAttachment[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (RentalRequestAttachment item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.RentalRequestAttachments.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all rental request attachments
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestattachmentsGetAsync()
        {
            List<RentalRequestAttachment> result = _context.RentalRequestAttachments.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete rental request attachment
        /// </summary>
        /// <param name="id">id of Rental Request Attachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Attachment not found</response>
        public virtual IActionResult RentalrequestattachmentsIdDeletePostAsync(int id)
        {
            bool exists = _context.RentalRequestAttachments.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequestAttachment item = _context.RentalRequestAttachments.First(a => a.Id == id);

                if (item != null)
                {
                    _context.RentalRequestAttachments.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental request attachment by id
        /// </summary>
        /// <param name="id">id of Rental Request Attachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Attachment not found</response>
        public virtual IActionResult RentalrequestattachmentsIdGetAsync(int id)
        {
            bool exists = _context.RentalRequestAttachments.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequestAttachment result = _context.RentalRequestAttachments.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update rental request attachment
        /// </summary>
        /// <param name="id">id of Rental Request Attachment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Attachment not found</response>
        public virtual IActionResult RentalrequestattachmentsIdPutAsync(int id, RentalRequestAttachment item)
        {
            bool exists = _context.RentalRequestAttachments.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.RentalRequestAttachments.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create rental request attachment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Rental Request Attachment created</response>
        public virtual IActionResult RentalrequestattachmentsPostAsync(RentalRequestAttachment item)
        {
            bool exists = _context.RentalRequestAttachments.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.RentalRequestAttachments.Update(item);
            }
            else
            {
                // record not found
                _context.RentalRequestAttachments.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
