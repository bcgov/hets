using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Current User Controller
    /// </summary>
    [Route("api/users/current")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CurrentUserController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CurrentUserController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CurrentUserController(DbAppContext context, IConfiguration configuration, IMapper mapper, ILogger<CurrentUserController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _env = env;
            _mapper = mapper;
        }

        /// <summary>
        /// Get favourites for a user (by type)
        /// </summary>
        /// <remarks>Returns a users favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouriteType">type of favourite to return</param>
        [HttpGet]
        [Route("favourites/{favouriteType?}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<UserFavouriteDto>> UsersCurrentFavouritesFavouriteTypeGet([FromRoute] string favouriteType)
        {
            // get the current user id
            string userId = _context.SmUserId;

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get favourites
            IEnumerable<HetUserFavourite> favourites = _context.HetUserFavourites.AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.User.SmUserId.ToUpper() == userId &&
                            x.DistrictId == districtId);

            if (favouriteType != null)
            {
                favourites = favourites.Where(x => x.Type == favouriteType);
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserFavouriteDto>>(favourites)));
        }

        /// <summary>
        /// Delete user favourite
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        [HttpPost]
        [Route("favourites/{id}/delete")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<UserFavouriteDto> UsersCurrentFavouritesIdDeletePost([FromRoute] int id)
        {
            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetUserFavourites
                .Where(x => x.User.SmUserId.ToUpper() == userId)
                .Any(a => a.UserFavouriteId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // delete favourite
            HetUserFavourite item = _context.HetUserFavourites.First(a => a.UserFavouriteId == id);

            _context.HetUserFavourites.Remove(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<UserFavouriteDto>(item)));
        }

        /// <summary>
        /// Create user favourite
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        [HttpPost]
        [Route("favourites")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<UserFavouriteDto> UsersCurrentFavouritesPost([FromBody] UserFavouriteDto item)
        {
            return UpdateFavourite(item);
        }

        /// <summary>
        /// Update a user favourite
        /// </summary>
        /// <remarks>Updates a favourite</remarks>
        /// <param name="item"></param>
        [HttpPut]
        [Route("favourites")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<UserFavouriteDto> UsersCurrentFavouritesPut([FromBody] UserFavouriteDto item)
        {
            return UpdateFavourite(item);
        }

        /// <summary>
        /// Get current logged on user
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<CurrentUserDto> UsersCurrentGet()
        {
            _logger.LogDebug("Get Current User");

            // get the current user id
            string businessGuid = _context.SmBusinessGuid;
            string userId = _context.SmUserId;

            _logger.LogDebug("User Id: {0}", userId);
            _logger.LogDebug("Business Guid: {0}", businessGuid);

            // not found - return an HTTP 401 error response
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(businessGuid)) return StatusCode(401);

            CurrentUserDto user = new CurrentUserDto();

            if (string.IsNullOrEmpty(businessGuid))
            {
                HetUser currentUser = _context.HetUsers
                    .Include(x => x.District)
                    .Include(x => x.HetUserRoles)
                        .ThenInclude(y => y.Role)
                            .ThenInclude(z => z.HetRolePermissions)
                                .ThenInclude(z => z.Permission)
                    .First(x => x.SmUserId.ToUpper() == userId);

                // remove inactive roles
                for (int i = currentUser.HetUserRoles.Count - 1; i >= 0; i--)
                {
                    if (currentUser.HetUserRoles.ElementAt(i).EffectiveDate > DateTime.UtcNow ||
                        (currentUser.HetUserRoles.ElementAt(i).ExpiryDate != null &&
                         currentUser.HetUserRoles.ElementAt(i).ExpiryDate < DateTime.UtcNow))
                    {
                        currentUser.HetUserRoles.Remove(currentUser.HetUserRoles.ElementAt(i));
                    }
                }

                user.Id = currentUser.UserId;
                user.SmUserId = currentUser.SmUserId;
                user.GivenName = currentUser.GivenName;
                user.Surname = currentUser.Surname;
                user.DisplayName = currentUser.GivenName + " " + currentUser.Surname;
                user.UserGuid = currentUser.Guid;
                user.BusinessUser = false;
                user.District = _mapper.Map<DistrictDto>(currentUser.District);
                user.UserDistricts = _mapper.Map<List<UserDistrictDto>>(currentUser.HetUserDistricts);
                user.UserRoles = _mapper.Map<List<UserRoleDto>>(currentUser.HetUserRoles);
                user.SmAuthorizationDirectory = currentUser.SmAuthorizationDirectory;

                // set environment
                user.Environment = "Development";

                if (_env.IsProduction())
                {
                    user.Environment = "Production";
                }
                else if (_env.IsStaging())
                {
                    user.Environment = "Test";
                }
                else if (_env.IsEnvironment("Training"))
                {
                    user.Environment = "Training";
                }
                else if (_env.IsEnvironment("UAT"))
                {
                    user.Environment = "UAT";
                }
            }
            else
            {
                HetBusinessUser tmpUser = _context.HetBusinessUsers.AsNoTracking()
                    .Include(x => x.HetBusinessUserRoles)
                        .ThenInclude(y => y.Role)
                            .ThenInclude(z => z.HetRolePermissions)
                                .ThenInclude(z => z.Permission)
                    .FirstOrDefault(x => x.BceidUserId.ToUpper() == userId);

                if (tmpUser != null)
                {
                    // get business
                    HetBusiness business = _context.HetBusinesses.AsNoTracking()
                        .First(x => x.BusinessId == tmpUser.BusinessId);

                    user.Id = tmpUser.BusinessUserId;
                    user.SmUserId = tmpUser.BceidUserId;
                    user.GivenName = tmpUser.BceidFirstName;
                    user.Surname = tmpUser.BceidLastName;
                    user.DisplayName = tmpUser.BceidDisplayName;
                    user.UserGuid = tmpUser.BceidGuid;
                    user.BusinessUser = true;
                    user.BusinessId = tmpUser.BusinessId;
                    user.BusinessGuid = business.BceidBusinessGuid;
                    user.SmAuthorizationDirectory = "BCeID";

                    int id = 0;

                    foreach (HetBusinessUserRole role in tmpUser.HetBusinessUserRoles)
                    {
                        id++;

                        HetUserRole userRole = new HetUserRole
                        {
                            UserRoleId = id,
                            UserId = role.BusinessUserId,
                            RoleId = role.RoleId,
                            Role = role.Role
                        };

                        if (user.UserRoles == null)
                        {
                            user.UserRoles = new List<UserRoleDto>();
                        }

                        user.UserRoles.Add(_mapper.Map<UserRoleDto>(userRole));
                    }
                }
            }

            return new ObjectResult(new HetsResponse(user));
        }

        /// <summary>
        /// Logout User - Switch back to Primary
        /// </summary>
        [HttpGet]
        [Route("logoff")]
        [AllowAnonymous]
        public virtual ActionResult<LogoffModel> UsersCurrentLogoffPost()
        {
            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get user district
            HetUserDistrict userDistrict = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .FirstOrDefault(x => x.User.SmUserId.ToUpper() == userId &&
                                     x.IsPrimary);

            // if we don't find a primary - look for the first one in the list
            if (userDistrict == null)
            {
                userDistrict = _context.HetUserDistricts.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.User.SmUserId.ToUpper() == userId);
            }

            // update the current district for the user
            if (userDistrict != null)
            {
                HetUser user = _context.HetUsers.First(a => a.SmUserId.ToUpper() == userId);
                user.DistrictId = userDistrict.District.DistrictId;

                _context.SaveChanges();
            }

            // get the correct logoff url and return
            string logoffUrl = _configuration.GetSection("Constants:LogoffUrl-Development").Value;

            if (_env.IsProduction())
            {
                logoffUrl = _configuration.GetSection("Constants:LogoffUrl-Production").Value;
            }
            else if (_env.IsStaging())
            {
                logoffUrl = _configuration.GetSection("Constants:LogoffUrl-Test").Value;
            }
            else if (_env.IsEnvironment("Training"))
            {
                logoffUrl = _configuration.GetSection("Constants:LogoffUrl-Training").Value;
            }
            else if (_env.IsEnvironment("UAT"))
            {
                logoffUrl = _configuration.GetSection("Constants:LogoffUrl-UAT").Value;
            }

            LogoffModel response = new LogoffModel { LogoffUrl = logoffUrl };

            return new ObjectResult(new HetsResponse(response));
        }

        #region Update Favourite

        private ActionResult<UserFavouriteDto> UpdateFavourite(UserFavouriteDto item)
        {
            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get user record
            bool userExists = _context.HetUsers.Any(a => a.SmUserId.ToUpper() == userId);

            if (!userExists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetUser user = _context.HetUsers.AsNoTracking()
                .First(a => a.SmUserId.ToUpper() == userId);

            // get favourites
            bool exists = _context.HetUserFavourites.Any(a => a.UserFavouriteId == item.UserFavouriteId);

            HetUserFavourite favourite;

            if (exists)
            {
                favourite = _context.HetUserFavourites
                    .First(a => a.UserFavouriteId == item.UserFavouriteId);

                favourite.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                favourite.Value = item.Value;
                favourite.Name = item.Name;
                favourite.IsDefault = item.IsDefault;
            }
            else
            {
                favourite = new HetUserFavourite
                {
                    UserId = user.UserId,
                    DistrictId = districtId,
                    Type = item.Type,
                    Value = item.Value,
                    Name = item.Name,
                    IsDefault = item.IsDefault
                };

                _context.HetUserFavourites.Add(favourite);
            }

            // save the changes
            _context.SaveChanges();

            int favouriteId = favourite.UserFavouriteId;

            // get record and return
            favourite = _context.HetUserFavourites.AsNoTracking()
                .First(x => x.UserFavouriteId == favouriteId);

            return new ObjectResult(new HetsResponse(_mapper.Map<UserFavouriteDto>(favourite)));
        }

        #endregion
    }
}
