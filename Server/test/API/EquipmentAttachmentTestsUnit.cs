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
	public class EquipmentAttachmentUnitTest 
    { 
		
		private readonly EquipmentAttachmentController _EquipmentAttachment;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public EquipmentAttachmentUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    EquipmentAttachmentService _service = new EquipmentAttachmentService(dbAppContext.Object);
			
                    _EquipmentAttachment = new EquipmentAttachmentController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsBulkPost
        /// </summary>
		public void TestEquipmentAttachmentsBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsGet
        /// </summary>
		public void TestEquipmentAttachmentsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdDeletePost
        /// </summary>
		public void TestEquipmentAttachmentsIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdGet
        /// </summary>
		public void TestEquipmentAttachmentsIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdPut
        /// </summary>
		public void TestEquipmentAttachmentsIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsPost
        /// </summary>
		public void TestEquipmentAttachmentsPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _EquipmentAttachmentController.EquipmentAttachmentsPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
