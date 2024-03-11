using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class RentalAgreementDto
    {
        public RentalAgreementDto()
        {
            RentalAgreementConditions = new List<RentalAgreementConditionDto>();
            RentalAgreementRates = new List<RentalAgreementRateDto>();
            TimeRecords = new List<TimeRecordDto>();
        }

        [JsonProperty("Id")]
        public int RentalAgreementId { get; set; }
        public string Number { get; set; }
        public int? EstimateHours { get; set; }

        private DateTime? _estimateStartWork;
        public DateTime? EstimateStartWork {
            get => _estimateStartWork is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _estimateStartWork = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string Note { get; set; }
        public float? EquipmentRate { get; set; }
        public string RateComment { get; set; }
        public int RatePeriodTypeId { get; set; }

        private DateTime? _datedOn;
        public DateTime? DatedOn {
            get => _datedOn is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _datedOn = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string AgreementCity { get; set; }
        public int RentalAgreementStatusTypeId { get; set; }
        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
        public int? DistrictId { get; set; }
        public int? RentalRequestId { get; set; }
        public int? RentalRequestRotationListId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public EquipmentDto Equipment { get; set; }
        public ProjectDto Project { get; set; }
        public DistrictDto District { get; set; }
        public RatePeriodTypeDto RatePeriodType { get; set; }
        public RentalAgreementStatusTypeDto RentalAgreementStatusType { get; set; }
        public RentalRequestDto RentalRequest { get; set; }
        public RentalRequestRotationListDto RentalRequestRotationList { get; set; }
        public List<RentalAgreementConditionDto> RentalAgreementConditions { get; set; }
        public List<RentalAgreementRateDto> RentalAgreementRates { get; set; }
        public List<TimeRecordDto> TimeRecords { get; set; }

        public string Status { get; set; }
        public string RatePeriod { get; set; }
        public string LocalAreaName { get; set; }
        public List<RentalAgreementRateDto> OvertimeRates { get; set; }
    }
}
