using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetsData.Dtos
{
    public class RoleDto
    {
        public RoleDto()
        {
            RolePermissions = new List<RolePermissionDto>();
        }

        [JsonProperty("Id")]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public List<RolePermissionDto> RolePermissions { get; set; }
    }
}
