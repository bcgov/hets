using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace HETSAPI.Test
{
	public class EquipmentAttachmentIntegrationTest : ApiIntegrationTestBase
    { 				
		[Fact]
		/// <summary>
        /// Integration test for EquipmentAttachmentsBulkPost
        /// </summary>
		public async Task TestEquipmentAttachmentsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentAttachments/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		        
		
		[Fact]
        /// <summary>
        /// Integration test for EquipmentAttachments
        /// </summary>
        public async Task TestEquipmentAttachmentsBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentAttachments");

            // create a new object.
            EquipmentAttachment equipmentattachment = new EquipmentAttachment
            {
                Description = initialName
            };

            string jsonString = equipmentattachment.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            equipmentattachment = JsonConvert.DeserializeObject<EquipmentAttachment>(jsonString);
            // get the id
            var id = equipmentattachment.Id;
            // change the name
            equipmentattachment.Description = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/equipmentAttachments/" + id)
            {
                Content = new StringContent(equipmentattachment.ToJson(), Encoding.UTF8, "application/json")
            };
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipmentAttachments/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            equipmentattachment = JsonConvert.DeserializeObject<EquipmentAttachment>(jsonString);

            // verify the change went through.
            Assert.Equal(equipmentattachment.Description, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentAttachments/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipmentAttachments/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
