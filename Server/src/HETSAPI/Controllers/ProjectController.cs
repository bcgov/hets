using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Project Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;

        /// <summary>
        /// Project Controller Constructor
        /// </summary>
        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk project records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        [HttpPost]
        [Route("/api/projects/bulk")]
        [SwaggerOperation("ProjectsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult ProjectsBulkPost([FromBody]Project[] items)
        {
            return _service.ProjectsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects")]
        [SwaggerOperation("ProjectsGet")]
        [SwaggerResponse(200, type: typeof(List<Project>))]
        public virtual IActionResult ProjectsGet()
        {
            return _service.ProjectsGetAsync();
        }

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpPost]
        [Route("/api/projects/{id}/delete")]
        [SwaggerOperation("ProjectsIdDeletePost")]
        public virtual IActionResult ProjectsIdDeletePost([FromRoute]int id)
        {
            return _service.ProjectsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get project by id
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
            return _service.ProjectsIdGetAsync(id);
        }

        /// <summary>
        /// Update project
        /// </summary>
        /// <param name="id">id of Project to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        [HttpPut]
        [Route("/api/projects/{id}")]
        [SwaggerOperation("ProjectsIdPut")]
        [SwaggerResponse(200, type: typeof(Project))]
        public virtual IActionResult ProjectsIdPut([FromRoute]int id, [FromBody]Project item)
        {
            return _service.ProjectsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        [HttpPost]
        [Route("/api/projects")]
        [SwaggerOperation("ProjectsPost")]
        [SwaggerResponse(200, type: typeof(Project))]
        public virtual IActionResult ProjectsPost([FromBody]Project item)
        {
            return _service.ProjectsPostAsync(item);
        }

        /// <summary>
        /// Searches Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="districts">Districts (comma seperated list of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <param name="status">if included, filter the results to those with a status matching this string</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/search")]
        [SwaggerOperation("ProjectsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<ProjectSearchResultViewModel>))]
        public virtual IActionResult ProjectsSearchGet([FromQuery]string districts, [FromQuery]string project, [FromQuery]bool? hasRequests, [FromQuery]bool? hasHires, [FromQuery]string status)
        {
            return _service.ProjectsSearchGetAsync(districts, project, hasRequests, hasHires, status);
        }

        #region Project Time Records

        /// <summary>
        /// Get time records associated with a project
        /// </summary>
        /// <remarks>Gets a Project&#39;s Time Records</remarks>
        /// <param name="id">id of Project to fetch Time Records for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/{id}/timeRecords")]
        [SwaggerOperation("ProjectsIdTimeRecordsGet")]
        [SwaggerResponse(200, type: typeof(List<TimeRecord>))]
        public virtual IActionResult ProjectsIdTimeRecordsGet([FromRoute]int id)
        {
            return _service.ProjectsIdTimeRecordsGetAsync(id);
        }

        /// <summary>
        /// Add a project time record
        /// </summary>
        /// <remarks>Adds Project Time Records</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="item">Adds to Project Time Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/timeRecord")]
        [SwaggerOperation("ProjectsIdTimeRecordsPost")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult ProjectsIdTimeRecordsPost([FromRoute]int id, [FromBody]TimeRecord item)
        {
            return _service.ProjectsIdTimeRecordsPostAsync(id, item);
        }
        
        /// <summary>
        /// pdate or create an array of time records associated with a project
        /// </summary>
        /// <remarks>Adds Project Time Records</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="items">Array of Project Time Records</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/timeRecords")]
        [SwaggerOperation("ProjectsIdTimeRecordsBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult ProjectsIdTimeRecordsBulkPostAsync([FromRoute]int id, [FromBody]TimeRecord[] items)
        {
            return _service.ProjectsIdTimeRecordsBulkPostAsync(id, items);
        }

        #endregion

        #region Project Equipment

        /// <summary>
        /// Get equipment associated with a project
        /// </summary>
        /// <remarks>Gets a Project&#39;s Equipment</remarks>
        /// <param name="id">id of Project to fetch Equipment for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/{id}/equipment")]
        [SwaggerOperation("ProjectsIdEquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreement>))]
        public virtual IActionResult ProjectsIdEquipmentGet([FromRoute]int id)
        {
            return _service.ProjectsIdEquipmentGetAsync(id);
        }

        #endregion

        #region Project Attachments

        /// <summary>
        /// Get attachments associated with a project
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
            return _service.ProjectsIdAttachmentsGetAsync(id);
        }

        #endregion

        #region Project Contacts

        /// <summary>
        /// Get contacts associated with a project
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
            return _service.ProjectsIdContactsGetAsync(id);
        }

        /// <summary>
        /// Add a project contact
        /// </summary>
        /// <remarks>Adds Project Contact</remarks>
        /// <param name="id">id of Project to add a contact for</param>
        /// <param name="primary">is this the primary contact</param>
        /// <param name="item">Adds to Project Contact</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/contacts/{primary}")]
        [SwaggerOperation("ProjectsIdContactsPost")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult ProjectsIdContactsPost([FromRoute]int id, [FromRoute]bool primary, [FromBody]Contact item)
        {
            return _service.ProjectsIdContactsPostAsync(id, item, primary);
        }

        /// <summary>
        /// Update all project contacts
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
            return _service.ProjectsIdContactsPutAsync(id, item);
        }

        #endregion

        #region Project History

        /// <summary>
        /// Get history associated with a project
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
            return _service.ProjectsIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// Create project history
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/projects/{id}/history")]
        [SwaggerOperation("ProjectsIdHistoryPost")]
        public virtual IActionResult ProjectsIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return _service.ProjectsIdHistoryPostAsync(id, item);
        }

        #endregion

        #region Project Note Records

        /// <summary>
        /// Get note records associated with project
        /// </summary>
        /// <param name="id">id of Project to fetch Notes for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/projects/{id}/notes")]
        [SwaggerOperation("ProjectsIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<Note>))]
        public virtual IActionResult ProjectsIdNotesGet([FromRoute]int id)
        {
            return _service.ProjectsIdNotesGetAsync(id);
        }

        /// <summary>
        /// Update or create a note associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="item">Project Note</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/note")]
        [SwaggerOperation("ProjectsIdNotePost")]
        [SwaggerResponse(200, type: typeof(Note))]
        public virtual IActionResult ProjectsIdNotePost([FromRoute]int id, [FromBody]Note item)
        {
            return _service.ProjectsIdNotesPostAsync(id, item);
        }

        /// <summary>
        /// pdate or create an array of notes associated with a project
        /// </summary>
        /// <remarks>Adds Note Records</remarks>
        /// <param name="id">id of Project to add notes for</param>
        /// <param name="items">Array of Project Notes</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/projects/{id}/notes")]
        [SwaggerOperation("ProjectsIdNotesBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult ProjectsIdNotesBulkPostAsync([FromRoute]int id, [FromBody]Note[] items)
        {
            return _service.ProjectsIdNotesBulkPostAsync(id, items);
        }

        #endregion        
    }
}
