using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetProvincialRateType
    {
        public string RateType { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string PeriodType { get; set; }
        public float? Rate { get; set; }
        public bool Overtime { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool IsPercentRate { get; set; }
        public bool IsRateEditable { get; set; }
        public bool IsInTotalEditable { get; set; }
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
    }
}
