using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace HETSAPI.Authorization
{
    /// <summary>
    /// Allows declarative claims based permissions to be applied to controller methods for authorization.
    /// </summary>
    /// <remarks>
    /// This attribute works well at the controller level.  Refer to <see cref="HETSAPI.Controllers.TestController"/>
    /// for examples.  That said this attribute is not triggered by default when applied to methods inside
    /// a service implementation of a controller.  For instance, many of the controllers in the project are
    /// auto-generated with the implementation being deferred to a service implementation which is injected
    /// at runtime (good pattern).  This attribute will work when applied directly to methods of the controller, 
    /// but will not be triggered when applied to the corresponding methods of the implementation.
    /// 
    /// As a workaround you can use the <see cref="HETSAPI.Authorization.ClaimsPrincipalExtensions.HasPermissions"/> 
    /// extension method, <see cref="HETSAPI.Controllers.TestController"/> contains samples of this too.
    /// You will need to have the HttpContext injected into the service implementation via dependency injection in order
    /// to access the User (ClaimsPrincipal) in order to perform the permissions check.    
    /// </remarks>
    /// <see href="http://benjamincollins.com/blog/practical-permission-based-authorization-in-asp-net-core/"/>
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Attribute Extension - Permissions Required
        /// </summary>
        /// <param name="permissions"></param>
        public RequiresPermissionAttribute(params string[] permissions)
           : base(typeof(ImplementationRequiresPermissionAttribute))
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
                AuthorizationResult result = await _authService.AuthorizeAsync(context.HttpContext.User,
                    context.ActionDescriptor.DisplayName,
                    _requiredPermissions);

                if (!result.Succeeded)
                {
                    context.Result = new UnauthorizedResult();
                    await context.Result.ExecuteResultAsync(context);
                }           

                await next();                
            }
        }
    }
}
