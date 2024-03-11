using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class ServiceAreaDto
    {
        [JsonProperty("Id")]
        public int ServiceAreaId { get; set; }
        public string Name { get; set; }
        public int? AreaNumber { get; set; }
        public int MinistryServiceAreaId { get; set; }

        private DateTime _fiscalStartDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime FiscalStartDate {
            get => DateTime.SpecifyKind(_fiscalStartDate, DateTimeKind.Utc);
            set => _fiscalStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime? _fiscalEndDate;
        public DateTime? FiscalEndDate {
            get => _fiscalEndDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _fiscalEndDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SupportingDocuments { get; set; }
        public int? DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
    }
}
