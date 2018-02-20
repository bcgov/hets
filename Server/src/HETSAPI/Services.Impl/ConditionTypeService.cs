using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Condition Type Service
    /// </summary>
    public class ConditionTypeService : ServiceBase, IConditionTypeService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Condition Type Service Constructor
        /// </summary>
        public ConditionTypeService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
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

                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }

                if (exists)
                {
                    _context.ConditionTypes.Update(item);
                }
                else
                {
                    _context.ConditionTypes.Add(item);
                }

                // Save the changes
                _context.SaveChanges();
            }            

            return new NoContentResult();
        }

        /// <summary>
        /// Get all condition types
        /// </summary>
        /// <remarks>Returns a list of condition types</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult ConditionTypesGetAsync()
        {
            // return for the current users district only
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            var result = _context.ConditionTypes.AsNoTracking()
                .Include(x => x.District)
                .Where(x => x.Active &&
                            x.District.Id == districtId)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get a specific contition record
        /// </summary>
        /// <param name="id">id of Condition to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ConditionTypesIdGetAsync(int id)
        {
            bool exists = _context.ConditionTypes.Any(a => a.Id == id);

            if (exists)
            {
                List<ConditionType> result = _context.ConditionTypes.AsNoTracking()
                    .Include(x => x.District)
                    .Where(x => x.Id == id)
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a condition type record
        /// </summary>
        /// <remarks>Update or create a condition type record</remarks>
        /// <param name="id">id of ConditionType for updating</param>
        /// <param name="item">Condition Type.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult ConditionTypesIdPostAsync(int id, ConditionType item)
        {
            if (id != item.Id)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update contact            
            if (item.Id > 0)
            {
                bool exists = _context.ConditionTypes.Any(a => a.Id == id);

                if (!exists)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // get record
                ConditionType condition = _context.ConditionTypes.First(x => x.Id == id);

                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }

                condition.ConditionTypeCode = item.ConditionTypeCode;
                condition.Description = item.Description;
                condition.Active = item.Active;

                if (item.District != null)
                {
                    condition.DistrictId = item.District.Id;                    
                }
                else
                {
                    condition.District = null;
                }
            }
            else  // add condition
            {
                // get record
                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }

                _context.ConditionTypes.Add(item);                
            }

            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.Id;

            // return the updated record
            List<ConditionType> result = _context.ConditionTypes.AsNoTracking()
                .Include(x => x.District)
                .Where(x => x.Id == id)
                .ToList();

            return new ObjectResult(new HetsResponse(result));     
        }
    }
}
