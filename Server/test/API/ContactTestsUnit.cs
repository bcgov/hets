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
	public class ContactUnitTest 
    { 
		
		private readonly ContactController _Contact;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public ContactUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    ContactService _service = new ContactService(dbAppContext.Object);
			
                    _Contact = new ContactController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsBulkPost
        /// </summary>
		public void TestContactsBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsGet
        /// </summary>
		public void TestContactsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdDeletePost
        /// </summary>
		public void TestContactsIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdGet
        /// </summary>
		public void TestContactsIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdPut
        /// </summary>
		public void TestContactsIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsPost
        /// </summary>
		public void TestContactsPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ContactController.ContactsPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
