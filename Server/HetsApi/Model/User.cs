using System.Collections.Generic;
using HetsData.Dtos;

namespace HetsApi.Model
{
    public class CurrentUserDto
    {
        public int Id { get; set; }
        public string SmUserId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string DisplayName { get; set; }
        public string UserGuid { get; set; }
        public string AgreementCity { get; set; }
        public string SmAuthorizationDirectory { get; set; }
        public int? BusinessId { get; set; }
        public string BusinessGuid { get; set; }
        public string Environment { get; set; }

        /// <summary>
        /// Business user flag
        /// </summary>
        public bool BusinessUser { get; set; }

        public DistrictDto District { get; set; }

        public List<UserDistrictDto> UserDistricts { get; set; }

        public List<UserRoleDto> UserRoles { get; set; }
    }
}
