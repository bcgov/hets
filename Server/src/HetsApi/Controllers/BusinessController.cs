using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Business Controller
    /// </summary>
    [Route("api/business")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BusinessController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public BusinessController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;    
            
            // set context data
            HetUser user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all businesses
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("BusinessesGet")]
        [SwaggerResponse(200, type: typeof(List<HetBusiness>))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BusinessesGet()
        {
            List<HetBusiness> businesses = _context.HetBusiness.AsNoTracking()
                .ToList();

            return new ObjectResult(new HetsResponse(businesses));
        }

        /// <summary>
        /// Get business by id
        /// </summary>
        /// <param name="id">id of Business to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("BusinessIdGet")]
        [SwaggerResponse(200, type: typeof(HetBusiness))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BusinessIdGet([FromRoute]int id)
        {
            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetOwner)               
                .FirstOrDefault(a => a.BusinessId == id);

            return new ObjectResult(new HetsResponse(business));
        }
    }
}
