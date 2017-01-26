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

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SchoolBusOwnerController : Controller
    {
        private readonly ISchoolBusOwnerService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public SchoolBusOwnerController(ISchoolBusOwnerService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns list of available FavouriteContextTypes</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/favouritecontexttypes")]
        [SwaggerOperation("FavouritecontexttypesGet")]
        [SwaggerResponse(200, type: typeof(List<FavouriteContextType>))]
        public virtual IActionResult FavouritecontexttypesGet()
        {
            return this._service.FavouritecontexttypesGetAsync();
        }
    }
}
