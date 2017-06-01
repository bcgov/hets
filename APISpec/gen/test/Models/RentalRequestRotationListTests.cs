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
    ///  Class for testing the model RentalRequestRotationList
    /// </summary>
    
    public class RentalRequestRotationListModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalRequestRotationList
        private RentalRequestRotationList instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalRequestRotationListModelTests()
        {
            instance = new RentalRequestRotationList();
        }

    
        /// <summary>
        /// Test an instance of RentalRequestRotationList
        /// </summary>
        [Fact]
        public void RentalRequestRotationListInstanceTest()
        {
            Assert.IsType<RentalRequestRotationList>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            // TODO unit test for the property 'Id'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RentalRequest'
        /// </summary>
        [Fact]
        public void RentalRequestTest()
        {
            // TODO unit test for the property 'RentalRequest'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RotationListSortOrder'
        /// </summary>
        [Fact]
        public void RotationListSortOrderTest()
        {
            // TODO unit test for the property 'RotationListSortOrder'
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
        /// Test the property 'WasAsked'
        /// </summary>
        [Fact]
        public void WasAskedTest()
        {
            // TODO unit test for the property 'WasAsked'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AskedDateTime'
        /// </summary>
        [Fact]
        public void AskedDateTimeTest()
        {
            // TODO unit test for the property 'AskedDateTime'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OfferResponse'
        /// </summary>
        [Fact]
        public void OfferResponseTest()
        {
            // TODO unit test for the property 'OfferResponse'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OfferRefusalReason'
        /// </summary>
        [Fact]
        public void OfferRefusalReasonTest()
        {
            // TODO unit test for the property 'OfferRefusalReason'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OfferResponseDatetime'
        /// </summary>
        [Fact]
        public void OfferResponseDatetimeTest()
        {
            // TODO unit test for the property 'OfferResponseDatetime'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OfferResponseNote'
        /// </summary>
        [Fact]
        public void OfferResponseNoteTest()
        {
            // TODO unit test for the property 'OfferResponseNote'
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

	}
	
}

