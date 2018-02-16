using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            EquipmentService _service = new EquipmentService(null, dbAppContext.Object, null);
            _Equipment = new EquipmentController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentBulkPost
        /// </summary>
		public void TestEquipmentBulkPost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentGet
        /// </summary>
		public void TestEquipmentGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdDeletePost
        /// </summary>
		public void TestEquipmentIdDeletePost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdGet
        /// </summary>
		public void TestEquipmentIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentIdPut
        /// </summary>
		public void TestEquipmentIdPut()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentPost
        /// </summary>
		public void TestEquipmentPost()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentSearchGet
        /// </summary>
		public void TestEquipmentSearchGet()
		{
			Assert.True(true);
		}		        
    }
}
