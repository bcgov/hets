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
	public class LocalAreaUnitTest 
    { 
		
		private readonly LocalAreaController _LocalArea;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public LocalAreaUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    LocalAreaService _service = new LocalAreaService(dbAppContext.Object);
			
                    _LocalArea = new LocalAreaController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasBulkPost
        /// </summary>
		public void TestLocalAreasBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasGet
        /// </summary>
		public void TestLocalAreasGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdDeletePost
        /// </summary>
		public void TestLocalAreasIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdGet
        /// </summary>
		public void TestLocalAreasIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdPut
        /// </summary>
		public void TestLocalAreasIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasPost
        /// </summary>
		public void TestLocalAreasPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _LocalAreaController.LocalAreasPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
