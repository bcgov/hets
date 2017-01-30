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
    public class ContactService : IContactService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public ContactService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(Contact item)
        {
            // Addresses is a list

            if (item.Addresses != null)
            {
                for (int i = 0; i < item.Addresses.Count; i++)
                {
                    ContactAddress address = item.Addresses[i];
                    if (address != null)
                    {
                        int address_id = address.Id;
                        bool address_exists = _context.ContactAddresss.Any(a => a.Id == address_id);
                        if (address_exists)
                        {
                            address = _context.ContactAddresss.First(a => a.Id == address_id);
                            item.Addresses[i] = address;
                        }
                        else
                        {
                            item.Addresses[i] = null;
                        }
                    }
                }
            }


            // Phones is a list     
            if (item.Phones != null)
            {
                for (int i = 0; i < item.Phones.Count; i++)
                {
                    ContactPhone contactphone = item.Phones[i];
                    if (contactphone != null)
                    {
                        int contactphone_id = contactphone.Id;
                        bool contactphone_exists = _context.ContactPhones.Any(a => a.Id == contactphone_id);
                        if (contactphone_exists)
                        {
                            contactphone = _context.ContactPhones.First(a => a.Id == contactphone_id);
                            item.Phones[i] = contactphone;
                        }
                        else
                        {
                            item.Phones[i] = null;
                        }
                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Contact created</response>
        public virtual IActionResult ContactsBulkPostAsync(Contact[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }         

            foreach (Contact item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update            
                bool exists = _context.Contacts.Any(a => a.Id == item.Id);
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
        public virtual IActionResult ContactsGetAsync()
        {
            var result = _context.Contacts
                .Include(x => x.Addresses)
                .Include(x => x.Phones)                
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdDeletePostAsync(int id)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Contact to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdGetAsync(int id)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Contact to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdPutAsync(int id, Contact item)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Contact created</response>
        public virtual IActionResult ContactsPostAsync(Contact item)
        {
            var result = "";
            return new ObjectResult(result);
        }
    }
}
