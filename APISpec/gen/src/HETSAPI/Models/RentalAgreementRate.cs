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
    /// The rate associated with an element of a rental agreement.
    /// </summary>
        [MetaDataExtension (Description = "The rate associated with an element of a rental agreement.")]

    public partial class RentalAgreementRate : AuditableEntity, IEquatable<RentalAgreementRate>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalAgreementRate()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAgreementRate" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a RentalAgreementRate (required).</param>
        /// <param name="RentalAgreement">A foreign key reference to the system-generated unique identifier for a Rental Agreement (required).</param>
        /// <param name="ComponentName">Name of the component for the Rental Agreement for which the attached rates apply..</param>
        /// <param name="IsAttachment">True if this rate is for an attachment to the piece of equipment..</param>
        /// <param name="Rate">The dollar rate associated with this component of the rental agreement..</param>
        /// <param name="PercentOfEquipmentRate">For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement..</param>
        /// <param name="RatePeriod">The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily..</param>
        /// <param name="Comment">A comment about the rental of this component of the Rental Agreement..</param>
        /// <param name="TimeRecords">TimeRecords.</param>
        public RentalAgreementRate(int Id, RentalAgreement RentalAgreement, string ComponentName = null, bool? IsAttachment = null, float? Rate = null, int? PercentOfEquipmentRate = null, string RatePeriod = null, string Comment = null, List<TimeRecord> TimeRecords = null)
        {   
            this.Id = Id;
            this.RentalAgreement = RentalAgreement;

            this.ComponentName = ComponentName;
            this.IsAttachment = IsAttachment;
            this.Rate = Rate;
            this.PercentOfEquipmentRate = PercentOfEquipmentRate;
            this.RatePeriod = RatePeriod;
            this.Comment = Comment;
            this.TimeRecords = TimeRecords;
        }

        /// <summary>
        /// A system-generated unique identifier for a RentalAgreementRate
        /// </summary>
        /// <value>A system-generated unique identifier for a RentalAgreementRate</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RentalAgreementRate")]
        public int Id { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Rental Agreement
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Rental Agreement</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Agreement")]
        public RentalAgreement RentalAgreement { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreement 
        /// </summary>   
        [ForeignKey("RentalAgreement")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Rental Agreement")]
        public int? RentalAgreementId { get; set; }
        
        /// <summary>
        /// Name of the component for the Rental Agreement for which the attached rates apply.
        /// </summary>
        /// <value>Name of the component for the Rental Agreement for which the attached rates apply.</value>
        [MetaDataExtension (Description = "Name of the component for the Rental Agreement for which the attached rates apply.")]
        [MaxLength(150)]
        
        public string ComponentName { get; set; }
        
        /// <summary>
        /// True if this rate is for an attachment to the piece of equipment.
        /// </summary>
        /// <value>True if this rate is for an attachment to the piece of equipment.</value>
        [MetaDataExtension (Description = "True if this rate is for an attachment to the piece of equipment.")]
        public bool? IsAttachment { get; set; }
        
        /// <summary>
        /// The dollar rate associated with this component of the rental agreement.
        /// </summary>
        /// <value>The dollar rate associated with this component of the rental agreement.</value>
        [MetaDataExtension (Description = "The dollar rate associated with this component of the rental agreement.")]
        public float? Rate { get; set; }
        
        /// <summary>
        /// For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement.
        /// </summary>
        /// <value>For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement.</value>
        [MetaDataExtension (Description = "For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement.")]
        public int? PercentOfEquipmentRate { get; set; }
        
        /// <summary>
        /// The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.
        /// </summary>
        /// <value>The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.</value>
        [MetaDataExtension (Description = "The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily.")]
        [MaxLength(50)]
        
        public string RatePeriod { get; set; }
        
        /// <summary>
        /// A comment about the rental of this component of the Rental Agreement.
        /// </summary>
        /// <value>A comment about the rental of this component of the Rental Agreement.</value>
        [MetaDataExtension (Description = "A comment about the rental of this component of the Rental Agreement.")]
        [MaxLength(2048)]
        
        public string Comment { get; set; }
        
        /// <summary>
        /// Gets or Sets TimeRecords
        /// </summary>
        public List<TimeRecord> TimeRecords { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RentalAgreementRate {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  ComponentName: ").Append(ComponentName).Append("\n");
            sb.Append("  IsAttachment: ").Append(IsAttachment).Append("\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
            sb.Append("  PercentOfEquipmentRate: ").Append(PercentOfEquipmentRate).Append("\n");
            sb.Append("  RatePeriod: ").Append(RatePeriod).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  TimeRecords: ").Append(TimeRecords).Append("\n");
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
            return Equals((RentalAgreementRate)obj);
        }

        /// <summary>
        /// Returns true if RentalAgreementRate instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalAgreementRate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalAgreementRate other)
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
                    this.ComponentName == other.ComponentName ||
                    this.ComponentName != null &&
                    this.ComponentName.Equals(other.ComponentName)
                ) &&                 
                (
                    this.IsAttachment == other.IsAttachment ||
                    this.IsAttachment != null &&
                    this.IsAttachment.Equals(other.IsAttachment)
                ) &&                 
                (
                    this.Rate == other.Rate ||
                    this.Rate != null &&
                    this.Rate.Equals(other.Rate)
                ) &&                 
                (
                    this.PercentOfEquipmentRate == other.PercentOfEquipmentRate ||
                    this.PercentOfEquipmentRate != null &&
                    this.PercentOfEquipmentRate.Equals(other.PercentOfEquipmentRate)
                ) &&                 
                (
                    this.RatePeriod == other.RatePeriod ||
                    this.RatePeriod != null &&
                    this.RatePeriod.Equals(other.RatePeriod)
                ) &&                 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.TimeRecords == other.TimeRecords ||
                    this.TimeRecords != null &&
                    this.TimeRecords.SequenceEqual(other.TimeRecords)
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
                }                if (this.ComponentName != null)
                {
                    hash = hash * 59 + this.ComponentName.GetHashCode();
                }                
                                if (this.IsAttachment != null)
                {
                    hash = hash * 59 + this.IsAttachment.GetHashCode();
                }                
                                if (this.Rate != null)
                {
                    hash = hash * 59 + this.Rate.GetHashCode();
                }                
                                if (this.PercentOfEquipmentRate != null)
                {
                    hash = hash * 59 + this.PercentOfEquipmentRate.GetHashCode();
                }                
                                if (this.RatePeriod != null)
                {
                    hash = hash * 59 + this.RatePeriod.GetHashCode();
                }                
                                if (this.Comment != null)
                {
                    hash = hash * 59 + this.Comment.GetHashCode();
                }                
                                   
                if (this.TimeRecords != null)
                {
                    hash = hash * 59 + this.TimeRecords.GetHashCode();
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
        public static bool operator ==(RentalAgreementRate left, RentalAgreementRate right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalAgreementRate left, RentalAgreementRate right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
