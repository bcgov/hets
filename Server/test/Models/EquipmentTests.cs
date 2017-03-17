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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI;
using HETSAPI.Models;
using System.Reflection;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model Equipment
    /// </summary>
    
    public class EquipmentModelTests
    {
        private Equipment instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentModelTests()
        {
            instance = new Equipment();
        }

    
        /// <summary>
        /// Test an instance of Equipment
        /// </summary>
        [Fact]
        public void EquipmentInstanceTest()
        {
            Assert.IsType<Equipment>(instance);  
        }

        /// <summary>
        /// Test the years of service calculation
        /// </summary>
        [Fact]
        public void YearsOfServiceTest()
        {
            // Sept. 18, 2012 should be 4.54 for 2017
            instance.ReceivedDate = new DateTime(2012, 9, 18);
            instance.CalculateYearsOfService(2017);
            float yearsOfService = (float) instance.YearsOfService;
            Assert.Equal(4.53424644F, yearsOfService);
        }     
        
        [Fact]   
        public void SeniorityTest()
        {
            /*
                Each piece of equipment has a Seniority Score that consists of:
                Years of Service* Constant +(Average number of hours worked per year for the last 3 Years)
                Constant is 60 for Dump Trucks and 50 for all other equipment.
                Example - 15 years of service Dump Truck with 456, 385 and 426 hours of service in the last 3 years, respectively
                Seniority = (15 * 60) + ((456 + 385 + 426) / 3)) = 1322(rounded)
            */
            instance.YearsOfService = 15.0F;
            instance.ServiceHoursLastYear = 456;
            instance.ServiceHoursTwoYearsAgo = 385;
            instance.ServiceHoursThreeYearsAgo = 426;
            instance.DumpTruck = new DumpTruck();
            instance.CalculateSeniority();

            Assert.Equal(instance.Seniority, 1322.33337F);
        }

    }	
}

