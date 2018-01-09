using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// User Service
    /// </summary>
    public class UserService : ServiceBase, IUserService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public UserService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
        }

        private void AdjustUser(User item)
        {
            if (item.District != null)
            {
                item.District = _context.Districts.FirstOrDefault(x => x.Id == item.District.Id);
            }
        }

        /// <summary>
        /// Create bulk user group records
        /// </summary>
        /// <remarks>Adds a number of user groups</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult UsergroupsBulkPostAsync(GroupMembership[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (GroupMembership item in items)
            {
                // adjust the user
                if (item.User != null)
                {
                    int userId = item.User.Id;

                    bool userExists = _context.Users.Any(a => a.Id == userId);

                    if (userExists)
                    {
                        User user = _context.Users.First(a => a.Id == userId);
                        item.User = user;
                    }
                    else
                    {
                        item.User = null;
                    }
                }

                // adjust the group
                if (item.Group != null)
                {
                    int groupId = item.Group.Id;

                    bool groupExists = _context.Groups.Any(a => a.Id == groupId);

                    if (groupExists)
                    {
                        Group group = _context.Groups.First(a => a.Id == groupId);
                        item.Group = group;
                    }
                    else
                    {
                        item.Group = null;
                    }
                }

                bool exists = _context.GroupMemberships.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.GroupMemberships.Update(item);
                }
                else
                {
                    _context.GroupMemberships.Add(item);
                }
            }

            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Create bulk user role records
        /// </summary>
        /// <remarks>Adds a number of user roles</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserrolesBulkPostAsync(UserRole[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (UserRole item in items)
            {
                // adjust the role
                if (item.Role != null)
                {
                    int roleId = item.Role.Id;

                    bool userExists = _context.Roles.Any(a => a.Id == roleId);

                    if (userExists)
                    {
                        Role role = _context.Roles.First(a => a.Id == roleId);
                        item.Role = role;
                    }
                }

                bool exists = _context.UserRoles.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.UserRoles.Update(item);
                }
                else
                {
                    _context.UserRoles.Add(item);
                }
            }

            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
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
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .Select(x => x.ToViewModel()).ToList();

            return new ObjectResult(result);
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
                .Include(x => x.GroupMemberships)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }

            if (user.UserRoles != null)
            {
                foreach (UserRole item in user.UserRoles)
                {
                    _context.UserRoles.Remove(item);
                }
            }

            if (user.GroupMemberships != null)
            {
                foreach (GroupMembership item in user.GroupMemberships)
                {
                    _context.GroupMemberships.Remove(item);
                }
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return new ObjectResult(user.ToViewModel());
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
                // Not Found
                return new StatusCodeResult(404);
            }

            List<UserFavourite> data = _context.UserFavourites
                .Where(x => x.User.Id == user.Id)
                .ToList();

            return new ObjectResult(data);
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

                // add new items.
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
            return new StatusCodeResult(404);
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
            return new StatusCodeResult(404);
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
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }

            return new ObjectResult(user.ToViewModel());
        }

        /// <summary>
        /// Get groups associated with a user
        /// </summary>
        /// <remarks>Returns all groups that a user is a member of</remarks>
        /// <param name="id">id of User to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdGroupsGetAsync(int id)
        {
            User user = _context.Users
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .First(x => x.Id == id);

            if (user == null)
            {
                // Not Found
                return new StatusCodeResult(404);
            }

            List<GroupMembershipViewModel> result = new List<GroupMembershipViewModel>();

            List<GroupMembership> data = user.GroupMemberships;

            foreach (GroupMembership item in data)
            {
                if (item != null)
                {
                    GroupMembershipViewModel record = item.ToViewModel();
                    result.Add(record);
                }
            }

            return new ObjectResult(result);
        }

        /// <summary>
        /// Update groups associated with a user
        /// </summary>
        /// <remarks>Updates the active set of groups for a user</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdGroupsPutAsync(int id, GroupMembershipViewModel[] items)
        {
            bool exists = _context.Users.Any(x => x.Id == id);

            // not found
            if (!exists) return new StatusCodeResult(400);
           
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .First(x => x.Id == id);

            if (user.GroupMemberships == null)
            {
                user.GroupMemberships = new List<GroupMembership>();
            }
            else
            {
                // existing data, clear it.
                foreach (GroupMembership groupMembership in user.GroupMemberships)
                {
                    if (_context.GroupMemberships.Any(x => x.Id == groupMembership.Id))
                    {
                        GroupMembership delete = _context.GroupMemberships.First(x => x.Id == groupMembership.Id);
                        _context.Remove(delete);
                    }
                }

                user.GroupMemberships.Clear();
            }

            foreach (GroupMembershipViewModel item in items)
            {
                if (item != null)
                {
                    // check the role id
                    bool groupExists = _context.Groups.Any(x => x.Id == item.GroupId);

                    if (groupExists)
                    {
                        // create a new UserRole based on the view model.
                        GroupMembership groupMembership = new GroupMembership();

                        Group group = _context.Groups.First(x => x.Id == item.GroupId);

                        groupMembership.Group = group;
                        groupMembership.User = user;

                        _context.Add(groupMembership);

                        if (!user.GroupMemberships.Contains(groupMembership))
                        {
                            user.GroupMemberships.Add(groupMembership);
                        }
                    }
                }
            }

            _context.Update(user);
            _context.SaveChanges();
            return new StatusCodeResult(201);            
        }

        /// <summary>
        /// Add a user to a group
        /// </summary>
        /// <remarks>Adds a user to groups</remarks>
        /// <param name="id">id of User to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersIdGroupsPostAsync(int id, GroupMembershipViewModel item)
        {
            bool exists = _context.Users.Any(a => a.Id == id);

            // record not found
            if (!exists || item == null) return new StatusCodeResult(404);
            
            // update the given user's group membership.
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .First(a => a.Id == id);

            // add new item
            bool found = false;

            foreach (GroupMembership parameterItem in user.GroupMemberships)
            {
                if (parameterItem.Group != null && parameterItem.Group.Id == item.GroupId)
                {
                    found = true;
                }
            }

            if (found) return new NoContentResult();
                
            GroupMembership groupMembership = new GroupMembership {User = user};

            // set user and group. 
            bool groupExists = _context.Groups.Any(a => a.Id == item.GroupId);

            if (groupExists)
            {
                Group group = _context.Groups.First(a => a.Id == item.GroupId);
                groupMembership.Group = group;
            }

            user.GroupMemberships.Add(groupMembership);

            _context.Update(user);
            _context.SaveChanges();                

            return new NoContentResult();        
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
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            // not found
            if (user == null) { return new StatusCodeResult(404); }

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

            return new ObjectResult(permissions);
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
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
                .Include(x => x.UserRoles)
                .ThenInclude(y => y.Role)
                .ThenInclude(z => z.RolePermissions)
                .ThenInclude(z => z.Permission)
                .FirstOrDefault(x => x.Id == id);

            // not found
            if (user == null) { return new StatusCodeResult(404); }

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

            // Save changes
            _context.Users.Update(user);
            _context.SaveChanges();
            return new ObjectResult(user.ToViewModel());
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

            // not found
            if (user == null) { return new StatusCodeResult(404); }

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

            return new ObjectResult(result);
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

            // not found
            if (!exists) { return new StatusCodeResult(404); }            
            
            // check the role id
            bool roleExists = _context.Roles.Any(x => x.Id == item.RoleId);

            // not found
            if (!roleExists) { return new StatusCodeResult(404); }
            
            User user = _context.Users
                .Include(x => x.District)
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
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
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
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
                // existing data, clear it.
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
            return new ObjectResult(user);
        }

        /// <summary>
        /// Searches Users
        /// </summary>
        /// <remarks>Used for the search users.</remarks>
        /// <param name="districts">Districts (array of id numbers)</param>
        /// <param name="surname"></param>
        /// <param name="includeInactive">True if Inactive users will be returned</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UsersSearchGetAsync(string districts, string surname, bool? includeInactive)
        {
            if (districts == null) throw new ArgumentNullException(nameof(districts));

            int?[] districtArray = ParseIntArray(districts);

            // Loading of related data
            IQueryable<User> data = _context.Users
                .Include(x => x.District)
                .Include(x => x.GroupMemberships)
                .ThenInclude(y => y.Group)
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

            return new ObjectResult(result);
        }
    }
}