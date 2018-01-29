/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HETSAPI.Test
{
    public class ContactIntegrationTest : ApiIntegrationTestBase
    { 
		
		[Fact]
		/// <summary>
        /// Integration test for ContactsBulkPost
        /// </summary>
		public async Task TestContactsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/contacts/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for Contacts
        /// </summary>
        public async Task TestContactsBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/contacts");

            // create a new object.
            Contact contact = new Contact();
            contact.Notes = initialName;
            string jsonString = contact.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contact = JsonConvert.DeserializeObject<Contact>(jsonString);

            // get the id
            var id = contact.Id;

            // change the name
            contact.Notes = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/contacts/" + id)
            {
                Content = new StringContent(contact.ToJson(), Encoding.UTF8, "application/json")
            };

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/contacts/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contact = JsonConvert.DeserializeObject<Contact>(jsonString);

            // verify the change went through.
            Assert.Equal(contact.Notes, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/contacts/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/contacts/" + id);
            response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }                
    }
}
