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
	public class EquipmentAttachmentTypeUnitTest 
    { 
		
		private readonly EquipmentAttachmentTypeController _EquipmentAttachmentType;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public EquipmentAttachmentTypeUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    EquipmentAttachmentTypeService _service = new EquipmentAttachmentTypeService(dbAppContext.Object);
			
                    _EquipmentAttachmentType = new EquipmentAttachmentTypeController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesBulkPost
        /// </summary>
		public void TestEquipmentAttachmentTypesBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesGet
        /// </summary>
		public void TestEquipmentAttachmentTypesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesIdDeletePost
        /// </summary>
		public void TestEquipmentAttachmentTypesIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesIdGet
        /// </summary>
		public void TestEquipmentAttachmentTypesIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesIdPut
        /// </summary>
		public void TestEquipmentAttachmentTypesIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentTypesPost
        /// </summary>
		public void TestEquipmentAttachmentTypesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentTypeController.EquipmentAttachmentTypesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
