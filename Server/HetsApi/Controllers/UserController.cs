using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    /// User Controller
    /// </summary>
    [Route("api/users")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public UserController(DbAppContext context, IConfiguration configuration, IMapper mapper, IUserRepository userRepo)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<UserDto>> UsersGet()
        {
            // get all user records and return to UI
            return new ObjectResult(new HetsResponse(_userRepo.GetRecords()));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<UserDto> UsersIdGet([FromRoute] int id)
        {
            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get user record and return to UI
            return new ObjectResult(new HetsResponse(_userRepo.GetRecord(id)));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">id of User to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<UserDto> UsersIdDeletePost([FromRoute] int id)
        {
            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = _context.HetUsers.AsNoTracking()
                .Include(x => x.HetUserRoles)
                .Include(x => x.HetUserDistricts)
                .Include(x => x.HetUserFavourites)
                .First(x => x.UserId == id);

            // delete user roles
            foreach (HetUserRole item in user.HetUserRoles)
            {
                _context.HetUserRoles.Remove(item);
            }

            // delete user districts
            foreach (HetUserDistrict item in user.HetUserDistricts)
            {
                _context.HetUserDistricts.Remove(item);
            }

            // delete user favourites
            foreach (var item in user.HetUserFavourites)
            {
                _context.HetUserFavourites.Remove(item);
            }

            // delete user
            _context.HetUsers.Remove(user);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<UserDto>(user)));
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<UserDto> UsersPost([FromBody] UserDto item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // validate that user id is unique
            // HETS-1033 - Post Live: Add validation on User ID while adding a new user
            item.SmUserId = item.SmUserId?.Trim().ToUpper();

            HetUser existingUser = _context.HetUsers.AsNoTracking()
                .FirstOrDefault(x => x.SmUserId.ToUpper() == item.SmUserId);

            if (existingUser != null) return new BadRequestObjectResult(new HetsResponse("HETS-38", ErrorViewModel.GetDescription("HETS-38", _configuration)));

            // add new user
            HetUser user = new HetUser
            {
                Active = item.Active,
                Email = item.Email?.Trim(),
                GivenName = item.GivenName?.Trim(),
                Surname = item.Surname?.Trim(),
                SmUserId = item.SmUserId,
                DistrictId = item.District.DistrictId,
                AgreementCity = item.AgreementCity
            };

            HetUserDistrict newUserDistrict = new HetUserDistrict
            {
                DistrictId = item.District.DistrictId,
                IsPrimary = true
            };

            user.HetUserDistricts.Add(newUserDistrict);

            _context.HetUsers.Add(user);
            _context.SaveChanges();

            int id = user.UserId;

            // get updated user record and return to UI
            return new ObjectResult(new HetsResponse(_userRepo.GetRecord(id)));
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<UserDto> UsersIdPut([FromRoute] int id, [FromBody] UserDto item)
        {
            if (item == null || id != item.UserId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = _context.HetUsers
                .Include(x => x.District)
                .Include(x => x.HetUserDistricts)
                .First(x => x.UserId == id);

            // validate that user id is unique
            // HETS-1033 - Post Live: Add validation on User ID while editing a user
            string smUserId = item.SmUserId?.Trim().ToUpper();

            HetUser existingUser = _context.HetUsers.AsNoTracking()
                .FirstOrDefault(x => x.SmUserId.ToUpper() == smUserId && x.UserId != user.UserId);

            if (existingUser != null) return new BadRequestObjectResult(new HetsResponse("HETS-38", ErrorViewModel.GetDescription("HETS-38", _configuration)));

            user.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            user.Active = item.Active;
            user.Email = item.Email;
            user.GivenName = item.GivenName;
            user.Surname = item.Surname;
            user.SmUserId = item.SmUserId;
            user.AgreementCity = item.AgreementCity;

            if (item.District != null)
            {
                bool districtExists = _context.HetDistricts.Any(x => x.DistrictId == item.District.DistrictId);

                if (districtExists)
                {
                    HetDistrict district = _context.HetDistricts
                        .Include(x => x.Region)
                        .First(x => x.DistrictId == item.District.DistrictId);

                    user.DistrictId = district.DistrictId;

                    // check if we need to add this to the User District List too
                    bool userDistrictExists = false;

                    foreach (HetUserDistrict userDistrict in user.HetUserDistricts)
                    {
                        if (userDistrict.DistrictId == item.District.DistrictId)
                        {
                            userDistrictExists = true;
                            break;
                        }
                    }

                    // if not found - then add it!
                    if (!userDistrictExists)
                    {
                        HetUserDistrict newUserDistrict = new HetUserDistrict
                        {
                            UserId = item.UserId,
                            DistrictId = district.DistrictId
                        };

                        if (user.HetUserDistricts == null)
                        {
                            user.HetUserDistricts = new List<HetUserDistrict>();
                            newUserDistrict.IsPrimary = true;
                        }

                        user.HetUserDistricts.Add(newUserDistrict);
                    }
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated user record and return to UI
            return new ObjectResult(new HetsResponse(_userRepo.GetRecord(id)));
        }

        #region User Search

        /// <summary>
        /// Search for users
        /// </summary>
        /// <remarks>Used to search users.</remarks>
        /// <param name="districts">Districts (comma separated list of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        [HttpGet]
        [Route("search")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<List<UserDto>> UsersSearchGet([FromQuery] string districts, [FromQuery] string surname, [FromQuery] bool? includeInactive)
        {
            int?[] districtArray = ArrayHelper.ParseIntArray(districts);

            IQueryable<HetUser> data = _context.HetUsers
                .Include(x => x.District)
                .Select(x => x);

            if (districtArray != null && districtArray.Length > 0)
            {
                data = data.Where(x => districtArray.Contains(x.District.DistrictId));
            }

            if (surname != null)
            {
                data = data.Where(x => x.Surname.ToLower().Contains(surname.ToLower()));
            }

            if (includeInactive == null || includeInactive == false)
            {
                data = data.Where(x => x.Active);
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDto>>(data)));
        }

        #endregion

        #region User Favourites

        /// <summary>
        /// Get user favorites
        /// </summary>
        /// <remarks>Returns a use favourites</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        [HttpGet]
        [Route("{id}/favourites")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<List<UserFavouriteDto>> UsersIdFavouritesGet([FromRoute] int id)
        {
            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get favourites records
            List<HetUserFavourite> favourites = _context.HetUserFavourites.AsNoTracking()
                .Where(x => x.User.UserId == id)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserFavouriteDto>>(favourites)));
        }

        #endregion

        #region User Permissions

        /// <summary>
        /// Get permissions for a user
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        [HttpGet]
        [Route("{id}/permissions")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<List<PermissionDto>> UsersIdPermissionsGet([FromRoute] int id)
        {
            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            var user = _userRepo.GetRecord(id);

            var permissions = new List<PermissionDto>();

            foreach (var item in user.UserRoles)
            {
                if (item.Role?.RolePermissions != null)
                {
                    foreach (var permission in item.Role.RolePermissions)
                    {
                        permissions.Add(permission.Permission);
                    }
                }
            }

            return new ObjectResult(new HetsResponse(permissions));
        }

        #endregion

        #region User Roles

        /// <summary>
        /// Get all roles for a user
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        [HttpGet]
        [Route("{id}/roles")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<List<UserRoleDto>> UsersIdRolesGet([FromRoute] int id)
        {
            bool exists = _context.HetUsers.Any(x => x.UserId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            var user = _userRepo.GetRecord(id);

            // return user roles
            return new ObjectResult(new HetsResponse(user.UserRoles));
        }

        /// <summary>
        /// Adds a role to a user
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/roles")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<List<UserRoleDto>> UsersIdRolesPost([FromRoute] int id, [FromBody] UserRoleDto item)
        {
            //check for user
            bool exists = _context.HetUsers.Any(x => x.UserId == id);
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // check the role id
            bool roleExists = _context.HetRoles.Any(x => x.RoleId == item.RoleId);
            if (!roleExists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // check the user exists and return only active roles
            var user = _userRepo.GetRecord(id, excludeInactiveRoles: true);
            if (user == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            //check if user already has the same role active
            bool activeRoleExists = user.UserRoles.Any(x => x.RoleId == item.RoleId);
            if (activeRoleExists) return new NotFoundObjectResult(new HetsResponse("HETS-45", ErrorViewModel.GetDescription("HETS-45", _configuration)));


            // create a new UserRole record
            HetUserRole userRole = new HetUserRole
            {
                RoleId = item.RoleId,
                UserId = id,
                EffectiveDate = item.EffectiveDate,
                ExpiryDate = item.ExpiryDate
            };

            _context.HetUserRoles.Add(userRole);

            _context.SaveChanges();

            // return updated roles
            user = _userRepo.GetRecord(id);

            // return user roles
            return new ObjectResult(new HetsResponse(user.UserRoles));
        }

        /// <summary>
        /// Expire roles for a user
        /// </summary>
        /// <remarks>Updates the roles' Effective Date for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        [HttpPut]
        [Route("{id}/roles")]
        [RequiresPermission(HetPermission.UserManagement, HetPermission.WriteAccess)]
        public virtual ActionResult<List<UserRoleDto>> UsersIdRolesPut([FromRoute] int id, [FromBody] UserRoleDto[] items)
        {

            //confirm user exists
            bool userExists = _context.HetUsers.Any(x => x.UserId == id);
            if (!userExists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            //confirm all items belong to the user, using UserRoleId
            //confirm all items exist in database
            foreach (var item in items)
            {
                var result = _context.HetUserRoles.FirstOrDefault(x => x.UserRoleId == item.UserRoleId && x.UserId == id);
                if (result == null) return new NotFoundObjectResult(new HetsResponse("HETS-44", ErrorViewModel.GetDescription("HETS-44", _configuration)));
            }

            // get record
            var user = _userRepo.GetRecord(id);
            if (user.UserRoles == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // iterate the roles and update effective date
            foreach (var item in items)
            {
                HetUserRole role = _context.HetUserRoles.First(x => x.UserRoleId == item.UserRoleId);

                if (role.ExpiryDate != item.ExpiryDate)
                {
                    role.ExpiryDate = item.ExpiryDate;
                }
            }
            _context.SaveChanges();
            // return updated roles
            user = _userRepo.GetRecord(id);

            // return user roles
            return new ObjectResult(new HetsResponse(user.UserRoles));
        }

        #endregion

        #region User Districts

        /// <summary>
        /// Get user districts
        /// </summary>
        /// <remarks>Returns a users districts</remarks>
        /// <param name="id">id of User to fetch districts for</param>
        [HttpGet]
        [Route("{id}/districts")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual ActionResult<List<UserDistrictDto>> UsersIdDistrictsGet([FromRoute] int id)
        {
            List<HetUserDistrict> districts = _context.HetUserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.UserId == id)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<UserDistrictDto>>(districts)));
        }

        #endregion
    }
}
