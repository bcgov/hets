using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// History View Model
    /// </summary>
    [DataContract]
    public sealed class HistoryViewModel : IEquatable<HistoryViewModel>
    {
        /// <summary>
        /// History View Model Constructor
        /// </summary>
        public HistoryViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a History (required).</param>
        /// <param name="historyText">The text of the history entry tracked against the related entity..</param>
        /// <param name="lastUpdateUserid">Audit information - SM User Id for the User who most recently updated the record..</param>
        /// <param name="lastUpdateTimestamp">Audit information - Timestamp for record modification.</param>
        /// <param name="affectedEntityId">The primary key of the affected record.</param>
        public HistoryViewModel(int id, string historyText = null, string lastUpdateUserid = null, DateTime? lastUpdateTimestamp = null, 
            int? affectedEntityId = null)
        {   
            Id = id;
            HistoryText = historyText;
            LastUpdateUserid = lastUpdateUserid;
            LastUpdateTimestamp = lastUpdateTimestamp;
            AffectedEntityId = affectedEntityId;
        }

        /// <summary>
        /// A system-generated unique identifier for a History
        /// </summary>
        /// <value>A system-generated unique identifier for a History</value>
        [DataMember(Name="id")]
        [MetaDataExtension (Description = "A system-generated unique identifier for a History")]
        public int Id { get; set; }

        /// <summary>
        /// The text of the history entry tracked against the related entity.
        /// </summary>
        /// <value>The text of the history entry tracked against the related entity.</value>
        [DataMember(Name="historyText")]
        [MetaDataExtension (Description = "The text of the history entry tracked against the related entity.")]
        public string HistoryText { get; set; }

        /// <summary>
        /// Audit information - SM User Id for the User who most recently updated the record.
        /// </summary>
        /// <value>Audit information - SM User Id for the User who most recently updated the record.</value>
        [DataMember(Name="lastUpdateUserid")]
        [MetaDataExtension (Description = "Audit information - SM User Id for the User who most recently updated the record.")]
        public string LastUpdateUserid { get; set; }

        /// <summary>
        /// Audit information - Timestamp for record modification
        /// </summary>
        /// <value>Audit information - Timestamp for record modification</value>
        [DataMember(Name="lastUpdateTimestamp")]
        [MetaDataExtension (Description = "Audit information - Timestamp for record modification")]
        public DateTime? LastUpdateTimestamp { get; set; }

        /// <summary>
        /// The primary key of the affected record
        /// </summary>
        /// <value>The primary key of the affected record</value>
        [DataMember(Name="affectedEntityId")]
        [MetaDataExtension (Description = "The primary key of the affected record")]
        public int? AffectedEntityId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class HistoryViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  HistoryText: ").Append(HistoryText).Append("\n");
            sb.Append("  LastUpdateUserid: ").Append(LastUpdateUserid).Append("\n");
            sb.Append("  LastUpdateTimestamp: ").Append(LastUpdateTimestamp).Append("\n");
            sb.Append("  AffectedEntityId: ").Append(AffectedEntityId).Append("\n");
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }

            return Equals((HistoryViewModel)obj);
        }

        /// <summary>
        /// Returns true if HistoryViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of HistoryViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(HistoryViewModel other)
        {
            if (ReferenceEquals(null, other)) { return false; }
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
                    LastUpdateUserid == other.LastUpdateUserid ||
                    LastUpdateUserid != null &&
                    LastUpdateUserid.Equals(other.LastUpdateUserid)
                ) &&                 
                (
                    LastUpdateTimestamp == other.LastUpdateTimestamp ||
                    LastUpdateTimestamp != null &&
                    LastUpdateTimestamp.Equals(other.LastUpdateTimestamp)
                ) &&                 
                (
                    AffectedEntityId == other.AffectedEntityId ||
                    AffectedEntityId != null &&
                    AffectedEntityId.Equals(other.AffectedEntityId)
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

                if (LastUpdateUserid != null)
                {
                    hash = hash * 59 + LastUpdateUserid.GetHashCode();
                }

                if (LastUpdateTimestamp != null)
                {
                    hash = hash * 59 + LastUpdateTimestamp.GetHashCode();
                }

                if (AffectedEntityId != null)
                {
                    hash = hash * 59 + AffectedEntityId.GetHashCode();
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
        public static bool operator ==(HistoryViewModel left, HistoryViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HistoryViewModel left, HistoryViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
