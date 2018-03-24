using Hangfire.Dashboard;

namespace HETSAPI.Authorization
{    
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }    
}
