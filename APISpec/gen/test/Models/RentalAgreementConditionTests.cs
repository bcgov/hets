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
    ///  Class for testing the model RentalAgreementCondition
    /// </summary>
    
    public class RentalAgreementConditionModelTests
    {
        // TODO uncomment below to declare an instance variable for RentalAgreementCondition
        private RentalAgreementCondition instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public RentalAgreementConditionModelTests()
        {
            instance = new RentalAgreementCondition();
        }

    
        /// <summary>
        /// Test an instance of RentalAgreementCondition
        /// </summary>
        [Fact]
        public void RentalAgreementConditionInstanceTest()
        {
            Assert.IsType<RentalAgreementCondition>(instance);  
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
        /// Test the property 'RentalAgreement'
        /// </summary>
        [Fact]
        public void RentalAgreementTest()
        {
            // TODO unit test for the property 'RentalAgreement'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'ConditionName'
        /// </summary>
        [Fact]
        public void ConditionNameTest()
        {
            // TODO unit test for the property 'ConditionName'
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

	}
	
}

