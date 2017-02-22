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
	public class RoleApiUnitTest 
    { 
		
		private readonly RoleController _RoleApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public RoleApiUnitTest()
		{
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);

            /*

            Here you will need to mock up the context.

    ItemType fakeItem = new ItemType(...);

    Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

    dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

            */

            RoleService _service = new RoleService(dbAppContext.Object);
			
                    _RoleApi = new RoleController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for RolesGet
        /// </summary>
		public void TestRolesGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdDelete
        /// </summary>
		public void TestRolesIdDelete()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdDelete();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdGet
        /// </summary>
		public void TestRolesIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdPermissionsGet
        /// </summary>
		public void TestRolesIdPermissionsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdPermissionsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdPermissionsPut
        /// </summary>
		public void TestRolesIdPermissionsPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdPermissionsPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdPut
        /// </summary>
		public void TestRolesIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdUsersGet
        /// </summary>
		public void TestRolesIdUsersGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdUsersGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesIdUsersPut
        /// </summary>
		public void TestRolesIdUsersPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesIdUsersPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for RolesPost
        /// </summary>
		public void TestRolesPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _RoleController.RolesPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
