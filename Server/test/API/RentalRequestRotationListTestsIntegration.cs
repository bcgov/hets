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
	public class RentalRequestRotationListTest : ApiIntegrationTestBase
    { 
		
		
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentsBulkPost
        /// </summary>
		public async void TestBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequestrotationlists/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		
        
		
		[Fact]
        /// <summary>
        /// Integration test for EquipmentAttachments
        /// </summary>
        public async void TestBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequestrotationlists");

            // create a new object.
            RentalRequestRotationList rentalRequestRotationList = new RentalRequestRotationList();
            rentalRequestRotationList.Note = initialName;
            string jsonString = rentalRequestRotationList.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            rentalRequestRotationList = JsonConvert.DeserializeObject<RentalRequestRotationList>(jsonString);
            // get the id
            var id = rentalRequestRotationList.Id;
            // change the name
            rentalRequestRotationList.Note = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/rentalrequestrotationlists/" + id);
            request.Content = new StringContent(rentalRequestRotationList.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequestrotationlists/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            rentalRequestRotationList = JsonConvert.DeserializeObject<RentalRequestRotationList>(jsonString);

            // verify the change went through.
            Assert.Equal(rentalRequestRotationList.Note, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequestrotationlists/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequestrotationlists/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }

    }
}
