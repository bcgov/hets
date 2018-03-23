using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using System.Security.Claims;
using HETSAPI.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Mappings;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// Current User Service
    /// </summary>
    public class CurrentUserService : ServiceBase, ICurrentUserService
    {
        private readonly HttpContext _httpContext;
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Current User Service Constructor
        /// </summary>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Delete user favourites
        /// </summary>
        /// <remarks>Removes a specific user favourite</remarks>
        /// <param name="id">id of Favourite to delete</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersCurrentFavouritesIdDeletePostAsync(int id)
        {
            // get the current user id
            int? userId = GetCurrentUserId();

            if (userId != null)
            {
                bool exists = _context.UserFavourites.Where(x => x.User.Id == userId)
                    .Any(a => a.Id == id);

                if (exists)
                {
                    UserFavourite item = _context.UserFavourites.First(a => a.Id == id);

                    _context.UserFavourites.Remove(item);

                    // Save the changes
                    _context.SaveChanges();

                    return new ObjectResult(new HetsResponse(item));
                }

                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create favourites for a user
        /// </summary>
        /// <remarks>Create new favourite for the current user</remarks>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        public virtual IActionResult UsersCurrentFavouritesPostAsync(UserFavourite item)
        {
            item.User = null;

            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                bool userExists = _context.Users.Any(a => a.Id == id);

                if (userExists)
                {
                    User user = _context.Users.First(a => a.Id == id);
                    item.User = user;
                }
            }

            // get favourites
            bool exists = _context.UserFavourites.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.UserFavourites.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found - add the record
            _context.UserFavourites.Add(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Updates user favourites
        /// </summary>
        /// <remarks>Updates a favourite</remarks>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        public virtual IActionResult UsersCurrentFavouritesPutAsync(UserFavourite item)
        {
            item.User = null;

            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                bool userExists = _context.Users.Any(a => a.Id == id);

                if (userExists)
                {
                    User user = _context.Users.First(a => a.Id == id);
                    item.User = user;
                }
            }

            // get favourites
            bool exists = _context.UserFavourites.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.UserFavourites.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get user favourites by type
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="favouritetype">type of favourite to return</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersCurrentFavouritesFavouritetypeGetAsync(string favouritetype)
        {
            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                IQueryable<UserFavourite> data = _context.UserFavourites
                    .Where(x => x.User.Id == id)
                    .Select(x => x);

                if (favouritetype != null)
                {
                    data = data.Where(x => x.Type == favouritetype);
                }

                return new ObjectResult(new HetsResponse(data.ToList()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersCurrentGetAsync()        
        {
            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                User currentUser = _context.Users
                        .Include(x => x.District)
                        .Include(x => x.UserRoles)
                            .ThenInclude(y => y.Role)
                                .ThenInclude(z => z.RolePermissions)
                                    .ThenInclude(z => z.Permission)
                        .First(x => x.Id == id);

                CurrentUserViewModel result = currentUser.ToCurrentUserViewModel();

                // get the name for the current logged in user
                result.GivenName = User.FindFirst(ClaimTypes.GivenName).Value;
                result.Surname = User.FindFirst(ClaimTypes.Surname).Value;

                // remove inactive roles
                for (int i = currentUser.UserRoles.Count - 1; i >= 0; i--)
                {
                    if (currentUser.UserRoles[i].EffectiveDate > DateTime.UtcNow ||
                        (currentUser.UserRoles[i].ExpiryDate != null &&
                         currentUser.UserRoles[i].ExpiryDate < DateTime.UtcNow))
                    {
                        currentUser.UserRoles.RemoveAt(i);
                    }
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // no record found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Logoff user - set district back to primary
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersCurrentLogoffPostAsync()
        {
            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                UserDistrict district = _context.UserDistricts.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.UserId == id &&
                                         x.IsPrimary);

                // if we don't find a primary - look for the first one in the list
                if (district == null)
                {
                    district = _context.UserDistricts.AsNoTracking()
                        .Include(x => x.User)
                        .Include(x => x.District)
                        .FirstOrDefault(x => x.User.Id == id);
                }

                // update the current district for the user
                User user = null;

                if (district != null)
                {
                    int? userId = GetCurrentUserId();

                    user = _context.Users.First(a => a.Id == userId);
                    user.DistrictId = district.Id;

                    _context.SaveChanges();
                }

                UserSettings.ClearUserSettings(_httpContext);

                return new ObjectResult(new HetsResponse(user));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }
    }
}
