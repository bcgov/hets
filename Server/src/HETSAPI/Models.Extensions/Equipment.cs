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
        /// Dump TRuck Constant
        /// </summary>
        private const float DUMP_TRUCK_CONSTANT = 60.0F;

        /// <summary>
        /// Non-Dump Truck Constant
        /// </summary>
        private const float OTHER_CONSTANT = 50.0F;

        /// <summary>
        /// Approved Status Code
        /// </summary>
        public const string STATUS_APPROVED = "Approved";

        /// <summary>
        /// Calculate the Seniority for a piece of equipment.
        /// </summary>
        public void CalculateSeniority()
        {
            /*
                Each piece of equipment has a Seniority Score that consists of:
                Years of Service* Constant +(Average number of hours worked per year for the last 3 Years)
                Constant is 60 for Dump Trucks and 50 for all other equipment.
                Example - 15 years of service Dump Truck with 456, 385 and 426 hours of service in the last 3 years, respectively
                Seniority = (15 * 60) + ((456 + 385 + 426) / 3)) = 1322(rounded)
            */

            float seniorityConstant = OTHER_CONSTANT;

            if (DumpTruck != null) // Dump trucks have a special constant
            {
                seniorityConstant = DUMP_TRUCK_CONSTANT;
            }

            float totalHoursWorked = 0.0F;

            if (ServiceHoursLastYear != null)
            {
                totalHoursWorked += (float) ServiceHoursLastYear;
            }

            if (ServiceHoursTwoYearsAgo != null)
            {
                totalHoursWorked += (float)ServiceHoursTwoYearsAgo;
            }

            if (ServiceHoursThreeYearsAgo != null)
            {
                totalHoursWorked += (float)ServiceHoursThreeYearsAgo;
            }

            float averageHoursWorked = totalHoursWorked / 3.0F;

            Seniority = (YearsOfService * seniorityConstant) + averageHoursWorked;
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

            YearsOfService = (float)yearDifference + remainder;                       
        }

        /// <summary>
        /// Determine if a Seniority Audit Record is required
        /// </summary>
        /// <param name="changed"></param>
        /// <returns>True if the changed record differes from this one</returns>
        public bool IsSeniorityAuditRequired(Equipment changed)            
        {
            bool result = (Seniority != changed.Seniority) || 
                          (LocalArea != changed.LocalArea) || 
                          (BlockNumber != changed.BlockNumber) || 
                          (Owner != changed.Owner) || 
                          (ServiceHoursLastYear != changed.ServiceHoursLastYear) || 
                          (ServiceHoursTwoYearsAgo != changed.ServiceHoursTwoYearsAgo) || 
                          (ServiceHoursThreeYearsAgo != changed.ServiceHoursThreeYearsAgo);

            return result;
        }

        /// <summary>
        /// Returns the YTD hours for a given year.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public float GetYTDServiceHours(DbAppContext context, int year)
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
