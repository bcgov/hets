/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI;
using System.Text;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace HETSAPI.Test
{
	public class EquipmentAttachmentTypeIntegrationTest 
    { 
		private readonly TestServer _server;
		private readonly HttpClient _client;
			
		/// <summary>
        /// Setup the test
        /// </summary>        
		public EquipmentAttachmentTypeIntegrationTest()
		{
			_server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());
            _client = _server.CreateClient();
		}
	
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesBulkPost
        /// </summary>
		public async void TestEquipmentAttachmentTypesBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentAttachmentTypes/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		
        
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesGet
        /// </summary>
		public async void TestEquipmentAttachmentTypesGet()
		{
			var response = await _client.GetAsync("/api/equipmentAttachmentTypes");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesIdDeletePost
        /// </summary>
		public async void TestEquipmentAttachmentTypesIdDeletePost()
		{
			var response = await _client.GetAsync("/api/equipmentAttachmentTypes/{id}/delete");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesIdGet
        /// </summary>
		public async void TestEquipmentAttachmentTypesIdGet()
		{
			var response = await _client.GetAsync("/api/equipmentAttachmentTypes/{id}");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesIdPut
        /// </summary>
		public async void TestEquipmentAttachmentTypesIdPut()
		{
			var response = await _client.GetAsync("/api/equipmentAttachmentTypes/{id}");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		
        
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentTypesPost
        /// </summary>
		public async void TestEquipmentAttachmentTypesPost()
		{
			var response = await _client.GetAsync("/api/equipmentAttachmentTypes");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		
        
    }
}
