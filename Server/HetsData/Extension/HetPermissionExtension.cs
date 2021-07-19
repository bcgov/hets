using System.Collections.Generic;

namespace HetsData.Entities
{
    /// <summary>
    /// Permission Database Model Extension
    /// </summary>
    public partial class HetPermission
    {
        /// <summary>
        /// Login (UI) Permission
        /// </summary>
        public const string Login = "Login";

        /// <summary>
        /// User Management Permission
        /// </summary>
        public const string UserManagement = "UserManagement";

        /// <summary>
        /// Roles and Permissions Permission
        /// </summary>
        public const string RolesAndPermissions = "RolesAndPermissions";

        /// <summary>
        /// Admin Permission
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Code Table Management Permission
        /// </summary>
        public const string CodeTableManagement = "CodeTableManagement";

        /// <summary>
        /// District Code Table Management Permission
        /// </summary>
        public const string DistrictCodeTableManagement = "DistrictCodeTableManagement";

        /// <summary>
        /// Business Login Permission
        /// </summary>
        public const string BusinessLogin = "BusinessLogin";

        /// <summary>
        /// District Rollover Permission
        /// </summary>
        public const string DistrictRollover = "DistrictRollover";

        /// <summary>
        /// Version Page Permission
        /// </summary>
        public const string Version = "Version";

        /// <summary>
        /// Write Access Permission
        /// </summary>
        public const string WriteAccess = "WriteAccess";

        /// <summary>
        /// All Permissions List
        /// </summary>
        public static readonly IEnumerable<HetPermission> AllPermissions = new List<HetPermission>
        {
            new HetPermission
            {
                Code = Login,
                Name = "Login",
                Description = "Permission to login to the application and perform all Clerk functions within their designated District"
            },
            new HetPermission
            {
                Code = UserManagement,
                Name = "User Management",
                Description = "Gives the user access to the User Management screens"
            },
            new HetPermission
            {
                Code = RolesAndPermissions,
                Name = "Roles and Permissions",
                Description = "Gives the user access to the Roles and Permissions screens"
            },
            new HetPermission
            {
                Code = Admin,
                Name = "Admin",
                Description = "Allows the user to perform special administrative tasks"
            },
            new HetPermission
            {
                Code = CodeTableManagement,
                Name = "Code Table Management",
                Description = "Gives the user access to the Code Table Management screens"
            },
            new HetPermission
            {
                Code = DistrictCodeTableManagement,
                Name = "District Code Table Management",
                Description = "Gives the user access to the District Code Table Management screens"
            },
            new HetPermission
            {
                Code = BusinessLogin,
                Name = "Business Login",
                Description = "Permission to login to the business or owner facing application"
            },
            new HetPermission
            {
                Code = DistrictRollover,
                Name = "District Rollover",
                Description = "Permission to kickoff the annual district rollover process"
            },
            new HetPermission
            {
                Code = Version,
                Name = "Version",
                Description = "Permission to view application's version page"
            },
            new HetPermission
            {
                Code = WriteAccess,
                Name = "WriteAccess",
                Description = "Permistion to add, update and delete records"
            }
        };
    }
}