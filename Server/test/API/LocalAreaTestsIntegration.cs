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
	public class LocalAreaIntegrationTest : ApiIntegrationTestBase
    { 
		[Fact]
		/// <summary>
        /// Integration test for LocalAreasBulkPost
        /// </summary>
		public async Task TestLocalAreasBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/localAreas/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for LocalAreas
        /// </summary>
        public async Task TestLocalAreasBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // localAreas have service areas.
            ServiceArea servicearea = new ServiceArea
            {
                Name = initialName
            };

            string jsonString = servicearea.ToJson();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas")
            {
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            servicearea = JsonConvert.DeserializeObject<ServiceArea>(jsonString);

            // test the POST.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/localAreas");

            // create a new object.
            LocalArea localarea = new LocalArea
            {
                ServiceArea = servicearea,
                Name = initialName
            };

            jsonString = localarea.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            localarea = JsonConvert.DeserializeObject<LocalArea>(jsonString);
            // get the id
            var id = localarea.Id;
            // change the name
            localarea.Name = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/localAreas/" + id)
            {
                Content = new StringContent(localarea.ToJson(), Encoding.UTF8, "application/json")
            };

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/localAreas/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            localarea = JsonConvert.DeserializeObject<LocalArea>(jsonString);

            // verify the change went through.
            Assert.Equal(localarea.Name, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/localAreas/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/localAreas/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas/" + servicearea.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/serviceareas/" + servicearea.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }                
    }
}
