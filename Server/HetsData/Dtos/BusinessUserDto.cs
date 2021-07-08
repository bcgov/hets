using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class BusinessUserDto
    {
        public BusinessUserDto()
        {
            UserRoles = new List<BusinessUserRoleDto>();
        }

        [JsonProperty("Id")]
        public int BusinessUserId { get; set; }
        public string BceidUserId { get; set; }
        public string BceidGuid { get; set; }
        public string BceidDisplayName { get; set; }
        public string BceidFirstName { get; set; }
        public string BceidLastName { get; set; }
        public string BceidEmail { get; set; }
        public string BceidTelephone { get; set; }
        public int? BusinessId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public BusinessDto Business { get; set; }
        public List<BusinessUserRoleDto> UserRoles { get; set; }
    }
}
