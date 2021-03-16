using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.AspNetCore.Authorization;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Permission Controller
    /// </summary>
    [Route("api/permissions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PermissionController : Controller
    {
        private readonly DbAppContext _context;

        public PermissionController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("PermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionLite>))]
        [AllowAnonymous]
        public virtual IActionResult PermissionsGet()
        {
            List<HetPermission> permissions = _context.HetPermission.ToList();

            // convert Permission Model to the "PermissionLite" Model
            List<PermissionLite> result = new List<PermissionLite>();

            foreach (HetPermission item in permissions)
            {
                result.Add(PermissionHelper.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
