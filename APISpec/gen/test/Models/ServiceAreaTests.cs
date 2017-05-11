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
    ///  Class for testing the model ServiceArea
    /// </summary>
    
    public class ServiceAreaModelTests
    {
        // TODO uncomment below to declare an instance variable for ServiceArea
        private ServiceArea instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ServiceAreaModelTests()
        {
            instance = new ServiceArea();
        }

    
        /// <summary>
        /// Test an instance of ServiceArea
        /// </summary>
        [Fact]
        public void ServiceAreaInstanceTest()
        {
            Assert.IsType<ServiceArea>(instance);  
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
        /// Test the property 'MinistryServiceAreaID'
        /// </summary>
        [Fact]
        public void MinistryServiceAreaIDTest()
        {
            // TODO unit test for the property 'MinistryServiceAreaID'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Name'
        /// </summary>
        [Fact]
        public void NameTest()
        {
            // TODO unit test for the property 'Name'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'District'
        /// </summary>
        [Fact]
        public void DistrictTest()
        {
            // TODO unit test for the property 'District'
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
        /// Test the property 'AreaNumber'
        /// </summary>
        [Fact]
        public void AreaNumberTest()
        {
            // TODO unit test for the property 'AreaNumber'
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

	}
	
}

