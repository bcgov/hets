export const getUserDistricts = () => ({
  data: [
    {
      id: 1231,
      isPrimary: true,
      userId: 123,
      districtId: 1,
      concurrencyControlNumber: 1,
      district: {
        id: 1,
        districtNumber: 1,
        name: "District 1",
        startDate: "1900-01-01T00:00:00Z",
        endDate: null,
        ministryDistrictId: 1,
        regionId: 3,
        concurrencyControlNumber: 1,
        region: null
      },
      user: {
        id: 123,
        surname: "Name",
        givenName: "My",
        initials: null,
        smUserId: "MNAME",
        smAuthorizationDirectory: null,
        guid: "123456AB789012C3456D78901EF2A3BC",
        email: "my.name@gov.bc.ca",
        agreementCity: "",
        active: true,
        districtId: 1,
        concurrencyControlNumber: 2,
        district: null,
        userDistricts: [],
        userRoles: []
      }
    }
  ],
});