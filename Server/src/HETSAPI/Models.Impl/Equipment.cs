using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HETSAPI.Models
{
    public partial class Equipment
    {
        const float DUMP_TRUCK_CONSTANT = 60.0F;
        const float OTHER_CONSTANT = 50.0F;

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

            if (this.DumpTruck != null) // Dump trucks have a special constant
            {
                seniorityConstant = DUMP_TRUCK_CONSTANT;
            }

            float averageHoursWorked = ((float)this.ServiceHoursLastYear + (float)this.ServiceHoursTwoYearsAgo + (float)this.ServiceHoursThreeYearsAgo) / 3.0F;

            this.Seniority = (this.YearsOfService * seniorityConstant) + averageHoursWorked;
        }

        public void CalculateYearsOfService(int year)
        {
            /*  
             *  Years of service is calculated annually as Apr. 1 - Date Registered as a decimal number of years. 
             *  E.g. for FY 2017, equipment registered on Sept. 18, 2012 would be 4.54 years.
             *  The Excel - yearfrac() function can be used to verify years of service calculations              
             */

            DateTime dateRegistered = (DateTime) this.ReceivedDate;

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

            System.TimeSpan diff = april1.Subtract(dateRegistered);

            float remainder = (float)(diff.TotalDays / daysInYear);

            this.YearsOfService = (float)yearDifference + remainder;
                       
        }
    }
}
