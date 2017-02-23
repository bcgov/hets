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
using System.ComponentModel.DataAnnotations;
using HETSAPI.Models;

namespace HETSAPI.Models
{
    /// <summary>
    /// A log entry created by the system based on a triggering event and related to an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.
    /// </summary>
        [MetaDataExtension (Description = "A log entry created by the system based on a triggering event and related to an entity in the application - e.g. piece of Equipment, an Owner, a Project and so on.")]

    public partial class History : AuditableEntity, IEquatable<History>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public History()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="History" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a History (required).</param>
        /// <param name="HistoryText">The text of the history entry tracked against the related entity. (required).</param>
        /// <param name="CreatedDate">Date the record is created..</param>
        public History(int Id, string HistoryText, DateTime? CreatedDate = null)
        {   
            this.Id = Id;
            this.HistoryText = HistoryText;

            this.CreatedDate = CreatedDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a History
        /// </summary>
        /// <value>A system-generated unique identifier for a History</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a History")]
        public int Id { get; set; }
        
        /// <summary>
        /// The text of the history entry tracked against the related entity.
        /// </summary>
        /// <value>The text of the history entry tracked against the related entity.</value>
        [MetaDataExtension (Description = "The text of the history entry tracked against the related entity.")]
        [MaxLength(2048)]
        
        public string HistoryText { get; set; }
        
        /// <summary>
        /// Date the record is created.
        /// </summary>
        /// <value>Date the record is created.</value>
        [MetaDataExtension (Description = "Date the record is created.")]
        public DateTime? CreatedDate { get; set; }
        
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((History)obj);
        }

        /// <summary>
        /// Returns true if History instances are equal
        /// </summary>
        /// <param name="other">Instance of History to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(History other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.HistoryText == other.HistoryText ||
                    this.HistoryText != null &&
                    this.HistoryText.Equals(other.HistoryText)
                ) &&                 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.HistoryText != null)
                {
                    hash = hash * 59 + this.HistoryText.GetHashCode();
                }                
                                if (this.CreatedDate != null)
                {
                    hash = hash * 59 + this.CreatedDate.GetHashCode();
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
