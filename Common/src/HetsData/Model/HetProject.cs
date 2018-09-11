using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetProject
    {
        public HetProject()
        {
            HetContact = new HashSet<HetContact>();
            HetDigitalFile = new HashSet<HetDigitalFile>();
            HetHistory = new HashSet<HetHistory>();
            HetNote = new HashSet<HetNote>();
            HetRentalAgreement = new HashSet<HetRentalAgreement>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
        }

        [JsonProperty("Id")]
        public int ProjectId { get; set; }

        public string ProvincialProjectNumber { get; set; }
        public string Name { get; set; }
        public int ProjectStatusTypeId { get; set; }
        public string Information { get; set; }
        public int? DistrictId { get; set; }
        public int? PrimaryContactId { get; set; }
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
        public HetContact PrimaryContact { get; set; }
        public HetProjectStatusType ProjectStatusType { get; set; }

        [JsonProperty("Contacts")]
        public ICollection<HetContact> HetContact { get; set; }

        [JsonProperty("Attachments")]
        public ICollection<HetDigitalFile> HetDigitalFile { get; set; }

        [JsonProperty("History")]
        public ICollection<HetHistory> HetHistory { get; set; }

        [JsonProperty("Notes")]
        public ICollection<HetNote> HetNote { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreement> HetRentalAgreement { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }
    }
}
