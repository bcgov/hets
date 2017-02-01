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
    public class OwnerService : IOwnerService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public OwnerService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(Owner item)
        {
            // Adjust the record to allow it to be updated / inserted
            if (item.LocalArea != null)
            {
                int localarea_id = item.LocalArea.Id;
                bool localarea_exists = _context.LocalAreas.Any(a => a.Id == localarea_id);
                if (localarea_exists)
                {
                    LocalArea localarea = _context.LocalAreas.First(a => a.Id == localarea_id);
                    item.LocalArea = localarea;
                }
                else
                {
                    item.LocalArea = null;
                }
            }

            // Adjust the owner contacts.
            
            if (item.Contacts != null)
            {
                for (int i = 0; i < item.Contacts.Count; i++)
                {
                    Contact contact = item.Contacts[i];
                    if (contact != null)
                    {
                        int contact_id = contact.Id;
                        bool contact_exists = _context.Contacts.Any(a => a.Id == contact_id);
                        if (contact_exists)
                        {
                            contact = _context.Contacts.First(a => a.Id == contact_id);
                            item.Contacts[i] = contact;
                        }
                        else
                        {
                            item.Contacts[i] = null;
                        }
                    }
                }
            }

            if (item.PrimaryContact != null)
            {
                int primaryContact_id = item.PrimaryContact.Id;
                bool primaryContact_exists = _context.Contacts.Any(a => a.Id == primaryContact_id);
                if (primaryContact_exists)
                {
                    item.PrimaryContact = _context.Contacts.First(a => a.Id == primaryContact_id);                         
                }
                else
                {
                    item.PrimaryContact = null;
                }               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        public virtual IActionResult OwnersBulkPostAsync(Owner[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (Owner item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update            
                bool exists = _context.Owners.Any(a => a.Id == item.Id);
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
        public virtual IActionResult OwnersGetAsync()
        {
            var result = _context.Owners
        .Include(x => x.LocalArea.ServiceArea.District.Region)        
        .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdDeletePostAsync(int id)
        {
            var exists = _context.Owners.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Owners.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Owners.Remove(item);
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
        /// 
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdGetAsync(int id)
        {
            var exists = _context.Owners.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Owners.First(a => a.Id == id);
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
        /// <param name="id">id of Owner to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdPutAsync(int id, Owner item)
        {
            var exists = _context.Owners.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Owners.Update(item);
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
        /// <response code="201">Owner created</response>
        public virtual IActionResult OwnersPostAsync(Owner item)
        {
            var exists = _context.Owners.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.Owners.Update(item);
            }
            else
            {
                // record not found
                _context.Owners.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
