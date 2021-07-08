using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RolePermissionDto
    {
        [JsonProperty("Id")]
        public int RolePermissionId { get; set; }
        public int? PermissionId { get; set; }
        public int? RoleId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public PermissionDto Permission { get; set; }
        public RoleDto Role { get; set; }
    }
}
