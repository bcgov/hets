/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;

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
            // Adjust the record to allow it to be updated / inserted
            if (item.Equipment != null)
            {
                int equipment_id = item.Equipment.Id;
                bool equipment_exists = _context.Equipments.Any(a => a.Id == equipment_id);
                if (equipment_exists)
                {
                    Equipment equipment = _context.Equipments.First(a => a.Id == equipment_id);
                    item.Equipment = equipment;
                }
                else
                {
                    item.Equipment = null;
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
