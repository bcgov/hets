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
    ///  Class for testing the model NotificationViewModel
    /// </summary>
    
    public class NotificationViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for NotificationViewModel
        private NotificationViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public NotificationViewModelModelTests()
        {
            instance = new NotificationViewModel();
        }

    
        /// <summary>
        /// Test an instance of NotificationViewModel
        /// </summary>
        [Fact]
        public void NotificationViewModelInstanceTest()
        {
            Assert.IsType<NotificationViewModel>(instance);  
        }

        /// <summary>
        /// Test the property 'Id'
        /// </summary>
        [Fact]
        public void IdTest()
        {
            // TODO unit test for this property
            Assert.True(true);
        }
        /// <summary>
        /// Test the property 'EventId'
        /// </summary>
        [Fact]
        public void EventIdTest()
        {
            // TODO unit test for the property 'EventId'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'Event2Id'
        /// </summary>
        [Fact]
        public void Event2IdTest()
        {
            // TODO unit test for the property 'Event2Id'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'HasBeenViewed'
        /// </summary>
        [Fact]
        public void HasBeenViewedTest()
        {
            // TODO unit test for the property 'HasBeenViewed'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsWatchNotification'
        /// </summary>
        [Fact]
        public void IsWatchNotificationTest()
        {
            // TODO unit test for the property 'IsWatchNotification'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsExpired'
        /// </summary>
        [Fact]
        public void IsExpiredTest()
        {
            // TODO unit test for the property 'IsExpired'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'IsAllDay'
        /// </summary>
        [Fact]
        public void IsAllDayTest()
        {
            // TODO unit test for the property 'IsAllDay'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'PriorityCode'
        /// </summary>
        [Fact]
        public void PriorityCodeTest()
        {
            // TODO unit test for the property 'PriorityCode'
			Assert.True(true);
        }
        /// <summary>
        /// Test the property 'UserId'
        /// </summary>
        [Fact]
        public void UserIdTest()
        {
            // TODO unit test for the property 'UserId'
			Assert.True(true);
        }

	}
	
}

