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
    ///  Class for testing the model RentalRequestSearchResultViewModel
    /// </summary>
    
    public class RentalRequestSearchResultViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalRequestSearchResultViewModel
        private RentalRequestSearchResultViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalRequestSearchResultViewModelModelTests()
        {
            instance = new RentalRequestSearchResultViewModel();
        }

    
        /// <summary>
        /// Test an instance of RentalRequestSearchResultViewModel
        /// </summary>
        [Fact]
        public void RentalRequestSearchResultViewModelInstanceTest()
        {
            Assert.IsType<RentalRequestSearchResultViewModel>(instance);  
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
        /// Test the property 'LocalArea'
        /// </summary>
        [Fact]
        public void LocalAreaTest()
        {
            // TODO unit test for the property 'LocalArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentCount'
        /// </summary>
        [Fact]
        public void EquipmentCountTest()
        {
            // TODO unit test for the property 'EquipmentCount'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentTypeName'
        /// </summary>
        [Fact]
        public void EquipmentTypeNameTest()
        {
            // TODO unit test for the property 'EquipmentTypeName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ProjectName'
        /// </summary>
        [Fact]
        public void ProjectNameTest()
        {
            // TODO unit test for the property 'ProjectName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PrimaryContact'
        /// </summary>
        [Fact]
        public void PrimaryContactTest()
        {
            // TODO unit test for the property 'PrimaryContact'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ProjectId'
        /// </summary>
        [Fact]
        public void ProjectIdTest()
        {
            // TODO unit test for the property 'ProjectId'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExpectedStartDate'
        /// </summary>
        [Fact]
        public void ExpectedStartDateTest()
        {
            // TODO unit test for the property 'ExpectedStartDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExpectedEndDate'
        /// </summary>
        [Fact]
        public void ExpectedEndDateTest()
        {
            // TODO unit test for the property 'ExpectedEndDate'
			Assert.True(true);
        }

	}
	
}

