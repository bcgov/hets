using System;
using System.Linq;
using HETSAPI.Models;
using System.Security.Claims;

namespace HETSAPI.Authorization
{
    /// <summary>
    /// Calaims Principal Extension
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Check if the user has permission to execute the method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool HasPermissions(this ClaimsPrincipal user, params string[] permissions)
        {
            if (!user.HasClaim(c => c.Type == User.PermissionClaim))
                return false;

            return permissions.Any(permission => !user.HasClaim(User.PermissionClaim, permission));
        }

        /// <summary>
        /// Check if the user is a member if the group
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool IsInGroup(this ClaimsPrincipal user, string group)
        {
            return user.HasClaim(c => c.Type == ClaimTypes.GroupSid && c.Value.Equals(group, StringComparison.OrdinalIgnoreCase));
        }
    }
}
