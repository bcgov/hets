using System;
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
using Microsoft.AspNetCore.Authorization;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Current User Controller
    /// </summary>
    [Route("api/users/current")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CurrentUserController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public CurrentUserController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Get favourites for a user (by type)
        /// </summary>
        /// <remarks>Returns a users favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouriteType">type of favourite to return</param>
        [HttpGet]
        [Route("favourites/{favouriteType}")]
        [SwaggerOperation("UsersCurrentFavouritesFavouriteTypeGet")]
        [SwaggerResponse(200, type: typeof(List<HetUserFavourite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UsersCurrentFavouritesFavouriteTypeGet([FromRoute]string favouriteType)
        {
            // get the current user id
            string userId = _context.SmUserId;
            
            // get favourites
            IEnumerable<HetUserFavourite> favourites = _context.HetUserFavourite.AsNoTracking()
                .Where(x => x.User.SmUserId == userId)
                .Select(x => x)
                .ToList();

            if (favouriteType != null)
            {
                favourites = favourites.Where(x => x.Type == favouriteType);
            }

            return new ObjectResult(new HetsResponse(favourites));
        }

        /// <summary>
        /// Delete user favourite
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        [HttpPost]
        [Route("favourites/{id}/delete")]
        [SwaggerOperation("UsersCurrentFavouritesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetUserFavourite))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UsersCurrentFavouritesIdDeletePost([FromRoute]int id)
        {
            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetUserFavourite
                .Where(x => x.User.SmUserId == userId)
                .Any(a => a.UserFavouriteId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // delete favourite
            HetUserFavourite item = _context.HetUserFavourite.First(a => a.UserFavouriteId == id);

            _context.HetUserFavourite.Remove(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Create user favourite
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        [HttpPost]
        [Route("favourites")]
        [SwaggerOperation("UsersCurrentFavouritesPost")]
        [SwaggerResponse(200, type: typeof(HetUserFavourite))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UsersCurrentFavouritesPost([FromBody]HetUserFavourite item)
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
        [SwaggerOperation("UsersCurrentFavouritesPut")]
        [SwaggerResponse(200, type: typeof(HetUserFavourite))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UsersCurrentFavouritesPut([FromBody]HetUserFavourite item)
        {
            return UpdateFavourite(item);
        }

        /// <summary>
        /// Get current logged on user
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("UsersCurrentGet")]
        [SwaggerResponse(200, type: typeof(User))]
        [AllowAnonymous]
        public virtual IActionResult UsersCurrentGet()
        {
            // get the current user id
            string businessGuid = _context.SmBusinessGuid;
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            User user = new User();

            if (businessGuid == null)
            {
                HetUser currentUser = _context.HetUser
                    .Include(x => x.District)
                    .Include(x => x.HetUserRole)
                        .ThenInclude(y => y.Role)
                            .ThenInclude(z => z.HetRolePermission)
                                .ThenInclude(z => z.Permission)
                    .First(x => x.SmUserId == userId);

                // remove inactive roles
                for (int i = currentUser.HetUserRole.Count - 1; i >= 0; i--)
                {
                    if (currentUser.HetUserRole.ElementAt(i).EffectiveDate > DateTime.UtcNow ||
                        (currentUser.HetUserRole.ElementAt(i).ExpiryDate != null &&
                         currentUser.HetUserRole.ElementAt(i).ExpiryDate < DateTime.UtcNow))
                    {
                        currentUser.HetUserRole.Remove(currentUser.HetUserRole.ElementAt(i));
                    }
                }

                user.Id = currentUser.UserId;
                user.SmUserId = currentUser.SmUserId;
                user.GivenName = currentUser.GivenName;
                user.Surname = currentUser.Surname;
                user.DisplayName = currentUser.GivenName + " " + currentUser.Surname;
                user.UserGuid = currentUser.Guid;
                user.BusinessUser = false;
                user.District = currentUser.District;
                user.HetUserDistrict = currentUser.HetUserDistrict;
                user.HetUserRole = currentUser.HetUserRole;
                user.SmAuthorizationDirectory = currentUser.SmAuthorizationDirectory;
            }
            else
            {
                HetBusinessUser tmpUser = _context.HetBusinessUser.AsNoTracking()
                    .Include(x => x.HetBusinessUserRole)
                        .ThenInclude(y => y.Role)
                            .ThenInclude(z => z.HetRolePermission)
                                .ThenInclude(z => z.Permission)
                    .FirstOrDefault(x => x.BceidUserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase));

                if (tmpUser != null)
                {
                    // get business
                    HetBusiness business = _context.HetBusiness.AsNoTracking()
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

                    foreach (HetBusinessUserRole role in tmpUser.HetBusinessUserRole)
                    {
                        id++;

                        HetUserRole userRole = new HetUserRole
                        {
                            UserRoleId = id,
                            UserId = role.BusinessUserId,
                            RoleId = role.RoleId,
                            Role = role.Role
                        };

                        if (user.HetUserRole == null)
                        {
                            user.HetUserRole = new List<HetUserRole>();
                        }

                        user.HetUserRole.Add(userRole);
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
        [SwaggerOperation("UserDistrictsIdLogoffPost")]
        [SwaggerResponse(200, type: typeof(HetUser))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult UsersCurrentLogoffPost()
        {
            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get user district
            HetUserDistrict userDistrict = _context.HetUserDistrict.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .FirstOrDefault(x => x.User.SmUserId == userId &&
                                     x.IsPrimary);

            // if we don't find a primary - look for the first one in the list
            if (userDistrict == null)
            {
                userDistrict = _context.HetUserDistrict.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.User.SmUserId == userId);
            }

            // update the current district for the user
            HetUser user = null;

            if (userDistrict != null)
            {
                user = _context.HetUser.First(a => a.SmUserId == userId);
                user.DistrictId = userDistrict.District.DistrictId;

                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(user));
        }

        #region Update Favourite

        private IActionResult UpdateFavourite(HetUserFavourite item)
        {
            item.User = null;

            // get the current user id
            string userId = _context.SmUserId;

            // not found
            if (userId == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get user record
            bool userExists = _context.HetUser.Any(a => a.SmUserId == userId);

            if (userExists)
            {
                HetUser user = _context.HetUser.First(a => a.SmUserId == userId);
                item.User = user;
            }

            // get favourites
            bool exists = _context.HetUserFavourite.Any(a => a.UserFavouriteId == item.UserFavouriteId);

            if (exists)
            {
                _context.HetUserFavourite.Update(item);
            }
            else
            {
                _context.HetUserFavourite.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        #endregion
    }
}
