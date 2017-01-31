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
            Assert.IsType<int>(instance.Id);
        }
        /// <summary>
        /// Test the property 'GeneratedTime'
        /// </summary>
        [Fact]
        public void GeneratedTimeTest()
        {
            // TODO unit test for the property 'GeneratedTime'
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
        /// Test the property 'BlockNum'
        /// </summary>
        [Fact]
        public void BlockNumTest()
        {
            // TODO unit test for the property 'BlockNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipCd'
        /// </summary>
        [Fact]
        public void EquipCdTest()
        {
            // TODO unit test for the property 'EquipCd'
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
        /// Test the property 'OwnerName'
        /// </summary>
        [Fact]
        public void OwnerNameTest()
        {
            // TODO unit test for the property 'OwnerName'
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
        /// Test the property 'YTD'
        /// </summary>
        [Fact]
        public void YTDTest()
        {
            // TODO unit test for the property 'YTD'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD1'
        /// </summary>
        [Fact]
        public void YTD1Test()
        {
            // TODO unit test for the property 'YTD1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD2'
        /// </summary>
        [Fact]
        public void YTD2Test()
        {
            // TODO unit test for the property 'YTD2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD3'
        /// </summary>
        [Fact]
        public void YTD3Test()
        {
            // TODO unit test for the property 'YTD3'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CycleHrsWrk'
        /// </summary>
        [Fact]
        public void CycleHrsWrkTest()
        {
            // TODO unit test for the property 'CycleHrsWrk'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FrozenOut'
        /// </summary>
        [Fact]
        public void FrozenOutTest()
        {
            // TODO unit test for the property 'FrozenOut'
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
        /// Test the property 'Working'
        /// </summary>
        [Fact]
        public void WorkingTest()
        {
            // TODO unit test for the property 'Working'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YearEndReg'
        /// </summary>
        [Fact]
        public void YearEndRegTest()
        {
            // TODO unit test for the property 'YearEndReg'
			Assert.True(true);
        }

	}
	
}

