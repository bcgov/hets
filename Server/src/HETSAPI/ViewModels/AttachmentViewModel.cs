using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Uploaded documents related to entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.
    /// </summary>
    [MetaData (Description = "Uploaded documents related to entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]
    [DataContract]
    public sealed class AttachmentViewModel : IEquatable<AttachmentViewModel>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public AttachmentViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for an Attachment (required).</param>
        /// <param name="fileName">Filename as passed by the user uploading the file (required).</param>
        /// <param name="fileSize">FileSize.</param>
        /// <param name="description">A note about the attachment,  optionally maintained by the user..</param>
        /// <param name="type">Type of attachment.</param>
        /// <param name="lastUpdateUserid">Audit information - SM User Id for the User who most recently updated the record..</param>
        /// <param name="lastUpdateTimestamp">Audit information - Timestamp for record modification.</param>
        public AttachmentViewModel(int id, string fileName, int? fileSize = null, string description = null, 
            string type = null, string lastUpdateUserid = null, DateTime? lastUpdateTimestamp = null)
        {   
            Id = id;
            FileName = fileName;
            FileSize = fileSize;
            Description = description;
            Type = type;
            LastUpdateUserid = lastUpdateUserid;
            LastUpdateTimestamp = lastUpdateTimestamp;
        }

        /// <summary>
        /// A system-generated unique identifier for an Attachment
        /// </summary>
        /// <value>A system-generated unique identifier for an Attachment</value>
        [DataMember(Name="id")]
        [MetaData (Description = "A system-generated unique identifier for an Attachment")]
        public int Id { get; set; }

        /// <summary>
        /// Filename as passed by the user uploading the file
        /// </summary>
        /// <value>Filename as passed by the user uploading the file</value>
        [DataMember(Name="fileName")]
        [MetaData (Description = "Filename as passed by the user uploading the file")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or Sets FileSize
        /// </summary>
        [DataMember(Name="fileSize")]
        public int? FileSize { get; set; }

        /// <summary>
        /// A note about the attachment,  optionally maintained by the user.
        /// </summary>
        /// <value>A note about the attachment,  optionally maintained by the user.</value>
        [DataMember(Name="description")]
        [MetaData (Description = "A note about the attachment,  optionally maintained by the user.")]
        public string Description { get; set; }

        /// <summary>
        /// Type of attachment
        /// </summary>
        /// <value>Type of attachment</value>
        [DataMember(Name="type")]
        [MetaData (Description = "Type of attachment")]
        public string Type { get; set; }

        /// <summary>
        /// Audit information - SM User Id for the User who most recently updated the record.
        /// </summary>
        /// <value>Audit information - SM User Id for the User who most recently updated the record.</value>
        [DataMember(Name="lastUpdateUserid")]
        [MetaData (Description = "Audit information - SM User Id for the User who most recently updated the record.")]
        public string LastUpdateUserid { get; set; }

        /// <summary>
        /// Audit information - Timestamp for record modification
        /// </summary>
        /// <value>Audit information - Timestamp for record modification</value>
        [DataMember(Name="lastUpdateTimestamp")]
        [MetaData (Description = "Audit information - Timestamp for record modification")]
        public DateTime? LastUpdateTimestamp { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AttachmentViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  FileName: ").Append(FileName).Append("\n");
            sb.Append("  FileSize: ").Append(FileSize).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  LastUpdateUserid: ").Append(LastUpdateUserid).Append("\n");
            sb.Append("  LastUpdateTimestamp: ").Append(LastUpdateTimestamp).Append("\n");
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
            return obj.GetType() == GetType() && Equals((AttachmentViewModel)obj);
        }

        /// <summary>
        /// Returns true if AttachmentViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of AttachmentViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AttachmentViewModel other)
        {

            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    FileName == other.FileName ||
                    FileName != null &&
                    FileName.Equals(other.FileName)
                ) &&                 
                (
                    FileSize == other.FileSize ||
                    FileSize != null &&
                    FileSize.Equals(other.FileSize)
                ) &&                 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) &&                 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
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
                                   
                hash = hash * 59 + Id.GetHashCode();                if (FileName != null)
                {
                    hash = hash * 59 + FileName.GetHashCode();
                }                
                                if (FileSize != null)
                {
                    hash = hash * 59 + FileSize.GetHashCode();
                }                
                                if (Description != null)
                {
                    hash = hash * 59 + Description.GetHashCode();
                }                
                                if (Type != null)
                {
                    hash = hash * 59 + Type.GetHashCode();
                }                
                                if (LastUpdateUserid != null)
                {
                    hash = hash * 59 + LastUpdateUserid.GetHashCode();
                }                
                                if (LastUpdateTimestamp != null)
                {
                    hash = hash * 59 + LastUpdateTimestamp.GetHashCode();
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
        public static bool operator ==(AttachmentViewModel left, AttachmentViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(AttachmentViewModel left, AttachmentViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
