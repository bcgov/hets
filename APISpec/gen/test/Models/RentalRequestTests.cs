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
    ///  Class for testing the model RentalRequest
    /// </summary>
    
    public class RentalRequestModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalRequest
        private RentalRequest instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalRequestModelTests()
        {
            instance = new RentalRequest();
        }

    
        /// <summary>
        /// Test an instance of RentalRequest
        /// </summary>
        [Fact]
        public void RentalRequestInstanceTest()
        {
            Assert.IsType<RentalRequest>(instance);  
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
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DistrictEquipmentType'
        /// </summary>
        [Fact]
        public void DistrictEquipmentTypeTest()
        {
            // TODO unit test for the property 'DistrictEquipmentType'
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
        /// Test the property 'FirstOnRotationList'
        /// </summary>
        [Fact]
        public void FirstOnRotationListTest()
        {
            // TODO unit test for the property 'FirstOnRotationList'
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
        /// <summary>
        /// Test the property 'RentalRequestAttachments'
        /// </summary>
        [Fact]
        public void RentalRequestAttachmentsTest()
        {
            // TODO unit test for the property 'RentalRequestAttachments'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalRequestRotationList'
        /// </summary>
        [Fact]
        public void RentalRequestRotationListTest()
        {
            // TODO unit test for the property 'RentalRequestRotationList'
			Assert.True(true);
        }

	}
	
}

