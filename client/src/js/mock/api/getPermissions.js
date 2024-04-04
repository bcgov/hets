export const getPermissions = () => ({
  data: [
    {
      id: 1,
      code: "Login",
      name: "Login",
      description: "Permission to login to the application and perform all Clerk functions within their designated District"
    },
    {
      id: 2,
      code: "UserManagement",
      name: "User Management",
      description: "Gives the user access to the User Management screens"
    },
    {
      id: 3,
      code: "RolesAndPermissions",
      name: "Roles and Permissions",
      description: "Gives the user access to the Roles and Permissions screens"
    },
    {
      id: 4,
      code: "Admin",
      name: "Administration",
      description: "Allows the user to perform special administrative tasks"
    },
    {
      id: 6,
      code: "CodeTableManagement",
      name: "Code Table Management",
      description: "Gives the user access to the Code Table Management screens"
    },
    {
      id: 7,
      code: "DistrictCodeTableManagement",
      name: "District Code Table Management",
      description: "Gives the user access to the District Code Table Management screens"
    },
    {
      id: 8,
      code: "BusinessLogin",
      name: "Business Login",
      description: "Permission to login to the business or owner facing application"
    },
    {
      id: 9,
      code: "DistrictRollover",
      name: "District Rollover",
      description: "Permission to kickoff the annual district rollover process"
    },
    {
      id: 10,
      code: "Version",
      name: "Version",
      description: "Permission to view application's version page"
    },
    {
      id: 11,
      code: "WriteAccess",
      name: "Write Access",
      description: "Permission to add, update, delete records"
    }
  ],
});