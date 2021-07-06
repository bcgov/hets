using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetsData.Dtos
{
    public class EquipmentStatusTypeDto
    {
        [JsonProperty("Id")]
        public int EquipmentStatusTypeId { get; set; }
        public string EquipmentStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
