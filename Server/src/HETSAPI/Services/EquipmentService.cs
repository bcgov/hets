using System;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    ///
    /// </summary>
    public interface IEquipmentService
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
        IActionResult EquipmentBulkPostAsync(Equipment[] items);

        /// <summary>
        ///
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult EquipmentGetAsync();

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdAttachmentsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdDeletePostAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdEquipmentattachmentsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        IActionResult EquipmentIdHistoryPostAsync(int id, History item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        IActionResult EquipmentIdPutAsync(int id, Equipment item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentIdViewGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        IActionResult EquipmentPostAsync(Equipment item);

        /// <summary>
        /// Recalculates seniority for the database
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records.</remarks>
        /// <param name="region">Region to recalculate</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentRecalcSeniorityGetAsync(int region);

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
    }
}
