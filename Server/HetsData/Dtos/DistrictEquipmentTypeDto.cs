using Newtonsoft.Json;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class DistrictEquipmentTypeDto
    {
        public DistrictEquipmentTypeDto()
        {
            LocalAreas = new List<LocalAreaEquipmentDto>();
        }
        [JsonProperty("Id")]
        public int DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }
        public int? DistrictId { get; set; }
        public int? ServiceAreaId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public bool Deleted { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public EquipmentTypeDto EquipmentType { get; set; }
        public int EquipmentCount { get; set; }
        public List<LocalAreaEquipmentDto> LocalAreas { get; set; }
    }
}
