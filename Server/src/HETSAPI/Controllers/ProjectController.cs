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
    public partial class ProjectController : Controller
    {
        private readonly IProjectService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        [HttpPost]
        [Route("/api/projects/bulk")]
        [SwaggerOperation("ProjectsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult ProjectsBulkPost([FromBody]Project[] items)
        {
            return this._service.ProjectsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects")]
        [SwaggerOperation("ProjectsGet")]
        [SwaggerResponse(200, type: typeof(List<Project>))]
        public virtual IActionResult ProjectsGet()
        {
            return this._service.ProjectsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns attachments for a particular Project</remarks>
        /// <param name="id">id of Project to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpGet]
        [Route("/api/projects/{id}/attachments")]
        [SwaggerOperation("ProjectsIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        public virtual IActionResult ProjectsIdAttachmentsGet([FromRoute]int id)
        {
            return this._service.ProjectsIdAttachmentsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Gets an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to fetch Contacts for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/{id}/contacts")]
        [SwaggerOperation("ProjectsIdContactsGet")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult ProjectsIdContactsGet([FromRoute]int id)
        {
            return this._service.ProjectsIdContactsGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds Project Contact</remarks>
        /// <param name="id">id of Project to add a contact for</param>
        /// <param name="item">Adds to Project Contact</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/contacts")]
        [SwaggerOperation("ProjectsIdContactsPost")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult ProjectsIdContactsPost([FromRoute]int id, [FromBody]Contact item)
        {
            return this._service.ProjectsIdContactsPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="item">Replacement Project contacts.</param>
        /// <response code="200">OK</response>
        [HttpPut]
        [Route("/api/projects/{id}/contacts")]
        [SwaggerOperation("ProjectsIdContactsPut")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult ProjectsIdContactsPut([FromRoute]int id, [FromBody]Contact[] item)
        {
            return this._service.ProjectsIdContactsPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpPost]
        [Route("/api/projects/{id}/delete")]
        [SwaggerOperation("ProjectsIdDeletePost")]
        public virtual IActionResult ProjectsIdDeletePost([FromRoute]int id)
        {
            return this._service.ProjectsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpGet]
        [Route("/api/projects/{id}")]
        [SwaggerOperation("ProjectsIdGet")]
        [SwaggerResponse(200, type: typeof(Project))]
        public virtual IActionResult ProjectsIdGet([FromRoute]int id)
        {
            return this._service.ProjectsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/{id}/history")]
        [SwaggerOperation("ProjectsIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult ProjectsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return this._service.ProjectsIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/projects/{id}/history")]
        [SwaggerOperation("ProjectsIdHistoryPost")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult ProjectsIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return this._service.ProjectsIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpPut]
        [Route("/api/projects/{id}")]
        [SwaggerOperation("ProjectsIdPut")]
        [SwaggerResponse(200, type: typeof(Project))]
        public virtual IActionResult ProjectsIdPut([FromRoute]int id, [FromBody]Project item)
        {
            return this._service.ProjectsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        [HttpPost]
        [Route("/api/projects")]
        [SwaggerOperation("ProjectsPost")]
        [SwaggerResponse(200, type: typeof(Project))]
        public virtual IActionResult ProjectsPost([FromBody]Project item)
        {
            return this._service.ProjectsPostAsync(item);
        }

        /// <summary>
        /// Searches Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/search")]
        [SwaggerOperation("ProjectsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<ProjectSearchResultViewModel>))]
        public virtual IActionResult ProjectsSearchGet([FromQuery]int?[] localareas, [FromQuery]string project, [FromQuery]bool? hasRequests, [FromQuery]bool? hasHires)
        {
            return this._service.ProjectsSearchGetAsync(localareas, project, hasRequests, hasHires);
        }
    }
}
