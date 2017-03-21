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
    
    public class EquipmentTypeModelTests
    {
        // TODO uncomment below to declare an instance variable for DistrictEquipmentType
        private DistrictEquipmentType instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentTypeModelTests()
        {
            instance = new DistrictEquipmentType();
        }

    
        /// <summary>
        /// Test an instance of DistrictEquipmentType
        /// </summary>
        [Fact]
        public void EquipmentTypeInstanceTest()
        {
            Assert.IsType<DistrictEquipmentType>(instance);  
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
        /// Test the property 'Code'
        /// </summary>
        [Fact]
        public void CodeTest()
        {
            // TODO unit test for the property 'Code'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Description'
        /// </summary>
        [Fact]
        public void DescriptionTest()
        {
            // TODO unit test for the property 'Description'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipRentalRateNo'
        /// </summary>
        [Fact]
        public void EquipRentalRateNoTest()
        {
            // TODO unit test for the property 'EquipRentalRateNo'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipRentalRatePage'
        /// </summary>
        [Fact]
        public void EquipRentalRatePageTest()
        {
            // TODO unit test for the property 'EquipRentalRatePage'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'MaxHours'
        /// </summary>
        [Fact]
        public void MaxHoursTest()
        {
            // TODO unit test for the property 'MaxHours'
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
        /// <summary>
        /// Test the property 'SecondBlk'
        /// </summary>
        [Fact]
        public void SecondBlkTest()
        {
            // TODO unit test for the property 'SecondBlk'
			Assert.True(true);
        }

	}
	
}

