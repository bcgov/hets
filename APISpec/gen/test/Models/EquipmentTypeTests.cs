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
    ///  Class for testing the model EquipmentType
    /// </summary>
    
    public class EquipmentTypeModelTests
    {
        // TODO uncomment below to declare an instance variable for EquipmentType
        private EquipmentType instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentTypeModelTests()
        {
            instance = new EquipmentType();
        }

    
        /// <summary>
        /// Test an instance of EquipmentType
        /// </summary>
        [Fact]
        public void EquipmentTypeInstanceTest()
        {
            Assert.IsType<EquipmentType>(instance);  
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
        /// Test the property 'Name'
        /// </summary>
        [Fact]
        public void NameTest()
        {
            // TODO unit test for the property 'Name'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsDumpTruck'
        /// </summary>
        [Fact]
        public void IsDumpTruckTest()
        {
            // TODO unit test for the property 'IsDumpTruck'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlueBookSection'
        /// </summary>
        [Fact]
        public void BlueBookSectionTest()
        {
            // TODO unit test for the property 'BlueBookSection'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'NumberOfBlocks'
        /// </summary>
        [Fact]
        public void NumberOfBlocksTest()
        {
            // TODO unit test for the property 'NumberOfBlocks'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlueBookRateNumber'
        /// </summary>
        [Fact]
        public void BlueBookRateNumberTest()
        {
            // TODO unit test for the property 'BlueBookRateNumber'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'MaximumHours'
        /// </summary>
        [Fact]
        public void MaximumHoursTest()
        {
            // TODO unit test for the property 'MaximumHours'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExtendHours'
        /// </summary>
        [Fact]
        public void ExtendHoursTest()
        {
            // TODO unit test for the property 'ExtendHours'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'MaxHoursSub'
        /// </summary>
        [Fact]
        public void MaxHoursSubTest()
        {
            // TODO unit test for the property 'MaxHoursSub'
			Assert.True(true);
        }

	}
	
}

