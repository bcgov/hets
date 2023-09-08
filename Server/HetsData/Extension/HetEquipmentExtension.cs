using HetsCommon;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    public partial class HetEquipment
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusApproved = "Approved";
        public const string StatusArchived = "Archived";
        public const string StatusUnapproved = "Unapproved";
        public const string StatusPending = "Pending";

        // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
        [NotMapped]
        public bool ActiveRentalRequest { get; set; }

        [NotMapped]
        public int EquipmentNumber { get; set; }

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public int SenioritySortOrder { get; set; }

        [NotMapped]
        public bool IsHired { get; set; }

        [NotMapped]
        public float? HoursYtd { get; set; }

        [NotMapped]
        public int NumberOfBlocks { get; set; }

        [NotMapped]
        public int MaximumHours { get; set; }

        [NotMapped]
        public string SeniorityString { get; set; }

        [NotMapped]
        public string OwnerName { get; set; }

        [NotMapped]
        public string EquipmentType { get; set; }

        [NotMapped]
        public int AttachmentCount { get; set; }

        [NotMapped]
        public string YearMinus1 { get; set; }

        [NotMapped]
        public string YearMinus2 { get; set; }

        [NotMapped]
        public string YearMinus3 { get; set; }

        [NotMapped]
        public string EquipmentDetails
        {
            get
            {
                string temp = Year;

                if (!string.IsNullOrEmpty(temp)) temp = temp + " ";
                temp = temp + Make;

                if (!string.IsNullOrEmpty(Model))
                {
                    temp = temp + "/" + Model;
                }

                if (!string.IsNullOrEmpty(Size))
                {
                    temp = temp + "/" + Size;
                }

                return temp;
            }
        }

        /// <summary>
        /// Calculate the Seniority for a piece of equipment
        /// Each piece of equipment has a Seniority Score that consists of:
        /// 1. Years of Service* SeniorityScoreConstant + (Average number of hours worked per year for the last 3 Years)
        ///    * Constant is 60 for Dump Trucks and
        ///    * Constant is 50 for all other equipment types
        /// (constants are stored in the appSettings file)
        /// </summary>
        public void CalculateSeniority(int seniorityScoringConstant)
        {
            // get total hours worked over the last three years
            float totalHoursWorked = 0.0F;

            // default to 0
            Seniority = 0.0F;

            if (ServiceHoursLastYear != null)
            {
                totalHoursWorked += (float)ServiceHoursLastYear;
            }

            if (ServiceHoursTwoYearsAgo != null)
            {
                totalHoursWorked += (float)ServiceHoursTwoYearsAgo;
            }

            if (ServiceHoursThreeYearsAgo != null)
            {
                totalHoursWorked += (float)ServiceHoursThreeYearsAgo;
            }

            // average the hours over the last three years
            float averageHoursWorked = totalHoursWorked / 3.0F;

            // calculate seniority
            Seniority = (YearsOfService * seniorityScoringConstant) + averageHoursWorked;
        }

        /// <summary>
        /// Calculate Years of Service for Equipment
        /// </summary>
        /// <param name="now"></param>
        public void CalculateYearsOfService(DateTime now)
        {
            // ***********************************************************************************
            // Calculate Years of Service
            // ***********************************************************************************
            // Business Rules:
            // 1. When the equipment is added the years registered is set to a fraction of the
            //    fiscal left from the registered date to the end of current fiscal
            //    (decimals: 3 places)
            // 2. On roll over the years registered increments by one for each year the equipment
            //    stays active ((might need use the TO_DATE field to track when last it was rolled over)
            //    TO_DATE = END OF CURRENT FISCAL

            // determine end of current fiscal year
            DateTime fiscalEnd;

            if (now.Month == 1 || now.Month == 2 || now.Month == 3)
            {
                fiscalEnd = DateUtils.ConvertPacificToUtcTime(
                    new DateTime(now.Year, 3, 31, 0, 0, 0, DateTimeKind.Unspecified));
            }
            else
            {
                fiscalEnd = DateUtils.ConvertPacificToUtcTime(
                    new DateTime(now.AddYears(1).Year, 3, 31, 0, 0, 0, DateTimeKind.Unspecified));
            }

            // calculate and set the To Date
            YearsOfService++;
            ToDate = fiscalEnd;
        }

        /// <summary>
        /// Determine if a Seniority Audit Record is required
        /// </summary>
        /// <param name="changed"></param>
        /// <returns>True if the changed record differs from this one</returns>
        public bool IsSeniorityAuditRequired(HetEquipment changed)
        {
            // change to seniority -> write audit
            if (Seniority != null && Seniority != changed.Seniority)
            {
                return true;
            }

            // change to local area -> write audit
            if (LocalArea != null && LocalArea.LocalAreaId != changed.LocalArea.LocalAreaId)
            {
                return true;
            }

            // change to block number -> write audit
            if (BlockNumber != null && BlockNumber != changed.BlockNumber)
            {
                return true;
            }

            // change to owner -> write audit
            if (Owner != null && Owner.OwnerId != changed.Owner.OwnerId)
            {
                return true;
            }

            // change to service hours
            if (ServiceHoursLastYear != null && ServiceHoursLastYear != changed.ServiceHoursLastYear)
            {
                return true;
            }

            if (ServiceHoursTwoYearsAgo != null && ServiceHoursTwoYearsAgo != changed.ServiceHoursTwoYearsAgo)
            {
                return true;
            }

            if (ServiceHoursThreeYearsAgo != null && ServiceHoursThreeYearsAgo != changed.ServiceHoursThreeYearsAgo)
            {
                return true;
            }

            return false;
        }
    }
}