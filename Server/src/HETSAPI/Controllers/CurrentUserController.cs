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
    public partial class CurrentUserController : Controller
    {
        private readonly ICurrentUserService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public CurrentUserController(ICurrentUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/users/current")]
        [SwaggerOperation("UsersCurrentGet")]
        [SwaggerResponse(200, type: typeof(CurrentUserViewModel))]
        public virtual IActionResult UsersCurrentGet()
        {
            return this._service.UsersCurrentGetAsync();
        }
    }
}
