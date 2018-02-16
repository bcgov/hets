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
using HETSAPI.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace HETSAPI.Test
{
    public class OwnerIntegrationTest : ApiIntegrationTestBase
    {
        [Fact]
        /// <summary>
        /// Integration test for OwnersBulkPost
        /// </summary>
        public async Task TestOwnersBulkPost()
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
        public async Task TestOwnersBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
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
            owner.OrganizationName = changedName;

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
            Assert.Equal(owner.OrganizationName, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for Owners
        /// </summary>
        public async Task TestOwnerContacts()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
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
            owner.OrganizationName = changedName;

            // get contacts should be empty.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list is empty.
            Assert.Empty(contacts);

            // add a contact.
            Contact contact = new Contact();
            contact.GivenName = initialName;

            contacts.Add(contact);


            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has one element.
            Assert.Single(contacts);
            Assert.Equal(contacts[0].GivenName, initialName);

            // get contacts should be 1
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has a record.
            Assert.Single(contacts);
            Assert.Equal(contacts[0].GivenName, initialName);

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
            Assert.Empty(contacts);

            // test the get
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has no records.
            Assert.Empty(contacts);

            // test the post.
            Contact newContact = new Contact();

            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            newContact = JsonConvert.DeserializeObject<Contact>(jsonString);

            // should be 0
            Assert.NotEqual(0, newContact.Id);

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // should be 0
            Assert.Empty(contacts);

            // delete the owner.            
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private Equipment createEquipment(Owner owner, string model)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment");

            // create a new object.
            Equipment equipment = new Equipment();
            equipment.Model = model;
            equipment.Owner = owner;
            string jsonString = equipment.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();

            Task<string> stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            // parse as JSON.
            jsonString = stringTask.Result;
            equipment = JsonConvert.DeserializeObject<Equipment>(jsonString);

            return equipment;
        }

        private void deleteEquipment(int id)
        {
            // do a delete.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipments/" + id + "/delete");
            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();
        }

        [Fact]
        /// <summary>
        /// TestOwnerEquipmentList
        /// </summary>
        /// 
        public async Task TestOwnerEquipmentList()
        {
            /*
             * Create Owner
             * Create several pieces of equipment
             * Populate the equipment list
             * Verify that all items in the equipment list still have owner.
            */
            string initialName = "InitialName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var owner_id = owner.Id;

            // create equipment.
            Equipment e1 = createEquipment(owner, "test1");
            Equipment e2 = createEquipment(owner, "test2");
            Equipment e3 = createEquipment(owner, "test3");

            Equipment[] equipmentList = new Equipment[3];

            equipmentList[0] = e1;
            equipmentList[1] = e2;
            equipmentList[2] = e3;

            // update the equipment list.

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + owner_id + "/equipment");
            jsonString = JsonConvert.SerializeObject(equipmentList, Formatting.Indented);

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            jsonString = await response.Content.ReadAsStringAsync();
            Equipment[] putReceived = JsonConvert.DeserializeObject<Equipment[]>(jsonString);
            Assert.Equal(putReceived[0].Owner.Id, owner_id);

            // now get the equipment list.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + owner_id + "/equipment");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            jsonString = await response.Content.ReadAsStringAsync();
            Equipment[] getReceived = JsonConvert.DeserializeObject<Equipment[]>(jsonString);

            Assert.Equal(3, getReceived.Length);
            Assert.Equal(getReceived[0].Owner.Id, owner_id);

            // clean up equipment

            Equipment[] blankList = new Equipment[0];

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + owner_id + "/equipment");
            jsonString = JsonConvert.SerializeObject(blankList, Formatting.Indented);

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            deleteEquipment(e1.Id);
            deleteEquipment(e2.Id);
            deleteEquipment(e3.Id);

            // delete owner
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + owner_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + owner_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// TestOwnerEquipmentList
        /// </summary>
        /// 
        public async Task TestOwnerEquipmentListDateVerified()
        {
            /*
             * Create Owner
             * Create several pieces of equipment
             * Populate the equipment list
             * Verify that all items in the equipment list still have owner.
            */
            string initialName = "InitialName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var owner_id = owner.Id;

            // create equipment.
            Equipment e1 = createEquipment(owner, "test1");
            Equipment e2 = createEquipment(owner, "test2");
            Equipment e3 = createEquipment(owner, "test3");

            // validate equipment number.
            Assert.Equal("TST-0003", e3.EquipmentCode);

            Equipment[] equipmentList = new Equipment[3];

            DateTime dateVerified = DateTime.UtcNow;

            e1.LastVerifiedDate = dateVerified;
            e2.LastVerifiedDate = dateVerified;
            e3.LastVerifiedDate = dateVerified;

            equipmentList[0] = e1;
            equipmentList[1] = e2;
            equipmentList[2] = e3;

            // update the equipment list.

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + owner_id + "/equipment");
            jsonString = JsonConvert.SerializeObject(equipmentList, Formatting.Indented);

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            jsonString = await response.Content.ReadAsStringAsync();
            Equipment[] putReceived = JsonConvert.DeserializeObject<Equipment[]>(jsonString);
            Assert.Equal(putReceived[0].Owner.Id, owner_id);

            // now get the equipment list.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + owner_id + "/equipment");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            jsonString = await response.Content.ReadAsStringAsync();
            Equipment[] getReceived = JsonConvert.DeserializeObject<Equipment[]>(jsonString);

            Assert.Equal(getReceived[0].LastVerifiedDate.ToString("MM/dd/yyyy HH:mm"), dateVerified.ToString("MM/dd/yyyy HH:mm"));

            // clean up equipment

            Equipment[] blankList = new Equipment[0];

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + owner_id + "/equipment");
            jsonString = JsonConvert.SerializeObject(blankList, Formatting.Indented);

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            deleteEquipment(e1.Id);
            deleteEquipment(e2.Id);
            deleteEquipment(e3.Id);

            // delete owner
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + owner_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + owner_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// TestOwnerEquipmentList
        /// </summary>
        /// 
        public async Task TestOwnerEquipmentCode()
        {
            /*
             * Create Owner
             * Create several pieces of equipment
             * Populate the equipment list
             * Verify that all items in the equipment list still have owner.
            */
            string initialName = "InitialName";
            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
            
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            owner = JsonConvert.DeserializeObject<Owner>(jsonString);
            // get the id
            var owner_id = owner.Id;

            // delete owner
            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + owner_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/owners/" + owner_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private ServiceArea CreateServiceArea( string name )
        {
            ServiceArea result = new ServiceArea();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas");

            result.Name = name;
            string jsonString = result.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();

            Task<string> stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            // parse as JSON.
            jsonString = stringTask.Result;
            result = JsonConvert.DeserializeObject<ServiceArea>(jsonString);

            return result;

        }

        private LocalArea CreateLocalArea(ServiceArea serviceArea, string name)
        {
            LocalArea result = new LocalArea();
            
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/localareas");

            result.Name = name;
            result.ServiceArea = serviceArea;
            string jsonString = result.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();

            Task<string> stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            // parse as JSON.
            jsonString = stringTask.Result;
            result = JsonConvert.DeserializeObject<LocalArea>(jsonString);

            return result;
        }



        private Owner CreateOwner(LocalArea localArea, string name)
        {
            Owner result = new Owner();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners");

            result.OrganizationName = name;
            result.LocalArea = localArea;

            string jsonString = result.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();

            Task<string> stringTask = response.Content.ReadAsStringAsync();
            stringTask.Wait();

            // parse as JSON.
            jsonString = stringTask.Result;
            result = JsonConvert.DeserializeObject<Owner>(jsonString);

            return result;
        }

        private void DeleteOwner(Owner owner)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + owner.Id + "/delete");
            
            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();
        }

        private void DeleteLocalArea(LocalArea localArea)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/localareas/" + localArea.Id + "/delete");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();
        }

        private void DeleteServiceArea(ServiceArea serviceArea)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas/" + serviceArea.Id + "/delete");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;
            response.EnsureSuccessStatusCode();
        }



        // automates the search
        private async Task<Owner[]> OwnerSearchHelper(string parameters)
        {
            var targetUrl = "/api/owners/search";
            

            var request = new HttpRequestMessage(HttpMethod.Get, targetUrl + parameters);

            var response = await _client.SendAsync(request);


            // parse as JSON.
            var jsonString = await response.Content.ReadAsStringAsync();
            // should be an array of schoolbus records.
            Owner[] searchresults = JsonConvert.DeserializeObject<Owner[]>(jsonString);
            return searchresults;
        }

        [Fact]
        /// <summary>
        /// Test the owner search.  Specifically test that searches for two items on a multi-select show the expected number of results.
        /// </summary>
        /// 
        public async Task TestOwnerSearch()
        {

            /* Test plan:
             * 1. create 3 local areas.
             * 2. put 2 owners in each area.        
             * 3. search for owners in local area 1 - should get 2 results.
             * 4. search for owners in local area 2 - should get 2 results.
             * 5. search for owners in local areas 1,2 - should get 4 results.
             * remove the owners
             * remove the local areas.
             */

            string initialName = "InitialName";
            
            // create 3 local areas.

            ServiceArea serviceArea = CreateServiceArea(initialName);
            LocalArea localArea1 = CreateLocalArea(serviceArea, "Local Area 1");
            LocalArea localArea2 = CreateLocalArea(serviceArea, "Local Area 2");
            LocalArea localArea3 = CreateLocalArea(serviceArea, "Local Area 3");

            // create 2 owners in each.

            Owner owner1 = CreateOwner(localArea1, "Owner 1");
            Owner owner2 = CreateOwner(localArea1, "Owner 2");
            Owner owner3 = CreateOwner(localArea2, "Owner 3");
            Owner owner4 = CreateOwner(localArea2, "Owner 4");
            Owner owner5 = CreateOwner(localArea3, "Owner 5");
            Owner owner6 = CreateOwner(localArea3, "Owner 6");
           
            Owner[] searchresults = await OwnerSearchHelper("?localareas=" + localArea2.Id);
            
            Assert.Equal(2, searchresults.Length);

            searchresults = await OwnerSearchHelper("?localareas=" + localArea2.Id );

            Assert.Equal(2, searchresults.Length);
           
            searchresults = await OwnerSearchHelper("?localareas=" + localArea1.Id + "%2C" + localArea2.Id);

            Assert.Equal(4, searchresults.Length);

            searchresults = await OwnerSearchHelper("?owner=" + owner1.Id);

            Assert.Single(searchresults);

            // cleanup
            DeleteOwner(owner1);
            DeleteOwner(owner2);
            DeleteOwner(owner3);
            DeleteOwner(owner4);
            DeleteOwner(owner5);
            DeleteOwner(owner6);

            DeleteLocalArea(localArea1);
            DeleteLocalArea(localArea2);
            DeleteLocalArea(localArea3);

            DeleteServiceArea(serviceArea);

        }
    }
}
