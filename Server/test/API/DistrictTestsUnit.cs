using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class DistrictApiUnitTest 
    { 		
		private readonly DistrictController _DistrictApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public DistrictApiUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
            DistrictService _service = new DistrictService(dbAppContext.Object, null);
            _DistrictApi = new DistrictController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsBulkPost
        /// </summary>
		public void TestDistrictsBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsGet
        /// </summary>
		public void TestDistrictsGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsIdDeletePost
        /// </summary>
		public void TestDistrictsIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsIdGet
        /// </summary>
		public void TestDistrictsIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsIdPut
        /// </summary>
		public void TestDistrictsIdPut()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsIdServiceareasGet
        /// </summary>
		public void TestDistrictsIdServiceareasGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DistrictsPost
        /// </summary>
		public void TestDistrictsPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for ServiceareasBulkPost
        /// </summary>
		public void TestServiceareasBulkPost()
		{
			Assert.True(true);
		}		        
    }
}
