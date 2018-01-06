using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// District Database Model
    /// </summary>
    [MetaData (Description = "The Ministry of Transportion and Infrastructure DISTRICT")]
    public sealed class District : AuditableEntity, IEquatable<District>
    {
        /// <summary>
        /// District Database Model Constructor (required by entity framework)
        /// </summary>
        public District()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="District" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a District (required).</param>
        /// <param name="ministryDistrictId">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="name">The Name of a Ministry District. (required).</param>
        /// <param name="region">The region in which the District is found. (required).</param>
        /// <param name="startDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="districtNumber">A number that uniquely defines a Ministry District..</param>
        /// <param name="endDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public District(int id, int ministryDistrictId, string name, Region region, DateTime startDate, 
            int? districtNumber = null, DateTime? endDate = null)
        {   
            Id = id;
            MinistryDistrictID = ministryDistrictId;
            Name = name;
            Region = region;
            StartDate = startDate;
            DistrictNumber = districtNumber;
            EndDate = endDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a District
        /// </summary>
        /// <value>A system-generated unique identifier for a District</value>
        [MetaData (Description = "A system-generated unique identifier for a District")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.
        /// </summary>
        /// <value>A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.</value>
        [MetaData (Description = "A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.")]
        public int MinistryDistrictID { get; set; }
        
        /// <summary>
        /// The Name of a Ministry District.
        /// </summary>
        /// <value>The Name of a Ministry District.</value>
        [MetaData (Description = "The Name of a Ministry District.")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The region in which the District is found.
        /// </summary>
        /// <value>The region in which the District is found.</value>
        [MetaData (Description = "The region in which the District is found.")]
        public Region Region { get; set; }
        
        /// <summary>
        /// Foreign key for Region 
        /// </summary>   
        [ForeignKey("Region")]
		[JsonIgnore]
		[MetaData (Description = "The region in which the District is found.")]
        public int? RegionId { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaData (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A number that uniquely defines a Ministry District.
        /// </summary>
        /// <value>A number that uniquely defines a Ministry District.</value>
        [MetaData (Description = "A number that uniquely defines a Ministry District.")]
        public int? DistrictNumber { get; set; }
        
        /// <summary>
        /// The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaData (Description = "The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class District {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  MinistryDistrictID: ").Append(MinistryDistrictID).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  DistrictNumber: ").Append(DistrictNumber).Append("\n");
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
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((District)obj);
        }

        /// <summary>
        /// Returns true if District instances are equal
        /// </summary>
        /// <param name="other">Instance of District to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(District other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    MinistryDistrictID == other.MinistryDistrictID ||
                    MinistryDistrictID.Equals(other.MinistryDistrictID)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    Region == other.Region ||
                    Region != null &&
                    Region.Equals(other.Region)
                ) &&                 
                (
                    StartDate == other.StartDate ||
                    StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    DistrictNumber == other.DistrictNumber ||
                    DistrictNumber != null &&
                    DistrictNumber.Equals(other.DistrictNumber)
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
                hash = hash * 59 + MinistryDistrictID.GetHashCode();

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                if (Region != null)
                {
                    hash = hash * 59 + Region.GetHashCode();
                }          
                
                hash = hash * 59 + StartDate.GetHashCode();
                
                if (DistrictNumber != null)
                {
                    hash = hash * 59 + DistrictNumber.GetHashCode();
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
        public static bool operator ==(District left, District right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(District left, District right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
