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
    ///  Class for testing the model UserFavourite
    /// </summary>    
    public class UserFavouriteModelTests
    {
        private readonly UserFavourite instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public UserFavouriteModelTests()
        {
            instance = new UserFavourite();
        }

    
        /// <summary>
        /// Test an instance of UserFavourite
        /// </summary>
        [Fact]
        public void UserFavouriteInstanceTest()
        {
            Assert.IsType<UserFavourite>(instance);  
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
        /// Test the property 'Name'
        /// </summary>
        [Fact]
        public void NameTest()
        {
            // TODO unit test for the property 'Name'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Value'
        /// </summary>
        [Fact]
        public void ValueTest()
        {
            // TODO unit test for the property 'Value'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsDefault'
        /// </summary>
        [Fact]
        public void IsDefaultTest()
        {
            // TODO unit test for the property 'IsDefault'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'FavouriteContextType'
        /// </summary>
        [Fact]
        public void FavouriteContextTypeTest()
        {
            // TODO unit test for the property 'FavouriteContextType'
			Assert.True(true);
        }

	}
	
}

