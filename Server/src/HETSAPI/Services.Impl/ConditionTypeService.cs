using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Condition Type Service
    /// </summary>
    public class ConditionTypeService : IConditionTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Condition Type Service Constructor
        /// </summary>
        public ConditionTypeService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bulk create condition type records
        /// </summary>
        /// <remarks>Adds a number of condition types</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ConditionTypesBulkPostAsync(ConditionType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (ConditionType item in items)
            {
                bool exists = _context.ConditionTypes.Any(a => a.ConditionTypeCode == item.ConditionTypeCode);

                if (exists)
                {
                    _context.ConditionTypes.Update(item);
                }
                else
                {
                    _context.ConditionTypes.Add(item);
                }                
            }

            // Save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all condition types
        /// </summary>
        /// <remarks>Returns a list of condition types</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult ConditionTypesGetAsync()
        {
            var result = _context.ConditionTypes.ToList();
            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
