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
    ///  Class for testing the model Equipment
    /// </summary>
    
    public class EquipmentModelTests
    {
        // TODO uncomment below to declare an instance variable for Equipment
        private Equipment instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentModelTests()
        {
            instance = new Equipment();
        }

    
        /// <summary>
        /// Test an instance of Equipment
        /// </summary>
        [Fact]
        public void EquipmentInstanceTest()
        {
            Assert.IsType<Equipment>(instance);  
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
        /// Test the property 'LocalArea'
        /// </summary>
        [Fact]
        public void LocalAreaTest()
        {
            // TODO unit test for the property 'LocalArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentType'
        /// </summary>
        [Fact]
        public void EquipmentTypeTest()
        {
            // TODO unit test for the property 'EquipmentType'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DumpTruckDetails'
        /// </summary>
        [Fact]
        public void DumpTruckDetailsTest()
        {
            // TODO unit test for the property 'DumpTruckDetails'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Owner'
        /// </summary>
        [Fact]
        public void OwnerTest()
        {
            // TODO unit test for the property 'Owner'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipCd'
        /// </summary>
        [Fact]
        public void EquipCdTest()
        {
            // TODO unit test for the property 'EquipCd'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Approval'
        /// </summary>
        [Fact]
        public void ApprovalTest()
        {
            // TODO unit test for the property 'Approval'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ApprovedDate'
        /// </summary>
        [Fact]
        public void ApprovedDateTest()
        {
            // TODO unit test for the property 'ApprovedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ReceivedDate'
        /// </summary>
        [Fact]
        public void ReceivedDateTest()
        {
            // TODO unit test for the property 'ReceivedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine1'
        /// </summary>
        [Fact]
        public void AddressLine1Test()
        {
            // TODO unit test for the property 'AddressLine1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine2'
        /// </summary>
        [Fact]
        public void AddressLine2Test()
        {
            // TODO unit test for the property 'AddressLine2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine3'
        /// </summary>
        [Fact]
        public void AddressLine3Test()
        {
            // TODO unit test for the property 'AddressLine3'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AddressLine4'
        /// </summary>
        [Fact]
        public void AddressLine4Test()
        {
            // TODO unit test for the property 'AddressLine4'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'City'
        /// </summary>
        [Fact]
        public void CityTest()
        {
            // TODO unit test for the property 'City'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Postal'
        /// </summary>
        [Fact]
        public void PostalTest()
        {
            // TODO unit test for the property 'Postal'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlockNumber'
        /// </summary>
        [Fact]
        public void BlockNumberTest()
        {
            // TODO unit test for the property 'BlockNumber'
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
        /// Test the property 'CycleHrsWrk'
        /// </summary>
        [Fact]
        public void CycleHrsWrkTest()
        {
            // TODO unit test for the property 'CycleHrsWrk'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FrozenOut'
        /// </summary>
        [Fact]
        public void FrozenOutTest()
        {
            // TODO unit test for the property 'FrozenOut'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LastVerifiedDate'
        /// </summary>
        [Fact]
        public void LastVerifiedDateTest()
        {
            // TODO unit test for the property 'LastVerifiedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Licence'
        /// </summary>
        [Fact]
        public void LicenceTest()
        {
            // TODO unit test for the property 'Licence'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Make'
        /// </summary>
        [Fact]
        public void MakeTest()
        {
            // TODO unit test for the property 'Make'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Model'
        /// </summary>
        [Fact]
        public void ModelTest()
        {
            // TODO unit test for the property 'Model'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Year'
        /// </summary>
        [Fact]
        public void YearTest()
        {
            // TODO unit test for the property 'Year'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Type'
        /// </summary>
        [Fact]
        public void TypeTest()
        {
            // TODO unit test for the property 'Type'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'NumYears'
        /// </summary>
        [Fact]
        public void NumYearsTest()
        {
            // TODO unit test for the property 'NumYears'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Operator'
        /// </summary>
        [Fact]
        public void OperatorTest()
        {
            // TODO unit test for the property 'Operator'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PayRate'
        /// </summary>
        [Fact]
        public void PayRateTest()
        {
            // TODO unit test for the property 'PayRate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RefuseRate'
        /// </summary>
        [Fact]
        public void RefuseRateTest()
        {
            // TODO unit test for the property 'RefuseRate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Seniority'
        /// </summary>
        [Fact]
        public void SeniorityTest()
        {
            // TODO unit test for the property 'Seniority'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SerialNum'
        /// </summary>
        [Fact]
        public void SerialNumTest()
        {
            // TODO unit test for the property 'SerialNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Size'
        /// </summary>
        [Fact]
        public void SizeTest()
        {
            // TODO unit test for the property 'Size'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ToDate'
        /// </summary>
        [Fact]
        public void ToDateTest()
        {
            // TODO unit test for the property 'ToDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Working'
        /// </summary>
        [Fact]
        public void WorkingTest()
        {
            // TODO unit test for the property 'Working'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YearEndReg'
        /// </summary>
        [Fact]
        public void YearEndRegTest()
        {
            // TODO unit test for the property 'YearEndReg'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PrevRegArea'
        /// </summary>
        [Fact]
        public void PrevRegAreaTest()
        {
            // TODO unit test for the property 'PrevRegArea'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD'
        /// </summary>
        [Fact]
        public void YTDTest()
        {
            // TODO unit test for the property 'YTD'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD1'
        /// </summary>
        [Fact]
        public void YTD1Test()
        {
            // TODO unit test for the property 'YTD1'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD2'
        /// </summary>
        [Fact]
        public void YTD2Test()
        {
            // TODO unit test for the property 'YTD2'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YTD3'
        /// </summary>
        [Fact]
        public void YTD3Test()
        {
            // TODO unit test for the property 'YTD3'
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
        /// Test the property 'ArchiveDate'
        /// </summary>
        [Fact]
        public void ArchiveDateTest()
        {
            // TODO unit test for the property 'ArchiveDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'DraftBlockNum'
        /// </summary>
        [Fact]
        public void DraftBlockNumTest()
        {
            // TODO unit test for the property 'DraftBlockNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RegDumpTruck'
        /// </summary>
        [Fact]
        public void RegDumpTruckTest()
        {
            // TODO unit test for the property 'RegDumpTruck'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EquipmentAttachments'
        /// </summary>
        [Fact]
        public void EquipmentAttachmentsTest()
        {
            // TODO unit test for the property 'EquipmentAttachments'
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
        /// Test the property 'SeniorityAudit'
        /// </summary>
        [Fact]
        public void SeniorityAuditTest()
        {
            // TODO unit test for the property 'SeniorityAudit'
			Assert.True(true);
        }

	}
	
}

