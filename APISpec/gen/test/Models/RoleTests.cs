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
    ///  Class for testing the model Role
    /// </summary>
    
    public class RoleModelTests
    {
        // TODO uncomment below to declare an instance variable for Role
        private Role instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RoleModelTests()
        {
            instance = new Role();
        }

    
        /// <summary>
        /// Test an instance of Role
        /// </summary>
        [Fact]
        public void RoleInstanceTest()
        {
            Assert.IsType<Role>(instance);  
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
        /// Test the property 'Description'
        /// </summary>
        [Fact]
        public void DescriptionTest()
        {
            // TODO unit test for the property 'Description'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RolePermissions'
        /// </summary>
        [Fact]
        public void RolePermissionsTest()
        {
            // TODO unit test for the property 'RolePermissions'
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

	}
	
}

