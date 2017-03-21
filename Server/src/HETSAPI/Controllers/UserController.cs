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
    public partial class UserController : Controller
    {
        private readonly IUserService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/usergroups/bulk")]
        [SwaggerOperation("UsergroupsBulkPost")]
        public virtual IActionResult UsergroupsBulkPost([FromBody]GroupMembership[] items)
        {
            return this._service.UsergroupsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/users/bulk")]
        [SwaggerOperation("UsersBulkPost")]
        public virtual IActionResult UsersBulkPost([FromBody]User[] items)
        {
            return this._service.UsersBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users")]
        [SwaggerOperation("UsersGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult UsersGet()
        {
            return this._service.UsersGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [Route("/api/users/{id}/delete")]
        [SwaggerOperation("UsersIdDeletePost")]
        public virtual IActionResult UsersIdDeletePost([FromRoute]int id)
        {
            return this._service.UsersIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given context type</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/favourites")]
        [SwaggerOperation("UsersIdFavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavouriteViewModel>))]
        public virtual IActionResult UsersIdFavouritesGet([FromRoute]int id)
        {
            return this._service.UsersIdFavouritesGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdGet")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersIdGet([FromRoute]int id)
        {
            return this._service.UsersIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns all groups that a user is a member of</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsGet")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsGet([FromRoute]int id)
        {
            return this._service.UsersIdGroupsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add to the active set of groups for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPost]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsPost")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsPost([FromRoute]int id, [FromBody]GroupMembershipViewModel item)
        {
            return this._service.UsersIdGroupsPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the active set of groups for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}/groups")]
        [SwaggerOperation("UsersIdGroupsPut")]
        [SwaggerResponse(200, type: typeof(List<GroupMembershipViewModel>))]
        public virtual IActionResult UsersIdGroupsPut([FromRoute]int id, [FromBody]GroupMembershipViewModel[] items)
        {
            return this._service.UsersIdGroupsPutAsync(id, items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/permissions")]
        [SwaggerOperation("UsersIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionViewModel>))]
        public virtual IActionResult UsersIdPermissionsGet([FromRoute]int id)
        {
            return this._service.UsersIdPermissionsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}")]
        [SwaggerOperation("UsersIdPut")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersIdPut([FromRoute]int id, [FromBody]UserViewModel item)
        {
            return this._service.UsersIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpGet]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesGet")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult UsersIdRolesGet([FromRoute]int id)
        {
            return this._service.UsersIdRolesGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="201">Role created for user</response>
        [HttpPost]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPost")]
        [SwaggerResponse(200, type: typeof(UserRoleViewModel))]
        public virtual IActionResult UsersIdRolesPost([FromRoute]int id, [FromBody]UserRoleViewModel item)
        {
            return this._service.UsersIdRolesPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        [HttpPut]
        [Route("/api/users/{id}/roles")]
        [SwaggerOperation("UsersIdRolesPut")]
        [SwaggerResponse(200, type: typeof(List<UserRoleViewModel>))]
        public virtual IActionResult UsersIdRolesPut([FromRoute]int id, [FromBody]UserRoleViewModel[] items)
        {
            return this._service.UsersIdRolesPutAsync(id, items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">User created</response>
        [HttpPost]
        [Route("/api/users")]
        [SwaggerOperation("UsersPost")]
        [SwaggerResponse(200, type: typeof(UserViewModel))]
        public virtual IActionResult UsersPost([FromBody]UserViewModel item)
        {
            return this._service.UsersPostAsync(item);
        }

        /// <summary>
        /// Searches Users
        /// </summary>
        /// <remarks>Used for the search users.</remarks>
        /// <param name="districts">Districts (array of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/search")]
        [SwaggerOperation("UsersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<UserViewModel>))]
        public virtual IActionResult UsersSearchGet([FromQuery]int?[] districts, [FromQuery]string surname, [FromQuery]bool? includeInactive)
        {
            return this._service.UsersSearchGetAsync(districts, surname, includeInactive);
        }
    }
}
