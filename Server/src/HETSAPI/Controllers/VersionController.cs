using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using HETSAPI.Models;
using HETSCommon;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Version Controller
    /// </summary>
    [Authorize]
    [Route("api")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class VersionController : Controller
    {
        // Hack in the git commit id.
        private const string CommitKey = "OPENSHIFT_BUILD_COMMIT";

        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Version Constoller Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        public VersionController(IConfiguration configuration, DbAppContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Commit id
        /// </summary>
        private string CommitId => _configuration[CommitKey];

        /// <summary>
        /// Get server version information
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("version")]
        public virtual IActionResult GetServerVersionInfo()
        {
            ProductVersionInfo info = new ProductVersionInfo();
            info.ApplicationVersions.Add(GetApplicationVersionInfo());
            info.DatabaseVersions.Add(GetDatabaseVersionInfo());
            return Ok(info);
        }

        /// <summary>
        /// Get server version
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("server/version")]
        public virtual IActionResult GetServerVersion()
        {
            return Ok(GetApplicationVersionInfo());
        }

        /// <summary>
        /// Get database version
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("database/version")]
        public virtual IActionResult GetDatabaseVersion()
        {
            return Ok(GetDatabaseVersionInfo());
        }

        private ApplicationVersionInfo GetApplicationVersionInfo()
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            return assembly.GetApplicationVersionInfo(CommitId);
        }

        private DatabaseVersionInfo GetDatabaseVersionInfo()
        {
            return _context.Database.GetDatabaseVersionInfo();
        }
    }
}
