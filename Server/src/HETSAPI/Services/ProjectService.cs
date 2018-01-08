using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    ///
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        IActionResult ProjectsBulkPostAsync(Project[] items);

        /// <summary>
        ///
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult ProjectsGetAsync();

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns attachments for a particular Project</remarks>
        /// <param name="id">id of Project to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdAttachmentsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Gets an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to fetch Contacts for</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Adds Project Contact</remarks>
        /// <param name="id">id of Project to add a contact for</param>
        /// <param name="item">Adds to Project Contact</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsPostAsync(int id, Contact item);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdContactsPutAsync(int id, Contact[] items);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdDeletePostAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult ProjectsIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        IActionResult ProjectsIdHistoryPostAsync(int id, History item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        IActionResult ProjectsIdPutAsync(int id, Project item);

        /// <summary>
        ///
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
    }
}
