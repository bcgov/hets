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
            Assert.Equal(contacts.Count(), 0);


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
            Assert.Equal(contacts.Count, 1);
            Assert.Equal(contacts[0].GivenName, initialName);

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

            // test the post.

            Contact newContact = new Contact();
            newContact.OrganizationName = "asdf";

            request = new HttpRequestMessage(HttpMethod.Post, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            newContact = JsonConvert.DeserializeObject<Contact>(jsonString);

            // should be 0
            Assert.NotEqual(newContact.Id, 0);

            request = new HttpRequestMessage(HttpMethod.Put, "/api/owners/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // should be 0
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
        public async void TestOwnerEquipmentList()
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

            Assert.Equal(getReceived.Length, 3);
            Assert.Equal(getReceived[0].Owner.Id , owner_id);

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
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Fact]
        /// <summary>
        /// TestOwnerEquipmentList
        /// </summary>
        /// 
        public async void TestOwnerEquipmentListDateVerified()
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

            Assert.Equal(getReceived[0].LastVerifiedDate.Value.ToString ("MM/dd/yyyy HH:mm"), dateVerified.ToString("MM/dd/yyyy HH:mm"));

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
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
        }
        
    }
}
