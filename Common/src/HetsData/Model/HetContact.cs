using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetContact
    {
        public HetContact()
        {
            HetOwner = new HashSet<HetOwner>();
            HetProject = new HashSet<HetProject>();
        }

        [JsonProperty("Id")]
        public int ContactId { get; set; }

        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Role { get; set; }
        public string Notes { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string FaxPhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetOwner Owner { get; set; }
        public HetProject Project { get; set; }

        [JsonIgnore]
        public ICollection<HetOwner> HetOwner { get; set; }

        [JsonIgnore]
        public ICollection<HetProject> HetProject { get; set; }
    }
}
