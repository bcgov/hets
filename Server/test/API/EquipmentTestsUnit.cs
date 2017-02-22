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
	public class EquipmentUnitTest 
    { 
		
		private readonly EquipmentController _Equipment;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public EquipmentUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    EquipmentService _service = new EquipmentService(dbAppContext.Object);
			
                    _Equipment = new EquipmentController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentBulkPost
        /// </summary>
		public void TestEquipmentBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentGet
        /// </summary>
		public void TestEquipmentGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdDeletePost
        /// </summary>
		public void TestEquipmentIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdGet
        /// </summary>
		public void TestEquipmentIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdPut
        /// </summary>
		public void TestEquipmentIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentPost
        /// </summary>
		public void TestEquipmentPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentSearchGet
        /// </summary>
		public void TestEquipmentSearchGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentController.EquipmentSearchGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
