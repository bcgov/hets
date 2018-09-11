using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetUser
    {
        public HetUser()
        {
            HetUserDistrict = new HashSet<HetUserDistrict>();
            HetUserFavourite = new HashSet<HetUserFavourite>();
            HetUserRole = new HashSet<HetUserRole>();
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
        public bool Active { get; set; }
        public int? DistrictId { get; set; }
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

        public HetDistrict District { get; set; }

        [JsonIgnore]
        public ICollection<HetUserDistrict> HetUserDistrict { get; set; }

        [JsonIgnore]
        public ICollection<HetUserFavourite> HetUserFavourite { get; set; }

        [JsonIgnore]
        public ICollection<HetUserRole> HetUserRole { get; set; }
    }
}
