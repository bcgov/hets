using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Rental Agreement Condition Database Model
    /// </summary>
    [MetaDataExtension (Description = "A condition about the rental agreement to be displayed on the Rental Agreement.")]
    public sealed class RentalAgreementCondition : AuditableEntity, IEquatable<RentalAgreementCondition>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalAgreementCondition()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalAgreementCondition" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a RentalAgreementCondition (required).</param>
        /// <param name="rentalAgreement">A foreign key reference to the system-generated unique identifier for a Rental Agreement (required).</param>
        /// <param name="conditionName">The name of the condition to be placed onto the Rental Agreement. (required).</param>
        /// <param name="comment">A comment about the condition to be applied to the Rental Agreement..</param>
        public RentalAgreementCondition(int id, RentalAgreement rentalAgreement, string conditionName, string comment = null)
        {   
            Id = id;
            RentalAgreement = rentalAgreement;
            ConditionName = conditionName;
            Comment = comment;
        }

        /// <summary>
        /// A system-generated unique identifier for a RentalAgreementCondition
        /// </summary>
        /// <value>A system-generated unique identifier for a RentalAgreementCondition</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a RentalAgreementCondition")]
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
        /// The name of the condition to be placed onto the Rental Agreement.
        /// </summary>
        /// <value>The name of the condition to be placed onto the Rental Agreement.</value>
        [MetaDataExtension (Description = "The name of the condition to be placed onto the Rental Agreement.")]
        [MaxLength(150)]        
        public string ConditionName { get; set; }
        
        /// <summary>
        /// A comment about the condition to be applied to the Rental Agreement.
        /// </summary>
        /// <value>A comment about the condition to be applied to the Rental Agreement.</value>
        [MetaDataExtension (Description = "A comment about the condition to be applied to the Rental Agreement.")]
        [MaxLength(2048)]        
        public string Comment { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RentalAgreementCondition {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  ConditionName: ").Append(ConditionName).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
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

            return Equals((RentalAgreementCondition)obj);
        }

        /// <summary>
        /// Returns true if RentalAgreementCondition instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalAgreementCondition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalAgreementCondition other)
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
                    ConditionName == other.ConditionName ||
                    ConditionName != null &&
                    ConditionName.Equals(other.ConditionName)
                ) &&                 
                (
                    Comment == other.Comment ||
                    Comment != null &&
                    Comment.Equals(other.Comment)
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

                if (ConditionName != null)
                {
                    hash = hash * 59 + ConditionName.GetHashCode();
                }

                if (Comment != null)
                {
                    hash = hash * 59 + Comment.GetHashCode();
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
        public static bool operator ==(RentalAgreementCondition left, RentalAgreementCondition right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalAgreementCondition left, RentalAgreementCondition right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
