using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Region Database Model
    /// </summary>
    [MetaData (Description = "The Ministry of Transportion and Infrastructure REGION.")]
    public sealed class Region : AuditableEntity, IEquatable<Region>
    {
        /// <summary>
        /// REgion Database Model Constructor (required by entity framework)
        /// </summary>
        public Region()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Region (required).</param>
        /// <param name="ministryRegionId">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="name">The name of a Minsitry Region. (required).</param>
        /// <param name="startDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="regionNumber">A code that uniquely defines a Region..</param>
        /// <param name="endDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public Region(int id, int ministryRegionId, string name, DateTime startDate, int? regionNumber = null, DateTime? endDate = null)
        {   
            Id = id;
            MinistryRegionID = ministryRegionId;
            Name = name;
            StartDate = startDate;
            RegionNumber = regionNumber;
            EndDate = endDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a Region
        /// </summary>
        /// <value>A system-generated unique identifier for a Region</value>
        [MetaData (Description = "A system-generated unique identifier for a Region")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.
        /// </summary>
        /// <value>A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.</value>
        [MetaData (Description = "A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.")]
        public int MinistryRegionID { get; set; }
        
        /// <summary>
        /// The name of a Minsitry Region.
        /// </summary>
        /// <value>The name of a Minsitry Region.</value>
        [MetaData (Description = "The name of a Minsitry Region.")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaData (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A code that uniquely defines a Region.
        /// </summary>
        /// <value>A code that uniquely defines a Region.</value>
        [MetaData (Description = "A code that uniquely defines a Region.")]
        public int? RegionNumber { get; set; }
        
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

            sb.Append("class Region {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  MinistryRegionID: ").Append(MinistryRegionID).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  RegionNumber: ").Append(RegionNumber).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Region)obj);
        }

        /// <summary>
        /// Returns true if Region instances are equal
        /// </summary>
        /// <param name="other">Instance of Region to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Region other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    MinistryRegionID == other.MinistryRegionID ||
                    MinistryRegionID.Equals(other.MinistryRegionID)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    StartDate == other.StartDate ||
                    StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    RegionNumber == other.RegionNumber ||
                    RegionNumber != null &&
                    RegionNumber.Equals(other.RegionNumber)
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
                hash = hash * 59 + MinistryRegionID.GetHashCode();

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }                
                                   
                hash = hash * 59 + StartDate.GetHashCode();
                
                if (RegionNumber != null)
                {
                    hash = hash * 59 + RegionNumber.GetHashCode();
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
        public static bool operator ==(Region left, Region right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Region left, Region right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
