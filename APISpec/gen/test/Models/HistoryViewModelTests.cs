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
    ///  Class for testing the model HistoryViewModel
    /// </summary>
    
    public class HistoryViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for HistoryViewModel
        private HistoryViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public HistoryViewModelModelTests()
        {
            instance = new HistoryViewModel();
        }

    
        /// <summary>
        /// Test an instance of HistoryViewModel
        /// </summary>
        [Fact]
        public void HistoryViewModelInstanceTest()
        {
            Assert.IsType<HistoryViewModel>(instance);  
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
        /// Test the property 'HistoryText'
        /// </summary>
        [Fact]
        public void HistoryTextTest()
        {
            // TODO unit test for the property 'HistoryText'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LastUpdateUserid'
        /// </summary>
        [Fact]
        public void LastUpdateUseridTest()
        {
            // TODO unit test for the property 'LastUpdateUserid'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'LastUpdateTimestamp'
        /// </summary>
        [Fact]
        public void LastUpdateTimestampTest()
        {
            // TODO unit test for the property 'LastUpdateTimestamp'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'AffectedEntityId'
        /// </summary>
        [Fact]
        public void AffectedEntityIdTest()
        {
            // TODO unit test for the property 'AffectedEntityId'
			Assert.True(true);
        }

	}
	
}

