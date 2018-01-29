using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGroupMembershipService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">GroupMembership created</response>
        IActionResult GroupmembershipsBulkPostAsync(GroupMembership[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult GroupmembershipsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        IActionResult GroupmembershipsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        IActionResult GroupmembershipsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        IActionResult GroupmembershipsIdPutAsync(int id, GroupMembership item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">GroupMembership created</response>
        IActionResult GroupmembershipsPostAsync(GroupMembership item);
    }
}
