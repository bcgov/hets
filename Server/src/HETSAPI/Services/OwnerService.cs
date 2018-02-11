using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.Services.Impl;

namespace HETSAPI.Services
{
    /// <summary>
    /// Owner Service
    /// </summary>
    public interface IOwnerService
    {
        /// <summary>
        /// Create bulk owner records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        IActionResult OwnersBulkPostAsync(Owner[] items);

        /// <summary>
        /// Get all owners
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult OwnersGetAsync();

        /// <summary>
        /// Delete owner
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdDeletePostAsync(int id);

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdGetAsync(int id);

        /// <summary>
        /// Update owner
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdPutAsync(int id, Owner item);

        /// <summary>
        /// Update owner status
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdStatusPutAsync(int id, OwnerStatus item);

        /// <summary>
        /// Create owner
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Owner created</response>
        IActionResult OwnersPostAsync(Owner item);

        /// <summary>
        /// Searches Owners
        /// </summary>
        /// <remarks>Used for the owner search page.</remarks>
        /// <param name="localAreas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="equipmentTypes">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="owner">Id for a specific Owner</param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <response code="200">OK</response>
        IActionResult OwnersSearchGetAsync(string localAreas, string equipmentTypes, int? owner, string status, bool? hired);

        /// <summary>
        /// Get owner equipment
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdEquipmentGetAsync(int id);

        /// <summary>
        /// Create owner equipment
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to replace Equipment for</param>
        /// <param name="items">Replacement Owner Equipment.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdEquipmentPutAsync(int id, Equipment[] items);

        /// <summary>
        /// Get owner attachments
        /// </summary>
        /// <remarks>Returns attachments for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdAttachmentsGetAsync(int id);

        /// <summary>
        /// Get owner contacts
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsGetAsync(int id);

        /// <summary>
        /// Creat owwner contact
        /// </summary>
        /// <remarks>Adds Owner Contact</remarks>
        /// <param name="id">id of Owner to add a contact for</param>
        /// <param name="item">Adds to Owner Contact</param>
        /// <param name="primary"></param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsPostAsync(int id, Contact item, bool primary);

        /// <summary>
        /// Update owner contacts
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="items">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsPutAsync(int id, Contact[] items);

        /// <summary>
        /// Get owner history
        /// </summary>
        /// <remarks>Returns History for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        /// Create owner history
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        IActionResult OwnersIdHistoryPostAsync(int id, History item);

        /// <summary>
        /// Get note records associated with owner
        /// </summary>
        /// <param name="id">id of Owner to fetch Notes for</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdNotesGetAsync(int id);

        /// <summary>
        /// Update or create a note associated with a owner
        /// </summary>
        /// <remarks>Update a Owner&#39;s Notes</remarks>
        /// <param name="id">id of Owner to update Notes for</param>
        /// <param name="item">Owner Note</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdNotesPostAsync(int id, Note item);

        /// <summary>
        /// Update or create an array of notes associated with a owner
        /// </summary>
        /// <remarks>Update a Owner&#39;s Notes</remarks>
        /// <param name="id">id of Owner to update Notes for</param>
        /// <param name="items">Array of Owner Notes</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdNotesBulkPostAsync(int id, Note[] items);
    }
}
