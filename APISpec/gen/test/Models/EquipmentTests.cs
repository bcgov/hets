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
            // TODO unit test for the property 'Id'
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
        /// Test the property 'DistrictEquipmentType'
        /// </summary>
        [Fact]
        public void DistrictEquipmentTypeTest()
        {
            // TODO unit test for the property 'DistrictEquipmentType'
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
        /// Test the property 'EquipmentCode'
        /// </summary>
        [Fact]
        public void EquipmentCodeTest()
        {
            // TODO unit test for the property 'EquipmentCode'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Status'
        /// </summary>
        [Fact]
        public void StatusTest()
        {
            // TODO unit test for the property 'Status'
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
        /// Test the property 'LastVerifiedDate'
        /// </summary>
        [Fact]
        public void LastVerifiedDateTest()
        {
            // TODO unit test for the property 'LastVerifiedDate'
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
        /// Test the property 'IsInformationUpdateNeeded'
        /// </summary>
        [Fact]
        public void IsInformationUpdateNeededTest()
        {
            // TODO unit test for the property 'IsInformationUpdateNeeded'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'InformationUpdateNeededReason'
        /// </summary>
        [Fact]
        public void InformationUpdateNeededReasonTest()
        {
            // TODO unit test for the property 'InformationUpdateNeededReason'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencePlate'
        /// </summary>
        [Fact]
        public void LicencePlateTest()
        {
            // TODO unit test for the property 'LicencePlate'
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
        /// Test the property 'SerialNumber'
        /// </summary>
        [Fact]
        public void SerialNumberTest()
        {
            // TODO unit test for the property 'SerialNumber'
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
        /// Test the property 'BlockNumber'
        /// </summary>
        [Fact]
        public void BlockNumberTest()
        {
            // TODO unit test for the property 'BlockNumber'
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
        /// Test the property 'NumberInBlock'
        /// </summary>
        [Fact]
        public void NumberInBlockTest()
        {
            // TODO unit test for the property 'NumberInBlock'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsSeniorityOverridden'
        /// </summary>
        [Fact]
        public void IsSeniorityOverriddenTest()
        {
            // TODO unit test for the property 'IsSeniorityOverridden'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SeniorityOverrideReason'
        /// </summary>
        [Fact]
        public void SeniorityOverrideReasonTest()
        {
            // TODO unit test for the property 'SeniorityOverrideReason'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SeniorityEffectiveDate'
        /// </summary>
        [Fact]
        public void SeniorityEffectiveDateTest()
        {
            // TODO unit test for the property 'SeniorityEffectiveDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'YearsOfService'
        /// </summary>
        [Fact]
        public void YearsOfServiceTest()
        {
            // TODO unit test for the property 'YearsOfService'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursLastYear'
        /// </summary>
        [Fact]
        public void ServiceHoursLastYearTest()
        {
            // TODO unit test for the property 'ServiceHoursLastYear'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursTwoYearsAgo'
        /// </summary>
        [Fact]
        public void ServiceHoursTwoYearsAgoTest()
        {
            // TODO unit test for the property 'ServiceHoursTwoYearsAgo'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ServiceHoursThreeYearsAgo'
        /// </summary>
        [Fact]
        public void ServiceHoursThreeYearsAgoTest()
        {
            // TODO unit test for the property 'ServiceHoursThreeYearsAgo'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ArchiveCode'
        /// </summary>
        [Fact]
        public void ArchiveCodeTest()
        {
            // TODO unit test for the property 'ArchiveCode'
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
        /// Test the property 'DumpTruck'
        /// </summary>
        [Fact]
        public void DumpTruckTest()
        {
            // TODO unit test for the property 'DumpTruck'
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

	}
	
}

