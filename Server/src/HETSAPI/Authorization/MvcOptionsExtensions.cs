using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using HETSAPI.Models;

namespace HetsApi.Authorization
{
    /// <summary>
    /// MVC Options Extension
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Add Authorization Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcOptions AddDefaultAuthorizationPolicyFilter(this MvcOptions options)
        {            
            Debug.WriteLine("Applying global authorization policy");
            
            // Default authorization policy enforced via a global authorization filter
            AuthorizationPolicy requireLoginPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireAssertion(                    
                    context => context.User.HasOnePermission(Permission.Login, Permission.BusinessLogin, Permission.ImportData)
                    )
                .Build();

            AuthorizeFilter filter = new AuthorizeFilter(requireLoginPolicy);
            options.Filters.Add(filter);
            return options;
        }
    }
}
