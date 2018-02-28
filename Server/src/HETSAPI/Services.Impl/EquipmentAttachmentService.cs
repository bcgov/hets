using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Equipment Attachment Service
    /// </summary>
    public class EquipmentAttachmentService : IEquipmentAttachmentService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Equipment Attachment Service Constructor
        /// </summary>
        public EquipmentAttachmentService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord (EquipmentAttachment item)
        {
            if (item != null && item.Equipment != null)
                item.Equipment = _context.Equipments.FirstOrDefault(a => a.Id == item.Equipment.Id);
        }

        /// <summary>
        /// Create bulk equipment attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Equipment created</response>
        public virtual IActionResult EquipmentAttachmentsBulkPostAsync(EquipmentAttachment[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (EquipmentAttachment item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update            
                bool exists = _context.EquipmentAttachments.Any(a => a.Id == item.Id);

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
        /// Delete equipment attachment 
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentAttachmentsIdDeletePostAsync(int id)
        {
            bool exists = _context.EquipmentAttachments.Any(a => a.Id == id);

            if (exists)
            {
                EquipmentAttachment item = _context.EquipmentAttachments.First(a => a.Id == id);

                _context.EquipmentAttachments.Remove(item);
                
                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update equipment attachment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentAttachmentsIdPutAsync(int id, EquipmentAttachment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.EquipmentAttachments                    
                    .Any(a => a.Id == id);

                if (exists && id == item.Id)
                {
                    _context.EquipmentAttachments.Update(item);

                    // Save the changes
                    _context.SaveChanges();

                    EquipmentAttachment result = _context.EquipmentAttachments
                        .Include(x => x.Equipment)                   
                        .First(a => a.Id == id);

                    return new ObjectResult(new HetsResponse(result));
                }

                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create equipment attachments
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Equipment Attachment created</response>
        public virtual IActionResult EquipmentAttachmentsPostAsync(EquipmentAttachment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.Equipments.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.EquipmentAttachments.Update(item);                    
                }
                else
                {
                    // record not found
                    _context.EquipmentAttachments.Add(item);
                }

                // Save the changes                    
                _context.SaveChanges();

                int itemId = item.Id;

                EquipmentAttachment result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)
                    .First(a => a.Id == itemId);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }        
    }
}
