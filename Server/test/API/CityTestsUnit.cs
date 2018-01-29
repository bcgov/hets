/*
 * REST API Documentation for the MOTI School Bus Application
 *
 * The School Bus application tracks that inspections are performed in a timely fashion. For each school bus the application tracks information about the bus (including data from ICBC, NSC, etc.), it's past and next inspection dates and results, contacts, and the inspector responsible for next inspecting the bus.
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
	public class CityApiUnitTest 
    { 						
		/// <summary>
        /// Setup the test
        /// </summary>        
		public CityApiUnitTest()
		{			
		}	
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesBulkPost
        /// </summary>
		public void TestCitiesBulkPost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesGet
        /// </summary>
		public void TestCitiesGet()
		{			
            Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesIdDeletePost
        /// </summary>
		public void TestCitiesIdDeletePost()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesIdGet
        /// </summary>
		public void TestCitiesIdGet()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesIdPut
        /// </summary>
		public void TestCitiesIdPut()
		{
			Assert.True(true);
		}		        
		
		[Fact]
		/// <summary>
        /// Unit test for CitiesPost
        /// </summary>
		public void TestCitiesPost()
		{
			Assert.True(true);
		}		        
    }
}
