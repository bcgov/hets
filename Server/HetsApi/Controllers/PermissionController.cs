using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Permission Controller
    /// </summary>
    [Route("api/permissions")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PermissionController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;

        public PermissionController(DbAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all permissions
        /// </summary>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<List<PermissionLite>> PermissionsGet()
        {
            List<HetPermission> permissions = _context.HetPermissions.ToList();

            // convert Permission Model to the "PermissionLite" Model
            List<PermissionLite> result = new List<PermissionLite>();

            foreach (HetPermission item in permissions)
            {
                result.Add(PermissionHelper.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
