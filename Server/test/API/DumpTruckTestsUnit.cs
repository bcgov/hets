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
	public class DumpTruckUnitTest 
    { 
		
		private readonly DumpTruckController _DumpTruck;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public DumpTruckUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    DumpTruckService _service = new DumpTruckService(dbAppContext.Object);
			
                    _DumpTruck = new DumpTruckController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksBulkPost
        /// </summary>
		public void TestDumptrucksBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksGet
        /// </summary>
		public void TestDumptrucksGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdDeletePost
        /// </summary>
		public void TestDumptrucksIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdGet
        /// </summary>
		public void TestDumptrucksIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdPut
        /// </summary>
		public void TestDumptrucksIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksPost
        /// </summary>
		public void TestDumptrucksPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _DumpTruckController.DumptrucksPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
