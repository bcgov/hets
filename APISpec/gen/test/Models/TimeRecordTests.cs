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
    ///  Class for testing the model TimeRecord
    /// </summary>
    
    public class TimeRecordModelTests
    {
        // TODO uncomment below to declare an instance variable for TimeRecord
        private TimeRecord instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public TimeRecordModelTests()
        {
            instance = new TimeRecord();
        }

    
        /// <summary>
        /// Test an instance of TimeRecord
        /// </summary>
        [Fact]
        public void TimeRecordInstanceTest()
        {
            Assert.IsType<TimeRecord>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            // TODO unit test for the property 'Id'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalAgreement'
        /// </summary>
        [Fact]
        public void RentalAgreementTest()
        {
            // TODO unit test for the property 'RentalAgreement'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'WorkedDate'
        /// </summary>
        [Fact]
        public void WorkedDateTest()
        {
            // TODO unit test for the property 'WorkedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Hours'
        /// </summary>
        [Fact]
        public void HoursTest()
        {
            // TODO unit test for the property 'Hours'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalAgreementRate'
        /// </summary>
        [Fact]
        public void RentalAgreementRateTest()
        {
            // TODO unit test for the property 'RentalAgreementRate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EnteredDate'
        /// </summary>
        [Fact]
        public void EnteredDateTest()
        {
            // TODO unit test for the property 'EnteredDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TimePeriod'
        /// </summary>
        [Fact]
        public void TimePeriodTest()
        {
            // TODO unit test for the property 'TimePeriod'
			Assert.True(true);
        }

	}
	
}

