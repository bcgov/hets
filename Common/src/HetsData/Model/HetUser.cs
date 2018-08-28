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

        public bool Active { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string Guid { get; set; }
        public string Initials { get; set; }
        public string SmAuthorizationDirectory { get; set; }
        public string SmUserId { get; set; }
        public string Surname { get; set; }
        public int? DistrictId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
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
