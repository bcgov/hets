using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [Route("api/roles")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RoleController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public RoleController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RoleDto>> RolesGet()
        {
            // get all roles
            List<HetRole> roles = _context.HetRoles.AsNoTracking().ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RoleDto>>(roles)));
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id">id of Role to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.RolesAndPermissions, HetPermission.WriteAccess)]
        public virtual ActionResult<RoleDto> RolesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetRoles.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRole role = _context.HetRoles.First(x => x.RoleId == id);

            // remove associated role permission records
            List<HetRolePermission> itemsToRemove = _context.HetRolePermissions
                .Where(x => x.Role.RoleId == role.RoleId)
                .ToList();

            foreach (HetRolePermission item in itemsToRemove)
            {
                _context.HetRolePermissions.Remove(item);
            }

            // remove role and save
            _context.HetRoles.Remove(role);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<RoleDto>(role)));
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">id of Role to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual ActionResult<RoleDto> RolesIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRoles.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRole role = _context.HetRoles.AsNoTracking().First(x => x.RoleId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<RoleDto>(role)));
        }

        /// <summary>
        /// Get permissions associated with a role
        /// </summary>
        /// <remarks>Get all the permissions for a role</remarks>
        /// <param name="id">id of Role to fetch</param>
        [HttpGet]
        [Route("{id}/permissions")]
        [RequiresPermission(HetPermission.RolesAndPermissions)]
        public virtual ActionResult<List<RolePermissionDto>> RolesIdPermissionsGet([FromRoute]int id)
        {
            HetRole role = _context.HetRoles.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // return role permissions
            return new ObjectResult(new HetsResponse(_mapper.Map<List<RolePermissionDto>>(role.HetRolePermissions.ToList())));
        }

        /// <summary>
        /// Add permission to a role
        /// </summary>
        /// <remarks>Adds a permissions to a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/permissions")]
        [RequiresPermission(HetPermission.RolesAndPermissions, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RolePermissionDto>> RolesIdPermissionsPost([FromRoute]int id, [FromBody]PermissionDto item)
        {
            HetRole role = _context.HetRoles.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetPermission> allPermissions = _context.HetPermissions.ToList();
            List<string> existingPermissionCodes = role.HetRolePermissions.Select(x => x.Permission.Code).ToList();

            if (!existingPermissionCodes.Contains(item.Code))
            {
                HetPermission permToAdd = allPermissions.FirstOrDefault(x => x.Code == item.Code);

                // invalid permission
                if (permToAdd == null) throw new ArgumentException(string.Format("Invalid Permission Code {0}", item.Code));

                // add permission
                HetRolePermission rolePermission = new HetRolePermission
                {
                    Permission = permToAdd,
                    Role = role
                };

                _context.HetRolePermissions.Add(rolePermission);
            }

            _context.SaveChanges();

            // get updated permissions
            role = _context.HetRoles.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .First();

            // return role permissions
            return new ObjectResult(new HetsResponse(_mapper.Map<List<RolePermissionDto>>(role.HetRolePermissions.ToList())));
        }

        /// <summary>
        /// Update permissions for a role
        /// </summary>
        /// <remarks>Updates the permissions for a role</remarks>
        /// <param name="id">id of Role to update</param>
        /// <param name="items"></param>
        [HttpPut]
        [Route("{id}/permissions")]
        [RequiresPermission(HetPermission.RolesAndPermissions, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RolePermissionDto>> RolesIdPermissionsPut([FromRoute]int id, [FromBody]PermissionDto[] items)
        {
            HetRole role = _context.HetRoles.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermissions)
                    .ThenInclude(rolePerm => rolePerm.Permission)
                .FirstOrDefault();

            // not found
            if (role == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get permissions
            List<HetPermission> allPermissions = _context.HetPermissions.AsNoTracking().ToList();

            List<int> permissionIds = items.Select(x => x.PermissionId).ToList();
            List<int> existingPermissionIds = role.HetRolePermissions.Select(x => x.Permission.PermissionId).ToList();
            List<int> permissionIdsToAdd = permissionIds.Where(x => !existingPermissionIds.Contains(x)).ToList();

            // permissions to add
            foreach (int permissionId in permissionIdsToAdd)
            {
                HetPermission permToAdd = allPermissions
                    .FirstOrDefault(x => x.PermissionId == permissionId);

                // invalid permission
                if (permToAdd == null) throw new ArgumentException("Invalid Permission Code");

                // add permission
                HetRolePermission rolePermission = new HetRolePermission
                {
                    PermissionId = permToAdd.PermissionId,
                    RoleId = role.RoleId
                };

                _context.HetRolePermissions.Add(rolePermission);
            }

            // permissions to remove
            List<HetRolePermission> permissionsToRemove = role.HetRolePermissions
                .Where(x => x.Permission != null &&
                            !permissionIds.Contains(x.Permission.PermissionId)).ToList();

            foreach (HetRolePermission perm in permissionsToRemove)
            {
                _context.HetRolePermissions.Remove(perm);
            }

            _context.SaveChanges();

            // get updated permissions
            role = _context.HetRoles.AsNoTracking()
                .Where(x => x.RoleId == id)
                .Include(x => x.HetRolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .First();

            // return role permissions
            return new ObjectResult(new HetsResponse(_mapper.Map<List<RolePermissionDto>>(role.HetRolePermissions.ToList())));
        }

        /// <summary>
        /// Update role
        /// </summary>
        /// <param name="id">id of Role to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.RolesAndPermissions, HetPermission.WriteAccess)]
        public virtual ActionResult<RoleDto> RolesIdPut([FromRoute]int id, [FromBody]RoleDto item)
        {
            bool exists = _context.HetRoles.Any(x => x.RoleId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRole role = _context.HetRoles.First(x => x.RoleId == id);

            role.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            role.Name = item.Name;
            role.Description = item.Description;

            // save changes
            _context.SaveChanges();

            // get updated role
            role = _context.HetRoles.AsNoTracking()
                .First(x => x.RoleId == id);

            // return role permissions
            return new ObjectResult(new HetsResponse(_mapper.Map<RoleDto>(role)));
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="item"></param>
        /// <response code="200">Role created</response>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.RolesAndPermissions, HetPermission.WriteAccess)]
        public virtual ActionResult<RoleDto> RolesPost([FromBody]RoleDto item)
        {
            HetRole role = new HetRole
            {
                Description = item.Description,
                Name = item.Name
            };

            // save changes
            _context.HetRoles.Add(role);
            _context.SaveChanges();

            int id = role.RoleId;

            // get updated role
            role = _context.HetRoles.AsNoTracking()
                .First(x => x.RoleId == id);

            // return role permissions
            return new ObjectResult(new HetsResponse(_mapper.Map<RoleDto>(role)));
        }
    }
}
