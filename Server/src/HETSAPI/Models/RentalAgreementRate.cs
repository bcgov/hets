using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// REntal Agreement Rate Database Model
    /// </summary>
    [MetaDataExtension (Description = "The rate associated with an element of a rental agreement.")]
    public class RentalAgreementRate : AuditableEntity, IEquatable<RentalAgreementRate>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalAgreementRate()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAgreementRate" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a RentalAgreementRate (required).</param>
        /// <param name="rentalAgreement">A foreign key reference to the system-generated unique identifier for a Rental Agreement (required).</param>
        /// <param name="componentName">Name of the component for the Rental Agreement for which the attached rates apply..</param>
        /// <param name="isAttachment">True if this rate is for an attachment to the piece of equipment..</param>
        /// <param name="rate">The dollar rate associated with this component of the rental agreement..</param>
        /// <param name="percentOfEquipmentRate">For other than the actual piece of equipment, the percent of the equipment rate to use for this component of the rental agreement..</param>
        /// <param name="ratePeriod">The period of the rental rate. The vast majority will be hourly, but the rate could apply across a different period, e.g. daily..</param>
        /// <param name="comment">A comment about the rental of this component of the Rental Agreement..</param>
        /// <param name="timeRecords">TimeRecords.</param>
        public RentalAgreementRate(int id, RentalAgreement rentalAgreement, string componentName = null, bool? isAttachment = null, 
            float? rate = null, int? percentOfEquipmentRate = null, string ratePeriod = null, string comment = null, 
            List<TimeRecord> timeRecords = null)
        {   
            Id = id;
            RentalAgreement = rentalAgreement;
            ComponentName = componentName;
            IsAttachment = isAttachment;
            Rate = rate;
            PercentOfEquipmentRate = percentOfEquipmentRate;
            RatePeriod = ratePeriod;
            Comment = comment;
            TimeRecords = timeRecords;
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
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    RentalAgreement == other.RentalAgreement ||
                    RentalAgreement != null &&
                    RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    ComponentName == other.ComponentName ||
                    ComponentName != null &&
                    ComponentName.Equals(other.ComponentName)
                ) &&                 
                (
                    IsAttachment == other.IsAttachment ||
                    IsAttachment != null &&
                    IsAttachment.Equals(other.IsAttachment)
                ) &&                 
                (
                    Rate == other.Rate ||
                    Rate != null &&
                    Rate.Equals(other.Rate)
                ) &&                 
                (
                    PercentOfEquipmentRate == other.PercentOfEquipmentRate ||
                    PercentOfEquipmentRate != null &&
                    PercentOfEquipmentRate.Equals(other.PercentOfEquipmentRate)
                ) &&                 
                (
                    RatePeriod == other.RatePeriod ||
                    RatePeriod != null &&
                    RatePeriod.Equals(other.RatePeriod)
                ) &&                 
                (
                    Comment == other.Comment ||
                    Comment != null &&
                    Comment.Equals(other.Comment)
                ) && 
                (
                    TimeRecords == other.TimeRecords ||
                    TimeRecords != null &&
                    TimeRecords.SequenceEqual(other.TimeRecords)
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
                
                if (RentalAgreement != null)
                {
                    hash = hash * 59 + RentalAgreement.GetHashCode();
                }

                if (ComponentName != null)
                {
                    hash = hash * 59 + ComponentName.GetHashCode();
                }

                if (IsAttachment != null)
                {
                    hash = hash * 59 + IsAttachment.GetHashCode();
                }

                if (Rate != null)
                {
                    hash = hash * 59 + Rate.GetHashCode();
                }

                if (PercentOfEquipmentRate != null)
                {
                    hash = hash * 59 + PercentOfEquipmentRate.GetHashCode();
                }

                if (RatePeriod != null)
                {
                    hash = hash * 59 + RatePeriod.GetHashCode();
                }

                if (Comment != null)
                {
                    hash = hash * 59 + Comment.GetHashCode();
                }                
                                   
                if (TimeRecords != null)
                {
                    hash = hash * 59 + TimeRecords.GetHashCode();
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
