using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDistrictStatus
    {
        public int DistrictId { get; set; }
        public int? CurrentFiscalYear { get; set; }
        public int? NextFiscalYear { get; set; }
        public DateTime? RolloverStartDate { get; set; }
        public DateTime? RolloverEndDate { get; set; }
        public int? LocalAreaCount { get; set; }
        public int? DistrictEquipmentTypeCount { get; set; }
        public int? LocalAreaCompleteCount { get; set; }
        public int? DistrictEquipmentTypeCompleteCount { get; set; }
        public int? ProgressPercentage { get; set; }
        public bool DisplayRolloverMessage { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string DbCreateUserId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public virtual HetDistrict District { get; set; }
    }
}
