using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Contact Service
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Create bulk contact records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Contact created</response>
        IActionResult ContactsBulkPostAsync(Contact[] items);

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        /// <response code="200">OK</response>
        IActionResult ContactsIdDeletePostAsync(int id);
    }
}
