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
    ///  Class for testing the model UserViewModel
    /// </summary>
    
    public class UserViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for UserViewModel
        private UserViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public UserViewModelModelTests()
        {
            instance = new UserViewModel();
        }

    
        /// <summary>
        /// Test an instance of UserViewModel
        /// </summary>
        [Fact]
        public void UserViewModelInstanceTest()
        {
            Assert.IsType<UserViewModel>(instance);  
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
        /// Test the property 'Active'
        /// </summary>
        [Fact]
        public void ActiveTest()
        {
            // TODO unit test for the property 'Active'
			Assert.True(true);
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
        /// Test the property 'Email'
        /// </summary>
        [Fact]
        public void EmailTest()
        {
            // TODO unit test for the property 'Email'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SmUserId'
        /// </summary>
        [Fact]
        public void SmUserIdTest()
        {
            // TODO unit test for the property 'SmUserId'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'UserRoles'
        /// </summary>
        [Fact]
        public void UserRolesTest()
        {
            // TODO unit test for the property 'UserRoles'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'GroupMemberships'
        /// </summary>
        [Fact]
        public void GroupMembershipsTest()
        {
            // TODO unit test for the property 'GroupMemberships'
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

	}
	
}

