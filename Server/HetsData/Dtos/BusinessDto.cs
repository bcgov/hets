using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class BusinessDto
    {
        public BusinessDto()
        {
            Owners = new List<OwnerDto>();
            BusinessUsers = new List<BusinessUserDto>();
        }

        [JsonProperty("Id")]
        public int BusinessId { get; set; }
        public string BceidLegalName { get; set; }
        public string BceidDoingBusinessAs { get; set; }
        public string BceidBusinessNumber { get; set; }
        public string BceidBusinessGuid { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public List<OwnerDto> Owners { get; set; }
        public List<BusinessUserDto> BusinessUsers { get; set; }
        public OwnerDto LinkedOwner { get; set; }
    }
}
