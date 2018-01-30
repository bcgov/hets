using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Condition Type Service
    /// </summary>
    public interface IConditionTypeService
    {
        /// <summary>
        /// Create bulk condition type records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">City created</response>
        IActionResult ConditionTypesBulkPostAsync(ConditionType[] items);

        /// <summary>
        /// Get all condition types
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ConditionTypesGetAsync();        
    }
}
