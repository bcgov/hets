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
            HetUser user = UserHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("PermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<PermissionLite>))]
        [RequiresPermission(HetPermission.Login)]
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
