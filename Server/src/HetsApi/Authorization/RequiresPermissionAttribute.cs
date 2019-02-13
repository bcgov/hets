using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

            /// <summary>
            /// Implements permission verification
            /// </summary>
            /// <param name="authService"></param>
            /// <param name="requiredPermissions"></param>
            public ImplementationRequiresPermissionAttribute(IAuthorizationService authService, PermissionRequirement requiredPermissions)
            {
                _authService = authService;
                _requiredPermissions = requiredPermissions;
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
                    context.Result = new UnauthorizedResult();

                    HttpResponse response = context.HttpContext.Response;

                    string responseText = "<HTML><HEAD><META http-equiv=\"Content - Type\" content=\"text / html; charset = windows - 1252\"></HEAD><BODY></BODY></HTML>";
                    byte[] data = Encoding.UTF8.GetBytes(responseText);

                    response.StatusCode = 403; // forbidden
                    response.Body.Write(data, 0, data.Length);
                    await response.Body.FlushAsync();
                }

                await next();
            }
        }
    }
}
