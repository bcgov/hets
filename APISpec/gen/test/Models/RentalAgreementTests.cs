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
    ///  Class for testing the model RentalAgreement
    /// </summary>
    
    public class RentalAgreementModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalAgreement
        private RentalAgreement instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalAgreementModelTests()
        {
            instance = new RentalAgreement();
        }

    
        /// <summary>
        /// Test an instance of RentalAgreement
        /// </summary>
        [Fact]
        public void RentalAgreementInstanceTest()
        {
            Assert.IsType<RentalAgreement>(instance);  
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
        /// Test the property 'Number'
        /// </summary>
        [Fact]
        public void NumberTest()
        {
            // TODO unit test for the property 'Number'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Equipment'
        /// </summary>
        [Fact]
        public void EquipmentTest()
        {
            // TODO unit test for the property 'Equipment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Project'
        /// </summary>
        [Fact]
        public void ProjectTest()
        {
            // TODO unit test for the property 'Project'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Note'
        /// </summary>
        [Fact]
        public void NoteTest()
        {
            // TODO unit test for the property 'Note'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EstimateStartWork'
        /// </summary>
        [Fact]
        public void EstimateStartWorkTest()
        {
            // TODO unit test for the property 'EstimateStartWork'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DatedOn'
        /// </summary>
        [Fact]
        public void DatedOnTest()
        {
            // TODO unit test for the property 'DatedOn'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EstimateHours'
        /// </summary>
        [Fact]
        public void EstimateHoursTest()
        {
            // TODO unit test for the property 'EstimateHours'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentRate'
        /// </summary>
        [Fact]
        public void EquipmentRateTest()
        {
            // TODO unit test for the property 'EquipmentRate'
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
        /// Test the property 'RateComment'
        /// </summary>
        [Fact]
        public void RateCommentTest()
        {
            // TODO unit test for the property 'RateComment'
			Assert.True(true);
        }

	}
	
}

