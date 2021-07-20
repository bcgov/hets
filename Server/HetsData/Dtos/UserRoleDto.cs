using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class UserRoleDto
    {
        [JsonProperty("Id")]
        public int UserRoleId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RoleDto Role { get; set; }
    }
}
