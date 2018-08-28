using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// City Controller
    /// </summary>
    [Route("api")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CityController : Controller
    {
        private readonly DbAppContext _context;

        public CityController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            // set context data
            HetUser user = ModelHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        [HttpGet]
        [Route("cities")]
        [SwaggerOperation("CitiesGet")]
        [SwaggerResponse(200, type: typeof(List<HetCity>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult CitiesGet()
        {
            // get all cities
            List<HetCity> cities = _context.HetCity.AsNoTracking().ToList();

            return new ObjectResult(cities);
        }
    }
}
