using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
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
		public async Task TestEquipmentBulkPost()
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
        public async Task TestEquipmentBasic()
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
            Assert.Equal("TST-0001", equipment.EquipmentCode);

            bool dateValid = true;
            DateTime twoDaysAgo = DateTime.UtcNow.AddDays(-2);
            if (equipment.LastVerifiedDate < twoDaysAgo)
            {
                dateValid = false;
            }
            Assert.True(dateValid);

            dateValid = true;
            if (equipment.LastVerifiedDate < twoDaysAgo)
            {
                dateValid = false;
            }
            Assert.True(dateValid);

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
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
