using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class BusinessUserRoleDto
    {
        [JsonProperty("Id")]
        public int BusinessUserRoleId { get; set; }

        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? BusinessUserId { get; set; }
        public int? RoleId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RoleDto Role { get; set; }
    }
}
