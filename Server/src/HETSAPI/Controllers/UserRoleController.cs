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
    public partial class UserRoleController : Controller
    {
        private readonly IUserRoleService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserRole created</response>
        [HttpPost]
        [Route("/api/userroles/bulk")]
        [SwaggerOperation("UserrolesBulkPost")]
        public virtual IActionResult UserrolesBulkPost([FromBody]UserRole[] items)
        {
            return this._service.UserrolesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/userroles")]
        [SwaggerOperation("UserrolesGet")]
        [SwaggerResponse(200, type: typeof(List<UserRole>))]
        public virtual IActionResult UserrolesGet()
        {
            return this._service.UserrolesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpPost]
        [Route("/api/userroles/{id}/delete")]
        [SwaggerOperation("UserrolesIdDeletePost")]
        public virtual IActionResult UserrolesIdDeletePost([FromRoute]int id)
        {
            return this._service.UserrolesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpGet]
        [Route("/api/userroles/{id}")]
        [SwaggerOperation("UserrolesIdGet")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        public virtual IActionResult UserrolesIdGet([FromRoute]int id)
        {
            return this._service.UserrolesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserRole to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserRole not found</response>
        [HttpPut]
        [Route("/api/userroles/{id}")]
        [SwaggerOperation("UserrolesIdPut")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        public virtual IActionResult UserrolesIdPut([FromRoute]int id, [FromBody]UserRole item)
        {
            return this._service.UserrolesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserRole created</response>
        [HttpPost]
        [Route("/api/userroles")]
        [SwaggerOperation("UserrolesPost")]
        [SwaggerResponse(200, type: typeof(UserRole))]
        public virtual IActionResult UserrolesPost([FromBody]UserRole item)
        {
            return this._service.UserrolesPostAsync(item);
        }
    }
}
