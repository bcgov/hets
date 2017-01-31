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
    public class EquipmentAttachmentTypeService : IEquipmentAttachmentTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public EquipmentAttachmentTypeService(DbAppContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentAttachmentType created</response>
        public virtual IActionResult EquipmentAttachmentTypesBulkPostAsync(EquipmentAttachmentType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (EquipmentAttachmentType item in items)
            {
                // determine if this is an insert or an update            
                bool exists = _context.EquipmentAttachmentTypes.Any(a => a.Id == item.Id);
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
        public virtual IActionResult EquipmentAttachmentTypesGetAsync()
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        public virtual IActionResult EquipmentAttachmentTypesIdDeletePostAsync(int id)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        public virtual IActionResult EquipmentAttachmentTypesIdGetAsync(int id)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of EquipmentAttachmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">EquipmentAttachmentType not found</response>
        public virtual IActionResult EquipmentAttachmentTypesIdPutAsync(int id, EquipmentAttachmentType item)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">EquipmentAttachmentType created</response>
        public virtual IActionResult EquipmentAttachmentTypesPostAsync(EquipmentAttachmentType item)
        {
            var result = "";
            return new ObjectResult(result);
        }
    }
}
