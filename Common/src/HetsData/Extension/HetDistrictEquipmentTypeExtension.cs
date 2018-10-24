using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetDistrictEquipmentType
    {
        [NotMapped]
        public int EquipmentCount { get; set; }

        [NotMapped]
        public List<LocalAreaEquipment> LocalAreas { get; set; } = new List<LocalAreaEquipment>();
    }

    public class LocalAreaEquipment
    {
        [NotMapped]
        public int Id { get; set; }

        [NotMapped]
        public string Name { get; set; }
    
        [NotMapped]
        public int EquipmentCount { get; set; }
    }
}
