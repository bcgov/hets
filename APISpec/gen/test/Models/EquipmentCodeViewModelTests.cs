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
    ///  Class for testing the model EquipmentCodeViewModel
    /// </summary>
    
    public class EquipmentCodeViewModelModelTests
    {
        // TODO uncomment below to declare an instance variable for EquipmentCodeViewModel
        private EquipmentCodeViewModel instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentCodeViewModelModelTests()
        {
            instance = new EquipmentCodeViewModel();
        }

    
        /// <summary>
        /// Test an instance of EquipmentCodeViewModel
        /// </summary>
        [Fact]
        public void EquipmentCodeViewModelInstanceTest()
        {
            Assert.IsType<EquipmentCodeViewModel>(instance);  
        }

        /// <summary>
        /// Test the property 'EquipmentCode'
        /// </summary>
        [Fact]
        public void EquipmentCodeTest()
        {
            // TODO unit test for the property 'EquipmentCode'
			Assert.True(true);
        }

	}
	
}

