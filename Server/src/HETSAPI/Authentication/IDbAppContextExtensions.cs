using System;
using HETSAPI.Models;

namespace HETSAPI.Authentication
{
    /// <summary>
    /// Db Extension - Validates User Credential against the HETS Database
    /// </summary>
    public static class DbAppContextExtensions
    {
        /// <summary>
        /// Load User from HETS database using their userId and guid
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static User LoadUser(this IDbAppContext context, string userId, string guid = null)
        {
            User user = null;

            if (!string.IsNullOrEmpty(guid))
                user = context.GetUserByGuid(guid);

            if (user == null)
                user = context.GetUserBySmUserId(userId);

            if (user == null)
                return null;

            if (guid == null)
                return user;

            if (string.IsNullOrEmpty(user.Guid))
            {
                // self register (write the users Guid to thd db)
                user.Guid = guid;
                context.SaveChanges();
            }
            else if (!user.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase))
            {
                // invalid account - guid doesn't match user credential
                return null;
            }

            return user;
        }
    }
}
