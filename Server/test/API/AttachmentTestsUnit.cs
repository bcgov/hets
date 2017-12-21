/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */
using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class AttachmentUnitTest 
    { 		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public AttachmentUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);			                   

            AttachmentService service = new AttachmentService(dbAppContext.Object);
            AttachmentController attachment = new AttachmentController (service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentBulkPost
        /// </summary>
		public void TestAttachmentBulkPost()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentGet
        /// </summary>
		public void TestAttachmentGet()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentIdDeletePost
        /// </summary>
		public void TestAttachmentIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentIdGet
        /// </summary>
		public void TestAttachmentIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentIdPut
        /// </summary>
		public void TestAttachmentIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for AttachmentPost
        /// </summary>
		public void TestAttachmentPost()
		{
			Assert.True(true);
		}		        
    }
}
