using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// History Database Model
    /// </summary>
    [MetaData (Description = "A log entry created by the system based on a triggering event and related to an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]
    public sealed class History : AuditableEntity, IEquatable<History>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public History()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="History" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a History (required).</param>
        /// <param name="historyText">The text of the history entry tracked against the related entity. (required).</param>
        /// <param name="createdDate">Date the record is created..</param>
        public History(int id, string historyText, DateTime? createdDate = null)
        {   
            Id = id;
            HistoryText = historyText;
            CreatedDate = createdDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a History
        /// </summary>
        /// <value>A system-generated unique identifier for a History</value>
        [MetaData (Description = "A system-generated unique identifier for a History")]
        public int Id { get; set; }
        
        /// <summary>
        /// The text of the history entry tracked against the related entity.
        /// </summary>
        /// <value>The text of the history entry tracked against the related entity.</value>
        [MetaData (Description = "The text of the history entry tracked against the related entity.")]
        [MaxLength(2048)]        
        public string HistoryText { get; set; }
        
        /// <summary>
        /// Date the record is created.
        /// </summary>
        /// <value>Date the record is created.</value>
        [MetaData (Description = "Date the record is created.")]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Link to the Owner
        /// </summary>
        /// <value>Link to the Owner</value>
        [MetaData(Description = "Link to the Owner.")]
        public Owner Owner { get; set; }

        /// <summary>
        /// Foreign key for Owner 
        /// </summary>   
        [ForeignKey("Owner")]
        [JsonIgnore]
        [MetaData(Description = "Link to the Owner.")]
        public int? OwnerId { get; set; }

        /// <summary>
        /// Link to the Project
        /// </summary>
        /// <value>Link to the Project</value>
        [MetaData(Description = "Link to the Project.")]
        public Project Project { get; set; }

        /// <summary>
        /// Foreign key for Project 
        /// </summary>   
        [ForeignKey("Project")]
        [JsonIgnore]
        [MetaData(Description = "Link to the Project.")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Link to the Equipment
        /// </summary>
        /// <value>Link to the Equipment</value>
        [MetaData(Description = "Link to the Equipment.")]
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>   
        [ForeignKey("Equipment")]
        [JsonIgnore]
        [MetaData(Description = "Link to the Equipment.")]
        public int? EquipmentId { get; set; }

        /// <summary>
        /// Link to the RentalRequest
        /// </summary>
        /// <value>Link to the Equipment</value>
        [MetaData(Description = "Link to the RentalRequest.")]
        public RentalRequest RentalRequest { get; set; }

        /// <summary>
        /// Foreign key for RentalRequest 
        /// </summary>   
        [ForeignKey("RentalRequest")]
        [JsonIgnore]
        [MetaData(Description = "Link to the RentalRequest.")]
        public int? RentalRequestId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class History {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  HistoryText: ").Append(HistoryText).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("}\n");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((History)obj);
        }

        /// <summary>
        /// Returns true if History instances are equal
        /// </summary>
        /// <param name="other">Instance of History to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(History other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    HistoryText == other.HistoryText ||
                    HistoryText != null &&
                    HistoryText.Equals(other.HistoryText)
                ) &&                 
                (
                    CreatedDate == other.CreatedDate ||
                    CreatedDate != null &&
                    CreatedDate.Equals(other.CreatedDate)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;

                // Suitable nullity checks                                   
                hash = hash * 59 + Id.GetHashCode();

                if (HistoryText != null)
                {
                    hash = hash * 59 + HistoryText.GetHashCode();
                }

                if (CreatedDate != null)
                {
                    hash = hash * 59 + CreatedDate.GetHashCode();
                }                

                return hash;
            }
        }

        #region Operators
        
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(History left, History right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(History left, History right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
