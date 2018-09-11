using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Region Controller
    /// </summary>
    [Route("api/regions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RegionController : Controller
    {
        private readonly DbAppContext _context;

        public RegionController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            
            // set context data
            HetUser user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("RegionsGet")]
        [SwaggerResponse(200, type: typeof(List<HetRegion>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RegionsGet()
        {
            List<HetRegion> regions = _context.HetRegion.ToList();

            return new ObjectResult(new HetsResponse(regions));
        }
    }
}
