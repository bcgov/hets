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

        /// <summary>
        /// Validate owner shared key - link to business
        /// </summary>
        /// <param name="id">id of Business to fetch</param>
        /// <param name="sharedKey"></param>
        [HttpGet]
        [Route("{id}/validateOwner")]
        [SwaggerOperation("BusinessIdValidateOwner")]
        [SwaggerResponse(200, type: typeof(HetBusiness))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BusinessIdValidateOwner([FromRoute]int id, [FromQuery]string sharedKey)
        {
            if (string.IsNullOrEmpty(sharedKey))
            {
                // shared key not provided
                return new ObjectResult(new HetsResponse("HETS-19", ErrorViewModel.GetDescription("HETS-19", _configuration)));
            }

            bool exists = _context.HetBusiness.Any(a => a.BusinessId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // find owner using shred key (exact match)
            HetOwner owner = _context.HetOwner.FirstOrDefault(a => a.SharedKey.Equals(sharedKey));

            // validate the key
            if (owner == null)
            {
                // shared key not found
                return new ObjectResult(new HetsResponse("HETS-20", ErrorViewModel.GetDescription("HETS-20", _configuration)));
            }

            if (owner.BusinessId != null && owner.BusinessId != id)
            {
                // shared key already used
                return new ObjectResult(new HetsResponse("HETS-21", ErrorViewModel.GetDescription("HETS-21", _configuration)));
            }

            // update owner
            owner.BusinessId = id;
            _context.SaveChanges();

            // get updated business record and return to the UI
            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetOwner)
                .FirstOrDefault(a => a.BusinessId == id);

            return new ObjectResult(new HetsResponse(business));
        }
    }
}
