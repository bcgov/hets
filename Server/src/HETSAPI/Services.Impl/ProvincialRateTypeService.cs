using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Provincial Rate Type Service
    /// </summary>
    public class ProvincialRateTypeService : IProvincialRateTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Condition Type Service Constructor
        /// </summary>
        public ProvincialRateTypeService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bulk create condition type records
        /// </summary>
        /// <remarks>Adds a number of condition types</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult ProvincialRateTypesBulkPostAsync(ProvincialRateType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (ProvincialRateType item in items)
            {
                bool exists = _context.ProvincialRateTypes.Any(a => a.RateType == item.RateType);

                if (exists)
                {
                    _context.ProvincialRateTypes.Update(item);
                }
                else
                {
                    _context.ProvincialRateTypes.Add(item);
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
        public virtual IActionResult ProvincialRateTypesGetAsync()
        {
            List<ProvincialRateType> result = _context.ProvincialRateTypes.AsNoTracking()
                .Where(x => x.Active)
                .ToList();

            int pseudoId = 0;
            foreach (ProvincialRateType rateType in result)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(result));
        }        
    }
}
