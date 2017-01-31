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
using HETSAPI.ViewModels;
using System.Reflection;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model CurrentUserViewModel
    /// </summary>
    
    public class CurrentUserViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for CurrentUserViewModel
        private CurrentUserViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public CurrentUserViewModelModelTests()
        {
            instance = new CurrentUserViewModel();
        }

    
        /// <summary>
        /// Test an instance of CurrentUserViewModel
        /// </summary>
        [Fact]
        public void CurrentUserViewModelInstanceTest()
        {
            Assert.IsType<CurrentUserViewModel>(instance);  
        }

        /// <summary>
        /// Test the property 'GivenName'
        /// </summary>
        [Fact]
        public void GivenNameTest()
        {
            // TODO unit test for the property 'GivenName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Surname'
        /// </summary>
        [Fact]
        public void SurnameTest()
        {
            // TODO unit test for the property 'Surname'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FullName'
        /// </summary>
        [Fact]
        public void FullNameTest()
        {
            // TODO unit test for the property 'FullName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DistrictName'
        /// </summary>
        [Fact]
        public void DistrictNameTest()
        {
            // TODO unit test for the property 'DistrictName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OverdueInspections'
        /// </summary>
        [Fact]
        public void OverdueInspectionsTest()
        {
            // TODO unit test for the property 'OverdueInspections'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ScheduledInspections'
        /// </summary>
        [Fact]
        public void ScheduledInspectionsTest()
        {
            // TODO unit test for the property 'ScheduledInspections'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DueNextMonthInspections'
        /// </summary>
        [Fact]
        public void DueNextMonthInspectionsTest()
        {
            // TODO unit test for the property 'DueNextMonthInspections'
			Assert.True(true);
        }

	}
	
}

