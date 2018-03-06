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
        private readonly Equipment instance;

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
            instance.CalculateSeniority(1);

            Assert.Equal(1322.33337F, instance.Seniority);
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
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            // test nulls 
            changed.Seniority = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.LocalArea = new LocalArea();
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.BlockNumber = 1;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.Owner = new Owner();
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.ServiceHoursLastYear = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            // test different values
            changed = new Equipment();
            original = new Equipment();

            changed.Seniority = 1.0f;
            original.Seniority = 2.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 1";

            original = new Equipment
            {
                LocalArea = new LocalArea()
            };

            original.LocalArea.Name = "Area 2";

            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();
            changed.BlockNumber = 1;
            original.BlockNumber = 2;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();

            changed.Owner = new Owner
            {
                OrganizationName = "Org 1"
            };

            original.Owner = new Owner
            {
                OrganizationName = "Org 2"
            };

            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment
            {
                ServiceHoursLastYear = 2.0f
            };

            changed.ServiceHoursLastYear = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment
            {
                ServiceHoursTwoYearsAgo = 2.0f
            };
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment
            {
                ServiceHoursThreeYearsAgo = 2.0f
            };
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));

            // now test cases where no change should be required
            changed = new Equipment();
            original = new Equipment();

            changed.Seniority = 2.0f;
            original.Seniority = 2.0f;
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment
            {
                LocalArea = new LocalArea()
            };
            changed.LocalArea.Name = "Area 1";
            original = new Equipment
            {
                LocalArea = new LocalArea()
            };
            original.LocalArea.Name = "Area 1";

            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();
            changed.BlockNumber = 1;
            original.BlockNumber = 1;
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();
            changed.Owner = new Owner
            {
                OrganizationName = "Org 1"
            };
            original.Owner = new Owner
            {
                OrganizationName = "Org 1"
            };

            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment
            {
                ServiceHoursLastYear = 1.0f
            };
            changed.ServiceHoursLastYear = 1.0f;
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursTwoYearsAgo = 1.0f;
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();
            original.ServiceHoursThreeYearsAgo = 1.0f;
            changed.ServiceHoursThreeYearsAgo = 1.0f;
            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            // test multiple fields
            changed = new Equipment();
            original = new Equipment();

            original.Seniority = 1.0f;            
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 1";
            original.BlockNumber = 1;            
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 1";
            original.ServiceHoursLastYear = 1.0f;
            original.ServiceHoursTwoYearsAgo = 1.0f;            
            original.ServiceHoursThreeYearsAgo = 1.0f;

            changed.Seniority = 1.0f;            
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 1";
            changed.BlockNumber = 1;
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 1";
            changed.ServiceHoursLastYear = 1.0f;
            changed.ServiceHoursTwoYearsAgo = 1.0f;
            changed.ServiceHoursThreeYearsAgo = 1.0f;

            Assert.False(original.IsSeniorityAuditRequired(changed));
            Assert.False(changed.IsSeniorityAuditRequired(original));

            changed = new Equipment();
            original = new Equipment();

            original.Seniority = 1.0f;            
            original.LocalArea = new LocalArea();
            original.LocalArea.Name = "Area 1";
            original.BlockNumber = 1;
            original.Owner = new Owner();
            original.Owner.OrganizationName = "Org 1";
            original.ServiceHoursLastYear = 1.0f;
            original.ServiceHoursTwoYearsAgo = 1.0f;
            original.ServiceHoursThreeYearsAgo = 1.0f;

            changed.Seniority = 2.0f;
            changed.LocalArea = new LocalArea();
            changed.LocalArea.Name = "Area 2";
            changed.BlockNumber = 2;
            changed.Owner = new Owner();
            changed.Owner.OrganizationName = "Org 2";
            changed.ServiceHoursLastYear = 2.0f;
            changed.ServiceHoursTwoYearsAgo = 2.0f;
            changed.ServiceHoursThreeYearsAgo = 2.0f;

            Assert.True(original.IsSeniorityAuditRequired(changed));
            Assert.True(changed.IsSeniorityAuditRequired(original));
        }
    }	
}

