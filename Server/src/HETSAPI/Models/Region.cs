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

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>

    public partial class Region : IEquatable<Region>
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
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="MinistryRegionID">The Ministry ID for the Region.</param>
        /// <param name="Name">The name of the Region.</param>
        /// <param name="StartDate">The effective date of the Region - NOT CURRENTLY ENFORCED.</param>
        /// <param name="EndDate">The end date of the Region; null if active - NOT CURRENTLY ENFORCED.</param>
        public Region(int Id, int? MinistryRegionID = null, string Name = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {   
            this.Id = Id;
            this.MinistryRegionID = MinistryRegionID;
            this.Name = Name;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// The Ministry ID for the Region
        /// </summary>
        /// <value>The Ministry ID for the Region</value>
        [MetaDataExtension (Description = "The Ministry ID for the Region")]
        public int? MinistryRegionID { get; set; }
        
        /// <summary>
        /// The name of the Region
        /// </summary>
        /// <value>The name of the Region</value>
        [MetaDataExtension (Description = "The name of the Region")]
        public string Name { get; set; }
        
        /// <summary>
        /// The effective date of the Region - NOT CURRENTLY ENFORCED
        /// </summary>
        /// <value>The effective date of the Region - NOT CURRENTLY ENFORCED</value>
        [MetaDataExtension (Description = "The effective date of the Region - NOT CURRENTLY ENFORCED")]
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// The end date of the Region; null if active - NOT CURRENTLY ENFORCED
        /// </summary>
        /// <value>The end date of the Region; null if active - NOT CURRENTLY ENFORCED</value>
        [MetaDataExtension (Description = "The end date of the Region; null if active - NOT CURRENTLY ENFORCED")]
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
                    this.MinistryRegionID != null &&
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.MinistryRegionID != null)
                {
                    hash = hash * 59 + this.MinistryRegionID.GetHashCode();
                }                
                                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
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
