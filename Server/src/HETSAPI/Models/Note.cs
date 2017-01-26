/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace HETSAPI.Models
{
    /// <summary>
    /// Notes entered by users.
    /// </summary>
        [MetaDataExtension (Description = "Notes entered by users.")]

    public partial class Note : IEquatable<Note>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Note()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Note" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="_Note">The contents of the note..</param>
        /// <param name="IsNoLongerRelevant">A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons, but identified to the user as no longer relevant..</param>
        public Note(int Id, string _Note = null, bool? IsNoLongerRelevant = null)
        {   
            this.Id = Id;
            this._Note = _Note;
            this.IsNoLongerRelevant = IsNoLongerRelevant;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// The contents of the note.
        /// </summary>
        /// <value>The contents of the note.</value>
        [MetaDataExtension (Description = "The contents of the note.")]
        public string _Note { get; set; }
        
        /// <summary>
        /// A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons, but identified to the user as no longer relevant.
        /// </summary>
        /// <value>A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons, but identified to the user as no longer relevant.</value>
        [MetaDataExtension (Description = "A user set flag that the note is no longer relevant. Allows the note to be retained for historical reasons, but identified to the user as no longer relevant.")]
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
            sb.Append("  _Note: ").Append(_Note).Append("\n");
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Note)obj);
        }

        /// <summary>
        /// Returns true if Note instances are equal
        /// </summary>
        /// <param name="other">Instance of Note to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Note other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this._Note == other._Note ||
                    this._Note != null &&
                    this._Note.Equals(other._Note)
                ) &&                 
                (
                    this.IsNoLongerRelevant == other.IsNoLongerRelevant ||
                    this.IsNoLongerRelevant != null &&
                    this.IsNoLongerRelevant.Equals(other.IsNoLongerRelevant)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this._Note != null)
                {
                    hash = hash * 59 + this._Note.GetHashCode();
                }                
                                if (this.IsNoLongerRelevant != null)
                {
                    hash = hash * 59 + this.IsNoLongerRelevant.GetHashCode();
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
