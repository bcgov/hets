using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetProject
    {
        public HetProject()
        {
            HetAttachment = new HashSet<HetAttachment>();
            HetContact = new HashSet<HetContact>();
            HetHistory = new HashSet<HetHistory>();
            HetNote = new HashSet<HetNote>();
            HetRentalAgreement = new HashSet<HetRentalAgreement>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
        }

        [JsonProperty("Id")]
        public int ProjectId { get; set; }

        public int? PrimaryContactId { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public int? DistrictId { get; set; }
        public string Information { get; set; }
        public string Name { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string Status { get; set; }
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
        public HetContact PrimaryContact { get; set; }

        [JsonIgnore]
        public ICollection<HetAttachment> HetAttachment { get; set; }

        [JsonIgnore]
        public ICollection<HetContact> HetContact { get; set; }

        [JsonIgnore]
        public ICollection<HetHistory> HetHistory { get; set; }

        [JsonIgnore]
        public ICollection<HetNote> HetNote { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreement> HetRentalAgreement { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }
    }
}
