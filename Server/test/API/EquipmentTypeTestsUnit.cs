using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            EquipmentTypeService _service = new EquipmentTypeService(dbAppContext.Object);
			_EquipmentType = new EquipmentTypeController (_service);
		}
			
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesBulkPost
        /// </summary>
		public void TestEquipmentTypesBulkPost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesGet
        /// </summary>
		public void TestEquipmentTypesGet()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdDeletePost
        /// </summary>
		public void TestEquipmentTypesIdDeletePost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdGet
        /// </summary>
		public void TestEquipmentTypesIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesIdPut
        /// </summary>
		public void TestEquipmentTypesIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for EquipmentTypesPost
        /// </summary>
		public void TestEquipmentTypesPost()
		{
			Assert.True(true);
		}		        
    }
}
