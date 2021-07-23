using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDistrict
    {
        public HetDistrict()
        {
            HetBatchReports = new HashSet<HetBatchReport>();
            HetConditionTypes = new HashSet<HetConditionType>();
            HetDistrictEquipmentTypes = new HashSet<HetDistrictEquipmentType>();
            HetProjects = new HashSet<HetProject>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
            HetServiceAreas = new HashSet<HetServiceArea>();
            HetUserDistricts = new HashSet<HetUserDistrict>();
            HetUserFavourites = new HashSet<HetUserFavourite>();
            HetUsers = new HashSet<HetUser>();
        }

        public int DistrictId { get; set; }
        public int? DistrictNumber { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MinistryDistrictId { get; set; }
        public int? RegionId { get; set; }
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

        public virtual HetRegion Region { get; set; }
        public virtual HetDistrictStatus HetDistrictStatus { get; set; }
        public virtual HetRolloverProgress HetRolloverProgress { get; set; }
        public virtual ICollection<HetBatchReport> HetBatchReports { get; set; }
        public virtual ICollection<HetConditionType> HetConditionTypes { get; set; }
        public virtual ICollection<HetDistrictEquipmentType> HetDistrictEquipmentTypes { get; set; }
        public virtual ICollection<HetProject> HetProjects { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual ICollection<HetServiceArea> HetServiceAreas { get; set; }
        public virtual ICollection<HetUserDistrict> HetUserDistricts { get; set; }
        public virtual ICollection<HetUserFavourite> HetUserFavourites { get; set; }
        public virtual ICollection<HetUser> HetUsers { get; set; }
    }
}
