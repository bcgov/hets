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
    ///  Class for testing the model DistrictEquipmentType
    /// </summary>
    
    public class DistrictEquipmentTypeModelTests
    {
        // TODO uncomment below to declare an instance variable for DistrictEquipmentType
        private DistrictEquipmentType instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public DistrictEquipmentTypeModelTests()
        {
            instance = new DistrictEquipmentType();
        }

    
        /// <summary>
        /// Test an instance of DistrictEquipmentType
        /// </summary>
        [Fact]
        public void DistrictEquipmentTypeInstanceTest()
        {
            Assert.IsType<DistrictEquipmentType>(instance);  
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
        /// Test the property 'EquipmentType'
        /// </summary>
        [Fact]
        public void EquipmentTypeTest()
        {
            // TODO unit test for the property 'EquipmentType'
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
        /// Test the property 'DistrictEquipmentName'
        /// </summary>
        [Fact]
        public void DistrictEquipmentNameTest()
        {
            // TODO unit test for the property 'DistrictEquipmentName'
			Assert.True(true);
        }

	}
	
}

