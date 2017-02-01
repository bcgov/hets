using System;
using HETSAPI.Models;
using System.Security.Claims;

namespace HETSAPI.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
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

        public static bool IsInGroup(this ClaimsPrincipal user, string group)
        {
            return user.HasClaim(c => c.Type == ClaimTypes.GroupSid && c.Value.Equals(group, StringComparison.OrdinalIgnoreCase));
        }
    }
}
