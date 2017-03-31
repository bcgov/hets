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
    public partial class AdminController : Controller
    {
        private readonly IAdminService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Starts the import process
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <param name="districts">comma seperated list of district IDs to process.</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found in system</response>
        [HttpGet]
        [Route("/api/admin/import")]
        [SwaggerOperation("AdminImportGet")]
        public virtual IActionResult AdminImportGet([FromQuery]string path, [FromQuery]string districts)
        {
            return this._service.AdminImportGetAsync(path, districts);
        }
    }
}
