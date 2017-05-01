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
    public interface IRentalRequestService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequest created</response>
        IActionResult RentalrequestsBulkPostAsync(RentalRequest[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalrequestsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns attachments for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        IActionResult RentalrequestsIdAttachmentsGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        IActionResult RentalrequestsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        IActionResult RentalrequestsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        IActionResult RentalrequestsIdHistoryGetAsync(int id, int? offset, int? limit);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Add a History record to the RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        IActionResult RentalrequestsIdHistoryPostAsync(int id, History item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        IActionResult RentalrequestsIdPutAsync(int id, RentalRequest item);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry.  Side effect is the LocalAreaRotationList is also updated</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="rentalRequestRotationListId">id of RentalRequestRotationList to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        IActionResult RentalrequestsIdRentalrequestrotationlistRentalRequestRotationListIdPutAsync(int id, int rentalRequestRotationListId, RentalRequestRotationList item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequest created</response>
        IActionResult RentalrequestsPostAsync(RentalRequest item);

        /// <summary>
        /// Searches RentalRequests
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="project">Searches equipmentAttachment type</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">Inspection start date</param>
        /// <param name="endDate">Inspection end date</param>
        /// <response code="200">OK</response>
        IActionResult RentalrequestsSearchGetAsync(int?[] localareas, string project, string status, DateTime? startDate, DateTime? endDate);
    }
}
