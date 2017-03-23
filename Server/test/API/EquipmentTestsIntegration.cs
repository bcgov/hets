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
	public class EquipmentIntegrationTest : ApiIntegrationTestBase
    { 
				
		[Fact]
		/// <summary>
        /// Integration test for EquipmentBulkPost
        /// </summary>
		public async void TestEquipmentBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        /// <summary>
        /// Basic Integration test for Equipment
        /// </summary>
        public async void TestEquipmentBasic()
        {

            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
            owner.OwnerEquipmentCodePrefix = "TST";
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var ownerId = owner.Id;

            // now create the equipment.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment");

            Equipment equipment = new Equipment();
            equipment.Model = initialName;
            equipment.Owner = owner;
            jsonString = equipment.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            equipment = JsonConvert.DeserializeObject<Equipment>(jsonString);
            // get the id
            var id = equipment.Id;

            // verify the fields that are created server side.
            Assert.Equal(equipment.EquipmentCode, "TST-0001");

            bool dateValid = true;
            DateTime twoDaysAgo = DateTime.UtcNow.AddDays(-2);
            if (equipment.LastVerifiedDate < twoDaysAgo)
            {
                dateValid = false;
            }
            Assert.Equal(dateValid, true);

            dateValid = true;
            if (equipment.LastVerifiedDate < twoDaysAgo)
            {
                dateValid = false;
            }
            Assert.Equal(dateValid, true);

            // change the name
            equipment.Model = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/equipment/" + id);
            request.Content = new StringContent(equipment.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipment/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            equipment = JsonConvert.DeserializeObject<Equipment>(jsonString);

            // verify the change went through.
            Assert.Equal(equipment.Model, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipment/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
