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
    ///  Class for testing the model Owner
    /// </summary>    
    public class OwnerModelTests
    {
        private readonly Owner instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public OwnerModelTests()
        {
            instance = new Owner();
        }

    
        /// <summary>
        /// Test an instance of Owner
        /// </summary>
        [Fact]
        public void OwnerInstanceTest()
        {
            Assert.IsType<Owner>(instance);  
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
        /// Test the property 'OwnerCd'
        /// </summary>
        [Fact]
        public void OwnerCdTest()
        {
            // TODO unit test for the property 'OwnerCd'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OwnerFirstName'
        /// </summary>
        [Fact]
        public void OwnerFirstNameTest()
        {
            // TODO unit test for the property 'OwnerFirstName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OwnerLastName'
        /// </summary>
        [Fact]
        public void OwnerLastNameTest()
        {
            // TODO unit test for the property 'OwnerLastName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ContactPerson'
        /// </summary>
        [Fact]
        public void ContactPersonTest()
        {
            // TODO unit test for the property 'ContactPerson'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LocalToArea'
        /// </summary>
        [Fact]
        public void LocalToAreaTest()
        {
            // TODO unit test for the property 'LocalToArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'MaintenanceContractor'
        /// </summary>
        [Fact]
        public void MaintenanceContractorTest()
        {
            // TODO unit test for the property 'MaintenanceContractor'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Comment'
        /// </summary>
        [Fact]
        public void CommentTest()
        {
            // TODO unit test for the property 'Comment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'WCBNum'
        /// </summary>
        [Fact]
        public void WCBNumTest()
        {
            // TODO unit test for the property 'WCBNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'WCBExpiryDate'
        /// </summary>
        [Fact]
        public void WCBExpiryDateTest()
        {
            // TODO unit test for the property 'WCBExpiryDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CGLCompany'
        /// </summary>
        [Fact]
        public void CGLCompanyTest()
        {
            // TODO unit test for the property 'CGLCompany'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CGLPolicy'
        /// </summary>
        [Fact]
        public void CGLPolicyTest()
        {
            // TODO unit test for the property 'CGLPolicy'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CGLStartDate'
        /// </summary>
        [Fact]
        public void CGLStartDateTest()
        {
            // TODO unit test for the property 'CGLStartDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CGLEndDate'
        /// </summary>
        [Fact]
        public void CGLEndDateTest()
        {
            // TODO unit test for the property 'CGLEndDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'StatusCd'
        /// </summary>
        [Fact]
        public void StatusCdTest()
        {
            // TODO unit test for the property 'StatusCd'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ArchiveCd'
        /// </summary>
        [Fact]
        public void ArchiveCdTest()
        {
            // TODO unit test for the property 'ArchiveCd'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ArchiveReason'
        /// </summary>
        [Fact]
        public void ArchiveReasonTest()
        {
            // TODO unit test for the property 'ArchiveReason'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LocalArea'
        /// </summary>
        [Fact]
        public void LocalAreaTest()
        {
            // TODO unit test for the property 'LocalArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PrimaryContact'
        /// </summary>
        [Fact]
        public void PrimaryContactTest()
        {
            // TODO unit test for the property 'PrimaryContact'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Contacts'
        /// </summary>
        [Fact]
        public void ContactsTest()
        {
            // TODO unit test for the property 'Contacts'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Notes'
        /// </summary>
        [Fact]
        public void NotesTest()
        {
            // TODO unit test for the property 'Notes'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Attachments'
        /// </summary>
        [Fact]
        public void AttachmentsTest()
        {
            // TODO unit test for the property 'Attachments'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'History'
        /// </summary>
        [Fact]
        public void HistoryTest()
        {
            // TODO unit test for the property 'History'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentList'
        /// </summary>
        [Fact]
        public void EquipmentListTest()
        {
            // TODO unit test for the property 'EquipmentList'
			Assert.True(true);
        }

	}
	
}

