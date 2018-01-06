using System.Net.Http;
using Xunit;
using System.Text;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace HETSAPI.Test
{
	public class DumpTruckIntegrationTest : ApiIntegrationTestBase
    { 				
		[Fact]
		/// <summary>
        /// Integration test for DumptrucksBulkPost
        /// </summary>
		public async Task TestDumpTrucksBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/dumptrucks/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for DumpTrucks
        /// </summary>
        public async Task TestDumpTrucksBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/dumptrucks");

            // create a new object.
            DumpTruck dumptruck = new DumpTruck
            {
                BoxCapacity = initialName
            };
            string jsonString = dumptruck.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            dumptruck = JsonConvert.DeserializeObject<DumpTruck>(jsonString);
            // get the id
            var id = dumptruck.Id;
            // change the name
            dumptruck.BoxCapacity = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/dumptrucks/" + id)
            {
                Content = new StringContent(dumptruck.ToJson(), Encoding.UTF8, "application/json")
            };
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/dumptrucks/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            dumptruck = JsonConvert.DeserializeObject<DumpTruck>(jsonString);

            // verify the change went through.
            Assert.Equal(dumptruck.BoxCapacity, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/dumptrucks/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/dumptrucks/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
