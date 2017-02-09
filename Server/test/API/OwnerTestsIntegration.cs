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
	public class OwnerIntegrationTest : ApiIntegrationTestBase
    { 
		[Fact]
		/// <summary>
        /// Integration test for OwnersBulkPost
        /// </summary>
		public async void TestOwnersBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        /// <summary>
        /// Basic Integration test for Owners
        /// </summary>
        public async void TestOwnersBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.Comment = initialName;
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var id = owner.Id;
            // change the name
            owner.Comment = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id);
            request.Content = new StringContent(owner.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            owner = JsonConvert.DeserializeObject<Owner>(jsonString);

            // verify the change went through.
            Assert.Equal(owner.Comment, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for Owners
        /// </summary>
        public async void TestOwnerContacts()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.Comment = initialName;
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var id = owner.Id;
            // change the name
            owner.Comment = changedName;

            // get contacts should be empty.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            List<Contact> contacts  = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list is empty.
            Assert.Equal(contacts.Count(), 0);


            // add a contact.
            Contact contact = new Contact();
            contact.GivenName = initialName;
            ContactPhone phone = new ContactPhone();
            phone.PhoneNumber = "1234";
            ContactAddress address = new ContactAddress();
            address.AddressLine1 = initialName;

            contact.Phones = new List<ContactPhone>();
            contact.Phones.Add(phone);

            contact.Addresses = new List<ContactAddress>();
            contact.Addresses.Add(address);

            contacts.Add(contact);


            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has one element.
            Assert.Equal(contacts.Count, 1);
            Assert.Equal(contacts[0].GivenName, initialName);
            Assert.Equal(contacts[0].Addresses[0].AddressLine1, initialName);
            Assert.Equal(contacts[0].Phones[0].PhoneNumber, "1234");

            // get contacts should be 1
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has a record.
            Assert.Equal(contacts.Count, 1);           
            Assert.Equal(contacts[0].GivenName, initialName);
            Assert.Equal(contacts[0].Addresses[0].AddressLine1, initialName);
            Assert.Equal(contacts[0].Phones[0].PhoneNumber, "1234");

            // test removing the contact.
            contacts.Clear();

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // should be 0
            Assert.Equal(contacts.Count, 0);
            
            // test the get
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has no records.
            Assert.Equal(contacts.Count, 0);

            // delete the owner.            
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
