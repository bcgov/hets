using System;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// The Ministry of Transportion and Infrastructure REGION.
    /// </summary>
        [MetaDataExtension (Description = "The Ministry of Transportion and Infrastructure REGION.")]

    public partial class Region : AuditableEntity, IEquatable<Region>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Region()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a Region (required).</param>
        /// <param name="MinistryRegionID">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="Name">The name of a Minsitry Region. (required).</param>
        /// <param name="StartDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="RegionNumber">A code that uniquely defines a Region..</param>
        /// <param name="EndDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public Region(int Id, int MinistryRegionID, string Name, DateTime StartDate, int? RegionNumber = null, DateTime? EndDate = null)
        {   
            this.Id = Id;
            this.MinistryRegionID = MinistryRegionID;
            this.Name = Name;
            this.StartDate = StartDate;



            this.RegionNumber = RegionNumber;
            this.EndDate = EndDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a Region
        /// </summary>
        /// <value>A system-generated unique identifier for a Region</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Region")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.
        /// </summary>
        /// <value>A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.</value>
        [MetaDataExtension (Description = "A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.")]
        public int MinistryRegionID { get; set; }
        
        /// <summary>
        /// The name of a Minsitry Region.
        /// </summary>
        /// <value>The name of a Minsitry Region.</value>
        [MetaDataExtension (Description = "The name of a Minsitry Region.")]
        [MaxLength(150)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaDataExtension (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A code that uniquely defines a Region.
        /// </summary>
        /// <value>A code that uniquely defines a Region.</value>
        [MetaDataExtension (Description = "A code that uniquely defines a Region.")]
        public int? RegionNumber { get; set; }
        
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((Region)obj);
        }

        /// <summary>
        /// Returns true if Region instances are equal
        /// </summary>
        /// <param name="other">Instance of Region to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Region other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.MinistryRegionID == other.MinistryRegionID ||
                    this.MinistryRegionID.Equals(other.MinistryRegionID)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    this.RegionNumber == other.RegionNumber ||
                    this.RegionNumber != null &&
                    this.RegionNumber.Equals(other.RegionNumber)
                ) &&                 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
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
                hash = hash * 59 + this.MinistryRegionID.GetHashCode();                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
                }                if (this.RegionNumber != null)
                {
                    hash = hash * 59 + this.RegionNumber.GetHashCode();
                }                
                                if (this.EndDate != null)
                {
                    hash = hash * 59 + this.EndDate.GetHashCode();
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
