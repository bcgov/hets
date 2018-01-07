using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Group Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GroupController : Controller
    {
        private readonly IGroupService _service;

        /// <summary>
        /// Group Controller Constructor
        /// </summary>
        public GroupController(IGroupService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk group records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Group created</response>
        [HttpPost]
        [Route("/api/groups/bulk")]
        [SwaggerOperation("GroupsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GroupsBulkPost([FromBody]Group[] items)
        {
            return _service.GroupsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/groups")]
        [SwaggerOperation("GroupsGet")]
        [SwaggerResponse(200, type: typeof(List<Group>))]
        public virtual JsonResult GroupsGet()
        {
            return _service.GroupsGetAsync();
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="id">id of Group to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        [HttpPost]
        [Route("/api/groups/{id}/delete")]
        [SwaggerOperation("GroupsIdDeletePost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GroupsIdDeletePost([FromRoute]int id)
        {
            return _service.GroupsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get group by id
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
            return _service.GroupsIdGetAsync(id);
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="id">id of Group to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        [HttpPut]
        [Route("/api/groups/{id}")]
        [SwaggerOperation("GroupsIdPut")]
        [SwaggerResponse(200, type: typeof(Group))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GroupsIdPut([FromRoute]int id, [FromBody]Group item)
        {
            return _service.GroupsIdPutAsync(id, item);
        }

        /// <summary>
        /// Get all users in a given group
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
            return _service.GroupsIdUsersGetAsync(id);
        }

        /// <summary>
        /// Create group
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Group created</response>
        [HttpPost]
        [Route("/api/groups")]
        [SwaggerOperation("GroupsPost")]
        [SwaggerResponse(200, type: typeof(Group))]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GroupsPost([FromBody]Group item)
        {
            return _service.GroupsPostAsync(item);
        }
    }
}
