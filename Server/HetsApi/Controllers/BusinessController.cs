using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;
using HetsData.Repositories;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Business Controller
    /// </summary>
    [Route("api/business")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BusinessController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepo;

        public BusinessController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, 
            IMapper mapper, IOwnerRepository ownerRepo)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _mapper = mapper;
            _ownerRepo = ownerRepo;
        }

        #region Get Business for the logged on business user

        /// <summary>
        /// Get business (for BCeID Business User)
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual ActionResult<BusinessDto> BceidBusinessGet()
        {
            string businessGuid = _context.SmBusinessGuid;

            if (businessGuid == null) return new NotFoundObjectResult(new HetsResponse(""));

            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea.District)
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return new NotFoundObjectResult(new HetsResponse(""));

            return new ObjectResult(new HetsResponse(_mapper.Map<BusinessDto>(business)));
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
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual ActionResult<BusinessDto> BceidValidateOwner([FromQuery]string sharedKey, [FromQuery]string postalCode)
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

            bool exists = _context.HetBusinesses.Any(a => a.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get business
            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .First(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            postalCode = postalCode.Replace(" ", "").ToLower();

            // find owner using shred key & postal code (exact match)
            HetOwner owner = _context.HetOwners
                .Include(a => a.Business)
                .Where(a => a.SharedKey == sharedKey)
                .ToList() //completes server evaluation before doing the client evaluation   
                .FirstOrDefault(a => a.PostalCode.Replace(" ", "").Equals(postalCode.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase));

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
            business = _context.HetBusinesses.AsNoTracking()
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District)
                .FirstOrDefault(a => a.BusinessId == business.BusinessId);

            var businessDto = _mapper.Map<BusinessDto>(business);

            businessDto.LinkedOwner = _mapper.Map<OwnerDto>(
                _context.HetOwners
                .AsNoTracking()
                .FirstOrDefault(x => x.OwnerId == ownerId)
            );

            return new ObjectResult(new HetsResponse(businessDto));
        }

        #endregion

        #region Get Owner and Equipment Data by Business

        /// <summary>
        /// Get all owners for a business
        /// </summary>
        [HttpGet]
        [Route("owners")]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual ActionResult<BusinessDto> BceidOwnersGet()
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessBusiness(business.BusinessId)) return StatusCode(StatusCodes.Status401Unauthorized);

            // get business
            HetBusiness businessDetail = _context.HetBusinesses.AsNoTracking()
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.HetOwners)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District)
                .FirstOrDefault(a => a.BusinessId == business.BusinessId);

            return new ObjectResult(new HetsResponse(_mapper.Map<BusinessDto>(businessDetail)));
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        [HttpGet]
        [Route("owner/{id}")]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual ActionResult<OwnerDto> BceidOwnerIdGet([FromRoute]int id)
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessOwner(business.BusinessId, id)) return StatusCode(StatusCodes.Status401Unauthorized);

            return new ObjectResult(new HetsResponse(_ownerRepo.GetRecord(id)));
        }

        /// <summary>
        /// Get equipment associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        [HttpGet]
        [Route("owner/{id}/equipment")]
        [RequiresPermission(HetPermission.BusinessLogin)]
        public virtual ActionResult<List<EquipmentDto>> BceidOwnerEquipmentGet([FromRoute]int id)
        {
            // get business
            string businessGuid = _context.SmBusinessGuid;

            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .FirstOrDefault(x => x.BceidBusinessGuid.ToLower().Trim() == businessGuid.ToLower().Trim());

            if (business == null) return StatusCode(StatusCodes.Status401Unauthorized);

            // check access
            if (!CanAccessOwner(business.BusinessId, id)) return StatusCode(StatusCodes.Status401Unauthorized);

            // retrieve the data and return
            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.DistrictEquipmentType)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetEquipmentAttachments)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetNotes)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetDigitalFiles)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetHistories)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<EquipmentDto>>(owner.HetEquipments)));
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
            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.Business)
                    .ThenInclude(x => x.HetBusinessUsers)
                .FirstOrDefault(x => x.BusinessId == businessId &&
                                     x.OwnerId == ownerId);

            // get user
            HetBusinessUser user = owner?.Business?.HetBusinessUsers
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
            HetBusiness business = _context.HetBusinesses.AsNoTracking()
                .Include(x => x.HetBusinessUsers)
                .FirstOrDefault(x => x.BusinessId == businessId);

            // get user
            HetBusinessUser user = business?.HetBusinessUsers
                .FirstOrDefault(x => x.BceidUserId.ToUpper() == userId);

            // no access to business or business doesn't exist
            return user != null;
        }

        #endregion
    }
}
