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
    ///  Class for testing the model Contact
    /// </summary>
    
    public class ContactModelTests
    {
        // TODO uncomment below to declare an instance variable for Contact
        private Contact instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ContactModelTests()
        {
            instance = new Contact();
        }

    
        /// <summary>
        /// Test an instance of Contact
        /// </summary>
        [Fact]
        public void ContactInstanceTest()
        {
            Assert.IsType<Contact>(instance);  
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
        /// Test the property 'Role'
        /// </summary>
        [Fact]
        public void RoleTest()
        {
            // TODO unit test for the property 'Role'
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
        /// Test the property 'Phones'
        /// </summary>
        [Fact]
        public void PhonesTest()
        {
            // TODO unit test for the property 'Phones'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Addresses'
        /// </summary>
        [Fact]
        public void AddressesTest()
        {
            // TODO unit test for the property 'Addresses'
			Assert.True(true);
        }

	}
	
}

