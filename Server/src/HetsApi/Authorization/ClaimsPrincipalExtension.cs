using System;
using System.Diagnostics;
using System.Security.Claims;
using HetsData.Model;

namespace HetsApi.Authorization
{
    /// <summary>
    /// Claims Principal Extension
    /// </summary>
    public static class ClaimsPrincipalExtension
    {        
        /// <summary>
        /// Check if the user has permission to execute the method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool HasPermissions(this ClaimsPrincipal user, params string[] permissions)
        {            
            if (!user.HasClaim(c => c.Type == HetUser.PermissionClaim))
                return false;

            bool hasRequiredPermissions = false;

            if (!user.HasClaim(c => c.Type == HetUser.PermissionClaim))
                return false;

            if (user.HasClaim(c => c.Type == HetUser.PermissionClaim))                
            {
                bool hasPermissions = true;
                
                foreach (string permission in permissions)
                {
                    if (!user.HasClaim(HetUser.PermissionClaim, permission))
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
        /// Check if the user has one of the required permission to execute the method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool HasOnePermission(this ClaimsPrincipal user, params string[] permissions)
        {
            Debug.WriteLine(string.Format("Global authorization policy checking for permission ({0})", user.Identity.Name));

            if (!user.HasClaim(c => c.Type == HetUser.PermissionClaim))
                return false;

            bool hasRequiredPermissions = false;

            if (!user.HasClaim(c => c.Type == HetUser.PermissionClaim))
                return false;

            if (user.HasClaim(c => c.Type == HetUser.PermissionClaim))
            {
                bool hasPermissions = false;

                foreach (string permission in permissions)
                {
                    if (user.HasClaim(HetUser.PermissionClaim, permission))
                    {
                        hasPermissions = true;
                        break;
                    }
                }

                hasRequiredPermissions = hasPermissions;
            }

            Debug.WriteLine(hasRequiredPermissions
                ? string.Format("Global authorization policy - permission granted ({0})", user.Identity.Name)
                : string.Format("Global authorization policy - permission denied ({0})", user.Identity.Name));

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
