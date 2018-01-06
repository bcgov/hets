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
    [MetaDataExtension (Description = "Uploaded documents related to entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]
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
        /// <param name="Id">A system-generated unique identifier for an Attachment (required).</param>
        /// <param name="FileName">Filename as passed by the user uploading the file (required).</param>
        /// <param name="FileSize">FileSize.</param>
        /// <param name="Description">A note about the attachment,  optionally maintained by the user..</param>
        /// <param name="Type">Type of attachment.</param>
        /// <param name="LastUpdateUserid">Audit information - SM User Id for the User who most recently updated the record..</param>
        /// <param name="LastUpdateTimestamp">Audit information - Timestamp for record modification.</param>
        public AttachmentViewModel(int Id, string FileName, int? FileSize = null, string Description = null, string Type = null, string LastUpdateUserid = null, DateTime? LastUpdateTimestamp = null)
        {   
            this.Id = Id;
            this.FileName = FileName;
            this.FileSize = FileSize;
            this.Description = Description;
            this.Type = Type;
            this.LastUpdateUserid = LastUpdateUserid;
            this.LastUpdateTimestamp = LastUpdateTimestamp;
        }

        /// <summary>
        /// A system-generated unique identifier for an Attachment
        /// </summary>
        /// <value>A system-generated unique identifier for an Attachment</value>
        [DataMember(Name="id")]
        [MetaDataExtension (Description = "A system-generated unique identifier for an Attachment")]
        public int Id { get; set; }

        /// <summary>
        /// Filename as passed by the user uploading the file
        /// </summary>
        /// <value>Filename as passed by the user uploading the file</value>
        [DataMember(Name="fileName")]
        [MetaDataExtension (Description = "Filename as passed by the user uploading the file")]
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
        [MetaDataExtension (Description = "A note about the attachment,  optionally maintained by the user.")]
        public string Description { get; set; }

        /// <summary>
        /// Type of attachment
        /// </summary>
        /// <value>Type of attachment</value>
        [DataMember(Name="type")]
        [MetaDataExtension (Description = "Type of attachment")]
        public string Type { get; set; }

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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((AttachmentViewModel)obj);
        }

        /// <summary>
        /// Returns true if AttachmentViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of AttachmentViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AttachmentViewModel other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.FileName == other.FileName ||
                    this.FileName != null &&
                    this.FileName.Equals(other.FileName)
                ) &&                 
                (
                    this.FileSize == other.FileSize ||
                    this.FileSize != null &&
                    this.FileSize.Equals(other.FileSize)
                ) &&                 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) &&                 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) &&                 
                (
                    this.LastUpdateUserid == other.LastUpdateUserid ||
                    this.LastUpdateUserid != null &&
                    this.LastUpdateUserid.Equals(other.LastUpdateUserid)
                ) &&                 
                (
                    this.LastUpdateTimestamp == other.LastUpdateTimestamp ||
                    this.LastUpdateTimestamp != null &&
                    this.LastUpdateTimestamp.Equals(other.LastUpdateTimestamp)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.FileName != null)
                {
                    hash = hash * 59 + this.FileName.GetHashCode();
                }                
                                if (this.FileSize != null)
                {
                    hash = hash * 59 + this.FileSize.GetHashCode();
                }                
                                if (this.Description != null)
                {
                    hash = hash * 59 + this.Description.GetHashCode();
                }                
                                if (this.Type != null)
                {
                    hash = hash * 59 + this.Type.GetHashCode();
                }                
                                if (this.LastUpdateUserid != null)
                {
                    hash = hash * 59 + this.LastUpdateUserid.GetHashCode();
                }                
                                if (this.LastUpdateTimestamp != null)
                {
                    hash = hash * 59 + this.LastUpdateTimestamp.GetHashCode();
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
