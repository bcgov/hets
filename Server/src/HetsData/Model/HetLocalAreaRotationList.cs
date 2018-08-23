using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetLocalAreaRotationList
    {
        public int LocalAreaRotationListId { get; set; }
        public int? AskNextBlock1Id { get; set; }
        public float? AskNextBlock1Seniority { get; set; }
        public int? AskNextBlock2Id { get; set; }
        public float? AskNextBlock2Seniority { get; set; }
        public int? AskNextBlockOpenId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public int? LocalAreaId { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment AskNextBlock1 { get; set; }
        public HetEquipment AskNextBlock2 { get; set; }
        public HetEquipment AskNextBlockOpen { get; set; }
        public HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public HetLocalArea LocalArea { get; set; }
    }
}
