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
	public class RegionApiUnitTest 
    { 
		
		private readonly RegionController _RegionApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public RegionApiUnitTest()
		{
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);

            /*

            Here you will need to mock up the context.

    ItemType fakeItem = new ItemType(...);

    Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

    dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

            */

            RegionService _service = new RegionService(dbAppContext.Object);
			
                    _RegionApi = new RegionController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for RegionsGet
        /// </summary>
		public void TestRegionsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RegionController.RegionsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RegionsIdCitiesGet
        /// </summary>
		public void TestRegionsIdCitiesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RegionController.RegionsIdCitiesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RegionsIdGet
        /// </summary>
		public void TestRegionsIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RegionController.RegionsIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RegionsIdLocalareasGet
        /// </summary>
		public void TestRegionsIdLocalareasGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RegionController.RegionsIdLocalareasGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RegionsPost
        /// </summary>
		public void TestRegionsPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RegionController.RegionsPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
