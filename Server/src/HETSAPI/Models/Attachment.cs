using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Attachment Database Model
    /// </summary>
    [MetaData (Description = "Uploaded documents related to entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]
    public sealed class Attachment : AuditableEntity, IEquatable<Attachment>
    {
        /// <summary>
        /// Attachment Database Model Constructor (required by entity framework)
        /// </summary>
        public Attachment()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for an Attachment (required).</param>
        /// <param name="fileName">Filename as passed by the user uploading the file (required).</param>
        /// <param name="fileContents">Binary contents of the file (required).</param>
        /// <param name="description">A note about the attachment,  optionally maintained by the user..</param>
        /// <param name="type">Type of attachment.</param>
        public Attachment(int id, string fileName, byte[] fileContents, string description = null, string type = null)
        {   
            Id = id;
            FileName = fileName;
            FileContents = fileContents;
            Description = description;
            Type = type;
        }

        /// <summary>
        /// A system-generated unique identifier for an Attachment
        /// </summary>
        /// <value>A system-generated unique identifier for an Attachment</value>
        [MetaData (Description = "A system-generated unique identifier for an Attachment")]
        public int Id { get; set; }
        
        /// <summary>
        /// Filename as passed by the user uploading the file
        /// </summary>
        /// <value>Filename as passed by the user uploading the file</value>
        [MetaData (Description = "Filename as passed by the user uploading the file")]
        [MaxLength(2048)]        
        public string FileName { get; set; }
        
        /// <summary>
        /// Binary contents of the file
        /// </summary>
        /// <value>Binary contents of the file</value>
        [MetaData (Description = "Binary contents of the file")]
        public byte[] FileContents { get; set; }
        
        /// <summary>
        /// A note about the attachment,  optionally maintained by the user.
        /// </summary>
        /// <value>A note about the attachment,  optionally maintained by the user.</value>
        [MetaData (Description = "A note about the attachment,  optionally maintained by the user.")]
        [MaxLength(2048)]        
        public string Description { get; set; }
        
        /// <summary>
        /// Type of attachment
        /// </summary>
        /// <value>Type of attachment</value>
        [MetaData (Description = "Type of attachment")]
        [MaxLength(255)]        
        public string Type { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class Attachment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  FileName: ").Append(FileName).Append("\n");
            sb.Append("  FileContents: ").Append(FileContents).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Attachment)obj);
        }

        /// <summary>
        /// Returns true if Attachment instances are equal
        /// </summary>
        /// <param name="other">Instance of Attachment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Attachment other)
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
                    FileContents == other.FileContents ||
                    FileContents != null &&
                    FileContents.Equals(other.FileContents)
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

                if (FileName != null)
                {
                    hash = hash * 59 + FileName.GetHashCode();
                }

                if (FileContents != null)
                {
                    hash = hash * 59 + FileContents.GetHashCode();
                }

                if (Description != null)
                {
                    hash = hash * 59 + Description.GetHashCode();
                }

                if (Type != null)
                {
                    hash = hash * 59 + Type.GetHashCode();
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
        public static bool operator ==(Attachment left, Attachment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Attachment left, Attachment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
