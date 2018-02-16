using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.Services.Impl;

namespace HETSAPI.Services
{
    /// <summary>
    /// Project Service
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Create bulk project records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        IActionResult ProjectsBulkPostAsync(Project[] items);

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ProjectsGetAsync();

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdDeletePostAsync(int id);

        /// <summary>
        /// Get project
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdGetAsync(int id);

        /// <summary>
        /// Update project
        /// </summary>
        /// <param name="id">id of Project to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdPutAsync(int id, Project item);

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        IActionResult ProjectsPostAsync(Project item);

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
        IActionResult ProjectsSearchGetAsync(string districts, string project, bool? hasRequests, bool? hasHires, string status);

        /// <summary>
        /// Get rental agreements associated with a project by id
        /// </summary>
        /// <param name="id">id of Project to fetch agreements for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdGetAgreementsAsync(int id);

        /// <summary>
        /// Update a rental agreement by cloning a previous project rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        /// <response code="201">Rental Agreement update</response>
        IActionResult ProjectsRentalAgreementClonePostAsync(int id, ProjectRentalAgreementClone item);

        /// <summary>
        /// Get time records for a project
        /// </summary>
        /// <remarks>Gets an Project&#39;s Time Records</remarks>
        /// <param name="id">id of Project to fetch Time Records for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdTimeRecordsGetAsync(int id);

        /// <summary>
        /// Add a time record to a project
        /// </summary>
        /// <remarks>Adds Project Time Record</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="item">Adds to Project Time Record</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdTimeRecordsPostAsync(int id, TimeRecord item);

        /// <summary>
        /// Update or create an array of time records associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Time Records</remarks>
        /// <param name="id">id of Project to update Time Records for</param>
        /// <param name="items">Array of Project Time Records</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdTimeRecordsBulkPostAsync(int id, TimeRecord[] items);

        /// <summary>
        /// Get equipment associated with a project
        /// </summary>
        /// <remarks>Gets a Project&#39;s Equipment</remarks>
        /// <param name="id">id of Project to fetch Equipment for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdEquipmentGetAsync(int id);

        /// <summary>
        /// Get attachments for a project
        /// </summary>
        /// <remarks>Returns attachments for a particular Project</remarks>
        /// <param name="id">id of Project to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdAttachmentsGetAsync(int id);

        /// <summary>
        /// Get contacts for a project
        /// </summary>
        /// <remarks>Gets an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to fetch Contacts for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsGetAsync(int id);

        /// <summary>
        /// Add a contact to a project
        /// </summary>
        /// <remarks>Adds Project Contact</remarks>
        /// <param name="id">id of Project to add a contact for</param>
        /// <param name="item">Adds to Project Contact</param>
        /// <param name="primary"></param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsPostAsync(int id, Contact item, bool primary);

        /// <summary>
        /// Replace a projects contacts (updat all)
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>        
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsPutAsync(int id, Contact[] items);

        /// <summary>
        /// Get history for a project
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        /// Add history for a project
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        IActionResult ProjectsIdHistoryPostAsync(int id, History item);

        /// <summary>
        /// Get note records associated with project
        /// </summary>
        /// <param name="id">id of Project to fetch Notes for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdNotesGetAsync(int id);

        /// <summary>
        /// Update or create a note associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="item">Project Note</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdNotesPostAsync(int id, Note item);

        /// <summary>
        /// Update or create an array of notes associated with a project
        /// </summary>
        /// <remarks>Update a Project&#39;s Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="items">Array of Project Notes</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdNotesBulkPostAsync(int id, Note[] items);
    }
}
