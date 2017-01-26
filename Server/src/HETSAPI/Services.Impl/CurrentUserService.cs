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

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// 
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {

        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public CurrentUserService (DbAppContext context)
        {
            _context = context;
        }
	
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>

        public virtual IActionResult UsersCurrentGetAsync ()        
        {
            var result = new CurrentUserViewModel();
            // get the name for the current logged in user
            result.GivenName = "Test";
            result.Surname = "User";
            result.FullName = result.GivenName + " " + result.Surname;
            result.OverdueInspections = 1;
            result.DueNextMonthInspections = 2;
            result.DistrictName = "Victoria";
            result.ScheduledInspections = 3;

            // get the number of inspections available for the current logged in user

            return new ObjectResult(result);
        }
    }
}
