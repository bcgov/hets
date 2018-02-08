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
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);           
            UserService _service = new UserService(null, dbAppContext.Object, null);			
            _UserApi = new UserController (_service);
		}
		
		[Fact]
		/// <summary>
        /// Unit test for UsersBulkPost
        /// </summary>
		public void TestUsersBulkPost()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersCurrentGet
        /// </summary>
		public void TestUsersCurrentGet()
		{		
            Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for UsersGet
        /// </summary>
		public void TestUsersGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdDelete
        /// </summary>
		public void TestUsersIdDelete()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdFavouritesGet
        /// </summary>
		public void TestUsersIdFavouritesGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGet
        /// </summary>
		public void TestUsersIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGroupsGet
        /// </summary>
		public void TestUsersIdGroupsGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdGroupsPut
        /// </summary>
		public void TestUsersIdGroupsPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdNotificationGet
        /// </summary>
		public void TestUsersIdNotificationGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdPermissionsGet
        /// </summary>
		public void TestUsersIdPermissionsGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdPut
        /// </summary>
		public void TestUsersIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesGet
        /// </summary>
		public void TestUsersIdRolesGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesPost
        /// </summary>
		public void TestUsersIdRolesPost()
		{
			Assert.True(true);
		}		
        		
		[Fact]
		/// <summary>
        /// Unit test for UsersIdRolesPut
        /// </summary>
		public void TestUsersIdRolesPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for UsersPost
        /// </summary>
		public void TestUsersPost()
		{
			Assert.True(true);
		}		        
    }
}
