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
    ///  Class for testing the model UserRole
    /// </summary>
    
    public class UserRoleModelTests
    {
        // TODO uncomment below to declare an instance variable for UserRole
        private UserRole instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public UserRoleModelTests()
        {
            instance = new UserRole();
        }

    
        /// <summary>
        /// Test an instance of UserRole
        /// </summary>
        [Fact]
        public void UserRoleInstanceTest()
        {
            Assert.IsType<UserRole>(instance);  
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
        /// Test the property 'EffectiveDate'
        /// </summary>
        [Fact]
        public void EffectiveDateTest()
        {
            // TODO unit test for the property 'EffectiveDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'User'
        /// </summary>
        [Fact]
        public void UserTest()
        {
            // TODO unit test for the property 'User'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Role'
        /// </summary>
        [Fact]
        public void RoleTest()
        {
            // TODO unit test for the property 'Role'
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

