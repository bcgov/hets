/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HETSAPI.Models
{
    public class AuditableEntity
    {
        [MetaDataExtension(Description = "Audit information - SM User Id for the User who created the record.")]
        [MaxLength(50)]
        [JsonIgnore]
        public string CreateUserid { get; set; }
        
        [MetaDataExtension(Description = "Audit information - Timestamp for record creation")]
        [JsonIgnore]
        public DateTime CreateTimestamp { get; set; }

        [MetaDataExtension(Description = "Audit information - SM User Id for the User who most recently updated the record")]
        [MaxLength(50)]
        [JsonIgnore]
        public string LastUpdateUserid { get; set; }

        [MetaDataExtension(Description = "Audit information - Timestamp for record modification")]
        [JsonIgnore]
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
