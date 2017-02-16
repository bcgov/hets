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
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="types">Equipment Types (array of id numbers)</param>
        /// <param name="attachments">Equipment Attachments (array of id numbers)</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        IActionResult EquipmentSearchGetAsync(int?[] localareas, int?[] types, int?[] attachments, int? owner, string status, bool? hired, DateTime? notverifiedsincedate);
    }
}
