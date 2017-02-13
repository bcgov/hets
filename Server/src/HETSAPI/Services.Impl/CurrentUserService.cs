/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
            int? user_id = GetCurrentUserId();
            if (user_id != null)
            {
                bool exists = _context.UserFavourites.Where(x => x.User.Id == user_id)
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
                bool user_exists = _context.Users.Any(a => a.Id == id);
                if (user_exists)
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
            else
            {
                // record not found. add the record.
                _context.UserFavourites.Add(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }

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
                bool user_exists = _context.Users.Any(a => a.Id == id);
                if (user_exists)
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
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a user&#39;s favourites of a given type.  If type is empty, returns all.</remarks>
        /// <param name="type">type of favourite to return</param>
        /// <response code="200">OK</response>
        /// <response code="404">User not found</response>
        public virtual IActionResult UsersCurrentFavouritesFavouritetypeGetAsync(string type)
        {
            // get the current user id
            int? id = GetCurrentUserId();

            if (id != null)
            {
                var data = _context.UserFavourites
                    .Where(x => x.User.Id == id)
                    .Select(x => x);
                if (type != null)
                {
                    data = data.Where(x => x.Type == type);
                }

                return new ObjectResult(data.ToList());
            }
            else
            {
                // no user context.
                return new StatusCodeResult(403);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Get the currently logged in user</remarks>
        /// <response code="200">OK</response>

        public virtual IActionResult UsersCurrentGetAsync ()        
        {
            var result = new CurrentUserViewModel();

            // get the name for the current logged in user
            result.GivenName = User.FindFirst(ClaimTypes.GivenName).Value;
            result.Surname = User.FindFirst(ClaimTypes.Surname).Value;

            result.FullName = result.GivenName + " " + result.Surname;

            // get the current user id
            int? id = GetCurrentUserId();

            result.DistrictName = "Victoria";
            
            // get the number of inspections available for the current logged in user

            return new ObjectResult(result);
        }


    }
}
