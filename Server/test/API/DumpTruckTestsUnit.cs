using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class DumpTruckUnitTest 
    { 		
		private readonly DumpTruckController _DumpTruck;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public DumpTruckUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
            DumpTruckService _service = new DumpTruckService(dbAppContext.Object);
            _DumpTruck = new DumpTruckController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksBulkPost
        /// </summary>
		public void TestDumptrucksBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksGet
        /// </summary>
		public void TestDumptrucksGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdDeletePost
        /// </summary>
		public void TestDumptrucksIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdGet
        /// </summary>
		public void TestDumptrucksIdGet()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksIdPut
        /// </summary>
		public void TestDumptrucksIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for DumptrucksPost
        /// </summary>
		public void TestDumptrucksPost()
		{
			Assert.True(true);
		}		        
    }
}
