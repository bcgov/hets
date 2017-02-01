using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HETSAPI.Authentication
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSiteminderAuthenticationHandler();
            app.UseDevAuthenticationHandler(env);
            return app;
        }
    }
}
