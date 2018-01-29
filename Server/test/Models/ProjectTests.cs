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
    ///  Class for testing the model Project
    /// </summary>    
    public class ProjectModelTests
    {
        private readonly Project instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ProjectModelTests()
        {
            instance = new Project();
        }

    
        /// <summary>
        /// Test an instance of Project
        /// </summary>
        [Fact]
        public void ProjectInstanceTest()
        {
            Assert.IsType<Project>(instance);  
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
        /// Test the property 'ServiceArea'
        /// </summary>
        [Fact]
        public void ServiceAreaTest()
        {
            // TODO unit test for the property 'ServiceArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ProjectNum'
        /// </summary>
        [Fact]
        public void ProjectNumTest()
        {
            // TODO unit test for the property 'ProjectNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'JobDesc1'
        /// </summary>
        [Fact]
        public void JobDesc1Test()
        {
            // TODO unit test for the property 'JobDesc1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'JobDesc2'
        /// </summary>
        [Fact]
        public void JobDesc2Test()
        {
            // TODO unit test for the property 'JobDesc2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Requests'
        /// </summary>
        [Fact]
        public void RequestsTest()
        {
            // TODO unit test for the property 'Requests'
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
        /// Test the property 'Contacts'
        /// </summary>
        [Fact]
        public void ContactsTest()
        {
            // TODO unit test for the property 'Contacts'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Notes'
        /// </summary>
        [Fact]
        public void NotesTest()
        {
            // TODO unit test for the property 'Notes'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Attachments'
        /// </summary>
        [Fact]
        public void AttachmentsTest()
        {
            // TODO unit test for the property 'Attachments'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'History'
        /// </summary>
        [Fact]
        public void HistoryTest()
        {
            // TODO unit test for the property 'History'
			Assert.True(true);
        }

	}
	
}

