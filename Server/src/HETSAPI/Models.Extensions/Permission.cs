using System;
using System.Collections.Generic;

namespace HETSAPI.Models
{
    /// <summary>
    /// Permission Database Model Extension
    /// </summary>
    public partial class Permission : IEquatable<Permission>
    {
        /// <summary>
        /// Login Permission
        /// </summary>
        public const string LOGIN = "LOGIN";

        /// <summary>
        /// User Management Permission
        /// </summary>
        public const string USER_MANAGEMENT = "USER_MANAGEMENT";

        /// <summary>
        /// Roles and Permissions Permission
        /// </summary>
        public const string ROLES_AND_PERMISSIONS = "ROLES_AND_PERMISSIONS";

        /// <summary>
        /// Admin Perission
        /// </summary>
        public const string ADMIN = "ADMIN";

        /// <summary>
        /// Recieve Notification Permission
        /// </summary>
        public const string RECEIVE_NOTIFICATIONS = "RECEIVE_NOTIFICATIONS";

        /// <summary>
        /// Assign Inspections Permission
        /// </summary>
        public const string ASSIGN_INSPECTORS = "ASSIGN_INSPECTORS";

        /// <summary>
        /// All Permissions List
        /// </summary>
        public static readonly IEnumerable<Permission> ALL_PERMISSIONS = new List<Permission>
        {
            new Permission
            {
                Code = LOGIN,
                Name = "Login",
                Description = "Permission to login to the application"
            },
            new Permission
            {
                Code = USER_MANAGEMENT,
                Name = "User Management",
                Description = "Gives the user access to the User Management screens"
            },
            new Permission
            {
                Code = ROLES_AND_PERMISSIONS,
                Name = "Roles and Permissions",
                Description = "Gives the user access to the Roles and Permissions screens"
            },
            new Permission
            {
                Code = ADMIN,
                Name = "Admin",
                Description = "Allows the user to perform special administrative tasks."
            },
            new Permission()
            {
                Code = RECEIVE_NOTIFICATIONS,
                Name = "Receive Notifications",
                Description = "Enables the user to receive notifications intended for the user's group."
            },
            new Permission()
            {
                Code = ASSIGN_INSPECTORS,
                Name = "Assign Inspectors",
                Description = "A user granted this permission will be able to assign inspectors."
            }
        };
    }
}
