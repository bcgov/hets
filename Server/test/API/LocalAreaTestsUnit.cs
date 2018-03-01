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
	public class LocalAreaUnitTest 
    { 
		
		private readonly LocalAreaController _LocalArea;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public LocalAreaUnitTest()
		{			
            DbContextOptions<DbAppContext> options = new DbContextOptions<DbAppContext>();
            Mock<DbAppContext> dbAppContext = new Mock<DbAppContext>(null, options);
            LocalAreaService _service = new LocalAreaService(dbAppContext.Object);
            _LocalArea = new LocalAreaController (_service);
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasBulkPost
        /// </summary>
		public void TestLocalAreasBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasGet
        /// </summary>
		public void TestLocalAreasGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdDeletePost
        /// </summary>
		public void TestLocalAreasIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdGet
        /// </summary>
		public void TestLocalAreasIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasIdPut
        /// </summary>
		public void TestLocalAreasIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for LocalAreasPost
        /// </summary>
		public void TestLocalAreasPost()
		{
			Assert.True(true);
		}		        
    }
}
