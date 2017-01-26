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
    /// 
    /// </summary>

    public partial class TimeRecord : IEquatable<TimeRecord>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public TimeRecord()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeRecord" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="RentalAgreement">RentalAgreement.</param>
        /// <param name="WorkedDate">WorkedDate.</param>
        /// <param name="EnteredDate">EnteredDate.</param>
        /// <param name="Hours">Hours.</param>
        /// <param name="Rate">Rate.</param>
        /// <param name="Hours2">Hours2.</param>
        /// <param name="Rate2">Rate2.</param>
        /// <param name="Hours3">Hours3.</param>
        /// <param name="Rate3">Rate3.</param>
        public TimeRecord(int Id, RentalAgreement RentalAgreement = null, DateTime? WorkedDate = null, DateTime? EnteredDate = null, float? Hours = null, float? Rate = null, float? Hours2 = null, float? Rate2 = null, float? Hours3 = null, float? Rate3 = null)
        {   
            this.Id = Id;
            this.RentalAgreement = RentalAgreement;
            this.WorkedDate = WorkedDate;
            this.EnteredDate = EnteredDate;
            this.Hours = Hours;
            this.Rate = Rate;
            this.Hours2 = Hours2;
            this.Rate2 = Rate2;
            this.Hours3 = Hours3;
            this.Rate3 = Rate3;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets RentalAgreement
        /// </summary>
        public RentalAgreement RentalAgreement { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreement 
        /// </summary>       
        [ForeignKey("RentalAgreement")]
        public int? RentalAgreementRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets WorkedDate
        /// </summary>
        public DateTime? WorkedDate { get; set; }
        
        /// <summary>
        /// Gets or Sets EnteredDate
        /// </summary>
        public DateTime? EnteredDate { get; set; }
        
        /// <summary>
        /// Gets or Sets Hours
        /// </summary>
        public float? Hours { get; set; }
        
        /// <summary>
        /// Gets or Sets Rate
        /// </summary>
        public float? Rate { get; set; }
        
        /// <summary>
        /// Gets or Sets Hours2
        /// </summary>
        public float? Hours2 { get; set; }
        
        /// <summary>
        /// Gets or Sets Rate2
        /// </summary>
        public float? Rate2 { get; set; }
        
        /// <summary>
        /// Gets or Sets Hours3
        /// </summary>
        public float? Hours3 { get; set; }
        
        /// <summary>
        /// Gets or Sets Rate3
        /// </summary>
        public float? Rate3 { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TimeRecord {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  WorkedDate: ").Append(WorkedDate).Append("\n");
            sb.Append("  EnteredDate: ").Append(EnteredDate).Append("\n");
            sb.Append("  Hours: ").Append(Hours).Append("\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
            sb.Append("  Hours2: ").Append(Hours2).Append("\n");
            sb.Append("  Rate2: ").Append(Rate2).Append("\n");
            sb.Append("  Hours3: ").Append(Hours3).Append("\n");
            sb.Append("  Rate3: ").Append(Rate3).Append("\n");
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
            return Equals((TimeRecord)obj);
        }

        /// <summary>
        /// Returns true if TimeRecord instances are equal
        /// </summary>
        /// <param name="other">Instance of TimeRecord to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TimeRecord other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.RentalAgreement == other.RentalAgreement ||
                    this.RentalAgreement != null &&
                    this.RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    this.WorkedDate == other.WorkedDate ||
                    this.WorkedDate != null &&
                    this.WorkedDate.Equals(other.WorkedDate)
                ) &&                 
                (
                    this.EnteredDate == other.EnteredDate ||
                    this.EnteredDate != null &&
                    this.EnteredDate.Equals(other.EnteredDate)
                ) &&                 
                (
                    this.Hours == other.Hours ||
                    this.Hours != null &&
                    this.Hours.Equals(other.Hours)
                ) &&                 
                (
                    this.Rate == other.Rate ||
                    this.Rate != null &&
                    this.Rate.Equals(other.Rate)
                ) &&                 
                (
                    this.Hours2 == other.Hours2 ||
                    this.Hours2 != null &&
                    this.Hours2.Equals(other.Hours2)
                ) &&                 
                (
                    this.Rate2 == other.Rate2 ||
                    this.Rate2 != null &&
                    this.Rate2.Equals(other.Rate2)
                ) &&                 
                (
                    this.Hours3 == other.Hours3 ||
                    this.Hours3 != null &&
                    this.Hours3.Equals(other.Hours3)
                ) &&                 
                (
                    this.Rate3 == other.Rate3 ||
                    this.Rate3 != null &&
                    this.Rate3.Equals(other.Rate3)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                   
                if (this.RentalAgreement != null)
                {
                    hash = hash * 59 + this.RentalAgreement.GetHashCode();
                }                if (this.WorkedDate != null)
                {
                    hash = hash * 59 + this.WorkedDate.GetHashCode();
                }                
                                if (this.EnteredDate != null)
                {
                    hash = hash * 59 + this.EnteredDate.GetHashCode();
                }                
                                if (this.Hours != null)
                {
                    hash = hash * 59 + this.Hours.GetHashCode();
                }                
                                if (this.Rate != null)
                {
                    hash = hash * 59 + this.Rate.GetHashCode();
                }                
                                if (this.Hours2 != null)
                {
                    hash = hash * 59 + this.Hours2.GetHashCode();
                }                
                                if (this.Rate2 != null)
                {
                    hash = hash * 59 + this.Rate2.GetHashCode();
                }                
                                if (this.Hours3 != null)
                {
                    hash = hash * 59 + this.Hours3.GetHashCode();
                }                
                                if (this.Rate3 != null)
                {
                    hash = hash * 59 + this.Rate3.GetHashCode();
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
        public static bool operator ==(TimeRecord left, TimeRecord right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TimeRecord left, TimeRecord right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
