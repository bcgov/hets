using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISeniorityAuditService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">SeniorityAudit created</response>
        IActionResult SeniorityauditsBulkPostAsync(SeniorityAudit[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult SeniorityauditsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        IActionResult SeniorityauditsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        IActionResult SeniorityauditsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        IActionResult SeniorityauditsIdPutAsync(int id, SeniorityAudit item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">SeniorityAudit created</response>
        IActionResult SeniorityauditsPostAsync(SeniorityAudit item);
    }
}
