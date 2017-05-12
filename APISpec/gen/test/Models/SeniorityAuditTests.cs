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
    ///  Class for testing the model SeniorityAudit
    /// </summary>
    
    public class SeniorityAuditModelTests
    {
        // TODO uncomment below to declare an instance variable for SeniorityAudit
        private SeniorityAudit instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public SeniorityAuditModelTests()
        {
            instance = new SeniorityAudit();
        }

    
        /// <summary>
        /// Test an instance of SeniorityAudit
        /// </summary>
        [Fact]
        public void SeniorityAuditInstanceTest()
        {
            Assert.IsType<SeniorityAudit>(instance);  
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
        /// Test the property 'StartDate'
        /// </summary>
        [Fact]
        public void StartDateTest()
        {
            // TODO unit test for the property 'StartDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EndDate'
        /// </summary>
        [Fact]
        public void EndDateTest()
        {
            // TODO unit test for the property 'EndDate'
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
        /// Test the property 'Equipment'
        /// </summary>
        [Fact]
        public void EquipmentTest()
        {
            // TODO unit test for the property 'Equipment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlockNumber'
        /// </summary>
        [Fact]
        public void BlockNumberTest()
        {
            // TODO unit test for the property 'BlockNumber'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Owner'
        /// </summary>
        [Fact]
        public void OwnerTest()
        {
            // TODO unit test for the property 'Owner'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OwnerOrganizationName'
        /// </summary>
        [Fact]
        public void OwnerOrganizationNameTest()
        {
            // TODO unit test for the property 'OwnerOrganizationName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Seniority'
        /// </summary>
        [Fact]
        public void SeniorityTest()
        {
            // TODO unit test for the property 'Seniority'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursLastYear'
        /// </summary>
        [Fact]
        public void ServiceHoursLastYearTest()
        {
            // TODO unit test for the property 'ServiceHoursLastYear'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursTwoYearsAgo'
        /// </summary>
        [Fact]
        public void ServiceHoursTwoYearsAgoTest()
        {
            // TODO unit test for the property 'ServiceHoursTwoYearsAgo'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursThreeYearsAgo'
        /// </summary>
        [Fact]
        public void ServiceHoursThreeYearsAgoTest()
        {
            // TODO unit test for the property 'ServiceHoursThreeYearsAgo'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsSeniorityOverridden'
        /// </summary>
        [Fact]
        public void IsSeniorityOverriddenTest()
        {
            // TODO unit test for the property 'IsSeniorityOverridden'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SeniorityOverrideReason'
        /// </summary>
        [Fact]
        public void SeniorityOverrideReasonTest()
        {
            // TODO unit test for the property 'SeniorityOverrideReason'
			Assert.True(true);
        }

	}
	
}

