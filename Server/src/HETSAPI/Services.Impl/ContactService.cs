using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Contact Service
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Contact Service Constructor
        /// </summary>
        public ContactService(DbAppContext context)
        {
            _context = context;
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
    }
}
