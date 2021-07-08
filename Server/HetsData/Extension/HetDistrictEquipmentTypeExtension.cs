using HetsData.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetDistrictEquipmentType
    {
        [NotMapped]
        public int EquipmentCount { get; set; }

        [NotMapped]
        public List<LocalAreaEquipmentDto> LocalAreas { get; set; } = new List<LocalAreaEquipmentDto>();
    }
}
