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
	public class RentalRequestIntegrationTest : ApiIntegrationTestBase
    { 
		
		
		[Fact]
		/// <summary>
        /// Integration test for BulkPost
        /// </summary>
		public async Task TestBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequests/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		        
		
		[Fact]
        /// <summary>
        /// Integration test 
        /// </summary>
        public async Task TestBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequests");

            // create a new object.
            RentalRequest rentalRequest = new RentalRequest();
            rentalRequest.Status = initialName;
            string jsonString = rentalRequest.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            rentalRequest = JsonConvert.DeserializeObject<RentalRequest>(jsonString);
            // get the id
            var id = rentalRequest.Id;
            // change the name
            rentalRequest.Status = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/rentalrequests/" + id);
            request.Content = new StringContent(rentalRequest.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequests/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            rentalRequest = JsonConvert.DeserializeObject<RentalRequest>(jsonString);

            // verify the change went through.
            Assert.Equal(rentalRequest.Status, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequests/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequests/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        Equipment CreateEquipment (Equipment equipment)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment");

            // create a new object.
            string jsonString = equipment.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Task<HttpResponseMessage> responseTask = _client.SendAsync(request);
            responseTask.Wait();

            HttpResponseMessage response = responseTask.Result;

            Task<string> stringTask  = response.Content.ReadAsStringAsync();
            stringTask.Wait();
            jsonString = stringTask.Result;

            // parse as JSON.
            Equipment result = JsonConvert.DeserializeObject<Equipment>(jsonString);
            return result;
        }
        
        [Fact]
        /// <summary>
        /// Test the creation of the rotation list, a side effect of rental request record creation.
        /// </summary>
        public async Task TestRotationListNonDumpTruck()
        {            
            string initialName = "InitialName";

            // create a temporary region.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/regions");
            Region region = new Region();
            region.Name = initialName;

            request.Content = new StringContent(region.ToJson(), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            string jsonString = await response.Content.ReadAsStringAsync();
            region = JsonConvert.DeserializeObject<Region>(jsonString);
            
            request = new HttpRequestMessage(HttpMethod.Post, "/api/districts");

            // create a new District
            District district = new District();
            district.Id = 0;
            district.Name = initialName;
            district.Region = region;
            jsonString = district.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();       

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            district = JsonConvert.DeserializeObject<District>(jsonString);            

            // create a new Service Area
            request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas");
            ServiceArea serviceArea = new ServiceArea();
            serviceArea.Id = 0;
            serviceArea.Name = initialName;
            serviceArea.District= district;
            jsonString = serviceArea.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            serviceArea = JsonConvert.DeserializeObject<ServiceArea>(jsonString);

            // create a new Local Area
            request = new HttpRequestMessage(HttpMethod.Post, "/api/localAreas");
            LocalArea localArea = new LocalArea();
            localArea.Id = 0;
            localArea.LocalAreaNumber = 1234;
            localArea.ServiceArea = serviceArea;
            jsonString = localArea.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            localArea = JsonConvert.DeserializeObject<LocalArea>(jsonString);

            // create a new Equipment Type
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentTypes");

            // create a new equipment type
            EquipmentType equipmentType = new EquipmentType();
            equipmentType.Id = 0;
            equipmentType.Name = initialName;            
            equipmentType.IsDumpTruck = false;
            equipmentType.NumberOfBlocks = 2;

            jsonString = equipmentType.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            equipmentType = JsonConvert.DeserializeObject<EquipmentType>(jsonString);

            request = new HttpRequestMessage(HttpMethod.Post, "/api/districtEquipmentTypes");

            // create a new District Equipment Type
            DistrictEquipmentType districtEquipmentType = new DistrictEquipmentType();
            districtEquipmentType.Id = 0;
            districtEquipmentType.DistrictEquipmentName = initialName;
            districtEquipmentType.District = district;
            districtEquipmentType.EquipmentType = equipmentType;
            jsonString = districtEquipmentType.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            districtEquipmentType = JsonConvert.DeserializeObject<DistrictEquipmentType>(jsonString);            

            // create equipment.
            int numberEquipment = 75;
            Equipment[] testEquipment = new Equipment[numberEquipment];
            int blockCount = 0;
            int currentBlock = 1;
            for (int i = 0; i < numberEquipment; i++)
            {
                testEquipment[i] = new Equipment();
                testEquipment[i].LocalArea = localArea;
                testEquipment[i].DistrictEquipmentType = districtEquipmentType;
                testEquipment[i].Seniority = (numberEquipment - i + 1) * 1.05F;
                testEquipment[i].IsSeniorityOverridden = true;
                testEquipment[i].BlockNumber = currentBlock;
                testEquipment[i] = CreateEquipment(testEquipment[i]);
                ++blockCount;
                if (blockCount >= 10 && currentBlock < 2)
                {
                    currentBlock++;
                    blockCount = 0;
                }                    

                // avoid database problems due to too many requests
                System.Threading.Thread.Sleep(200);
            }
            
            // Now create the rental request.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequests");            
            RentalRequest rentalRequest = new RentalRequest();
            rentalRequest.Status = initialName;
            rentalRequest.LocalArea = localArea;
            rentalRequest.DistrictEquipmentType = districtEquipmentType;

            jsonString = rentalRequest.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            rentalRequest = JsonConvert.DeserializeObject<RentalRequest>(jsonString);
            // get the id
            var id = rentalRequest.Id;

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequests/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            rentalRequest = JsonConvert.DeserializeObject<RentalRequest>(jsonString);

            // should be the same number of equipment.
            Assert.Equal(rentalRequest.RentalRequestRotationList.Count, numberEquipment);
            
            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/rentalrequests/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/rentalrequests/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // remove equipment.

            for (int i = 0; i < numberEquipment; i++)
            {
                request = new HttpRequestMessage(HttpMethod.Post, "/api/equipment/" + testEquipment[i].Id + "/delete");
                response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // should get a 404 if we try a get now.
                request = new HttpRequestMessage(HttpMethod.Get, "/api/equipment/" + testEquipment[i].Id);
                response = await _client.SendAsync(request);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            // now remove the other temporary objects.

            // districtEquipmentType
            request = new HttpRequestMessage(HttpMethod.Post, "/api/districtEquipmentTypes/" + districtEquipmentType.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/districtEquipmentTypes/" + districtEquipmentType.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // equipmentType
            request = new HttpRequestMessage(HttpMethod.Post, "/api/equipmentTypes/" + equipmentType.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/equipmentTypes/" + equipmentType.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // localArea
            request = new HttpRequestMessage(HttpMethod.Post, "/api/localAreas/" + localArea.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/localAreas/" + localArea.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // Service Area
            request = new HttpRequestMessage(HttpMethod.Post, "/api/serviceareas/" + serviceArea.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/serviceareas/" + serviceArea.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // District
            request = new HttpRequestMessage(HttpMethod.Post, "/api/districts/" + district.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/districts/" + district.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // Region
            request = new HttpRequestMessage(HttpMethod.Post, "/api/regions/" + region.Id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            request = new HttpRequestMessage(HttpMethod.Get, "/api/regions/" + region.Id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
