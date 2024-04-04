export const getCurrentUser = () => ({
  data: {
    id: 123,
    smUserId: "MNAME",
    surname: "Name",
    givenName: "My",
    displayName: "My Name",
    userGuid: "123456AB789012C3456D78901EF2A3BC",
    agreementCity: null,
    smAuthorizationDirectory: null,
    businessId: null,
    businessGuid: null,
    environment: "Development",
    businessUser: false,
    district: {
      id: 1,
      districtNumber: 1,
      name: "District 1",
      startDate: "1900-01-01T00:00:00Z",
      endDate: null,
      ministryDistrictId: 1,
      regionId: 1,
      concurrencyControlNumber: 1,
      region: null
    },
    userDistricts: [],
    userRoles: [
      {
        id: 1231,
        effectiveDate: "2023-01-01T12:00:00Z",
        expiryDate: "2099-01-01T12:00:00Z",
        userId: 123,
        roleId: 1,
        concurrencyControlNumber: 1,
        role: {
          id: 1,
          name: "System Administrator",
          description: "Full Access to the Whole System",
          concurrencyControlNumber: 1,
          rolePermissions: [
            {
              id: 27,
              permissionId: 11,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 11,
                code: "WriteAccess",
                name: "Write Access",
                description: "Permission to add, update, delete records",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 1,
              permissionId: 1,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 1,
                code: "Login",
                name: "Login",
                description: "Permission to login to the application and perform all Clerk functions within their designated District",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 2,
              permissionId: 7,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 7,
                code: "DistrictCodeTableManagement",
                name: "District Code Table Management",
                description: "Gives the user access to the District Code Table Management screens",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 3,
              permissionId: 6,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 6,
                code: "CodeTableManagement",
                name: "Code Table Management",
                description: "Gives the user access to the Code Table Management screens",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 4,
              permissionId: 2,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 2,
                code: "UserManagement",
                name: "User Management",
                description: "Gives the user access to the User Management screens",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 5,
              permissionId: 3,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 3,
                code: "RolesAndPermissions",
                name: "Roles and Permissions",
                description: "Gives the user access to the Roles and Permissions screens",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 6,
              permissionId: 4,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 4,
                code: "Admin",
                name: "Administration",
                description: "Allows the user to perform special administrative tasks",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 8,
              permissionId: 9,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 9,
                code: "DistrictRollover",
                name: "District Rollover",
                description: "Permission to kickoff the annual district rollover process",
                concurrencyControlNumber: 0
              }
            },
            {
              id: 9,
              permissionId: 10,
              roleId: 1,
              concurrencyControlNumber: 0,
              permission: {
                id: 10,
                code: "Version",
                name: "Version",
                description: "Permission to view application's version page",
                concurrencyControlNumber: 0
              }
            }
          ]
        }
      }
    ],
  },
});

export const getCurrentUserFavourites = () => ({
  data: [],
});
