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
    ///  Class for testing the model Region
    /// </summary>    
    public class RegionModelTests
    {
        private readonly Region instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RegionModelTests()
        {
            instance = new Region();
        }

    
        /// <summary>
        /// Test an instance of Region
        /// </summary>
        [Fact]
        public void RegionInstanceTest()
        {
            Assert.IsType<Region>(instance);  
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
        /// Test the property 'MinistryRegionID'
        /// </summary>
        [Fact]
        public void MinistryRegionIDTest()
        {
            // TODO unit test for the property 'MinistryRegionID'
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

