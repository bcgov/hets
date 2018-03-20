using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HETSAPI.Models
{
    /// <summary>
    /// Equipment Database Model Extension
    /// </summary>
    public sealed partial class Equipment
    {        
        /// <summary>
        /// Approved Status Code
        /// </summary>
        public const string StatusApproved = "Approved";
        public const string StatusArchived = "Archived";
        public const string StatusPending = "Unapproved";

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

            // determine end of current fscal year
            DateTime fiscalEnd;

            if (now.Month == 1 || now.Month == 2 || now.Month == 3)
            {
                fiscalEnd = new DateTime(now.Year, 3, 31);
            }
            else
            {
                fiscalEnd = new DateTime(now.AddYears(1).Year, 3, 31);
            }

            // calculate and set the To Date
            YearsOfService = YearsOfService + 1;
            ToDate = fiscalEnd;               
        }

        /// <summary>
        /// Determine if a Seniority Audit Record is required
        /// </summary>
        /// <param name="changed"></param>
        /// <returns>True if the changed record differes from this one</returns>
        public bool IsSeniorityAuditRequired(Equipment changed)
        {            
            // change to seniority -> write audit
            if (Seniority != null && Seniority != changed.Seniority)
            {
                return true;
            }

            // change to local area -> write audit
            if (LocalArea != null && LocalArea.Id != changed.LocalArea.Id)            
            {
                return true;
            }

            // change to block number -> write audit
            if (BlockNumber != null && BlockNumber != changed.BlockNumber)
            {
                return true;
            }

            // change to owner -> write audit
            if (Owner != null && Owner.Id != changed.Owner.Id)
            {
                return true;
            }

            // chenge to service hours
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

        /// <summary>
        /// Returns the YTD hours for a given year.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public float GetYtdServiceHours(DbAppContext context)
        {
            float result = 0.0F;

            // *******************************************************************************
            // determine current fscal year - check for existing rotation lists this year
            // *******************************************************************************
            DateTime fiscalStart;

            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
            }
            else
            {
                fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
            }

            // *******************************************************************************
            // get all the time data for the current fiscal year
            // *******************************************************************************
            float? summation = context.TimeRecords
                   .Include(x => x.RentalAgreement.Equipment)
                   .Where(x => x.RentalAgreement.Equipment.Id == Id && 
                               x.WorkedDate >= fiscalStart)
                   .Sum(x => x.Hours);

            if (summation != null)
            {
                result = (float)summation;
            }

            return result;
        }
    }
}
