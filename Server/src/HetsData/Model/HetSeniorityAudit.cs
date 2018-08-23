using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetSeniorityAudit
    {
        public int SeniorityAuditId { get; set; }
        public int? BlockNumber { get; set; }
        public int? EquipmentId { get; set; }
        public DateTime StartDate { get; set; }
        public int? LocalAreaId { get; set; }
        public int? OwnerId { get; set; }
        public float? Seniority { get; set; }
        public float? ServiceHoursLastYear { get; set; }
        public float? ServiceHoursThreeYearsAgo { get; set; }
        public float? ServiceHoursTwoYearsAgo { get; set; }
        public DateTime EndDate { get; set; }
        public string OwnerOrganizationName { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public bool? IsSeniorityOverridden { get; set; }
        public string SeniorityOverrideReason { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment Equipment { get; set; }
        public HetLocalArea LocalArea { get; set; }
        public HetOwner Owner { get; set; }
    }
}
