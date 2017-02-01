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
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class ContactAddressUnitTest 
    { 
		
		private readonly ContactAddressController _ContactAddress;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public ContactAddressUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    ContactAddressService _service = new ContactAddressService(dbAppContext.Object);
			
                    _ContactAddress = new ContactAddressController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesBulkPost
        /// </summary>
		public void TestContactaddressesBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesGet
        /// </summary>
		public void TestContactaddressesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesIdDeletePost
        /// </summary>
		public void TestContactaddressesIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesIdGet
        /// </summary>
		public void TestContactaddressesIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesIdPut
        /// </summary>
		public void TestContactaddressesIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactaddressesPost
        /// </summary>
		public void TestContactaddressesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactAddressController.ContactaddressesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
