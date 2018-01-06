using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// District Equipment Type Database Model
    /// </summary>
    [MetaData (Description = "An Equipment Type defined at the District level. Links to a provincial Equipment Type for the name of the equipment but supports the District HETS Clerk setting a local name for the Equipment Type. Within a given District, the same provincial Equipment Type might be reused multiple times in, for example, separate sizes (small, medium, large). This enables local areas with large number of the same Equipment Type to have multiple lists.")]
    public sealed partial class DistrictEquipmentType : AuditableEntity, IEquatable<DistrictEquipmentType>
    {
        /// <summary>
        /// District Equipment Type Database Model Constructor (required by entity framework)
        /// </summary>
        public DistrictEquipmentType()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictEquipmentType" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for an EquipmentType (required).</param>
        /// <param name="equipmentType">EquipmentType (required).</param>
        /// <param name="district">District (required).</param>
        /// <param name="districtEquipmentName">The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size. (required).</param>
        public DistrictEquipmentType(int id, EquipmentType equipmentType, District district, string districtEquipmentName)
        {
            Id = id;
            EquipmentType = equipmentType;
            District = district;
            DistrictEquipmentName = districtEquipmentName;            
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentType
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentType</value>
        [MetaData (Description = "A system-generated unique identifier for an EquipmentType")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets EquipmentType
        /// </summary>
        public EquipmentType EquipmentType { get; set; }

        /// <summary>
        /// Foreign key for EquipmentType
        /// </summary>
        [ForeignKey("EquipmentType")]
		[JsonIgnore]
        public int? EquipmentTypeId { get; set; }

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
        /// The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size.
        /// </summary>
        /// <value>The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size.</value>
        [MetaData (Description = "The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size.")]
        [MaxLength(255)]
        public string DistrictEquipmentName { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class DistrictEquipmentType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  DistrictEquipmentName: ").Append(DistrictEquipmentName).Append("\n");
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
            return obj.GetType() == GetType() && Equals((DistrictEquipmentType)obj);
        }

        /// <summary>
        /// Returns true if DistrictEquipmentType instances are equal
        /// </summary>
        /// <param name="other">Instance of DistrictEquipmentType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DistrictEquipmentType other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&
                (
                    EquipmentType == other.EquipmentType ||
                    EquipmentType != null &&
                    EquipmentType.Equals(other.EquipmentType)
                ) &&
                (
                    District == other.District ||
                    District != null &&
                    District.Equals(other.District)
                ) &&
                (
                    DistrictEquipmentName == other.DistrictEquipmentName ||
                    DistrictEquipmentName != null &&
                    DistrictEquipmentName.Equals(other.DistrictEquipmentName)
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

                if (EquipmentType != null)
                {
                    hash = hash * 59 + EquipmentType.GetHashCode();
                }

                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
                }
                if (DistrictEquipmentName != null)
                {
                    hash = hash * 59 + DistrictEquipmentName.GetHashCode();
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
        public static bool operator ==(DistrictEquipmentType left, DistrictEquipmentType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DistrictEquipmentType left, DistrictEquipmentType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
