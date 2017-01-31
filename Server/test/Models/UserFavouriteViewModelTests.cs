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
using HETSAPI.ViewModels;
using System.Reflection;

namespace HETSAPI.Test
{
    /// <summary>
    ///  Class for testing the model UserFavouriteViewModel
    /// </summary>
    
    public class UserFavouriteViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for UserFavouriteViewModel
        private UserFavouriteViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public UserFavouriteViewModelModelTests()
        {
            instance = new UserFavouriteViewModel();
        }

    
        /// <summary>
        /// Test an instance of UserFavouriteViewModel
        /// </summary>
        [Fact]
        public void UserFavouriteViewModelInstanceTest()
        {
            Assert.IsType<UserFavouriteViewModel>(instance);  
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
        /// Test the property 'FavouriteContextTypeId'
        /// </summary>
        [Fact]
        public void FavouriteContextTypeIdTest()
        {
            // TODO unit test for the property 'FavouriteContextTypeId'
			Assert.True(true);
        }

	}
	
}

