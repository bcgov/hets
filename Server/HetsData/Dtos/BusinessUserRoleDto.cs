using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class BusinessUserRoleDto
    {
        [JsonProperty("Id")]
        public int BusinessUserRoleId { get; set; }

        private DateTime _effectiveDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime EffectiveDate {
            get => DateTime.SpecifyKind(_effectiveDate, DateTimeKind.Utc);
            set => _effectiveDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime? _expiryDate;
        public DateTime? ExpiryDate {
            get => _expiryDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _expiryDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int? BusinessUserId { get; set; }
        public int? RoleId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RoleDto Role { get; set; }
    }
}
