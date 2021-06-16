using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
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
        private readonly HttpContext _httpContext;
        private readonly IWebHostEnvironment _env;

        public BusinessController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnv)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _env = hostingEnv;
        }

        #region Get Business for the logged on business user

        /// <summary>
        /// Get business (for BCeID Business User)
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("BceidBusinessGet")]
        [SwaggerResponse(200, type: typeof(HetBusiness))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BceidBusinessGet()
        {
            string businessGuid = _context.SmBusinessGuid;

            if (businessGuid == null) return new NotFoundObjectResult(new HetsResponse(""));

            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea.District)
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return new NotFoundObjectResult(new HetsResponse(""));

            return new ObjectResult(new HetsResponse(business));
        }

        #endregion

        #region Validate a shared Key and link the owner to business records

        /// <summary>
        /// Validate owner shared key - link to business
        /// </summary>
        /// <param name="sharedKey"></param>
        /// <param name="postalCode"></param>
        [HttpGet]
        [Route("validateOwner")]
        [SwaggerOperation("BceidValidateOwner")]
        [SwaggerResponse(200, type: typeof(HetBusiness))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BceidValidateOwner([FromQuery]string sharedKey, [FromQuery]string postalCode)
        {
            string businessGuid = _context.SmBusinessGuid;

            if (string.IsNullOrEmpty(sharedKey))
            {
                // shared key not provided
                return new BadRequestObjectResult(new HetsResponse("HETS-19", ErrorViewModel.GetDescription("HETS-19", _configuration)));
            }

            if (string.IsNullOrEmpty(postalCode))
            {
                // postal code not provided
                return new BadRequestObjectResult(new HetsResponse("HETS-22", ErrorViewModel.GetDescription("HETS-22", _configuration)));
            }

            bool exists = _context.HetBusiness.Any(a => a.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get business
            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .First(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            // find owner using shred key & postal code (exact match)
            HetOwner owner = _context.HetOwner
                .Include(a => a.Business)
                .FirstOrDefault(a => a.SharedKey.Equals(sharedKey) &&
                                     a.PostalCode.Replace(" ", "").ToLower().Equals(postalCode.Replace(" ", "").ToLower(), StringComparison.InvariantCultureIgnoreCase));

            // validate the key
            if (owner == null)
            {
                // shared key not found
                return new BadRequestObjectResult(new HetsResponse("HETS-20", ErrorViewModel.GetDescription("HETS-20", _configuration)));
            }

            if (owner.BusinessId != null)
            {
                // shared key already used
                return new BadRequestObjectResult(new HetsResponse("HETS-21", ErrorViewModel.GetDescription("HETS-21", _configuration)));
            }

            // update owner
            int ownerId = owner.OwnerId;
            owner.BusinessId = business.BusinessId;
            owner.SharedKey = null;
            _context.SaveChanges();

            // get updated business record and return to the UI
            business = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.Business)
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District)
                .FirstOrDefault(a => a.BusinessId == business.BusinessId);

            // get updated owner record (linked owner) and return to the UI too
            if (business != null)
            {
                business.LinkedOwner = _context.HetOwner.AsNoTracking()
                    .FirstOrDefault(x => x.OwnerId == ownerId);
            }

            return new ObjectResult(new HetsResponse(business));
        }

        #endregion

        #region Get Owner and Equipment Data by Business

        /// <summary>
        /// Get all owners for a business
        /// </summary>
        [HttpGet]
        [Route("owners")]
        [SwaggerOperation("BceidOwnersGet")]
        [SwaggerResponse(200, type: typeof(List<HetOwner>))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BceidOwnersGet()
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessBusiness(business.BusinessId)) return StatusCode(StatusCodes.Status401Unauthorized);

            // get business
            HetBusiness businessDetail = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.Business)
                .Include(x => x.HetOwner)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District)
                .FirstOrDefault(a => a.BusinessId == business.BusinessId);

            return new ObjectResult(new HetsResponse(businessDetail));
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        [HttpGet]
        [Route("owner/{id}")]
        [SwaggerOperation("BceidOwnerIdGet")]
        [SwaggerResponse(200, type: typeof(HetOwner))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BceidOwnerIdGet([FromRoute]int id)
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessOwner(business.BusinessId, id)) return StatusCode(StatusCodes.Status401Unauthorized);

            return new ObjectResult(new HetsResponse(OwnerHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Get equipment associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        [HttpGet]
        [Route("owner/{id}/equipment")]
        [SwaggerOperation("BceidOwnerEquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<HetEquipment>))]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual IActionResult BceidOwnerEquipmentGet([FromRoute]int id)
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessOwner(business.BusinessId, id)) return StatusCode(StatusCodes.Status401Unauthorized);

            // retrieve the data and return
            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.DistrictEquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetEquipmentAttachment)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetNote)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetDigitalFile)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetHistory)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(owner.HetEquipment));
        }

        #endregion

        #region Check access to Business and Owner

        private bool CanAccessOwner(int businessId, int ownerId)
        {
            // validate that the current user can access this record
            string userId = _context.SmUserId;
            bool isBusiness = UserAccountHelper.IsBusiness(_httpContext);

            // not a business user
            if (string.IsNullOrEmpty(userId) || !isBusiness) return false;

            // get business & owner record
            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.Business)
                    .ThenInclude(x => x.HetBusinessUser)
                .FirstOrDefault(x => x.BusinessId == businessId &&
                                     x.OwnerId == ownerId);

            // get user
            HetBusinessUser user = owner?.Business?.HetBusinessUser
                .FirstOrDefault(x => x.BceidUserId.ToUpper() == userId);

            // no access to business or business doesn't exist
            return user != null;
        }

        private bool CanAccessBusiness(int businessId)
        {
            // validate that the current user can access this record
            string userId = _context.SmUserId;
            bool isBusiness = UserAccountHelper.IsBusiness(_httpContext);

            // not a business user
            if (string.IsNullOrEmpty(userId) || !isBusiness) return false;

            // get business record
            HetBusiness business = _context.HetBusiness.AsNoTracking()
                .Include(x => x.HetBusinessUser)
                .FirstOrDefault(x => x.BusinessId == businessId);

            // get user
            HetBusinessUser user = business?.HetBusinessUser
                .FirstOrDefault(x => x.BceidUserId.ToUpper() == userId);

            // no access to business or business doesn't exist
            return user != null;
        }

        #endregion
    }
}
