/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HETSAPI.Models
{
    public partial class Equipment
    {
        private const float DUMP_TRUCK_CONSTANT = 60.0F;
        private const float OTHER_CONSTANT = 50.0F;

        public const string STATUS_ACTIVE = "Active";

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

        /// <summary>
        /// Determine if a Seniority Audit Record is required
        /// </summary>
        /// <param name="changed"></param>
        /// <returns>True if the changed record differes from this one</returns>
        public bool IsSeniorityAuditRequired( Equipment changed)            
        {
            bool result = false;
            if ( (this.Seniority != changed.Seniority)
                || (this.LocalArea != changed.LocalArea)
                || (this.BlockNumber != changed.BlockNumber)
                || (this.Owner != changed.Owner)
                || (this.ServiceHoursLastYear != changed.ServiceHoursLastYear)
                || (this.ServiceHoursTwoYearsAgo != changed.ServiceHoursTwoYearsAgo)
                || (this.ServiceHoursThreeYearsAgo != changed.ServiceHoursThreeYearsAgo)
                )
            {
                result = true;
            }
            return result;
        }
               
        
    }
}
