using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Service Area Database Model
    /// </summary>
    [MetaDataExtension (Description = "The Ministry of Transportation and Infrastructure SERVICE AREA.")]
    public sealed class ServiceArea : AuditableEntity, IEquatable<ServiceArea>
    {
        /// <summary>
        /// Service Area Database Model Constructor (required by entity framework)
        /// </summary>
        public ServiceArea()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceArea" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a ServiceArea (required).</param>
        /// <param name="ministryServiceAreaId">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="name">The Name of a Ministry Service Area. (required).</param>
        /// <param name="district">The district in which the Service Area is found. (required).</param>
        /// <param name="startDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="areaNumber">A number that uniquely defines a Ministry Service Area..</param>
        /// <param name="endDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public ServiceArea(int id, int ministryServiceAreaId, string name, District district, DateTime startDate, 
            int? areaNumber = null, DateTime? endDate = null)
        {   
            Id = id;
            MinistryServiceAreaID = ministryServiceAreaId;
            Name = name;
            District = district;
            StartDate = startDate;
            AreaNumber = areaNumber;
            EndDate = endDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a ServiceArea
        /// </summary>
        /// <value>A system-generated unique identifier for a ServiceArea</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a ServiceArea")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.
        /// </summary>
        /// <value>A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.</value>
        [MetaDataExtension (Description = "A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.")]
        public int MinistryServiceAreaID { get; set; }
        
        /// <summary>
        /// The Name of a Ministry Service Area.
        /// </summary>
        /// <value>The Name of a Ministry Service Area.</value>
        [MetaDataExtension (Description = "The Name of a Ministry Service Area.")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The district in which the Service Area is found.
        /// </summary>
        /// <value>The district in which the Service Area is found.</value>
        [MetaDataExtension (Description = "The district in which the Service Area is found.")]
        public District District { get; set; }
        
        /// <summary>
        /// Foreign key for District 
        /// </summary>   
        [ForeignKey("District")]
		[JsonIgnore]
		[MetaDataExtension (Description = "The district in which the Service Area is found.")]
        public int? DistrictId { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaDataExtension (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A number that uniquely defines a Ministry Service Area.
        /// </summary>
        /// <value>A number that uniquely defines a Ministry Service Area.</value>
        [MetaDataExtension (Description = "A number that uniquely defines a Ministry Service Area.")]
        public int? AreaNumber { get; set; }
        
        /// <summary>
        /// The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaDataExtension (Description = "The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class ServiceArea {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  MinistryServiceAreaID: ").Append(MinistryServiceAreaID).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  AreaNumber: ").Append(AreaNumber).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
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

            return Equals((ServiceArea)obj);
        }

        /// <summary>
        /// Returns true if ServiceArea instances are equal
        /// </summary>
        /// <param name="other">Instance of ServiceArea to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ServiceArea other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    MinistryServiceAreaID == other.MinistryServiceAreaID ||
                    MinistryServiceAreaID.Equals(other.MinistryServiceAreaID)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    District == other.District ||
                    District != null &&
                    District.Equals(other.District)
                ) &&                 
                (
                    StartDate == other.StartDate ||
                    StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    AreaNumber == other.AreaNumber ||
                    AreaNumber != null &&
                    AreaNumber.Equals(other.AreaNumber)
                ) &&                 
                (
                    EndDate == other.EndDate ||
                    EndDate != null &&
                    EndDate.Equals(other.EndDate)
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
                hash = hash * 59 + MinistryServiceAreaID.GetHashCode();

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
                }

                hash = hash * 59 + StartDate.GetHashCode();

                if (AreaNumber != null)
                {
                    hash = hash * 59 + AreaNumber.GetHashCode();
                }

                if (EndDate != null)
                {
                    hash = hash * 59 + EndDate.GetHashCode();
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
        public static bool operator ==(ServiceArea left, ServiceArea right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(ServiceArea left, ServiceArea right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
