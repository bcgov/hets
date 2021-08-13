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
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
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
