using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetSeniorityAudit
    {
        [JsonProperty("Id")]
        public int SeniorityAuditId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OwnerOrganizationName { get; set; }
        public float? Seniority { get; set; }
        public int? BlockNumber { get; set; }
        public bool? IsSeniorityOverridden { get; set; }
        public string SeniorityOverrideReason { get; set; }
        public float? ServiceHoursLastYear { get; set; }
        public float? ServiceHoursThreeYearsAgo { get; set; }
        public float? ServiceHoursTwoYearsAgo { get; set; }
        public int? EquipmentId { get; set; }
        public int? LocalAreaId { get; set; }
        public int? OwnerId { get; set; }
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

        public HetEquipment Equipment { get; set; }
        public HetLocalArea LocalArea { get; set; }
        public HetOwner Owner { get; set; }
    }
}
