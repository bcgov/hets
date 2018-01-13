using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Contact Service
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Contact Service Constructor
        /// </summary>
        public ContactService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk contact records
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
        /// Get all contacts
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult ContactsGetAsync()
        {
            List<Contact> result = _context.Contacts.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdDeletePostAsync(int id)
        {
            bool exists = _context.Contacts.Any(a => a.Id == id);

            if (exists)
            {
                Contact item = _context.Contacts.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Contacts.Remove(item);

                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="id">id of Contact to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdGetAsync(int id)
        {
            bool exists = _context.Contacts.Any(a => a.Id == id);

            if (exists)
            {
                Contact result = _context.Contacts.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="id">id of Contact to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdPutAsync(int id, Contact item)
        {
            bool exists = _context.Contacts.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Contacts.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Contact created</response>
        public virtual IActionResult ContactsPostAsync(Contact item)
        {
            bool exists = _context.Contacts.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Contacts.Update(item);
            }
            else
            {
                // record not found
                _context.Contacts.Add(item);
            }

            // Save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
