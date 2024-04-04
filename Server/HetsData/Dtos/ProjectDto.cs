using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class ProjectDto
    {
        public ProjectDto()
        {
            Contacts = new List<ContactDto>();
            RentalAgreements = new List<RentalAgreementDto>();
            RentalRequests = new List<RentalRequestDto>();
        }

        [JsonProperty("Id")]
        public int ProjectId { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public string Name { get; set; }
        public int ProjectStatusTypeId { get; set; }
        public string Information { get; set; }
        public string FiscalYear { get; set; }
        public string ResponsibilityCentre { get; set; }
        public string ServiceLine { get; set; }
        public string Stob { get; set; }
        public string Product { get; set; }
        public string BusinessFunction { get; set; }
        public string WorkActivity { get; set; }
        public string CostType { get; set; }
        public int? DistrictId { get; set; }
        public int? PrimaryContactId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public ContactDto PrimaryContact { get; set; }
        public List<ContactDto> Contacts { get; set; }
        public List<RentalAgreementDto> RentalAgreements { get; set; }
        public List<RentalRequestDto> RentalRequests { get; set; }

        public bool CanEditStatus { get; set; }
        public string Status { get; set; }

        private DateTime _fiscalYearStartDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime FiscalYearStartDate {
            get => DateTime.SpecifyKind(_fiscalYearStartDate, DateTimeKind.Utc);
            set => _fiscalYearStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
