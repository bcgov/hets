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
    /// An Equipment Type defined at the District level. Links to a provincial Equipment Type for the name of the equipment but supports the District HETS Clerk setting a local name for the Equipment Type. Within a given District, the same provincial Equipment Type might be reused multiple times in, for example, separate sizes (small, medium, large). This enables local areas with large number of the same Equipment Type to have multiple lists.
    /// </summary>
        [MetaDataExtension (Description = "An Equipment Type defined at the District level. Links to a provincial Equipment Type for the name of the equipment but supports the District HETS Clerk setting a local name for the Equipment Type. Within a given District, the same provincial Equipment Type might be reused multiple times in, for example, separate sizes (small, medium, large). This enables local areas with large number of the same Equipment Type to have multiple lists.")]

    public partial class DistrictEquipmentType : AuditableEntity, IEquatable<DistrictEquipmentType>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public DistrictEquipmentType()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictEquipmentType" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for an EquipmentType (required).</param>
        /// <param name="EquipmentType">EquipmentType (required).</param>
        /// <param name="District">District (required).</param>
        /// <param name="DistrictEquipmentName">The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size. (required).</param>
        public DistrictEquipmentType(int Id, EquipmentType EquipmentType, District District, string DistrictEquipmentName)
        {   
            this.Id = Id;
            this.EquipmentType = EquipmentType;
            this.District = District;
            this.DistrictEquipmentName = DistrictEquipmentName;



        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentType
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentType</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for an EquipmentType")]
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
        [MetaDataExtension (Description = "The name of this equipment type used at the District Level. This could be just the equipmentName if this is the only EquipmentType defined in this District, or could be a name that separates out multiple EquipmentTypes used within a District to, for instance, separate out the same EquipmentName by size.")]
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((DistrictEquipmentType)obj);
        }

        /// <summary>
        /// Returns true if DistrictEquipmentType instances are equal
        /// </summary>
        /// <param name="other">Instance of DistrictEquipmentType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DistrictEquipmentType other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.EquipmentType == other.EquipmentType ||
                    this.EquipmentType != null &&
                    this.EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    this.District == other.District ||
                    this.District != null &&
                    this.District.Equals(other.District)
                ) &&                 
                (
                    this.DistrictEquipmentName == other.DistrictEquipmentName ||
                    this.DistrictEquipmentName != null &&
                    this.DistrictEquipmentName.Equals(other.DistrictEquipmentName)
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
                if (this.EquipmentType != null)
                {
                    hash = hash * 59 + this.EquipmentType.GetHashCode();
                }                   
                if (this.District != null)
                {
                    hash = hash * 59 + this.District.GetHashCode();
                }                if (this.DistrictEquipmentName != null)
                {
                    hash = hash * 59 + this.DistrictEquipmentName.GetHashCode();
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
