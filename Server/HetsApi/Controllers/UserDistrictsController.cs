using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// User District Controller
    /// </summary>
    [Route("api/userDistricts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserDistrictController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;

        public UserDistrictController(DbAppContext context, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all districts for the logged on user
        /// </summary>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<List<UserDistrictDto>> UserDistrictsGet()
        {
            // return for the current user only
            string userId = _context.SmUserId;

            List<HetUserDistrict> result = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.SmUserId.ToUpper() == userId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDistrictDto>>(result)));
        }

        /// <summary>
        /// Delete user district
        /// </summary>
        /// <param name="id">id of User District to delete</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<List<UserDistrictDto>> UserDistrictsIdDeletePost([FromRoute] int id)
        {
            bool exists = _context.HetUserDistricts.Any(a => a.UserDistrictId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUserDistrict item = _context.HetUserDistricts
                .Include(x => x.User)
                .First(a => a.UserDistrictId == id);

            // cannot delete record that is primary.
            if (item.IsPrimary) return new BadRequestObjectResult(new HetsResponse("HETS-47", ErrorViewModel.GetDescription("HETS-47", _configuration)));

            int userId = item.User.UserId;

            // remove record
            _context.HetUserDistricts.Remove(item);
            _context.SaveChanges();

            // return the updated user district records
            List<HetUserDistrict> result = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.UserId == userId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDistrictDto>>(result)));
        }

        /// <summary>
        /// Create a User District
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<List<UserDistrictDto>> UserDistrictsIdPost([FromRoute] int id, [FromBody] UserDistrictDto item)
        {
            // record not found
            if (id != item.UserDistrictId) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // district not provided
            if (item.DistrictId == null) return new BadRequestObjectResult(new HetsResponse("HETS-18", ErrorViewModel.GetDescription("HETS-18", _configuration)));

            // user not provided
            if (item.UserId == null) return new BadRequestObjectResult(new HetsResponse("HETS-17", ErrorViewModel.GetDescription("HETS-17", _configuration)));

            var user = _context.HetUsers.FirstOrDefault(a => a.UserId == item.UserId);
            //user does not exist 
            if (user == null) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            var district = _context.HetDistricts.FirstOrDefault(a => a.DistrictId == item.DistrictId);
            //district does not exist
            if (district == null) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get userDistricts
            List<HetUserDistrict> userDistricts = _context.HetUserDistricts
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.UserId == item.UserId)
                .ToList();

            bool districtExists = userDistricts.Exists(a => a.District.DistrictId == item.DistrictId);
            //User already has district being created
            if (districtExists) return new BadRequestObjectResult(new HetsResponse("HETS-46", ErrorViewModel.GetDescription("HETS-46", _configuration)));

            //manage primary attribute logic      
            bool hasPrimary = false;
            foreach (HetUserDistrict existingUserDistrict in userDistricts)
            {
                if (existingUserDistrict.IsPrimary)
                {
                    hasPrimary = true;
                    break;
                }
            }

            if (!hasPrimary) //if the list does not have a primary district we force the new district to be primary.
            {
                item.IsPrimary = true;
            }

            if (item.IsPrimary)
            {
                item.IsPrimary = true;

                foreach (HetUserDistrict existingUserDistrict in userDistricts)
                {
                    if (existingUserDistrict.IsPrimary)
                    {
                        existingUserDistrict.IsPrimary = false;
                        break;
                    }
                }
            }

            _context.HetUserDistricts.Add(_mapper.Map<HetUserDistrict>(item));

            _context.SaveChanges();

            // return the updated user district records
            List<HetUserDistrict> result = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.UserId == item.UserId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDistrictDto>>(result)));
        }

        /// <summary>
        /// Update a User District
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<List<UserDistrictDto>> UserDistrictsIdPut([FromRoute] int id, [FromBody] UserDistrictDto item)
        {
            // record not found
            if (id != item.UserDistrictId) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // district not provided
            if (item.DistrictId == null) return new BadRequestObjectResult(new HetsResponse("HETS-18", ErrorViewModel.GetDescription("HETS-18", _configuration)));

            // user not provided
            if (item.UserId == null) return new BadRequestObjectResult(new HetsResponse("HETS-17", ErrorViewModel.GetDescription("HETS-17", _configuration)));

            //user does not exist 
            var user = _context.HetUsers.FirstOrDefault(a => a.UserId == item.UserId);
            if (user == null) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            //district does not exist
            var district = _context.HetDistricts.FirstOrDefault(a => a.DistrictId == item.DistrictId);
            if (district == null) return new BadRequestObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get userDistricts
            List<HetUserDistrict> userDistricts = _context.HetUserDistricts
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.UserId == item.UserId)
                .ToList();

            int index = userDistricts.FindIndex(a => a.UserDistrictId == item.UserDistrictId);

            // userDistrict to update - error if not found
            if (index < 0) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            userDistricts.ElementAt(index).UserId = item.UserId;
            userDistricts.ElementAt(index).DistrictId = item.DistrictId;

            // manage the primary attribute
            bool hasPrimary = false;
            foreach (HetUserDistrict existingUserDistrict in userDistricts)
            {
                if (existingUserDistrict.IsPrimary)
                {
                    hasPrimary = true;
                    break;
                }
            }

            if (!hasPrimary) //if the list does not have a primary district we force the new district to be primary.
            {
                item.IsPrimary = true;
            }

            if (item.IsPrimary)
            {
                userDistricts.ElementAt(index).IsPrimary = true;

                foreach (HetUserDistrict existingUserDistrict in userDistricts)
                {
                    if (existingUserDistrict.IsPrimary &&
                        existingUserDistrict.UserDistrictId != item.UserDistrictId)
                    {
                        existingUserDistrict.IsPrimary = false;
                        break;
                    }
                }
            }

            _context.SaveChanges();

            // return the updated user district records
            List<HetUserDistrict> result = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.UserId == item.UserId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDistrictDto>>(result)));
        }

        /// <summary>
        /// Switch User District
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        [Route("{id}/switch")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<UserDto> UserDistrictsIdSwitchPost([FromRoute] int id)
        {
            bool exists = _context.HetUserDistricts.Any(a => a.UserDistrictId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUserDistrict userDistrict = _context.HetUserDistricts.First(a => a.UserDistrictId == id);

            string userId = _context.SmUserId;

            HetUser user = _context.HetUsers.First(a => a.SmUserId.ToUpper() == userId);
            user.DistrictId = userDistrict.DistrictId;

            _context.SaveChanges();

            // create new district switch cookie
            _httpContext.Response.Cookies.Append(
                "HETSDistrict",
                userDistrict.DistrictId.ToString(),
                new CookieOptions
                {
                    Path = "/",
                    Secure = true,
                    SameSite = SameSiteMode.None
                }
            );

            return new ObjectResult(new HetsResponse(_mapper.Map<UserDto>(user)));
        }
    }
}
