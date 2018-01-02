using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

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
