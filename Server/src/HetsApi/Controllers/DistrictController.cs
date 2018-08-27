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

namespace HetsApi.Controllers
{
    /// <summary>
    /// District Controller
    /// </summary>
    [Route("/api/districts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DistrictController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public DistrictController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;    
            
            // set context data
            HetUser user = ModelHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Get all districts
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("DistrictsGet")]
        [SwaggerResponse(200, type: typeof(List<District>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictsGet()
        {
            List<HetDistrict> districts = _context.HetDistrict.AsNoTracking()
                .Include(x => x.Region)
                .ToList();

            // convert to UI model
            List<District> response = new List<District>();

            foreach (HetDistrict district in districts)
            {
                if (district != null)
                {
                    District temp = new District();
                    response.Add((District)ModelHelper.CopyProperties(district, temp));
                }
            }

            return new ObjectResult(response);
        }

        /// <summary>
        /// Get all owners by district
        /// </summary>
        [HttpGet]
        [Route("/api/district/{id}/owners")]
        [SwaggerOperation("DistrictOwnersGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictOwnersGet([FromRoute]int id)
        {
            bool exists = _context.HetDistrict.Any(a => a.DistrictId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetOwner> owners = _context.HetOwner.AsNoTracking()
                .Where(x => x.LocalArea.ServiceArea.District.DistrictId == id)
                .OrderBy(x => x.OrganizationName)
                .ToList();

            // convert to UI model
            List<Owner> response = new List<Owner>();

            foreach (HetOwner owner in owners)
            {
                if (owner != null)
                {
                    Owner temp = new Owner();
                    response.Add((Owner)ModelHelper.CopyProperties(owner, temp));
                }
            }

            return new ObjectResult(response);
        }

        /// <summary>
        /// Get all local areas by district
        /// </summary>
        [HttpGet]
        [Route("/api/district/{id}/localAreas")]
        [SwaggerOperation("DistrictLocalAreasGet")]
        [SwaggerResponse(200, type: typeof(List<LocalArea>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictLocalAreasGet([FromRoute]int id)
        {
            bool exists = _context.HetDistrict.Any(a => a.DistrictId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            List<HetLocalArea> localAreas = _context.HetLocalArea.AsNoTracking()
                .Where(x => x.ServiceArea.District.DistrictId == id)
                .OrderBy(x => x.Name)
                .ToList();

            // convert to UI model
            List<LocalArea> response = new List<LocalArea>();

            foreach (HetLocalArea localArea in localAreas)
            {
                if (localArea != null)
                {
                    LocalArea temp = new LocalArea();
                    response.Add((LocalArea)ModelHelper.CopyProperties(localArea, temp));
                }
            }

            return new ObjectResult(response);
        }
    }
}
