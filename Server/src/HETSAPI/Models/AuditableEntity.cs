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
        /// The user account name of the application user who performed the action that created the record (e.g. 'JSMITH'). This value is not preceded by the directory name. 
        /// </summary>
        [MetaData(Description = "The user account name of the application user who performed the action that created the record (e.g. JSMITH). This value is not preceded by the directory name. ")]
        [MaxLength(255)]
        [JsonIgnore]
        public string AppCreateUserid { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that created the record
        /// </summary>
        [MetaData(Description = "The Globally Unique Identifier of the application user who performed the action that created the record.")]
        [MaxLength(255)]
        [JsonIgnore]
        public string AppCreateUserGuid { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that created the record
        /// </summary>
        [MetaData(Description = "The directory in which APP_CREATE_USERID is defined.")]
        [MaxLength(50)]
        [JsonIgnore]
        public string AppCreateUserDirectory { get; set; }

        /// <summary>
        /// The date and time of the application action that created the record.
        /// </summary>
        [MetaData(Description = "The date and time of the application action that created the record.")]
        [JsonIgnore]
        public DateTime AppCreateTimestamp { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that (last) updated the record
        /// </summary>
        [MetaData(Description = "The user account name of the application user who performed the action that created or last updated the record (e.g. JSMITH). This value is not preceded by the directory name.")]
        [MaxLength(255)]
        [JsonIgnore]
        public string AppLastUpdateUserid { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that created the record
        /// </summary>
        [MetaData(Description = "The Globally Unique Identifier of the application user who most recently updated the record.")]
        [MaxLength(255)]
        [JsonIgnore]
        public string AppLastUpdateUserGuid { get; set; }

        /// <summary>
        /// Siteminder UserId of the user that created the record
        /// </summary>
        [MetaData(Description = "The directory in which APP_LAST_UPDATE_USERID is defined.")]
        [MaxLength(50)]
        [JsonIgnore]
        public string AppLastUpdateUserDirectory { get; set; }

        /// <summary>
        /// Timestamp when the record was (last) updated
        /// </summary>
        [MetaData(Description = "The date and time of the application action that created or last updated the record.")]
        [JsonIgnore]
        public DateTime AppLastUpdateTimestamp { get; set; }
        
        /// <summary>
        /// The date and time the record was created.
        /// </summary>
        [MetaData(Description = "The date and time the record was created.")]
        [JsonIgnore]
        public DateTime DbCreateTimestamp { get; set; }

        /// <summary>
        /// The user or proxy account that created the record.
        /// </summary>
        [MetaData(Description = "The user or proxy account that created the record.")]
        [MaxLength(63)]
        [JsonIgnore]
        public string DbCreateUserId { get; set; }

        /// <summary>
        /// The date and time the record was created or last updated.
        /// </summary>
        [MetaData(Description = "The date and time the record was created or last updated.")]
        [JsonIgnore]
        public DateTime DbLastUpdateTimestamp { get; set; }

        /// <summary>
        /// The user or proxy account that created or last updated the record.
        /// </summary>
        [MetaData(Description = "The user or proxy account that created or last updated the record.")]
        [MaxLength(63)]
        [JsonIgnore]
        public string DbLastUpdateUserId { get; set; }
       
    }
}
