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
    ///  Class for testing the model LocalAreaRotationList
    /// </summary>
    
    public class LocalAreaRotationListModelTests
    {
        // TODO uncomment below to declare an instance variable for LocalAreaRotationList
        private LocalAreaRotationList instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public LocalAreaRotationListModelTests()
        {
            instance = new LocalAreaRotationList();
        }

    
        /// <summary>
        /// Test an instance of LocalAreaRotationList
        /// </summary>
        [Fact]
        public void LocalAreaRotationListInstanceTest()
        {
            Assert.IsType<LocalAreaRotationList>(instance);  
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
        /// Test the property 'DistrictEquipmentType'
        /// </summary>
        [Fact]
        public void DistrictEquipmentTypeTest()
        {
            // TODO unit test for the property 'DistrictEquipmentType'
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
        /// Test the property 'AskNextBlock1'
        /// </summary>
        [Fact]
        public void AskNextBlock1Test()
        {
            // TODO unit test for the property 'AskNextBlock1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskNextBlock1Seniority'
        /// </summary>
        [Fact]
        public void AskNextBlock1SeniorityTest()
        {
            // TODO unit test for the property 'AskNextBlock1Seniority'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskNextBlock2'
        /// </summary>
        [Fact]
        public void AskNextBlock2Test()
        {
            // TODO unit test for the property 'AskNextBlock2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskNextBlock2Seniority'
        /// </summary>
        [Fact]
        public void AskNextBlock2SeniorityTest()
        {
            // TODO unit test for the property 'AskNextBlock2Seniority'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskNextBlockOpen'
        /// </summary>
        [Fact]
        public void AskNextBlockOpenTest()
        {
            // TODO unit test for the property 'AskNextBlockOpen'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskNextBlockOpenSeniority'
        /// </summary>
        [Fact]
        public void AskNextBlockOpenSeniorityTest()
        {
            // TODO unit test for the property 'AskNextBlockOpenSeniority'
			Assert.True(true);
        }

	}
	
}

