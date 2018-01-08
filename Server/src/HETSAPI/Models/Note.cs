using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Note Database Model
    /// </summary>
    [MetaData (Description = "Text entered about an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]
    public sealed class Note : AuditableEntity, IEquatable<Note>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Note()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Note (required).</param>
        /// <param name="text">Notes entered by users about instance of entities - e.g. School Buses and School Bus Owners (required).</param>
        /// <param name="isNoLongerRelevant">A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons,  but identified to other users as no longer applicable..</param>
        public Note(int id, string text, bool? isNoLongerRelevant = null)
        {   
            Id = id;
            Text = text;
            IsNoLongerRelevant = isNoLongerRelevant;
        }

        /// <summary>
        /// A system-generated unique identifier for a Note
        /// </summary>
        /// <value>A system-generated unique identifier for a Note</value>
        [MetaData (Description = "A system-generated unique identifier for a Note")]
        public int Id { get; set; }
        
        /// <summary>
        /// Notes entered by users about instance of entities - e.g. School Buses and School Bus Owners
        /// </summary>
        /// <value>Notes entered by users about instance of entities - e.g. School Buses and School Bus Owners</value>
        [MetaData (Description = "Notes entered by users about instance of entities - e.g. School Buses and School Bus Owners")]
        [MaxLength(2048)]        
        public string Text { get; set; }
        
        /// <summary>
        /// A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons,  but identified to other users as no longer applicable.
        /// </summary>
        /// <value>A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons,  but identified to other users as no longer applicable.</value>
        [MetaData (Description = "A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons,  but identified to other users as no longer applicable.")]
        public bool? IsNoLongerRelevant { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class Note {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("  IsNoLongerRelevant: ").Append(IsNoLongerRelevant).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Note)obj);
        }

        /// <summary>
        /// Returns true if Note instances are equal
        /// </summary>
        /// <param name="other">Instance of Note to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Note other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Text == other.Text ||
                    Text != null &&
                    Text.Equals(other.Text)
                ) &&                 
                (
                    IsNoLongerRelevant == other.IsNoLongerRelevant ||
                    IsNoLongerRelevant != null &&
                    IsNoLongerRelevant.Equals(other.IsNoLongerRelevant)
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

                if (Text != null)
                {
                    hash = hash * 59 + Text.GetHashCode();
                }

                if (IsNoLongerRelevant != null)
                {
                    hash = hash * 59 + IsNoLongerRelevant.GetHashCode();
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
        public static bool operator ==(Note left, Note right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Note left, Note right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
