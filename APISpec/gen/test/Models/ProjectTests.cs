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
        // TODO uncomment below to declare an instance variable for Project
        private Project instance;

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
            // TODO unit test for the property 'Id'
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
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ProvincialProjectNumber'
        /// </summary>
        [Fact]
        public void ProvincialProjectNumberTest()
        {
            // TODO unit test for the property 'ProvincialProjectNumber'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Information'
        /// </summary>
        [Fact]
        public void InformationTest()
        {
            // TODO unit test for the property 'Information'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalRequests'
        /// </summary>
        [Fact]
        public void RentalRequestsTest()
        {
            // TODO unit test for the property 'RentalRequests'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalAgreements'
        /// </summary>
        [Fact]
        public void RentalAgreementsTest()
        {
            // TODO unit test for the property 'RentalAgreements'
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

