using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.IO;
using System.Net.Http;

namespace HETSAPI.Test
{
    public abstract class ApiIntegrationTestBase
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        /// <summary>
        /// Setup the test
        /// </summary>        
        protected ApiIntegrationTestBase()
        {
            _server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>());          

            string testUserName = "TMcTesterson";
            _client = _server.CreateClient();
            _client.DefaultRequestHeaders.Add("DEV-USER", testUserName);
        }
    }
}
