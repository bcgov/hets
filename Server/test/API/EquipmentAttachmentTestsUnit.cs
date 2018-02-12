using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            EquipmentAttachmentService _service = new EquipmentAttachmentService(dbAppContext.Object, null);
            _EquipmentAttachment = new EquipmentAttachmentController (_service);
		}
			
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsBulkPost
        /// </summary>
		public void TestEquipmentAttachmentsBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsGet
        /// </summary>
		public void TestEquipmentAttachmentsGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdDeletePost
        /// </summary>
		public void TestEquipmentAttachmentsIdDeletePost()
		{
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdGet
        /// </summary>
		public void TestEquipmentAttachmentsIdGet()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsIdPut
        /// </summary>
		public void TestEquipmentAttachmentsIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentAttachmentsPost
        /// </summary>
		public void TestEquipmentAttachmentsPost()
		{			
            Assert.True(true);
		}		        
    }
}
