using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Mappings;

namespace HETSAPI.Services.Impl
{ 
    /// <summary>
    /// 
    /// </summary>
    public class CurrentUserService : ServiceBase, ICurrentUserService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
        }

        /// <summary>
        /// 
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
                    var item = _context.UserFavourites.First(a => a.Id == id);

                    _context.UserFavourites.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                    return new ObjectResult(item);
                }
                else
                {
                    // record not found
                    return new StatusCodeResult(404);
                }
            }
            else
            {
                return new StatusCodeResult(403);
            }


        }

        /// <summary>
        /// 
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

            bool exists = _context.UserFavourites.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.UserFavourites.Update(item);

                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }

            // record not found. add the record.
            _context.UserFavourites.Add(item);

            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);

        }

        /// <summary>
        /// 
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

            bool exists = _context.UserFavourites.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.UserFavourites.Update(item);

                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }

            // record not found
            return new StatusCodeResult(404);
        }

        /// <summary>
        /// 
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

                return new ObjectResult(data.ToList());
            }

            // no user context.
            return new StatusCodeResult(403);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersCurrentGetAsync ()        
        {
            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                User currentUser = _context.Users
                                        .Include(x => x.District)
                                        .Include(x => x.GroupMemberships)
                                        .ThenInclude(y => y.Group)
                                        .Include(x => x.UserRoles)
                                        .ThenInclude(y => y.Role)
                                        .ThenInclude(z => z.RolePermissions)
                                        .ThenInclude(z => z.Permission)
                                        .First(x => x.Id == id);

                var result = currentUser.ToCurrentUserViewModel();

                // get the name for the current logged in user
                result.GivenName = User.FindFirst(ClaimTypes.GivenName).Value;
                result.Surname = User.FindFirst(ClaimTypes.Surname).Value;
                               

                return new ObjectResult(result);
            }
            else
            {
                return new StatusCodeResult(404); // no current user ID
            }
        }


    }
}
