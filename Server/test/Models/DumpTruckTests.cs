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
    ///  Class for testing the model DumpTruck
    /// </summary>    
    public class DumpTruckModelTests
    {
        private readonly DumpTruck instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public DumpTruckModelTests()
        {
            instance = new DumpTruck();
        }

    
        /// <summary>
        /// Test an instance of DumpTruck
        /// </summary>
        [Fact]
        public void DumpTruckInstanceTest()
        {
            Assert.IsType<DumpTruck>(instance);  
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
        /// Test the property 'SingleAxle'
        /// </summary>
        [Fact]
        public void SingleAxleTest()
        {
            // TODO unit test for the property 'SingleAxle'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TandemAxle'
        /// </summary>
        [Fact]
        public void TandemAxleTest()
        {
            // TODO unit test for the property 'TandemAxle'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PUP'
        /// </summary>
        [Fact]
        public void PUPTest()
        {
            // TODO unit test for the property 'PUP'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BellyDump'
        /// </summary>
        [Fact]
        public void BellyDumpTest()
        {
            // TODO unit test for the property 'BellyDump'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Tridem'
        /// </summary>
        [Fact]
        public void TridemTest()
        {
            // TODO unit test for the property 'Tridem'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RockBox'
        /// </summary>
        [Fact]
        public void RockBoxTest()
        {
            // TODO unit test for the property 'RockBox'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'HiliftGate'
        /// </summary>
        [Fact]
        public void HiliftGateTest()
        {
            // TODO unit test for the property 'HiliftGate'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'WaterTruck'
        /// </summary>
        [Fact]
        public void WaterTruckTest()
        {
            // TODO unit test for the property 'WaterTruck'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'SealCoatHitch'
        /// </summary>
        [Fact]
        public void SealCoatHitchTest()
        {
            // TODO unit test for the property 'SealCoatHitch'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RearAxleSpacing'
        /// </summary>
        [Fact]
        public void RearAxleSpacingTest()
        {
            // TODO unit test for the property 'RearAxleSpacing'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FrontTireSize'
        /// </summary>
        [Fact]
        public void FrontTireSizeTest()
        {
            // TODO unit test for the property 'FrontTireSize'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FrontTireUOM'
        /// </summary>
        [Fact]
        public void FrontTireUOMTest()
        {
            // TODO unit test for the property 'FrontTireUOM'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FrontAxleCapacity'
        /// </summary>
        [Fact]
        public void FrontAxleCapacityTest()
        {
            // TODO unit test for the property 'FrontAxleCapacity'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'RearAxleCapacity'
        /// </summary>
        [Fact]
        public void RearAxleCapacityTest()
        {
            // TODO unit test for the property 'RearAxleCapacity'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LegalLoad'
        /// </summary>
        [Fact]
        public void LegalLoadTest()
        {
            // TODO unit test for the property 'LegalLoad'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LegalCapacity'
        /// </summary>
        [Fact]
        public void LegalCapacityTest()
        {
            // TODO unit test for the property 'LegalCapacity'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LegalPUPTareWeight'
        /// </summary>
        [Fact]
        public void LegalPUPTareWeightTest()
        {
            // TODO unit test for the property 'LegalPUPTareWeight'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedGVW'
        /// </summary>
        [Fact]
        public void LicencedGVWTest()
        {
            // TODO unit test for the property 'LicencedGVW'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedGVWUOM'
        /// </summary>
        [Fact]
        public void LicencedGVWUOMTest()
        {
            // TODO unit test for the property 'LicencedGVWUOM'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedTareWeight'
        /// </summary>
        [Fact]
        public void LicencedTareWeightTest()
        {
            // TODO unit test for the property 'LicencedTareWeight'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedPUPTareWeight'
        /// </summary>
        [Fact]
        public void LicencedPUPTareWeightTest()
        {
            // TODO unit test for the property 'LicencedPUPTareWeight'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedLoad'
        /// </summary>
        [Fact]
        public void LicencedLoadTest()
        {
            // TODO unit test for the property 'LicencedLoad'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LicencedCapacity'
        /// </summary>
        [Fact]
        public void LicencedCapacityTest()
        {
            // TODO unit test for the property 'LicencedCapacity'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BoxLength'
        /// </summary>
        [Fact]
        public void BoxLengthTest()
        {
            // TODO unit test for the property 'BoxLength'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BoxWidth'
        /// </summary>
        [Fact]
        public void BoxWidthTest()
        {
            // TODO unit test for the property 'BoxWidth'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BoxHeight'
        /// </summary>
        [Fact]
        public void BoxHeightTest()
        {
            // TODO unit test for the property 'BoxHeight'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'BoxCapacity'
        /// </summary>
        [Fact]
        public void BoxCapacityTest()
        {
            // TODO unit test for the property 'BoxCapacity'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TrailerBoxLength'
        /// </summary>
        [Fact]
        public void TrailerBoxLengthTest()
        {
            // TODO unit test for the property 'TrailerBoxLength'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TrailerBoxWidth'
        /// </summary>
        [Fact]
        public void TrailerBoxWidthTest()
        {
            // TODO unit test for the property 'TrailerBoxWidth'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TrailerBoxHeight'
        /// </summary>
        [Fact]
        public void TrailerBoxHeightTest()
        {
            // TODO unit test for the property 'TrailerBoxHeight'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'TrailerBoxCapacity'
        /// </summary>
        [Fact]
        public void TrailerBoxCapacityTest()
        {
            // TODO unit test for the property 'TrailerBoxCapacity'
			Assert.True(true);
        }

	}
	
}

