using System;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.Services.Impl;

namespace HETSAPI.Services
{
    /// <summary>
    /// Equipment Service
    /// </summary>
    public interface IEquipmentService
    {
        /// <summary>
        /// Create bulk equipment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
        IActionResult EquipmentBulkPostAsync(Equipment[] items);

        /// <summary>
        /// Get all equipment
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult EquipmentGetAsync();

        /// <summary>
        /// Delete equipment
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdDeletePostAsync(int id);
        
        /// <summary>
        /// Update equipment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdPutAsync(int id, Equipment item);

        /// <summary>
        /// Update equipment status
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdStatusPutAsync(int id, EquipmentStatus item);

        /// <summary>
        /// Get equipment by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdGetAsync(int id);              

        /// <summary>
        /// Get equipment by id (ViewModel)
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdViewGetAsync(int id);

        /// <summary>
        /// Create equipment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        IActionResult EquipmentPostAsync(Equipment item);
        
        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="equipmentAttachment">Searches equipmentAttachment type</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentSearchGetAsync(string localareas, string types, string equipmentAttachment, int? owner, string status, bool? hired, DateTime? notverifiedsincedate);

        /// <summary>
        /// Get all duplicate equipment records
        /// </summary>
        /// <remarks>Gets all duplicate equipment records.</remarks>
        /// <param name="id">Equipment id</param>
        /// <param name="serialNumber">Serial Number</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdEquipmentduplicatesGetAsync(int id, string serialNumber);

        /// <summary>
        /// Recalculates seniority for the database
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records.</remarks>
        /// <param name="region">Region to recalculate</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentRecalcSeniorityGetAsync(int region);

        /// <summary>
        /// Get "equipment attachments" associated with an equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdEquipmentattachmentsGetAsync(int id);

        /// <summary>
        /// Get attachments associated with an equipment record
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdAttachmentsGetAsync(int id);        

        /// <summary>
        /// Get history for equipment
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        /// Add history for equipment
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        IActionResult EquipmentIdHistoryPostAsync(int id, History item);

        /// <summary>
        /// Get note records associated with equipment
        /// </summary>
        /// <param name="id">id of Equipment to fetch Notes for</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdNotesGetAsync(int id);

        /// <summary>
        /// Update or create a note associated with a equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="item">Equipment Note</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdNotesPostAsync(int id, Note item);

        /// <summary>
        /// Update or create an array of notes associated with a equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="items">Array of Equipment Notes</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdNotesBulkPostAsync(int id, Note[] items);
    }
}
