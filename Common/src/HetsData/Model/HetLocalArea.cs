using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetLocalArea
    {
        public HetLocalArea()
        {
            HetEquipment = new HashSet<HetEquipment>();
            HetLocalAreaRotationList = new HashSet<HetLocalAreaRotationList>();
            HetOwner = new HashSet<HetOwner>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
            HetSeniorityAudit = new HashSet<HetSeniorityAudit>();
        }

        [JsonProperty("Id")]
        public int LocalAreaId { get; set; }

        public int LocalAreaNumber { get; set; }
        public string Name { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int? ServiceAreaId { get; set; }
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

        public HetServiceArea ServiceArea { get; set; }

        [JsonIgnore]
        public ICollection<HetEquipment> HetEquipment { get; set; }

        [JsonIgnore]
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationList { get; set; }

        [JsonIgnore]
        public ICollection<HetOwner> HetOwner { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }

        [JsonIgnore]
        public ICollection<HetSeniorityAudit> HetSeniorityAudit { get; set; }
    }
}
