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

namespace HETSAPI.Models
{
    /// <summary>
    /// The Ministry of Transportion and Infrastructure DISTRICT
    /// </summary>
        [MetaDataExtension (Description = "The Ministry of Transportion and Infrastructure DISTRICT")]

    public partial class District : AuditableEntity,  IEquatable<District>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public District()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="District" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a District (required).</param>
        /// <param name="MinistryDistrictID">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="Name">The Name of a Ministry District. (required).</param>
        /// <param name="Region">The region in which the District is found. (required).</param>
        /// <param name="StartDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="DistrictNumber">A number that uniquely defines a Ministry District..</param>
        /// <param name="EndDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public District(int Id, int MinistryDistrictID, string Name, Region Region, DateTime StartDate, int? DistrictNumber = null, DateTime? EndDate = null)
        {   
            this.Id = Id;
            this.MinistryDistrictID = MinistryDistrictID;
            this.Name = Name;
            this.Region = Region;
            this.StartDate = StartDate;




            this.DistrictNumber = DistrictNumber;
            this.EndDate = EndDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a District
        /// </summary>
        /// <value>A system-generated unique identifier for a District</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a District")]
        public int Id { get; set; }
        
        /// <summary>
        /// A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.
        /// </summary>
        /// <value>A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.</value>
        [MetaDataExtension (Description = "A system generated unique identifier. NOT GENERATED IN THIS SYSTEM.")]
        public int MinistryDistrictID { get; set; }
        
        /// <summary>
        /// The Name of a Ministry District.
        /// </summary>
        /// <value>The Name of a Ministry District.</value>
        [MetaDataExtension (Description = "The Name of a Ministry District.")]
        [MaxLength(150)]
        
        public string Name { get; set; }
        
        /// <summary>
        /// The region in which the District is found.
        /// </summary>
        /// <value>The region in which the District is found.</value>
        [MetaDataExtension (Description = "The region in which the District is found.")]
        public Region Region { get; set; }
        
        /// <summary>
        /// Foreign key for Region 
        /// </summary>       
        [ForeignKey("Region")]
        public int? RegionRefId { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaDataExtension (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A number that uniquely defines a Ministry District.
        /// </summary>
        /// <value>A number that uniquely defines a Ministry District.</value>
        [MetaDataExtension (Description = "A number that uniquely defines a Ministry District.")]
        public int? DistrictNumber { get; set; }
        
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((District)obj);
        }

        /// <summary>
        /// Returns true if District instances are equal
        /// </summary>
        /// <param name="other">Instance of District to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(District other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.MinistryDistrictID == other.MinistryDistrictID ||
                    this.MinistryDistrictID.Equals(other.MinistryDistrictID)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.Region == other.Region ||
                    this.Region != null &&
                    this.Region.Equals(other.Region)
                ) &&                 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    this.DistrictNumber == other.DistrictNumber ||
                    this.DistrictNumber != null &&
                    this.DistrictNumber.Equals(other.DistrictNumber)
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
                hash = hash * 59 + this.MinistryDistrictID.GetHashCode();                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.Region != null)
                {
                    hash = hash * 59 + this.Region.GetHashCode();
                }                   
                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
                }                if (this.DistrictNumber != null)
                {
                    hash = hash * 59 + this.DistrictNumber.GetHashCode();
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
