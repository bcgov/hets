using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetRegion
    {
        public HetRegion()
        {
            HetDistrict = new HashSet<HetDistrict>();
        }

        public int RegionId { get; set; }
        public DateTime? EndDate { get; set; }
        public int MinistryRegionId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public int? RegionNumber { get; set; }
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

        public ICollection<HetDistrict> HetDistrict { get; set; }
    }
}
