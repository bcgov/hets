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
    ///  Class for testing the model Attachment
    /// </summary>
    
    public class AttachmentModelTests
    {
        // TODO uncomment below to declare an instance variable for Attachment
        private Attachment instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public AttachmentModelTests()
        {
            instance = new Attachment();
        }

    
        /// <summary>
        /// Test an instance of Attachment
        /// </summary>
        [Fact]
        public void AttachmentInstanceTest()
        {
            Assert.IsType<Attachment>(instance);  
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
        /// Test the property 'InternalFileName'
        /// </summary>
        [Fact]
        public void InternalFileNameTest()
        {
            // TODO unit test for this property
            Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ExternalFileName'
        /// </summary>
        [Fact]
        public void ExternalFileNameTest()
        {
            // TODO unit test for this property
            Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Description'
        /// </summary>
        [Fact]
        public void DescriptionTest()
        {
            // TODO unit test for this property
            Assert.True(true);
        }

	}
	
}

