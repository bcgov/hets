using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Provincial Rate Types Controller
    /// </summary>
    [Route("api/provincialRateTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProvincialRateTypeController : Controller
    {
        private readonly DbAppContext _context;

        public ProvincialRateTypeController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            
            // set context data
            HetUser user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all provincial rate types
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("ProvincialRateTypesGet")]
        [SwaggerResponse(200, type: typeof(List<HetProvincialRateType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProvincialRateTypesGet()
        {
            List<HetProvincialRateType> rates = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Active)
                .ToList();

            int pseudoId = 0;

            foreach (HetProvincialRateType rateType in rates)
            {
                pseudoId++;
                rateType.Id = pseudoId;
            }

            return new ObjectResult(new HetsResponse(rates));
        }
    }
}
