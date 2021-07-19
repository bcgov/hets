using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using HetsData.Entities;

namespace HetsApi.Authorization
{
    /// <summary>
    /// Permission Handler Extension
    /// </summary>
    public static class PermissionHandlerExtensions
    {
        /// <summary>
        /// Registers <see cref="PermissionHandler"/> with Dependency Injection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterPermissionHandler(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private ILogger _logger;
        private IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            foreach (var permission in requirement.RequiredPermissions)
            {
                if (!user.HasClaim(HetUser.PermissionClaim, permission))
                {
                    _logger.Information("RequiresPermission - {user} - {url} - {permission}", user.Identity.Name, _httpContextAccessor.HttpContext.Request.Path, permission);

                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

}
