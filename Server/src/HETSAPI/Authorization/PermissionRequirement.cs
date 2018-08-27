using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HetsApi.Authorization
{
    /// <summary>
    /// Permission Requirements
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// All required permissions
        /// </summary>
        public IEnumerable<string> RequiredPermissions { get; }

        /// <summary>
        /// Set required permissions
        /// </summary>
        /// <param name="permissions"></param>
        public PermissionRequirement(params string[] permissions)
        {
            RequiredPermissions = permissions;
        }
    }
}
