using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SeniorityAuditService : ISeniorityAuditService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public SeniorityAuditService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of Seniority Audits.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult SeniorityauditsBulkPostAsync(SeniorityAudit[] items)
        {
            // by design not implemented
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a list of available districts</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult SeniorityauditsGetAsync()
        {
            var result = _context.SeniorityAudits
                .Include(x => x.Equipment)
                .Include(x => x.Owner)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .OrderByDescending(x => x.Id)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Deletes a Service Area</remarks>
        /// <param name="id">id of Service Area to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult SeniorityauditsIdDeletePostAsync(int id)
        {
            // by design not implemented
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a specific Service Area</remarks>
        /// <param name="id">id of Service Area to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult SeniorityauditsIdGetAsync(int id)
        {
            var exists = _context.SeniorityAudits.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.SeniorityAudits
                    .Include(x => x.Equipment)
                    .Include(x => x.Owner)
                    .Include(x => x.LocalArea.ServiceArea.District.Region)               
                    .FirstOrDefault(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a Service Area</remarks>
        /// <param name="id">id of Service Area to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Service Area not found</response>
        public virtual IActionResult SeniorityauditsIdPutAsync(int id, SeniorityAudit body)
        {
            // by design not implemented
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a Service Area</remarks>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult SeniorityauditsPostAsync(SeniorityAudit item)
        {
            // by design not implemented
            return new NoContentResult();
        }
    }
}
