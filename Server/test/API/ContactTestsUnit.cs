using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class ContactUnitTest 
    { 		
		private readonly ContactController _Contact;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public ContactUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);			                   

            ContactService _service = new ContactService(dbAppContext.Object);			
            _Contact = new ContactController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsBulkPost
        /// </summary>
		public void TestContactsBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsGet
        /// </summary>
		public void TestContactsGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdDeletePost
        /// </summary>
		public void TestContactsIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdGet
        /// </summary>
		public void TestContactsIdGet()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for ContactsIdPut
        /// </summary>
		public void TestContactsIdPut()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for ContactsPost
        /// </summary>
		public void TestContactsPost()
		{
			Assert.True(true);
		}		        
    }
}
