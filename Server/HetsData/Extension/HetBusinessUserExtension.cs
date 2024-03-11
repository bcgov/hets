using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HetsData.Entities
{
    public partial class HetBusinessUser
    {
        /// <summary>
        /// User Permission Claim Property
        /// </summary>
        public const string PermissionClaim = "permission_claim";

        /// <summary>
        /// UserId Claim Property
        /// </summary>
        public const string UseridClaim = "userid_claim";
        
        /// <summary>
        /// Convert User to ClaimsPrincipal
        /// </summary>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public ClaimsPrincipal ToClaimsPrincipal(string authenticationType)
        {
            return new ClaimsPrincipal(ToClaimsIdentity());
        }

        public ClaimsIdentity ToClaimsIdentity()
        {
            return new ClaimsIdentity(GetClaims());
        }

        private List<Claim> GetClaims()
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, BceidUserId),
                new Claim(ClaimTypes.Actor, "BusinessUser")
            };

            if (!string.IsNullOrEmpty(BceidDisplayName))
                claims.Add(new Claim(ClaimTypes.Name, BceidDisplayName));

            if (!string.IsNullOrEmpty(BceidEmail))
                claims.Add(new Claim(ClaimTypes.Email, BceidEmail));

            if (BusinessId != 0)
                claims.Add(new Claim(UseridClaim, BusinessId.ToString()));

            var permissions = GetActivePermissions()
                .Select(p => new Claim(PermissionClaim, p.Code)).ToList();

            if (permissions.Any())
                claims.AddRange(permissions);

            var roles = GetActiveRoles()
                .Select(r => new Claim(ClaimTypes.Role, r.Name)).ToList();

            if (roles.Any())
                claims.AddRange(roles);

            return claims;
        }

        private List<HetPermission> GetActivePermissions()
        {
            List<HetPermission> result = null;

            List<HetRole> activeRoles = GetActiveRoles();

            if (activeRoles != null)
            {
                IEnumerable<HetRolePermission> rolePermissions = activeRoles
                        .Where(x => x?.HetRolePermissions != null)
                        .SelectMany(x => x.HetRolePermissions);

                result = rolePermissions.Select(x => x.Permission).Distinct().ToList();
            }

            return result;
        }

        private List<HetRole> GetActiveRoles()
        {
            List<HetRole> roles = new();

            if (HetBusinessUserRoles == null)
            {
                return roles;
            }

            roles = HetBusinessUserRoles
                .Where(x => x.Role != null && 
                            x.EffectiveDate <= DateTime.UtcNow && 
                            (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                .Select(x => x.Role).ToList();

            return roles;
        }               
    }
}
