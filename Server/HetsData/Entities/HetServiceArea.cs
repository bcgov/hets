using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetServiceArea
    {
        public HetServiceArea()
        {
            HetLocalAreas = new HashSet<HetLocalArea>();
        }

        public int ServiceAreaId { get; set; }
        public string Name { get; set; }
        public int? AreaNumber { get; set; }
        public int MinistryServiceAreaId { get; set; }
        public DateTime FiscalStartDate { get; set; }
        public DateTime? FiscalEndDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SupportingDocuments { get; set; }
        public int? DistrictId { get; set; }
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
        public virtual ICollection<HetLocalArea> HetLocalAreas { get; set; }
    }
}
