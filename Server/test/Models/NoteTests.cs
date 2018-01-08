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
    ///  Class for testing the model Note
    /// </summary>    
    public class NoteModelTests
    {
        private readonly Note instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public NoteModelTests()
        {
            instance = new Note();
        }

    
        /// <summary>
        /// Test an instance of Note
        /// </summary>
        [Fact]
        public void NoteInstanceTest()
        {
            Assert.IsType<Note>(instance);  
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
        /// Test the property '_Note'
        /// </summary>
        [Fact]
        public void _NoteTest()
        {
            // TODO unit test for the property '_Note'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsNoLongerRelevant'
        /// </summary>
        [Fact]
        public void IsNoLongerRelevantTest()
        {
            // TODO unit test for the property 'IsNoLongerRelevant'
			Assert.True(true);
        }

	}
	
}

