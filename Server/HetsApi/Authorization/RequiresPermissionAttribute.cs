using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using HetsApi.Model;
using Microsoft.Extensions.Configuration;

namespace HetsApi.Authorization
{
    /// <summary>
    /// Allows declarative claims based permissions to be applied to controller methods for authorization.
    /// </summary>
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Attribute Extension - Permissions Required
        /// </summary>
        /// <param name="permissions"></param>
        public RequiresPermissionAttribute(params string[] permissions) : base(typeof(ImplementationRequiresPermissionAttribute))
        {
            Arguments = new object[] { new PermissionRequirement(permissions) };
        }

        /// <summary>
        /// Permission verification
        /// </summary>
        public class ImplementationRequiresPermissionAttribute : Attribute, IAsyncResourceFilter
        {
            private readonly IAuthorizationService _authService;
            private readonly PermissionRequirement _requiredPermissions;
            private readonly IConfiguration _configuration;

            /// <summary>
            /// Implements permission verification
            /// </summary>
            /// <param name="authService"></param>
            /// <param name="requiredPermissions"></param>
            public ImplementationRequiresPermissionAttribute(IAuthorizationService authService, PermissionRequirement requiredPermissions, IConfiguration configuration)
            {
                _authService = authService;
                _requiredPermissions = requiredPermissions;
                _configuration = configuration;
            }

            /// <summary>
            /// Validate authorization
            /// </summary>
            /// <param name="context"></param>
            /// <param name="next"></param>
            /// <returns></returns>
            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                // check the context - ignore certain paths
                string url = context.HttpContext.Request.Path;

                Debug.WriteLine("Authorizing request (user: {0} | path: {1})", context.HttpContext.User.Identity.Name, url);

                if (url.Contains("/authentication/dev") ||
                    url.Contains("/error") ||
                    url.Contains("/hangfire") ||
                    url.Contains("/swagger") ||
                    url.Contains(".map") ||
                    url.Contains(".png") ||
                    url.Contains(".css") ||
                    url.Contains(".ico") ||
                    url.Contains(".eot") ||
                    url.Contains(".woff") ||
                    url.Contains(".ttf") ||
                    url.Contains(".js"))
                {
                    await next();
                }

                AuthorizationResult result = await _authService.AuthorizeAsync(context.HttpContext.User,
                    context.ActionDescriptor.DisplayName,
                    _requiredPermissions);

                if (!result.Succeeded)
                {
                    context.Result = new BadRequestObjectResult(new HetsResponse("HETS-43", ErrorViewModel.GetDescription("HETS-43", _configuration)));
                }
                else
                {
                    await next();
                }

            }
        }
    }
}
