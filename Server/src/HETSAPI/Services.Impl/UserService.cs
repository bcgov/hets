using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// User Service
    /// </summary>
    public class UserService : ServiceBase, IUserService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// User Service Constructor
        /// </summary>
        public UserService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustUser(User item)
        {
            if (item.District != null)
            {
                item.District = _context.Districts.FirstOrDefault(x => x.Id == item.District.Id);
            }
        }                

        /// <summary>
        /// Create bulk user records
        /// </summary>
        /// <remarks>Adds a number of users</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersBulkPostAsync(User[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (User item in items)
            {
                AdjustUser(item);

                bool exists = _context.Users.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Users.Update(item);
                }
                else
                {
                    _context.Users.Add(item);
                }
            }

            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <remarks>Returns all users</remarks>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersGetAsync()
        {
            List<UserViewModel> result = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .Select(x => x.ToViewModel()).ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <remarks>Deletes a user</remarks>
        /// <param name="id">id of User to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdDeletePostAsync(int id)
        {
            User user = _context.Users
                .Include(x => x.UserRoles)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            if (user.UserRoles != null)
            {
                foreach (UserRole item in user.UserRoles)
                {
                    _context.UserRoles.Remove(item);
                }
            }            

            _context.Users.Remove(user);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(user.ToViewModel()));
        }

        /// <summary>
        /// Get user favourites by id
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given context type</remarks>
        /// <param name="id">id of User to fetch favorites for</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdFavouritesGetAsync(int id)
        {
            User user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            List<UserFavourite> result = _context.UserFavourites
                .Where(x => x.User.Id == user.Id)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Update favourites associated with a user
        /// </summary>
        /// <remarks>Updates the active set of favourites for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdFavouritesPutAsync(int id, UserFavourite[] items)
        {
            bool exists = _context.Users.Any(a => a.Id == id);

            if (exists)
            {
                // update the given user's group membership.
                User user = _context.Users.First(a => a.Id == id);

                IQueryable<UserFavourite> data = _context.UserFavourites
                    .Where(x => x.User.Id == user.Id);

                foreach (UserFavourite item in data)
                {
                    bool found = false;

                    foreach (UserFavourite parameterItem in items)
                    {
                        if (parameterItem == item)
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        _context.UserFavourites.Remove(item);
                    }
                }

                // add new items
                foreach (UserFavourite parameterItem in items)
                {
                    bool found = false;

                    foreach (UserFavourite item in data)
                    {
                        if (parameterItem == item)
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        // adjust user  
                        parameterItem.User = user;
                        _context.UserFavourites.Add(parameterItem);
                    }
                }

                _context.SaveChanges();
                return new NoContentResult();
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update favourtites associated with a user
        /// </summary>
        /// <remarks>Adds fovourites to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdFavouritesPostAsync(int id, UserFavourite[] items)
        {
            bool exists = _context.Users.Any(a => a.Id == id);

            if (exists)
            {
                // update the given user's favourites
                User user = _context.Users.First(a => a.Id == id);

                IQueryable<UserFavourite> data = _context.UserFavourites
                    .Where(x => x.User.Id == user.Id);

                foreach (UserFavourite item in data)
                {
                    bool found = false;

                    foreach (UserFavourite parameterItem in items)
                    {
                        if (parameterItem == item)
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        _context.UserFavourites.Remove(item);
                    }
                }

                // add new items
                foreach (UserFavourite parameterItem in items)
                {
                    bool found = false;

                    foreach (UserFavourite item in data)
                    {
                        if (parameterItem == item)
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        // adjust user and group. 
                        parameterItem.User = user;

                        _context.UserFavourites.Add(parameterItem);
                    }
                }

                _context.SaveChanges();
                return new NoContentResult();
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <remarks>Returns data for a particular user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdGetAsync(int id)
        {
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            return new ObjectResult(new HetsResponse(user.ToViewModel()));
        }                    
        
        /// <summary>
        /// Get permissions associated with a user
        /// </summary>
        /// <remarks>Returns the set of permissions for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdPermissionsGetAsync(int id)
        {
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            // not found
            if (user == null) { return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration))); }

            List<PermissionViewModel> permissions = new List<PermissionViewModel>();

            if (user.UserRoles != null)
            {
                foreach (UserRole item in user.UserRoles)
                {
                    if (item.Role != null && item.Role.RolePermissions != null)
                    {
                        foreach (var permission in item.Role.RolePermissions)
                        {
                            permissions.Add(permission.Permission.ToViewModel());
                        }
                    }
                }
            }

            return new ObjectResult(new HetsResponse(permissions));
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <remarks>Updates a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdPutAsync(int id, UserViewModel item)
        {
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            // record not found
            if (user == null)
            {
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            user.Active = item.Active;
            user.Email = item.Email;
            user.GivenName = item.GivenName;
            user.Surname = item.Surname;
            user.SmUserId = item.SmUserId;            

            if (item.District != null)
            {
                bool districtExists = _context.Districts.Any(x => x.Id == item.District.Id);

                if (districtExists)
                {
                    District district = _context.Districts
                        .Include(x => x.Region)
                        .First(x => x.Id == item.District.Id);

                    user.District = district;
                }
            }

            // save changes
            _context.Users.Update(user);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(user.ToViewModel()));
        }

        /// <summary>
        /// Get roles associates with a user
        /// </summary>
        /// <remarks>Returns the roles for a user</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdRolesGetAsync(int id)
        {
            User user = _context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .First(x => x.Id == id);

            // reord not found
            if (user == null)
            {
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            List<UserRoleViewModel> result = new List<UserRoleViewModel>();

            List<UserRole> data = user.UserRoles;

            foreach (UserRole item in data)
            {
                if (item != null)
                {
                    int userRoleId = item.Id;

                    bool exists = _context.UserRoles.Any(x => x.Id == userRoleId);

                    if (exists)
                    {
                        UserRole userRole = _context.UserRoles
                            .Include(x => x.Role)
                            .First(x => x.Id == userRoleId);

                        UserRoleViewModel record = userRole.ToViewModel();

                        record.UserId = user.Id;
                        result.Add(record);
                    }
                }
            }

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Add user to a role
        /// </summary>
        /// <remarks>Adds a role to a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="201">Role created for user</response>
        public virtual IActionResult UsersIdRolesPostAsync(int id, UserRoleViewModel item)
        {
            bool exists = _context.Users.Any(x => x.Id == id);

            // record not found
            if (!exists)
            {
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }            
            
            // check the role id
            bool roleExists = _context.Roles.Any(x => x.Id == item.RoleId);

            // record not found
            if (!roleExists)
            {
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }
            
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .First(x => x.Id == id);

            if (user.UserRoles == null)
            {
                user.UserRoles = new List<UserRole>();
            }

            // create a new UserRole based on the view model.
            UserRole userRole = new UserRole();

            Role role = _context.Roles.First(x => x.Id == item.RoleId);

            userRole.Role = role;
            userRole.EffectiveDate = item.EffectiveDate;
            userRole.ExpiryDate = item.ExpiryDate;

            if (!user.UserRoles.Contains(userRole))
            {
                user.UserRoles.Add(userRole);
            }

            _context.Update(user);
            _context.SaveChanges();

            return new StatusCodeResult(201);                                
        }

        /// <summary>
        /// Update all roles associated with a user
        /// </summary>
        /// <remarks>Updates the roles for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdRolesPutAsync(int id, UserRoleViewModel[] items)
        {
            bool exists = _context.Users.Any(x => x.Id == id);

            // not found
            if (!exists || items == null) { return new StatusCodeResult(400); }
           
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .First(x => x.Id == id);

            if (user.UserRoles == null)
            {
                user.UserRoles = new List<UserRole>();
            }
            else
            {
                // existing data, clear it
                foreach (UserRole userRole in user.UserRoles)
                {
                    if (_context.UserRoles.Any(x => x.Id == userRole.Id))
                    {
                        UserRole delete = _context.UserRoles.First(x => x.Id == userRole.Id);
                        _context.Remove(delete);
                    }
                }

                user.UserRoles.Clear();
            }

            foreach (UserRoleViewModel item in items)
            {
                // check the role id
                bool roleExists = _context.Roles.Any(x => x.Id == item.RoleId);

                if (roleExists)
                {
                    // create a new UserRole based on the view model.
                    UserRole userRole = new UserRole();

                    Role role = _context.Roles.First(x => x.Id == item.RoleId);

                    userRole.Role = role;
                    userRole.EffectiveDate = item.EffectiveDate;
                    userRole.ExpiryDate = item.ExpiryDate;                       

                    _context.Add(userRole);

                    if (!user.UserRoles.Contains(userRole))
                    {
                        user.UserRoles.Add(userRole);
                    }
                }
            }

            _context.Update(user);
            _context.SaveChanges();

            return new StatusCodeResult(201);            
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="item"></param>
        /// <remarks>Create new user</remarks>
        /// <response code="201">User created</response>
        public virtual IActionResult UsersPostAsync(UserViewModel item)
        {
            User user = new User
            {
                Active = item.Active,
                Email = item.Email,
                GivenName = item.GivenName,
                Surname = item.Surname,
                District = item.District,
                SmUserId = item.SmUserId
            };

            AdjustUser(user);
            bool exists = _context.Users.Any(x => x.Id == user.Id);

            if (exists)
            {
                _context.Users.Update(user);
            }
            else
            {
                _context.Users.Add(user);
            }

            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(user));
        }

        /// <summary>
        /// Search users
        /// </summary>
        /// <remarks>Used for the search users.</remarks>
        /// <param name="districts">Districts (array of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersSearchGetAsync(string districts, string surname, bool? includeInactive)
        {
            int?[] districtArray = ParseIntArray(districts);

            // Loading of related data
            IQueryable<User> data = _context.Users
                .Include(x => x.District)
                .Include(x => x.UserRoles)
                    .ThenInclude(y => y.Role)
                        .ThenInclude(z => z.RolePermissions)
                            .ThenInclude(z => z.Permission)
                .Select(x => x);

            // Note that Districts searches SchoolBus Districts, not SchoolBusOwner Districts
            if (districtArray != null && districtArray.Length > 0)
            {
                data = data.Where(x => districtArray.Contains (x.District.Id));
            }

            if (surname != null)
            {
                data = data.Where(x => x.Surname.ToLower().Contains(surname.ToLower()));
            }

            if (includeInactive == null || includeInactive == false)
            {
                data = data.Where(x => x.Active);
            }

            // convert the results to the view model
            var result = new List<UserViewModel>();
            foreach (User item in data)
            {
                UserViewModel record = item.ToViewModel();
                result.Add(record);
            }

            return new ObjectResult(new HetsResponse(result));
        }
    }
}