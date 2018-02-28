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
    }
}
