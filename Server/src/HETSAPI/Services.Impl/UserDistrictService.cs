using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// User District Service
    /// </summary>
    public class UserDistrictService : ServiceBase, IUserDistrictService
    {
        private readonly HttpContext _httpContext;
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// User District Service Constructor
        /// </summary>
        public UserDistrictService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Bulk create user district records
        /// </summary>
        /// <remarks>Adds a number of user districts</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsBulkPostAsync(UserDistrict[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (UserDistrict item in items)
            {
                bool exists = _context.UserDistricts.Any(a => a.Id == item.Id);

                if (item.District != null)
                {
                    item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                }

                if (exists)
                {
                    _context.UserDistricts.Update(item);
                }
                else
                {
                    _context.UserDistricts.Add(item);
                }

                // Save the changes
                _context.SaveChanges();
            }            

            return new NoContentResult();
        }

        /// <summary>
        /// Get all user districts for the current logged on user
        /// </summary>
        /// <remarks>Returns a list of user districts</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsGetAsync()
        {
            // return for the current user only
            int? userId = GetCurrentUserId();

            List<UserDistrict> result = _context.UserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.Id == userId)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }        

        /// <summary>
        /// Get a specific user district record
        /// </summary>
        /// <param name="id">id of User District to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsIdGetAsync(int id)
        {
            bool exists = _context.UserDistricts.Any(a => a.Id == id);

            if (exists)
            {
                List<UserDistrict> result = _context.UserDistricts.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .Where(x => x.Id == id)
                    .ToList();

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Delete user district
        /// </summary>
        /// <param name="id">id of user district to delete</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsIdDeletePostAsync(int id)
        {
            bool exists = _context.UserDistricts.Any(a => a.Id == id);

            if (exists)
            {
                UserDistrict item = _context.UserDistricts
                    .Include(x => x.User)
                    .First(a => a.Id == id);

                int userId = item.User.Id;               

                // remove record
                _context.UserDistricts.Remove(item);

                // save the changes
                _context.SaveChanges();

                // return the updated user district records
                List<UserDistrict> userDistricts = _context.UserDistricts
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .Where(x => x.User.Id == userId)
                    .ToList();

                return new ObjectResult(new HetsResponse(userDistricts));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a user district record
        /// </summary>
        /// <remarks>Update or create a user district record</remarks>
        /// <param name="id">id of UserDistrict for updating</param>
        /// <param name="item">User District.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsIdPostAsync(int id, UserDistrict item)
        {
            if (id != item.Id)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            if (item.User == null)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            int userId = item.User.Id;

            List<UserDistrict> userDistricts = _context.UserDistricts
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.Id == userId)
                .ToList();

            bool districtExists;
            bool hasPrimary = false;

            // add or update user district            
            if (item.Id > 0)
            {
                int index = userDistricts.FindIndex(a => a.Id == item.Id);

                if (index < 0)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }                                                

                // check if this district already exists
                districtExists = userDistricts.Exists(a => a.District.Id == item.District.Id &&
                                                           a.Id != item.Id);

                // update the record
                if (!districtExists)
                {
                    if (item.User != null)
                    {
                        userDistricts[index].UserId = item.User.Id;
                    }
                    else
                    {
                        // user required
                        return new ObjectResult(new HetsResponse("HETS-17", ErrorViewModel.GetDescription("HETS-17", _configuration)));
                    }

                    if (item.District != null)
                    {
                        userDistricts[index].DistrictId = item.District.Id;
                    }
                    else
                    {
                        // district required
                        return new ObjectResult(new HetsResponse("HETS-18", ErrorViewModel.GetDescription("HETS-18", _configuration)));
                    }

                    // manage the primary attribute
                    if (item.IsPrimary)
                    {
                        userDistricts[index].IsPrimary = true;

                        foreach (UserDistrict existingUserDistrict in userDistricts)
                        {
                            if (existingUserDistrict.IsPrimary && existingUserDistrict.Id != item.Id)
                            {
                                existingUserDistrict.IsPrimary = false;
                                break;
                            }
                        }                        
                    }
                    else
                    {
                        userDistricts[index].IsPrimary = false;

                        foreach (UserDistrict existingUserDistrict in userDistricts)
                        {
                            if (existingUserDistrict.IsPrimary && existingUserDistrict.Id != item.Id)
                            {
                                hasPrimary = true;
                                break;
                            }
                        }

                        if (!hasPrimary)
                        {
                            userDistricts[index].IsPrimary = true;
                        }
                    }                                        
                }
            }
            else  // add user district
            {                
                // check if this district already exists
                districtExists = userDistricts.Exists(a => a.District.Id == item.District.Id);

                // add the record
                if (!districtExists)
                {
                    if (item.User != null)
                    {
                        item.User = _context.Users.FirstOrDefault(a => a.Id == item.User.Id);
                    }
                    else
                    {
                        // user required
                        return new ObjectResult(new HetsResponse("HETS-17", ErrorViewModel.GetDescription("HETS-17", _configuration)));
                    }

                    if (item.District != null)
                    {
                        item.District = _context.Districts.FirstOrDefault(a => a.Id == item.District.Id);
                    }
                    else
                    {
                        // district required
                        return new ObjectResult(new HetsResponse("HETS-18", ErrorViewModel.GetDescription("HETS-18", _configuration)));
                    }

                    if (item.IsPrimary)
                    {
                        item.IsPrimary = true;

                        foreach (UserDistrict existingUserDistrict in userDistricts)
                        {
                            if (existingUserDistrict.IsPrimary)
                            {
                                existingUserDistrict.IsPrimary = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        item.IsPrimary = false;

                        foreach (UserDistrict existingUserDistrict in userDistricts)
                        {
                            if (existingUserDistrict.IsPrimary)
                            {
                                hasPrimary = true;
                                break;
                            }
                        }

                        if (!hasPrimary)
                        {
                            item.IsPrimary = true;
                        }
                    }

                    _context.UserDistricts.Add(item);
                }
            }                                                                             

            _context.SaveChanges();

            // **************************************************************************
            // return the updated user district records
            // **************************************************************************    
            List<UserDistrict> results = _context.UserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .Where(x => x.User.Id == userId)
                .ToList();

            return new ObjectResult(new HetsResponse(results));     
        }

        /// <summary>
        /// Switch user district
        /// </summary>
        /// <param name="id">id of user district to switch to</param>
        /// <response code="200">OK</response>
        public virtual IActionResult UserDistrictsIdSwitchPostAsync(int id)
        {
            bool exists = _context.UserDistricts.Any(a => a.Id == id);

            if (exists)
            {
                UserDistrict userDistrict = _context.UserDistricts.First(a => a.Id == id);

                if (userDistrict == null)
                {
                    // record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                int? userId = GetCurrentUserId();

                User user = _context.Users.First(a => a.Id == userId);
                user.DistrictId = userDistrict.DistrictId;

                _context.SaveChanges();

                // create new district switch cookie
                _httpContext.Response.Cookies.Append(
                    "HETSDistrict",
                    userDistrict.DistrictId.ToString(),
                    new CookieOptions
                    {
                        Path = "/",
                        SameSite = SameSiteMode.None
                    }
                );

                return new ObjectResult(new HetsResponse(user));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }        

        /// <summary>
        /// Set the users district back to its default
        /// </summary>
        public void ResetUserDistrict()
        {
            int? userId = GetCurrentUserId();

            // get default district (if one exists)
            UserDistrict userDistrict = _context.UserDistricts.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.District)
                .FirstOrDefault(x => x.IsPrimary &&
                                     x.User.Id == userId);

            if (userDistrict == null)
            {
                // get first user district
                userDistrict = _context.UserDistricts.AsNoTracking()
                    .Include(x => x.User)
                    .Include(x => x.District)
                    .FirstOrDefault(x => x.User.Id == userId);
            }

            if (userDistrict != null)
            {
                User user = _context.Users.First(a => a.Id == userId);
                user.DistrictId = userDistrict.DistrictId;

                _context.SaveChanges();
            }        
        }
    }
}
