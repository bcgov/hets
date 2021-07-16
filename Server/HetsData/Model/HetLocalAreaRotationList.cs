using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Model
{
    public partial class HetLocalAreaRotationList
    {
        public int LocalAreaRotationListId { get; set; }
        public int? LocalAreaId { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public int? AskNextBlock1Id { get; set; }
        public float? AskNextBlock1Seniority { get; set; }
        public int? AskNextBlock2Id { get; set; }
        public float? AskNextBlock2Seniority { get; set; }
        public int? AskNextBlockOpenId { get; set; }
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

        public virtual HetEquipment AskNextBlock1 { get; set; }
        public virtual HetEquipment AskNextBlock2 { get; set; }
        public virtual HetEquipment AskNextBlockOpen { get; set; }
        public virtual HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public virtual HetLocalArea LocalArea { get; set; }
    }
}
