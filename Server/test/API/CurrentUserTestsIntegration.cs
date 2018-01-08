using System.Threading.Tasks;
using Xunit;

namespace HETSAPI.Test
{
	public class CurrentUserIntegrationTest : ApiIntegrationTestBase
    { 		
		[Fact]
		/// <summary>
        /// Integration test for UsersCurrentGet
        /// </summary>
		public async Task TestUsersCurrentGet()
		{
			var response = await _client.GetAsync("/api/users/current");
            response.EnsureSuccessStatusCode();
			
			// update this to test the API.
			Assert.True(true);
		}		        
    }
}
