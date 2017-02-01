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
	public class ContactPhoneUnitTest 
    { 
		
		private readonly ContactPhoneController _ContactPhone;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public ContactPhoneUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    ContactPhoneService _service = new ContactPhoneService(dbAppContext.Object);
			
                    _ContactPhone = new ContactPhoneController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesBulkPost
        /// </summary>
		public void TestContactphonesBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesGet
        /// </summary>
		public void TestContactphonesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesIdDeletePost
        /// </summary>
		public void TestContactphonesIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesIdGet
        /// </summary>
		public void TestContactphonesIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesIdPut
        /// </summary>
		public void TestContactphonesIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactphonesPost
        /// </summary>
		public void TestContactphonesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactPhoneController.ContactphonesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
