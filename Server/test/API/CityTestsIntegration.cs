using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace HETSAPI.Test
{
	public class CityApiIntegrationTest : ApiIntegrationTestBase
    { 
        [Fact]
        /// <summary>
        /// Integration test for Cities
        /// </summary>
        public async Task TestCitiesBulk()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/cities/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
		/// <summary>
        /// Integration test for Cities
        /// </summary>
		public async Task TestCities()
		{
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/cities");

            // create a new object.
            City city = new City();
            string jsonString = city.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            city = JsonConvert.DeserializeObject<City>(jsonString);
            // get the id
            var id = city.Id;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/cities/" + id)
            {
                Content = new StringContent(city.ToJson(), Encoding.UTF8, "application/json")
            };

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/cities/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            JsonConvert.DeserializeObject<City>(jsonString);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/cities/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/cities/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }		               
    }
}
