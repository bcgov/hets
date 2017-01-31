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
    ///  Class for testing the model RotationListBlock
    /// </summary>
    
    public class RotationListBlockModelTests
    {
        // TODO uncomment below to declare an instance variable for RotationListBlock
        private RotationListBlock instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RotationListBlockModelTests()
        {
            instance = new RotationListBlock();
        }

    
        /// <summary>
        /// Test an instance of RotationListBlock
        /// </summary>
        [Fact]
        public void RotationListBlockInstanceTest()
        {
            Assert.IsType<RotationListBlock>(instance);  
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
        /// Test the property 'RotationList'
        /// </summary>
        [Fact]
        public void RotationListTest()
        {
            // TODO unit test for the property 'RotationList'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlockNum'
        /// </summary>
        [Fact]
        public void BlockNumTest()
        {
            // TODO unit test for the property 'BlockNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'CycleNum'
        /// </summary>
        [Fact]
        public void CycleNumTest()
        {
            // TODO unit test for the property 'CycleNum'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'MaxCycle'
        /// </summary>
        [Fact]
        public void MaxCycleTest()
        {
            // TODO unit test for the property 'MaxCycle'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LastHiredEquipment'
        /// </summary>
        [Fact]
        public void LastHiredEquipmentTest()
        {
            // TODO unit test for the property 'LastHiredEquipment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'StartCycleEquipment'
        /// </summary>
        [Fact]
        public void StartCycleEquipmentTest()
        {
            // TODO unit test for the property 'StartCycleEquipment'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Moved'
        /// </summary>
        [Fact]
        public void MovedTest()
        {
            // TODO unit test for the property 'Moved'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'StartWasZero'
        /// </summary>
        [Fact]
        public void StartWasZeroTest()
        {
            // TODO unit test for the property 'StartWasZero'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RotatedBlock'
        /// </summary>
        [Fact]
        public void RotatedBlockTest()
        {
            // TODO unit test for the property 'RotatedBlock'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BlockName'
        /// </summary>
        [Fact]
        public void BlockNameTest()
        {
            // TODO unit test for the property 'BlockName'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Closed'
        /// </summary>
        [Fact]
        public void ClosedTest()
        {
            // TODO unit test for the property 'Closed'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ClosedComments'
        /// </summary>
        [Fact]
        public void ClosedCommentsTest()
        {
            // TODO unit test for the property 'ClosedComments'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ClosedDate'
        /// </summary>
        [Fact]
        public void ClosedDateTest()
        {
            // TODO unit test for the property 'ClosedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ClosedBy'
        /// </summary>
        [Fact]
        public void ClosedByTest()
        {
            // TODO unit test for the property 'ClosedBy'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ReservedDate'
        /// </summary>
        [Fact]
        public void ReservedDateTest()
        {
            // TODO unit test for the property 'ReservedDate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ReservedBy'
        /// </summary>
        [Fact]
        public void ReservedByTest()
        {
            // TODO unit test for the property 'ReservedBy'
			Assert.True(true);
        }

	}
	
}

