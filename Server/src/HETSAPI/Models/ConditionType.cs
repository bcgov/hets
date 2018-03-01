using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace HETSAPI.Models
{
    /// <summary>
    /// Provincial Rate Type Database Model
    /// </summary>
    [MetaData (Description = "The standard conditions used in creating a new rental agreement.")]
    public sealed class ConditionType : AuditableEntity, IEquatable<ConditionType>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public ConditionType()
        {
            ConditionTypeCode = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionType" /> class.
        /// </summary>
        /// <param name="id">A unique id for the ConditionType record (required).</param>
        /// <param name="conditionTypeCode">A code value for a ConditionType (required).</param>
        /// <param name="description">A description of the ConditionType as used in the rental agreement (required).</param>
        /// <param name="active">A flag indicating if this ConditionType should be used on new rental agreements (required).</param>
        /// <param name="district">A District that this condition is associated with</param>
        public ConditionType(int id, string conditionTypeCode, string description, bool active, District district)
        {
            Id = id;
            ConditionTypeCode = conditionTypeCode;
            Description = description;
            Active = active;
            District = district;
        }

        /// <summary>
        /// A unique id for the ConditionType record (required).
        /// </summary>   
        /// <value>A unique id for the ConditionType record (required).</value>
        [MetaData(Description = "A unique id for the ConditionType record (required).")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets District
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// Foreign key for District
        /// </summary>   
        [ForeignKey("District")]
        [JsonIgnore]
        public int? DistrictId { get; set; }

        /// <summary>
        /// A unique code value for a ConditionType.
        /// </summary>
        /// <value>A unique code value for a ConditionType.</value>
        [MetaData (Description = "A code value for a ConditionType (required).")]
        [MaxLength(20)]        
        public string ConditionTypeCode { get; set; }

        /// <summary>
        /// A description of the ConditionType as used in the rental agreement.
        /// </summary>
        /// <value>A description of the ConditionType as used in the rental agreement.</value>
        [MetaData(Description = "A description of the ConditionType as used in the rental agreement.")]
        [MaxLength(2048)]
        public string Description { get; set; }

        /// <summary>
        /// A flag indicating if this ConditionType should be used on new rental agreements.
        /// </summary>
        /// <value>A flag indicating if this ConditionType should be used on new rental agreements.</value>
        [MetaData (Description = "A flag indicating if this ConditionType should be used on new rental agreements.")]
        public bool Active { get; set; }        

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class ConditionType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  ConditionTypeCode: ").Append(ConditionTypeCode).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ConditionType)obj);
        }

        /// <summary>
        /// Returns true if ConditionType instances are equal
        /// </summary>
        /// <param name="other">Instance of ConditionType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ConditionType other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&
                (
                    District == other.District ||
                    District.Equals(other.District)
                ) &&
                (
                    ConditionTypeCode == other.ConditionTypeCode ||
                    ConditionTypeCode.Equals(other.ConditionTypeCode)
                ) &&                 
                (
                    Description == other.Description ||
                    Description.Equals(other.Description)
                ) &&
                (
                    Active == other.Active ||
                    Active.Equals(other.Active)
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

                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
                }

                hash = hash * 59 + ConditionTypeCode.GetHashCode();                   
                hash = hash * 59 + Description.GetHashCode();
                hash = hash * 59 + Active.GetHashCode();                

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
        public static bool operator ==(ConditionType left, ConditionType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ConditionType left, ConditionType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
