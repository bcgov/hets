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
	public class EquipmentTypeUnitTest 
    { 
		
		private readonly EquipmentTypeController _EquipmentType;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public EquipmentTypeUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    EquipmentTypeService _service = new EquipmentTypeService(dbAppContext.Object);
			
                    _EquipmentType = new EquipmentTypeController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesBulkPost
        /// </summary>
		public void TestEquipmentTypesBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesGet
        /// </summary>
		public void TestEquipmentTypesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdDeletePost
        /// </summary>
		public void TestEquipmentTypesIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdGet
        /// </summary>
		public void TestEquipmentTypesIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdPut
        /// </summary>
		public void TestEquipmentTypesIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesPost
        /// </summary>
		public void TestEquipmentTypesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentTypeController.EquipmentTypesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
