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
        /// <param name="year"></param>
        public void CalculateYearsOfService(int year)
        {
            /*  
             *  Years of service is calculated annually as Apr. 1 - Date Registered as a decimal number of years. 
             *  E.g. for FY 2017, equipment registered on Sept. 18, 2012 would be 4.54 years.
             *  The Excel - yearfrac() function can be used to verify years of service calculations              
             */

            DateTime dateRegistered = ReceivedDate;

            // first calculate the year difference.
            DateTime april1 = new DateTime(year, 4, 1);

            int yearDifference = april1.Year - dateRegistered.Year;

            DateTime firstApril1 = new DateTime(dateRegistered.Year, 4, 1);

            if (dateRegistered > firstApril1)
            {
                // reduce the year count by 1.
                --yearDifference;
            }

            dateRegistered = dateRegistered.AddYears( yearDifference);

            // now calculate the difference in the last fiscal year.

            // since the date is calculated from April 1, we are only concerned about sitations where the current year is a leap year.
            float daysInYear = 365.0F;

            if (DateTime.IsLeapYear(april1.Year))
            {
                daysInYear = 366.0F;
            }                            

            TimeSpan diff = april1.Subtract(dateRegistered);

            float remainder = (float)(diff.TotalDays / daysInYear);

            YearsOfService = yearDifference + remainder;                       
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
        /// <param name="year"></param>
        /// <returns></returns>
        public float GetYtdServiceHours(DbAppContext context, int year)
        {
            float result = 0.0F;

            DateTime startingDate = new DateTime(year, 3, 31);

            // get all the time data since then
            float? summation = context.TimeRecords
                   .Include(x => x.RentalAgreement.Equipment)
                   .Where(x => x.RentalAgreement.Equipment.Id == Id && 
                               x.WorkedDate >= startingDate)
                   .Sum(x => x.Hours);

            if (summation != null)
            {
                result = (float)summation;
            }

            return result;
        }
    }
}
