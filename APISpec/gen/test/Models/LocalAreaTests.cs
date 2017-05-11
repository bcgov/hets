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
    ///  Class for testing the model LocalArea
    /// </summary>
    
    public class LocalAreaModelTests
    {
        // TODO uncomment below to declare an instance variable for LocalArea
        private LocalArea instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public LocalAreaModelTests()
        {
            instance = new LocalArea();
        }

    
        /// <summary>
        /// Test an instance of LocalArea
        /// </summary>
        [Fact]
        public void LocalAreaInstanceTest()
        {
            Assert.IsType<LocalArea>(instance);  
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
        /// Test the property 'LocalAreaNumber'
        /// </summary>
        [Fact]
        public void LocalAreaNumberTest()
        {
            // TODO unit test for the property 'LocalAreaNumber'
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
        /// Test the property 'ServiceArea'
        /// </summary>
        [Fact]
        public void ServiceAreaTest()
        {
            // TODO unit test for the property 'ServiceArea'
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

	}
	
}

