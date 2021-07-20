using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
            UserDistricts = new List<UserDistrictDto>();
            UserRoles = new List<UserRoleDto>();
        }

        [JsonProperty("Id")]
        public int UserId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string SmUserId { get; set; }
        public string SmAuthorizationDirectory { get; set; }
        public string Guid { get; set; }
        public string Email { get; set; }
        public string AgreementCity { get; set; }
        public bool Active { get; set; }
        public int? DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public List<UserDistrictDto> UserDistricts { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}
