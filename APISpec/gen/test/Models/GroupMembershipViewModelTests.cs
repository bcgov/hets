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
    ///  Class for testing the model GroupMembershipViewModel
    /// </summary>
    
    public class GroupMembershipViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for GroupMembershipViewModel
        private GroupMembershipViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public GroupMembershipViewModelModelTests()
        {
            instance = new GroupMembershipViewModel();
        }

    
        /// <summary>
        /// Test an instance of GroupMembershipViewModel
        /// </summary>
        [Fact]
        public void GroupMembershipViewModelInstanceTest()
        {
            Assert.IsType<GroupMembershipViewModel>(instance);  
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
        /// Test the property 'GroupId'
        /// </summary>
        [Fact]
        public void GroupIdTest()
        {
            // TODO unit test for the property 'GroupId'
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
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            // TODO unit test for the property 'Id'
			Assert.True(true);
        }

	}
	
}

