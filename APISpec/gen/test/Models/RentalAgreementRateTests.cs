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
    ///  Class for testing the model RentalAgreementRate
    /// </summary>
    
    public class RentalAgreementRateModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalAgreementRate
        private RentalAgreementRate instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalAgreementRateModelTests()
        {
            instance = new RentalAgreementRate();
        }

    
        /// <summary>
        /// Test an instance of RentalAgreementRate
        /// </summary>
        [Fact]
        public void RentalAgreementRateInstanceTest()
        {
            Assert.IsType<RentalAgreementRate>(instance);  
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
        /// Test the property 'ComponentName'
        /// </summary>
        [Fact]
        public void ComponentNameTest()
        {
            // TODO unit test for the property 'ComponentName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsAttachment'
        /// </summary>
        [Fact]
        public void IsAttachmentTest()
        {
            // TODO unit test for the property 'IsAttachment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Rate'
        /// </summary>
        [Fact]
        public void RateTest()
        {
            // TODO unit test for the property 'Rate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PercentOfEquipmentRate'
        /// </summary>
        [Fact]
        public void PercentOfEquipmentRateTest()
        {
            // TODO unit test for the property 'PercentOfEquipmentRate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RatePeriod'
        /// </summary>
        [Fact]
        public void RatePeriodTest()
        {
            // TODO unit test for the property 'RatePeriod'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Comment'
        /// </summary>
        [Fact]
        public void CommentTest()
        {
            // TODO unit test for the property 'Comment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TimeRecords'
        /// </summary>
        [Fact]
        public void TimeRecordsTest()
        {
            // TODO unit test for the property 'TimeRecords'
			Assert.True(true);
        }

	}
	
}

