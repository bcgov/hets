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
    ///  Class for testing the model ImportMap
    /// </summary>
    
    public class ImportMapModelTests
    {
        // TODO uncomment below to declare an instance variable for ImportMap
        private ImportMap instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public ImportMapModelTests()
        {
            instance = new ImportMap();
        }

    
        /// <summary>
        /// Test an instance of ImportMap
        /// </summary>
        [Fact]
        public void ImportMapInstanceTest()
        {
            Assert.IsType<ImportMap>(instance);  
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
        /// Test the property 'OldTable'
        /// </summary>
        [Fact]
        public void OldTableTest()
        {
            // TODO unit test for the property 'OldTable'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'NewTable'
        /// </summary>
        [Fact]
        public void NewTableTest()
        {
            // TODO unit test for the property 'NewTable'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'OldKey'
        /// </summary>
        [Fact]
        public void OldKeyTest()
        {
            // TODO unit test for the property 'OldKey'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'NewKey'
        /// </summary>
        [Fact]
        public void NewKeyTest()
        {
            // TODO unit test for the property 'NewKey'
			Assert.True(true);
        }

	}
	
}

