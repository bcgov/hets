using System;
using HETSAPI.Models;
using System.Security.Claims;

namespace HETSAPI.Authorization
{
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
            bool hasRequiredPermissions = false;

            if (user.HasClaim(c => c.Type == User.PERMISSION_CLAIM))
            {
                bool hasPermissions = true;
                foreach (string permission in permissions)
                {
                    if (!user.HasClaim(User.PERMISSION_CLAIM, permission))
                    {
                        hasPermissions = false;
                        break;
                    }
                }

                hasRequiredPermissions = hasPermissions;
            }
            return hasRequiredPermissions;
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
