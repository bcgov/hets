/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI;
using HETSAPI.Models;
using System.Reflection;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model HireOffer
    /// </summary>
    
    public class HireOfferModelTests
    {
        // TODO uncomment below to declare an instance variable for HireOffer
        private HireOffer instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public HireOfferModelTests()
        {
            instance = new HireOffer();
        }

    
        /// <summary>
        /// Test an instance of HireOffer
        /// </summary>
        [Fact]
        public void HireOfferInstanceTest()
        {
            Assert.IsType<HireOffer>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            Assert.IsType<int>(instance.Id);
        }
        /// <summary>
        /// Test the property 'Request'
        /// </summary>
        [Fact]
        public void RequestTest()
        {
            // TODO unit test for the property 'Request'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Equipment'
        /// </summary>
        [Fact]
        public void EquipmentTest()
        {
            // TODO unit test for the property 'Equipment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalAgreement'
        /// </summary>
        [Fact]
        public void RentalAgreementTest()
        {
            // TODO unit test for the property 'RentalAgreement'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsForceHire'
        /// </summary>
        [Fact]
        public void IsForceHireTest()
        {
            // TODO unit test for the property 'IsForceHire'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Asked'
        /// </summary>
        [Fact]
        public void AskedTest()
        {
            // TODO unit test for the property 'Asked'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskedDate'
        /// </summary>
        [Fact]
        public void AskedDateTest()
        {
            // TODO unit test for the property 'AskedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AcceptedOffer'
        /// </summary>
        [Fact]
        public void AcceptedOfferTest()
        {
            // TODO unit test for the property 'AcceptedOffer'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RefuseReason'
        /// </summary>
        [Fact]
        public void RefuseReasonTest()
        {
            // TODO unit test for the property 'RefuseReason'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Note'
        /// </summary>
        [Fact]
        public void NoteTest()
        {
            // TODO unit test for the property 'Note'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentVerifiedActive'
        /// </summary>
        [Fact]
        public void EquipmentVerifiedActiveTest()
        {
            // TODO unit test for the property 'EquipmentVerifiedActive'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FlagEquipmentUpdate'
        /// </summary>
        [Fact]
        public void FlagEquipmentUpdateTest()
        {
            // TODO unit test for the property 'FlagEquipmentUpdate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentUpdateReason'
        /// </summary>
        [Fact]
        public void EquipmentUpdateReasonTest()
        {
            // TODO unit test for the property 'EquipmentUpdateReason'
			Assert.True(true);
        }

	}
	
}

