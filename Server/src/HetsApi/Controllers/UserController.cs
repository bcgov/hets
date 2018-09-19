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
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/users")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }
        
        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("UsersGet")]
        [SwaggerResponse(200, type: typeof(List<HetUser>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersGet()
        {
            // get all user records and return to UI          
            return new ObjectResult(new HetsResponse(UserHelper.GetRecords( _context)));            
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">id of User to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("UsersIdGet")]
        [SwaggerResponse(200, type: typeof(HetUser))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdGet([FromRoute]int id)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get user record and return to UI          
            return new ObjectResult(new HetsResponse(UserHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">id of User to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("UsersIdDeletePost")]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = _context.HetUser.AsNoTracking()
                .Include(x => x.HetUserRole)
                .Include(x => x.HetUserDistrict)
                .First(x => x.UserId == id);

            // delete user roles
            foreach (HetUserRole item in user.HetUserRole)
            {
                _context.HetUserRole.Remove(item);
            }            

            // delete user districts            
            foreach (HetUserDistrict item in user.HetUserDistrict)
            {
                _context.HetUserDistrict.Remove(item);
            }            

            // delete user
            _context.HetUser.Remove(user);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(user));
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("UsersPost")]
        [SwaggerResponse(200, type: typeof(HetUser))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersPost([FromBody]HetUser item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetUser user = new HetUser
            {
                Active = item.Active,
                Email = item.Email,
                GivenName = item.GivenName,
                Surname = item.Surname,
                District = item.District,
                SmUserId = item.SmUserId
            };

            HetUserDistrict newUserDistrict = new HetUserDistrict
            {
                UserId = item.UserId,
                DistrictId = item.District.DistrictId
            };

            if (user.HetUserDistrict == null)
            {
                user.HetUserDistrict = new List<HetUserDistrict>();
                newUserDistrict.IsPrimary = true;
            }

            user.HetUserDistrict.Add(newUserDistrict);

            // create or update user record
            bool exists = _context.HetUser.Any(x => x.UserId == user.UserId);

            if (exists)
            {
                _context.HetUser.Update(user);
            }
            else
            {
                _context.HetUser.Add(user);
            }

            _context.SaveChanges();

            int id = user.UserId;

            // get updated user record and return to UI          
            return new ObjectResult(new HetsResponse(UserHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("UsersIdPut")]
        [SwaggerResponse(200, type: typeof(HetUser))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdPut([FromRoute]int id, [FromBody]HetUser item)
        {
            if (item == null || id != item.UserId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = _context.HetUser
                .Include(x => x.District)
                .Include(x => x.HetUserDistrict)
                .First(x => x.UserId == id);

            user.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            user.Active = item.Active;
            user.Email = item.Email;
            user.GivenName = item.GivenName;
            user.Surname = item.Surname;
            user.SmUserId = item.SmUserId;

            if (item.District != null)
            {
                bool districtExists = _context.HetDistrict.Any(x => x.DistrictId == item.District.DistrictId);

                if (districtExists)
                {
                    HetDistrict district = _context.HetDistrict
                        .Include(x => x.Region)
                        .First(x => x.DistrictId == item.District.DistrictId);

                    user.DistrictId = district.DistrictId;

                    // check if we need to add this to the User District List too
                    bool userDistrictExists = false;

                    foreach (HetUserDistrict userDistrict in user.HetUserDistrict)
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

                        if (user.HetUserDistrict == null)
                        {
                            user.HetUserDistrict = new List<HetUserDistrict>();
                            newUserDistrict.IsPrimary = true;
                        }

                        user.HetUserDistrict.Add(newUserDistrict);
                    }
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated user record and return to UI          
            return new ObjectResult(new HetsResponse(UserHelper.GetRecord(id, _context)));
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
        [SwaggerOperation("UsersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<HetUser>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersSearchGet([FromQuery]string districts, [FromQuery]string surname, [FromQuery]bool? includeInactive)
        {
            int?[] districtArray = ArrayHelper.ParseIntArray(districts);

            IQueryable<HetUser> data = _context.HetUser
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

            return new ObjectResult(new HetsResponse(data));
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
        [SwaggerOperation("UsersIdFavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<HetUserFavourite>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdFavouritesGet([FromRoute]int id)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get favourites records
            List<HetUserFavourite> favourites = _context.HetUserFavourite.AsNoTracking()
                .Where(x => x.User.UserId == id)
                .ToList();

            return new ObjectResult(new HetsResponse(favourites));
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
        [SwaggerOperation("UsersIdPermissionsGet")]
        [SwaggerResponse(200, type: typeof(List<HetPermission>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdPermissionsGet([FromRoute]int id)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = UserHelper.GetRecord(id, _context);

            List<HetPermission> permissions = new List<HetPermission>();
            
            foreach (HetUserRole item in user.HetUserRole)
            {
                if (item.Role?.HetRolePermission != null)
                {
                    foreach (HetRolePermission permission in item.Role.HetRolePermission)
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
        [SwaggerOperation("UsersIdRolesGet")]
        [SwaggerResponse(200, type: typeof(List<HetUserRole>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdRolesGet([FromRoute]int id)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = UserHelper.GetRecord(id, _context);

            // return user roles
            return new ObjectResult(new HetsResponse(user.HetUserRole));
        }

        /// <summary>
        /// Adds a role to a user
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/roles")]
        [SwaggerOperation("UsersIdRolesPost")]
        [SwaggerResponse(200, type: typeof(List<HetUserRole>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdRolesPost([FromRoute]int id, [FromBody]HetUserRole item)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // check the role id
            bool roleExists = _context.HetRole.Any(x => x.RoleId == item.RoleId);

            // record not found
            if (!roleExists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // get record
            HetUser user = UserHelper.GetRecord(id, _context);

            if (user.HetUserRole == null)
            {
                user.HetUserRole = new List<HetUserRole>();
            }

            // create a new UserRole record
            HetUserRole userRole = new HetUserRole
            {
                RoleId = item.RoleId,
                EffectiveDate = item.EffectiveDate,
                ExpiryDate = item.ExpiryDate
            };

            if (!user.HetUserRole.Contains(userRole))
            {
                user.HetUserRole.Add(userRole);
            }

            _context.SaveChanges();

            // return updated roles
            user = UserHelper.GetRecord(id, _context);

            // return user roles
            return new ObjectResult(new HetsResponse(user.HetUserRole));
        }

        /// <summary>
        /// Add roles to a user
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        [HttpPut]
        [Route("{id}/roles")]
        [SwaggerOperation("UsersIdRolesPut")]
        [SwaggerResponse(200, type: typeof(List<HetUserRole>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdRolesPut([FromRoute]int id, [FromBody]HetUserRole[] items)
        {
            bool exists = _context.HetUser.Any(x => x.UserId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetUser user = UserHelper.GetRecord(id, _context);

            if (user.HetUserRole == null)
            {
                user.HetUserRole = new List<HetUserRole>();
            }
            else
            {
                // existing data - clear it
                foreach (HetUserRole userRole in user.HetUserRole)
                {
                    HetUserRole delete = _context.HetUserRole.First(x => x.UserRoleId == userRole.UserRoleId);
                    _context.Remove(delete);                    
                }

                user.HetUserRole.Clear();
            }

            foreach (HetUserRole item in items)
            {
                // check the role id
                bool roleExists = _context.HetRole.Any(x => x.RoleId == item.RoleId);

                if (roleExists)
                {
                    // create a new UserRole
                    HetUserRole userRole = new HetUserRole
                    {
                        RoleId = item.RoleId,
                        EffectiveDate = item.EffectiveDate,
                        ExpiryDate = item.ExpiryDate
                    };

                    if (!user.HetUserRole.Contains(userRole))
                    {
                        user.HetUserRole.Add(userRole);
                    }
                }
            }

            _context.SaveChanges();

            // return updated roles
            user = UserHelper.GetRecord(id, _context);

            // return user roles
            return new ObjectResult(new HetsResponse(user.HetUserRole));
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
        [SwaggerOperation("UsersIdDistrictsGet")]
        [SwaggerResponse(200, type: typeof(List<HetUserDistrict>))]
        [RequiresPermission(HetPermission.UserManagement)]
        public virtual IActionResult UsersIdDistrictsGet([FromRoute]int id)
        {
            List<HetUserDistrict> districts = _context.HetUserDistrict.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.UserId == id)
                .ToList();

            return new ObjectResult(new HetsResponse(districts));
        }

        #endregion
    }
}
