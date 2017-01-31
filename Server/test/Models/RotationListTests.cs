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
    ///  Class for testing the model RotationList
    /// </summary>
    
    public class RotationListModelTests
    {
        // TODO uncomment below to declare an instance variable for RotationList
        private RotationList instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RotationListModelTests()
        {
            instance = new RotationList();
        }

    
        /// <summary>
        /// Test an instance of RotationList
        /// </summary>
        [Fact]
        public void RotationListInstanceTest()
        {
            Assert.IsType<RotationList>(instance);  
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
        /// Test the property 'Blocks'
        /// </summary>
        [Fact]
        public void BlocksTest()
        {
            // TODO unit test for the property 'Blocks'
			Assert.True(true);
        }

	}
	
}

