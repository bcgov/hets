using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using HETSAPI.Models;
using HETSCommon;

namespace HETSAPI.Controllers
{
    [Authorize]
    [Route("api")]
    public class VersionController : Controller
    {
        // Hack in the git commit id.
        private const string _commitKey = "OPENSHIFT_BUILD_COMMIT";

        private readonly DbContext _context;
        private readonly IConfiguration _configuration;

        public VersionController(IConfiguration configuration, DbAppContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        private string CommitId
        {
            get
            {
                return _configuration[_commitKey];
            }
        }

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

        [AllowAnonymous]
        [HttpGet]
        [Route("server/version")]
        public virtual IActionResult GetServerVersion()
        {
            return Ok(GetApplicationVersionInfo());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("database/version")]
        public virtual IActionResult GetDatabaseVersion()
        {
            return Ok(GetDatabaseVersionInfo());
        }

        private ApplicationVersionInfo GetApplicationVersionInfo()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            return assembly.GetApplicationVersionInfo(this.CommitId);
        }

        private DatabaseVersionInfo GetDatabaseVersionInfo()
        {
            return _context.Database.GetDatabaseVersionInfo();
        }
    }
}
