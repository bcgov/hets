using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace HETSAPI.Models
{
    /// <summary>
    /// Provincial Rate Type Database Model
    /// </summary>
    [MetaData (Description = "The standard rates used in creating a new rental agreement.")]
    public sealed class ProvincialRateType : AuditableEntity, IEquatable<ProvincialRateType>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public ProvincialRateType()
        {
            RateType = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvincialRateType" /> class.
        /// </summary>
        /// <param name="rateType">A unique code value for a ProvincialRateType (required).</param>
        /// <param name="description">A description of the ProvincialRateType as used in the rental agreement (required).</param>
        /// <param name="active">A flag indicating if this ProvincialRateType should be used on new rental agreements (required).</param>
        /// <param name="periodType">The period this ProvincialRateTYpe is billed at (either Hourly or Daily) (required).</param>
        /// <param name="rate">Rate in dollars for this ProvincialRateType.</param>
        /// <param name="isPercentRate">Indicates this ProvincialRateType is calculated as a percentage of the base rate (required).</param>
        /// <param name="isRateEditable">Indicates if a user can modify the rate for this ProvincialRateType (required).</param>
        /// <param name="isIncludedInTotal">Indicates if this rate is added to the total in the rental agreement (required).</param>
        public ProvincialRateType(string rateType, string description, bool active, string periodType,
            float? rate, bool isPercentRate, bool isRateEditable, bool isIncludedInTotal)
        {   
            RateType = rateType;
            Description = description;
            Active = active;
            PeriodType = periodType;
            Rate = rate;
            IsPercentRate = isPercentRate;
            IsRateEditable = isRateEditable;
            IsIncludedInTotal = isIncludedInTotal;
        }

        /// <summary>
        /// A unique code value for a ProvincialRateType.
        /// </summary>
        /// <value>A unique code value for a ProvincialRateType.</value>
        [MetaData (Description = "A unique code value for a ProvincialRateType.")]
        [MaxLength(20)]
        [Key]
        public string RateType { get; set; }

        /// <summary>
        /// A description of the ProvincialRateType as used in the rental agreement.
        /// </summary>
        /// <value>A description of the ProvincialRateType as used in the rental agreement.</value>
        [MetaData(Description = "A description of the ProvincialRateType as used in the rental agreement.")]
        [MaxLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// A flag indicating if this ProvincialRateType should be used on new rental agreements.
        /// </summary>
        /// <value>A flag indicating if this ProvincialRateType should be used on new rental agreements.</value>
        [MetaData (Description = "A flag indicating if this ProvincialRateType should be used on new rental agreements.")]
        public bool Active { get; set; }

        /// <summary>
        /// The period this ProvincialRateTYpe is billed at (either Hourly or Daily).
        /// </summary>
        /// <value>The period this ProvincialRateTYpe is billed at (either Hourly or Daily).</value>
        [MetaData(Description = "The period this ProvincialRateTYpe is billed at (either Hourly or Daily).")]
        [MaxLength(20)]
        public string PeriodType { get; set; }

        /// <summary>
        /// Rate in dollars for this ProvincialRateType.
        /// </summary>
        /// <value>Rate in dollars for this ProvincialRateType.</value>
        [MetaData(Description = "Rate in dollars for this ProvincialRateType.")]
        public float? Rate { get; set; }

        /// <summary>
        /// Indicates this ProvincialRateType is calculated as a percentage of the base rate.
        /// </summary>
        /// <value>Indicates this ProvincialRateType is calculated as a percentage of the base rate.</value>
        [MetaData(Description = "Indicates this ProvincialRateType is calculated as a percentage of the base rate.")]
        public bool IsPercentRate { get; set; }

        /// <summary>
        /// Indicates if a user can modify the rate for this ProvincialRateType.
        /// </summary>
        /// <value>Indicates if a user can modify the rate for this ProvincialRateType.</value>
        [MetaData(Description = "Indicates if a user can modify the rate for this ProvincialRateType.")]
        public bool IsRateEditable { get; set; }

        /// <summary>
        /// Indicates if this rate is added to the total in the rental agreement.
        /// </summary>
        /// <value>Indicates if this rate is added to the total in the rental agreement.</value>
        [MetaData(Description = "Indicates if this rate is added to the total in the rental agreement.")]
        public bool IsIncludedInTotal { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class ProvincialRateType {\n");
            sb.Append("  RateType: ").Append(RateType).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Active: ").Append(Active).Append("\n");
            sb.Append("  PeriodType: ").Append(PeriodType).Append("\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
            sb.Append("  IsPercentRate: ").Append(IsPercentRate).Append("\n");
            sb.Append("  IsRateEditable: ").Append(IsRateEditable).Append("\n");
            sb.Append("  IsIncludedInTotal: ").Append(IsIncludedInTotal).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ProvincialRateType)obj);
        }

        /// <summary>
        /// Returns true if ProvincialRateType instances are equal
        /// </summary>
        /// <param name="other">Instance of ProvincialRateType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ProvincialRateType other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    RateType == other.RateType ||
                    RateType.Equals(other.RateType)
                ) &&                 
                (
                    Description == other.Description ||
                    Description.Equals(other.Description)
                ) &&
                (
                    Active == other.Active ||
                    Active.Equals(other.Active)
                ) &&                 
                (
                    PeriodType == other.PeriodType ||
                    PeriodType != null &&
                    PeriodType.Equals(other.PeriodType)
                ) &&                 
                (
                    Rate == other.Rate ||
                    Rate != null &&
                    Rate.Equals(other.Rate)
                ) &&
                (
                    IsPercentRate == other.IsPercentRate ||
                    IsPercentRate.Equals(other.IsPercentRate)
                ) &&
                (
                    IsRateEditable == other.IsRateEditable ||
                    IsRateEditable.Equals(other.IsRateEditable)
                ) &&
                (
                    IsIncludedInTotal == other.IsIncludedInTotal ||
                    IsIncludedInTotal.Equals(other.IsIncludedInTotal)
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
                hash = hash * 59 + RateType.GetHashCode();                   
                hash = hash * 59 + Description.GetHashCode();
                hash = hash * 59 + Active.GetHashCode();
                hash = hash * 59 + PeriodType.GetHashCode();

                if (Rate != null)
                {
                    hash = hash * 59 + Rate.GetHashCode();
                }

                hash = hash * 59 + IsPercentRate.GetHashCode();
                hash = hash * 59 + IsRateEditable.GetHashCode();
                hash = hash * 59 + IsIncludedInTotal.GetHashCode();

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
        public static bool operator ==(ProvincialRateType left, ProvincialRateType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ProvincialRateType left, ProvincialRateType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
