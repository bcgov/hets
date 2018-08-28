using System.Linq;
using Microsoft.AspNetCore.Http;
using HetsData.Model;

namespace HetsApi.Helpers
{
    public static class UserHelper
    {
        public static string GetUserId(HttpContext httpContext)
        {
            string userId = httpContext.User.Identity.Name;
            return userId;
        }

        public static HetUser GetUser(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            HetUser user = context.HetUser.FirstOrDefault(x => x.SmUserId == userId);
            return user;
        }

        public static int? GetUsersDistrictId(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            int? districtId = context.HetUser.FirstOrDefault(x => x.SmUserId == userId)?.DistrictId;
            return districtId;
        }        
    }
}
