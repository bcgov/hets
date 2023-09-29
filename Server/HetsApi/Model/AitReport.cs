using HetsCommon;
using HetsData.Entities;
using Newtonsoft.Json;
using System;

namespace HetsApi.Model
{
    /// <summary>
    /// View Model for AIT Report
    /// </summary>
    public class AitReport
    {
        [JsonProperty("Id")]
        public int RentalAgreementId { get; set; }

        /// <summary>
        /// Rental Agreement Number
        /// </summary>
        public string RentalAgreementNumber { get; set; }

        public int? EquipmentId { get; set; }
        public string EquipmentCode { get; set; }

        public int? DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }

        public int? ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }

        public DateTime? DatedOn { get; set; }
        public DateTime? StartDate { get; set; }

        public static AitReport MapFromHetRentalAgreement(HetRentalAgreement agreement)
        {
            var report = new AitReport
            {
                RentalAgreementId = agreement.RentalAgreementId,
                RentalAgreementNumber = agreement.Number,

                EquipmentId = agreement.EquipmentId,
                EquipmentCode = agreement.Equipment?.EquipmentCode,

                DistrictEquipmentTypeId = agreement.Equipment?.DistrictEquipmentType?.DistrictEquipmentTypeId,
                DistrictEquipmentName = agreement.Equipment?.DistrictEquipmentType?.DistrictEquipmentName,

                ProjectId = agreement.ProjectId,
                ProjectNumber = agreement.Project?.ProvincialProjectNumber,
                ProjectName = agreement.Project?.Name,

                DatedOn = agreement.DatedOn is DateTime datedOnUtc ? 
                    DateUtils.AsUTC(datedOnUtc) : null,
                    
                StartDate = agreement.EstimateStartWork is DateTime estimateStartWorkUtc ? 
                    DateUtils.AsUTC(estimateStartWorkUtc) : null,
            };

            return report;
        }
    }
}
