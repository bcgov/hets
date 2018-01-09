using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Auditiable Entity Annotation Extension
    /// </summary>
    public class AuditableEntity
    {
        /// <summary>
        /// Siteminder UserId of the user that created the record
        /// </summary>
        [MetaData(Description = "Audit information - SM User Id for the User who created the record.")]
        [MaxLength(50)]
        [JsonIgnore]
        public string CreateUserid { get; set; }
        
        /// <summary>
        /// Timestamp when the record was created
        /// </summary>
        [MetaData(Description = "Audit information - Timestamp for record creation")]
        [JsonIgnore]
        public DateTime CreateTimestamp { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that (last) updated the record
        /// </summary>
        [MetaData(Description = "Audit information - SM User Id for the User who most recently updated the record")]
        [MaxLength(50)]
        [JsonIgnore]
        public string LastUpdateUserid { get; set; }

        /// <summary>
        /// Timestamp when the record was (last) updated
        /// </summary>
        [MetaData(Description = "Audit information - Timestamp for record modification")]
        [JsonIgnore]
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
