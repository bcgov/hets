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
    ///  Class for testing the model ContactAddress
    /// </summary>
    
    public class ContactAddressModelTests
    {
        // TODO uncomment below to declare an instance variable for ContactAddress
        private ContactAddress instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ContactAddressModelTests()
        {
            instance = new ContactAddress();
        }

    
        /// <summary>
        /// Test an instance of ContactAddress
        /// </summary>
        [Fact]
        public void ContactAddressInstanceTest()
        {
            Assert.IsType<ContactAddress>(instance);  
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
        /// Test the property 'Type'
        /// </summary>
        [Fact]
        public void TypeTest()
        {
            // TODO unit test for the property 'Type'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine1'
        /// </summary>
        [Fact]
        public void AddressLine1Test()
        {
            // TODO unit test for the property 'AddressLine1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine2'
        /// </summary>
        [Fact]
        public void AddressLine2Test()
        {
            // TODO unit test for the property 'AddressLine2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'City'
        /// </summary>
        [Fact]
        public void CityTest()
        {
            // TODO unit test for the property 'City'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Province'
        /// </summary>
        [Fact]
        public void ProvinceTest()
        {
            // TODO unit test for the property 'Province'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PostalCode'
        /// </summary>
        [Fact]
        public void PostalCodeTest()
        {
            // TODO unit test for the property 'PostalCode'
			Assert.True(true);
        }

	}
	
}

