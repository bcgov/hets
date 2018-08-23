using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Permission Service
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Create bulk permission records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Permission created</response>
        IActionResult PermissionsBulkPostAsync(Permission[] items);

        /// <summary>
        /// Get all permissions
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult PermissionsGetAsync();        
    }
}
