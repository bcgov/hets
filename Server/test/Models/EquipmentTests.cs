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
    ///  Class for testing the model Equipment
    /// </summary>
    
    public class EquipmentModelTests
    {
        private Equipment instance;

        /// <summary>
        /// Setup the test.
        /// </summary>        
        public EquipmentModelTests()
        {
            instance = new Equipment();
        }

    
        /// <summary>
        /// Test an instance of Equipment
        /// </summary>
        [Fact]
        public void EquipmentInstanceTest()
        {
            Assert.IsType<Equipment>(instance);  
        }

        /// <summary>
        /// Test the years of service calculation
        /// </summary>
        [Fact]
        public void YearsOfServiceTest()
        {
            // Sept. 18, 2012 should be 4.54 for 2017
            instance.ReceivedDate = new DateTime(2012, 9, 18);
            instance.CalculateYearsOfService(2017);
            float yearsOfService = (float) instance.YearsOfService;
            Assert.Equal(4.53424644F, yearsOfService);
        }     
        
        [Fact]   
        public void SeniorityTest()
        {
            /*
                Each piece of equipment has a Seniority Score that consists of:
                Years of Service* Constant +(Average number of hours worked per year for the last 3 Years)
                Constant is 60 for Dump Trucks and 50 for all other equipment.
                Example - 15 years of service Dump Truck with 456, 385 and 426 hours of service in the last 3 years, respectively
                Seniority = (15 * 60) + ((456 + 385 + 426) / 3)) = 1322(rounded)
            */
            instance.YearsOfService = 15.0F;
            instance.ServiceHoursLastYear = 456;
            instance.ServiceHoursTwoYearsAgo = 385;
            instance.ServiceHoursThreeYearsAgo = 426;
            instance.DumpTruck = new DumpTruck();
            instance.CalculateSeniority();

            Assert.Equal(instance.Seniority, 1322.33337F);
        }

        /// <summary>
        /// Tests the IsSeniorityAudite
        /// </summary>
        [Fact]
        
        public void SeniorityAuditRequiredTest()
        {
            Equipment original = new Equipment();
            Equipment changed = new Equipment();

            // both records have empty values
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);
            // test nulls 
            changed.Seniority = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.LocalArea = new LocalArea();
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);


            changed = new Equipment();
            changed.BlockNumber = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.Owner = new Owner();
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.ServiceHoursLastYear = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            // test different values
            changed = new Equipment();
            original = new Equipment();

            changed.Seniority = 1.0f;
            original.Seniority = 2.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 1";
            original = new Equipment();
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 2";

            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            original = new Equipment();
            changed.BlockNumber = 1.0f;
            original.BlockNumber = 2.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            original = new Equipment();
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 1";
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 2";

            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursLastYear = 2.0f;
            changed.ServiceHoursLastYear = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursTwoYearsAgo = 2.0f;
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursThreeYearsAgo = 2.0f;
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

            // now test cases where no change should be required.

            changed = new Equipment();
            original = new Equipment();

            changed.Seniority = 2.0f;
            original.Seniority = 2.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 1";
            original = new Equipment();
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 1";

            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();
            changed.BlockNumber = 1.0f;
            original.BlockNumber = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 1";
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 1";

            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursLastYear = 1.0f;
            changed.ServiceHoursLastYear = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursTwoYearsAgo = 1.0f;
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursThreeYearsAgo = 1.0f;
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            // test multiple fields

            changed = new Equipment();
            original = new Equipment();

            original.Seniority = 1.0f;            
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 1";
            original.BlockNumber = 1.0f;            
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 1";
            original.ServiceHoursLastYear = 1.0f;
            original.ServiceHoursTwoYearsAgo = 1.0f;            
            original.ServiceHoursThreeYearsAgo = 1.0f;

            changed.Seniority = 1.0f;            
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 1";
            changed.BlockNumber = 1.0f;
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 1";
            changed.ServiceHoursLastYear = 1.0f;
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            changed.ServiceHoursThreeYearsAgo = 1.0f;

            Assert.Equal(original.IsSeniorityAuditRequired(changed), false);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), false);

            changed = new Equipment();
            original = new Equipment();

            original.Seniority = 1.0f;            
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 1";
            original.BlockNumber = 1.0f;
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 1";
            original.ServiceHoursLastYear = 1.0f;
            original.ServiceHoursTwoYearsAgo = 1.0f;
            original.ServiceHoursThreeYearsAgo = 1.0f;

            changed.Seniority = 2.0f;
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 2";
            changed.BlockNumber = 2.0f;
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 2";
            changed.ServiceHoursLastYear = 2.0f;
            changed.ServiceHoursTwoYearsAgo = 2.0f;
            changed.ServiceHoursThreeYearsAgo = 2.0f;

            Assert.Equal(original.IsSeniorityAuditRequired(changed), true);
            Assert.Equal(changed.IsSeniorityAuditRequired(original), true);

        }

    }	
}

