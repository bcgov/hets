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
    /// The Ministry of Transportation and Infrastructure SERVICE AREA.
    /// </summary>
        [MetaDataExtension (Description = "The Ministry of Transportation and Infrastructure SERVICE AREA.")]

    public partial class ServiceArea : AuditableEntity,  IEquatable<ServiceArea>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public ServiceArea()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceArea" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a ServiceArea (required).</param>
        /// <param name="MinistryServiceAreaID">A system generated unique identifier. NOT GENERATED IN THIS SYSTEM. (required).</param>
        /// <param name="Name">The Name of a Ministry Service Area. (required).</param>
        /// <param name="District">The district in which the Service Area is found. (required).</param>
        /// <param name="StartDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="AreaNumber">A number that uniquely defines a Ministry Service Area..</param>
        /// <param name="EndDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public ServiceArea(int Id, int MinistryServiceAreaID, string Name, District District, DateTime StartDate, int? AreaNumber = null, DateTime? EndDate = null)
        {   
            this.Id = Id;
            this.MinistryServiceAreaID = MinistryServiceAreaID;
            this.Name = Name;
            this.District = District;
            this.StartDate = StartDate;




            this.AreaNumber = AreaNumber;
            this.EndDate = EndDate;
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
        public int? DistrictRefId { get; set; }
        
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
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.MinistryServiceAreaID == other.MinistryServiceAreaID ||
                    this.MinistryServiceAreaID.Equals(other.MinistryServiceAreaID)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.District == other.District ||
                    this.District != null &&
                    this.District.Equals(other.District)
                ) &&                 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    this.AreaNumber == other.AreaNumber ||
                    this.AreaNumber != null &&
                    this.AreaNumber.Equals(other.AreaNumber)
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
                hash = hash * 59 + this.MinistryServiceAreaID.GetHashCode();                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.District != null)
                {
                    hash = hash * 59 + this.District.GetHashCode();
                }                   
                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
                }                if (this.AreaNumber != null)
                {
                    hash = hash * 59 + this.AreaNumber.GetHashCode();
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
