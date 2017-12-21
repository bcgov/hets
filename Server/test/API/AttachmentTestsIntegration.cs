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
	public class AttachmentIntegrationTest : ApiIntegrationTestBase
    { 		
				
		[Fact]
		/// <summary>
        /// Integration test for AttachmentBulkPost
        /// </summary>
		public async Task TestAttachmentBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/attachments/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for Attachments
        /// </summary>
        public async Task TestAttachmentBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/attachments");

            // create a new object.
            Attachment attachment = new Attachment()
            {
                Description = initialName
            };
            
            string jsonString = attachment.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            attachment = JsonConvert.DeserializeObject<Attachment>(jsonString);
            // get the id
            var id = attachment.Id;
            // change the name
            attachment.Description = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/attachments/" + id)
            {
                Content = new StringContent(attachment.ToJson(), Encoding.UTF8, "application/json")
            };
            
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/attachments/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            attachment = JsonConvert.DeserializeObject<Attachment>(jsonString);

            // verify the change went through.
            Assert.Equal(attachment.Description, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/attachments/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/attachments/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
                
    }
}
