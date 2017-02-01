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
	public class ProjectUnitTest 
    { 
		
		private readonly ProjectController _Project;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public ProjectUnitTest()
		{			
                    DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
                    Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(options);
			
                    /*
			
                    Here you will need to mock up the context.
			
            ItemType fakeItem = new ItemType(...);

            Mock<DbSet<ItemType>> mockList = MockDbSet.Create(fakeItem);

            dbAppContext.Setup(x => x.ModelEndpoint).Returns(mockItem.Object);

                    */

                    ProjectService _service = new ProjectService(dbAppContext.Object);
			
                    _Project = new ProjectController (_service);

		}
	
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsBulkPost
        /// </summary>
		public void TestProjectsBulkPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsBulkPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsGet
        /// </summary>
		public void TestProjectsGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsIdDeletePost
        /// </summary>
		public void TestProjectsIdDeletePost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsIdDeletePost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsIdGet
        /// </summary>
		public void TestProjectsIdGet()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsIdGet();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsIdPut
        /// </summary>
		public void TestProjectsIdPut()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsIdPut();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Unit test for ProjectsPost
        /// </summary>
		public void TestProjectsPost()
		{
			// Add test code here
			// it may look like: 
			//  var result = _ProjectController.ProjectsPost();
			//  Assert.True (result == expected-result);

            Assert.True(true);
		}		
        
    }
}
