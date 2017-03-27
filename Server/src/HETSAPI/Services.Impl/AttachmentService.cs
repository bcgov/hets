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
        public virtual IActionResult AttachmentBulkPostAsync(Attachment[] items)
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
        public virtual IActionResult AttachmentGetAsync()
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
        public virtual IActionResult AttachmentIdDeletePostAsync(int id)
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
        public virtual IActionResult AttachmentIdGetAsync(int id)
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
        public virtual IActionResult AttachmentIdPutAsync(int id, Attachment item)
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
        public virtual IActionResult AttachmentPostAsync(Attachment item)
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
