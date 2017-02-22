/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GroupMembershipController : Controller
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
