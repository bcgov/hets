using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentAttachmentService : IEquipmentAttachmentService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public EquipmentAttachmentService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord (EquipmentAttachment item)
        {
            if (item != null)
            {
                // Adjust the record to allow it to be updated / inserted
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments.FirstOrDefault(a => a.Id == item.Equipment.Id);                    
                }
            }           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
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
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentAttachmentsGetAsync()
        {            
            var result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)                  
                    .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentAttachmentsIdDeletePostAsync(int id)
        {
            var exists = _context.EquipmentAttachments.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.EquipmentAttachments.First(a => a.Id == id);
                _context.EquipmentAttachments.Remove(item);
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
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentAttachmentsIdGetAsync(int id)
        {
            var exists = _context.EquipmentAttachments.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)                  
                    .First(a => a.Id == id);
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
        /// <param name="id">id of Equipment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentAttachmentsIdPutAsync(int id, EquipmentAttachment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.EquipmentAttachments                    
                    .Any(a => a.Id == id);
                if (exists && id == item.Id)
                {
                    _context.EquipmentAttachments.Update(item);
                    // Save the changes
                    _context.SaveChanges();

                    var result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)                   
                    .First(a => a.Id == id);
                    
                    return new ObjectResult(result);
                }
                else
                {
                    // record not found
                    return new StatusCodeResult(404);
                }
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
        /// <response code="201">Equipment Attachment created</response>
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
                int item_id = item.Id;
                var result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)
                    .First(a => a.Id == item_id);

                return new ObjectResult(result);                
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }

        }        
    }
}
