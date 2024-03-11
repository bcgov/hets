using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class RentalRequestDto
    {
        public RentalRequestDto()
        {
            RentalRequestAttachments = new List<RentalRequestAttachmentDto>();
            RentalRequestRotationList = new List<RentalRequestRotationListDto>();
        }

        [JsonProperty("Id")]
        public int RentalRequestId { get; set; }
        public int EquipmentCount { get; set; }

        private DateTime? _expectedStartDate;
        public DateTime? ExpectedStartDate {
            get => _expectedStartDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _expectedStartDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        private DateTime? _expectedEndDate;
        public DateTime? ExpectedEndDate {
            get => _expectedEndDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _expectedEndDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int? ExpectedHours { get; set; }
        public int? FirstOnRotationListId { get; set; }
        public int RentalRequestStatusTypeId { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public int? LocalAreaId { get; set; }
        public int? ProjectId { get; set; }
        public int FiscalYear { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictEquipmentTypeDto DistrictEquipmentType { get; set; }
        public LocalAreaDto LocalArea { get; set; }
        public ProjectDto Project { get; set; }
        public List<RentalRequestAttachmentDto> RentalRequestAttachments { get; set; }
        public List<RentalRequestRotationListDto> RentalRequestRotationList { get; set; }

        public int YesCount { get; set; }
        public int NumberOfBlocks { get; set; }
        public string Status { get; set; }
        public string LocalAreaName { get; set; }
    }
}
