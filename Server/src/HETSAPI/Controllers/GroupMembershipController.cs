using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Group Membership Controller
    /// </summary>
    public class GroupMembershipController : Controller
    {
        private readonly IGroupMembershipService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public GroupMembershipController(IGroupMembershipService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">GroupMembership created</response>
        [HttpPost]
        [Route("/api/groupmemberships/bulk")]
        [SwaggerOperation("GroupmembershipsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GroupmembershipsBulkPost([FromBody]GroupMembership[] items)
        {
            return this._service.GroupmembershipsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/groupmemberships")]
        [SwaggerOperation("GroupmembershipsGet")]
        [SwaggerResponse(200, type: typeof(List<GroupMembership>))]
        public virtual IActionResult GroupmembershipsGet()
        {
            return this._service.GroupmembershipsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        [HttpPost]
        [Route("/api/groupmemberships/{id}/delete")]
        [SwaggerOperation("GroupmembershipsIdDeletePost")]
        public virtual IActionResult GroupmembershipsIdDeletePost([FromRoute]int id)
        {
            return this._service.GroupmembershipsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        [HttpGet]
        [Route("/api/groupmemberships/{id}")]
        [SwaggerOperation("GroupmembershipsIdGet")]
        [SwaggerResponse(200, type: typeof(GroupMembership))]
        public virtual IActionResult GroupmembershipsIdGet([FromRoute]int id)
        {
            return this._service.GroupmembershipsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of GroupMembership to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">GroupMembership not found</response>
        [HttpPut]
        [Route("/api/groupmemberships/{id}")]
        [SwaggerOperation("GroupmembershipsIdPut")]
        [SwaggerResponse(200, type: typeof(GroupMembership))]
        public virtual IActionResult GroupmembershipsIdPut([FromRoute]int id, [FromBody]GroupMembership item)
        {
            return this._service.GroupmembershipsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">GroupMembership created</response>
        [HttpPost]
        [Route("/api/groupmemberships")]
        [SwaggerOperation("GroupmembershipsPost")]
        [SwaggerResponse(200, type: typeof(GroupMembership))]
        public virtual IActionResult GroupmembershipsPost([FromBody]GroupMembership item)
        {
            return this._service.GroupmembershipsPostAsync(item);
        }
    }
}
