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
using HETSAPI.ViewModels;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model UserRoleViewModel
    /// </summary>
    
    public class UserRoleViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for UserRoleViewModel
        private UserRoleViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public UserRoleViewModelModelTests()
        {
            instance = new UserRoleViewModel();
        }

    
        /// <summary>
        /// Test an instance of UserRoleViewModel
        /// </summary>
        [Fact]
        public void UserRoleViewModelInstanceTest()
        {
            Assert.IsType<UserRoleViewModel>(instance);  
        }

        /// <summary>
        /// Test the property 'EffectiveDate'
        /// </summary>
        [Fact]
        public void EffectiveDateTest()
        {
            // TODO unit test for the property 'EffectiveDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'UserId'
        /// </summary>
        [Fact]
        public void UserIdTest()
        {
            // TODO unit test for the property 'UserId'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RoleId'
        /// </summary>
        [Fact]
        public void RoleIdTest()
        {
            // TODO unit test for the property 'RoleId'
			Assert.True(true);
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
        /// Test the property 'ExpiryDate'
        /// </summary>
        [Fact]
        public void ExpiryDateTest()
        {
            // TODO unit test for the property 'ExpiryDate'
			Assert.True(true);
        }

	}
	
}

