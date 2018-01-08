using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

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
            // to do
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
            var exists = _context.Contacts.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Contacts.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Contacts.Remove(item);
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
        /// <param name="id">id of Contact to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdGetAsync(int id)
        {
            var exists = _context.Contacts.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Contacts.First(a => a.Id == id);
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
        /// <param name="id">id of Contact to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        public virtual IActionResult ContactsIdPutAsync(int id, Contact item)
        {
            var exists = _context.Contacts.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Contacts.Update(item);
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
        /// <response code="201">Contact created</response>
        public virtual IActionResult ContactsPostAsync(Contact item)
        {
            var exists = _context.Contacts.Any(a => a.Id == item.Id);
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
            return new ObjectResult(item);
        }
    }
}
