using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using HETSAPI.Authentication;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;
using System.Threading.Tasks;

namespace HETSAPI.Test
{
    /// <summary>
    /// Tests to ensure the UserSeeder registered the initial users defined in schoolbus\Server\test\Data\initialUsers.json
    /// The best way to ensure this is working properly is to run it against an empty database.
    /// </summary>
    public class UserSeederTests
    {        
		private readonly HttpClient _client;        

        public UserSeederTests()
        {
            TestServer server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());
            _client = server.CreateClient();            
        }

        [Fact]
        public async Task InitialUserJoeIsAdmin()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test/admin/permission/attribute");
            request.Headers.Add("DEV-USER", "JDow");

            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task InitialUserJaneIsAdmin()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test/admin/permission/attribute");
            request.Headers.Add("DEV-USER", "JDow");

            var response = await _client.SendAsync(request);            
            await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
