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
	public class UserUnitTest 
    { 
		
		private readonly UserController _UserApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public UserUnitTest()
		{
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);

            /*

            Here you will need to mock up the context.

    ItemType fakeItem = new ItemType(...);

    Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

    dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

            */

            UserService _service = new UserService(dbAppContext.Object);
			
                    _UserApi = new UserController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for UsersBulkPost
        /// </summary>
		public void TestUsersBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersCurrentGet
        /// </summary>
		public void TestUsersCurrentGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersCurrentGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersGet
        /// </summary>
		public void TestUsersGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdDelete
        /// </summary>
		public void TestUsersIdDelete()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdDelete();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdFavouritesGet
        /// </summary>
		public void TestUsersIdFavouritesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdFavouritesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGet
        /// </summary>
		public void TestUsersIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGroupsGet
        /// </summary>
		public void TestUsersIdGroupsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdGroupsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGroupsPut
        /// </summary>
		public void TestUsersIdGroupsPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdGroupsPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdNotificationGet
        /// </summary>
		public void TestUsersIdNotificationGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdNotificationGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdPermissionsGet
        /// </summary>
		public void TestUsersIdPermissionsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdPermissionsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdPut
        /// </summary>
		public void TestUsersIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesGet
        /// </summary>
		public void TestUsersIdRolesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdRolesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesPost
        /// </summary>
		public void TestUsersIdRolesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdRolesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesPut
        /// </summary>
		public void TestUsersIdRolesPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersIdRolesPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersPost
        /// </summary>
		public void TestUsersPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _UserController.UsersPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
