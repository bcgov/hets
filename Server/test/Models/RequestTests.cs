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
    ///  Class for testing the model Request
    /// </summary>
    
    public class RequestModelTests
    {
        // TODO uncomment below to declare an instance variable for Request
        private Request instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RequestModelTests()
        {
            instance = new Request();
        }

    
        /// <summary>
        /// Test an instance of Request
        /// </summary>
        [Fact]
        public void RequestInstanceTest()
        {
            Assert.IsType<Request>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            Assert.IsType<int>(instance.Id);
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
        /// Test the property 'LocalArea'
        /// </summary>
        [Fact]
        public void LocalAreaTest()
        {
            // TODO unit test for the property 'LocalArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentType'
        /// </summary>
        [Fact]
        public void EquipmentTypeTest()
        {
            // TODO unit test for the property 'EquipmentType'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentCount'
        /// </summary>
        [Fact]
        public void EquipmentCountTest()
        {
            // TODO unit test for the property 'EquipmentCount'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExpectedHours'
        /// </summary>
        [Fact]
        public void ExpectedHoursTest()
        {
            // TODO unit test for the property 'ExpectedHours'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExpectedStartDate'
        /// </summary>
        [Fact]
        public void ExpectedStartDateTest()
        {
            // TODO unit test for the property 'ExpectedStartDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExpectedEndDate'
        /// </summary>
        [Fact]
        public void ExpectedEndDateTest()
        {
            // TODO unit test for the property 'ExpectedEndDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RotationList'
        /// </summary>
        [Fact]
        public void RotationListTest()
        {
            // TODO unit test for the property 'RotationList'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'HireOffers'
        /// </summary>
        [Fact]
        public void HireOffersTest()
        {
            // TODO unit test for the property 'HireOffers'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Notes'
        /// </summary>
        [Fact]
        public void NotesTest()
        {
            // TODO unit test for the property 'Notes'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Attachments'
        /// </summary>
        [Fact]
        public void AttachmentsTest()
        {
            // TODO unit test for the property 'Attachments'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'History'
        /// </summary>
        [Fact]
        public void HistoryTest()
        {
            // TODO unit test for the property 'History'
			Assert.True(true);
        }

	}
	
}

