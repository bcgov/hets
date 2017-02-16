/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSCommon;
using System.Reflection;

namespace HETSAPI.Controllers
{
    [Route("api")]
    public class VersionApiController : Controller
    {
        private readonly DbContext _context;

        public VersionApiController(DbAppContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("version")]
        public virtual IActionResult GetServerVersionInfo()
        {
            ProductVersionInfo info = new ProductVersionInfo();
            info.ApplicationVersions.Add(GetApplicationVersionInfo());
            info.DatabaseVersions.Add(GetDatabaseVersionInfo());
            return Ok(info);
        }

        [HttpGet]
        [Route("server/version")]
        public virtual IActionResult GetServerVersion()
        {
            return Ok(GetApplicationVersionInfo());
        }

        [HttpGet]
        [Route("database/version")]
        public virtual IActionResult GetDatabaseVersion()
        {
            return Ok(GetDatabaseVersionInfo());
        }

        private ApplicationVersionInfo GetApplicationVersionInfo()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            return assembly.GetApplicationVersionInfo();
        }

        private DatabaseVersionInfo GetDatabaseVersionInfo()
        {
            return _context.Database.GetDatabaseVersionInfo();
        }
    }
}
