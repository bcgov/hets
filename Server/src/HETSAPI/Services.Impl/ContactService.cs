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
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        /// <response code="200">OK</response>
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
    }
}
