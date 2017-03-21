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
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GroupController : Controller
    {
        private readonly IGroupService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public GroupController(IGroupService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Group created</response>
        [HttpPost]
        [Route("/api/groups/bulk")]
        [SwaggerOperation("GroupsBulkPost")]
        public virtual IActionResult GroupsBulkPost([FromBody]Group[] items)
        {
            return this._service.GroupsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/groups")]
        [SwaggerOperation("GroupsGet")]
        [SwaggerResponse(200, type: typeof(List<Group>))]
        public virtual IActionResult GroupsGet()
        {
            return this._service.GroupsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Group to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        [HttpPost]
        [Route("/api/groups/{id}/delete")]
        [SwaggerOperation("GroupsIdDeletePost")]
        public virtual IActionResult GroupsIdDeletePost([FromRoute]int id)
        {
            return this._service.GroupsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Group to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        [HttpGet]
        [Route("/api/groups/{id}")]
        [SwaggerOperation("GroupsIdGet")]
        [SwaggerResponse(200, type: typeof(Group))]
        public virtual IActionResult GroupsIdGet([FromRoute]int id)
        {
            return this._service.GroupsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Group to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        [HttpPut]
        [Route("/api/groups/{id}")]
        [SwaggerOperation("GroupsIdPut")]
        [SwaggerResponse(200, type: typeof(Group))]
        public virtual IActionResult GroupsIdPut([FromRoute]int id, [FromBody]Group item)
        {
            return this._service.GroupsIdPutAsync(id, item);
        }

        /// <summary>
        /// returns users in a given Group
        /// </summary>
        /// <remarks>Used to get users in a given Group</remarks>
        /// <param name="id">id of Group to fetch Users for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/groups/{id}/users")]
        [SwaggerOperation("GroupsIdUsersGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult GroupsIdUsersGet([FromRoute]int id)
        {
            return this._service.GroupsIdUsersGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Group created</response>
        [HttpPost]
        [Route("/api/groups")]
        [SwaggerOperation("GroupsPost")]
        [SwaggerResponse(200, type: typeof(Group))]
        public virtual IActionResult GroupsPost([FromBody]Group item)
        {
            return this._service.GroupsPostAsync(item);
        }
    }
}
