using Hangfire.Dashboard;

namespace HetsApi.Authorization
{    
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }    
}
