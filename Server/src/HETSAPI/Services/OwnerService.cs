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
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Services
{
    /// <summary>
    ///
    /// </summary>
    public interface IOwnerService
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        IActionResult OwnersBulkPostAsync(Owner[] items);

        /// <summary>
        ///
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult OwnersGetAsync();

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns attachments for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdAttachmentsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Adds Owner Contact</remarks>
        /// <param name="id">id of Owner to add a contact for</param>
        /// <param name="item">Adds to Owner Contact</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsPostAsync(int id, Contact item);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="item">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdContactsPutAsync(int id, Contact[] item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdDeletePostAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdEquipmentGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to replace Equipment for</param>
        /// <param name="item">Replacement Owner Equipment.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdEquipmentPutAsync(int id, Equipment[] item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdGetAsync(int id);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns History for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult OwnersIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        IActionResult OwnersIdHistoryPostAsync(int id, History item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        IActionResult OwnersIdPutAsync(int id, Owner item);

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Owner created</response>
        IActionResult OwnersPostAsync(Owner item);

        /// <summary>
        /// Searches Owners
        /// </summary>
        /// <remarks>Used for the owner search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="equipmenttypes">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="owner">Id for a specific Owner</param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <response code="200">OK</response>
        IActionResult OwnersSearchGetAsync(string localareas, string equipmenttypes, int? owner, string status, bool? hired);
    }
}
