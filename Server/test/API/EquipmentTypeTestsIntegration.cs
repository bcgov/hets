using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace HETSAPI.Test
{
	public class EquipmentTypeIntegrationTest : ApiIntegrationTestBase
    { 				
		[Fact]
		/// <summary>
        /// Integration test for EquipmentTypesBulkPost
        /// </summary>
		public async Task TestEquipmentTypesBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentTypes/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for EquipmentTypes
        /// </summary>
        public async Task TestEquipmentTypesBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentTypes");

            // create a new object.
            EquipmentType equipmenttype = new EquipmentType
            {
                Name = initialName
            };
            string jsonString = equipmenttype.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            equipmenttype = JsonConvert.DeserializeObject<EquipmentType>(jsonString);
            // get the id
            var id = equipmenttype.Id;
            // change the name
            equipmenttype.Name = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/equipmentTypes/" + id)
            {
                Content = new StringContent(equipmenttype.ToJson(), Encoding.UTF8, "application/json")
            };
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipmentTypes/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            equipmenttype = JsonConvert.DeserializeObject<EquipmentType>(jsonString);

            // verify the change went through.
            Assert.Equal(equipmenttype.Name, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentTypes/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipmentTypes/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
