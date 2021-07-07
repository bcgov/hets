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
        public DateTime FiscalStartDate { get; set; }
        public DateTime? FiscalEndDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SupportingDocuments { get; set; }
        public int? DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
    }
}
