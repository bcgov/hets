using HetsData;
using System.Security.Claims;

namespace HetsApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static (string username, string userGuid, string directory, string bizGuid, string bizName, string email) GetUserInfo(this ClaimsPrincipal principal)
        {
            var preferredUsername = principal.FindFirstValue("preferred_username");

            var usernames = preferredUsername?.Split("@");
            var username = principal.FindFirstValue("idir_username");
            var directory = usernames?[1].ToUpperInvariant();

            var userGuidClaim = directory == Constants.IDIR ? "idir_user_guid" : "bceid_user_guid";

            var userGuid = principal.FindFirstValue(userGuidClaim)?.ToUpperInvariant();

            var bizGuid = directory == Constants.IDIR ? "" : principal.FindFirstValue("bceid_business_guid");
            var bizName = directory == Constants.IDIR ? "" : principal.FindFirstValue("bceid_business_name");

            var email = principal.FindFirstValue(ClaimTypes.Email);

            return (username, userGuid, directory, bizGuid, bizName, email);
        }
    }
}
