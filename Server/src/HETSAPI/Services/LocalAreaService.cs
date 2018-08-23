using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Local Area Service
    /// </summary>
    public interface ILocalAreaService
    {
        /// <summary>
        /// Create bulk local area records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalArea created</response>
        IActionResult LocalAreasBulkPostAsync(LocalArea[] items);  
    }
}
