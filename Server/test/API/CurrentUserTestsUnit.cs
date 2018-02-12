using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class CurrentUserUnitTest 
    { 		
		private readonly CurrentUserController _CurrentUser;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public CurrentUserUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
            CurrentUserService _service = new CurrentUserService(null, dbAppContext.Object, null);
            _CurrentUser = new CurrentUserController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for UsersCurrentGet
        /// </summary>
		public void TestUsersCurrentGet()
		{
			Assert.True(true);
		}		        
    }
}
