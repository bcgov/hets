using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Service Areas Controller
    /// </summary>
    [Route("api/serviceAreas")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ServiceAreaController : Controller
    {
        private readonly DbAppContext _context;

        public ServiceAreaController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;  
            
            // set context data
            HetUser user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all service areas
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("ServiceAreasGet")]
        [SwaggerResponse(200, type: typeof(List<HetServiceArea>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ServiceAreasGet()
        {
            List<HetServiceArea> serviceAreas = _context.HetServiceArea
                .Include(x => x.District.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(serviceAreas));
        }
    }
}
