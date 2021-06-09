using System.Collections.Generic;

namespace HetsData.Helpers
{
    #region Merge Record Model

    public class MergeRecord
    {
        public int DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }
        public string EquipmentPrefix { get; set; }
        public int? DistrictId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public bool Master { get; set; }
        public int? MasterDistrictEquipmentTypeId { get; set; }
    }

    #endregion

    public class DistrictEquipmentTypeAgreementSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> AgreementIds { get; set; }
        public List<int?> ProjectIds { get; set; }
    }
}
