using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HetsCommon;
using HetsData.Dtos;
using HetsData.Entities;

namespace HetsData.Helpers
{
    #region Time Record Models   

    public class TimeRecordLite
    {
        public string EquipmentCode { get; set; }
        public string ProjectName { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public float? HoursYtd { get; set; }
        public int MaximumHours { get; set; }
        public List<TimeRecordDto> TimeRecords { get; set; }
    }

    public class TimeRecordSearchLite
    {
        public int Id { get; set; }
        public float? Hours { get; set; }
        public DateTime WorkedDate { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string LocalAreaName { get; set; }
        public int LocalAreaId { get; set; }
        public int ServiceAreaId { get; set; }
        public int OwnerId { get; set; }
        public int RentalAgreementId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerCode { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentPrefix { get; set; }
        public int EquipmentNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string Year { get; set; }
        public int ProjectId { get; set; }
        public string ProvincialProjectNumber { get; set; }
    }    

    #endregion

    public static class TimeRecordHelper
    {
        #region Convert full time record search result to a "Lite" version

        /// <summary>
        /// Convert to Time Record Lite Model
        /// </summary>
        /// <param name="timeRecord"></param>
        public static TimeRecordSearchLite ToLiteModel(HetTimeRecord timeRecord)
        {
            TimeRecordSearchLite timeLite = new();

            if (timeRecord != null)
            {
                timeLite.Id = timeRecord.TimeRecordId;
                timeLite.Hours = timeRecord.Hours;
                timeLite.WorkedDate = DateUtils.AsUTC(timeRecord.WorkedDate);
                timeLite.EnteredDate = timeRecord.EnteredDate is DateTime enteredDateUtc ? 
                    DateUtils.AsUTC(enteredDateUtc) : null;
                timeLite.RentalAgreementId = timeRecord.RentalAgreement.RentalAgreementId;
                timeLite.LocalAreaId = timeRecord.RentalAgreement.Equipment.LocalArea.LocalAreaId;
                timeLite.LocalAreaName = timeRecord.RentalAgreement.Equipment.LocalArea.Name;
                timeLite.ServiceAreaId = timeRecord.RentalAgreement.Equipment.LocalArea.ServiceArea.ServiceAreaId;

                timeLite.OwnerId = timeRecord.RentalAgreement.Equipment.Owner.OwnerId;
                timeLite.OwnerCode = timeRecord.RentalAgreement.Equipment.Owner.OwnerCode;
                timeLite.OwnerName = timeRecord.RentalAgreement.Equipment.Owner.OrganizationName;

                timeLite.EquipmentId = timeRecord.RentalAgreement.Equipment.EquipmentId;
                timeLite.EquipmentCode = timeRecord.RentalAgreement.Equipment.EquipmentCode;
                timeLite.EquipmentPrefix = Regex.Match(timeRecord.RentalAgreement.Equipment.EquipmentCode, @"^[^\d-]+").Value;
                timeLite.EquipmentNumber = int.Parse(Regex.Match(timeRecord.RentalAgreement.Equipment.EquipmentCode, @"\d+").Value);
                timeLite.Make = timeRecord.RentalAgreement.Equipment.Make;
                timeLite.Model = timeRecord.RentalAgreement.Equipment.Model;                
                timeLite.Size = timeRecord.RentalAgreement.Equipment.Size;
                timeLite.Year = timeRecord.RentalAgreement.Equipment.Year;

                timeLite.ProjectId = timeRecord.RentalAgreement.Project.ProjectId;
                timeLite.ProvincialProjectNumber = timeRecord.RentalAgreement.Project.ProvincialProjectNumber;
            }

            return timeLite;
        }

        #endregion        
    }
}
