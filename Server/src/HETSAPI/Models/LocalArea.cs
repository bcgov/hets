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
    /// A HETS-application defined area that is within a Service Area.
    /// </summary>
        [MetaDataExtension (Description = "A HETS-application defined area that is within a Service Area.")]

    public partial class LocalArea : AuditableEntity,  IEquatable<LocalArea>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public LocalArea()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalArea" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a LocalArea (required).</param>
        /// <param name="StartDate">The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM (required).</param>
        /// <param name="LocalAreaNumber">A system-generated, visible to the user number for the Local Area.</param>
        /// <param name="Name">The full name of the Local Area.</param>
        /// <param name="ServiceArea">The Service Area in which the Local Area is found..</param>
        /// <param name="EndDate">The DATE the business information ceased to be in effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM.</param>
        public LocalArea(int Id, DateTime StartDate, int? LocalAreaNumber = null, string Name = null, ServiceArea ServiceArea = null, DateTime? EndDate = null)
        {   
            this.Id = Id;
            this.StartDate = StartDate;

            this.LocalAreaNumber = LocalAreaNumber;
            this.Name = Name;
            this.ServiceArea = ServiceArea;
            this.EndDate = EndDate;
        }

        /// <summary>
        /// A system-generated unique identifier for a LocalArea
        /// </summary>
        /// <value>A system-generated unique identifier for a LocalArea</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a LocalArea")]
        public int Id { get; set; }
        
        /// <summary>
        /// The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM
        /// </summary>
        /// <value>The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM</value>
        [MetaDataExtension (Description = "The DATE the business information came into effect. - NOT CURRENTLY ENFORCED IN THIS SYSTEM")]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// A system-generated, visible to the user number for the Local Area
        /// </summary>
        /// <value>A system-generated, visible to the user number for the Local Area</value>
        [MetaDataExtension (Description = "A system-generated, visible to the user number for the Local Area")]
        public int? LocalAreaNumber { get; set; }
        
        /// <summary>
        /// The full name of the Local Area
        /// </summary>
        /// <value>The full name of the Local Area</value>
        [MetaDataExtension (Description = "The full name of the Local Area")]
        [MaxLength(150)]
        
        public string Name { get; set; }
        
        /// <summary>
        /// The Service Area in which the Local Area is found.
        /// </summary>
        /// <value>The Service Area in which the Local Area is found.</value>
        [MetaDataExtension (Description = "The Service Area in which the Local Area is found.")]
        public ServiceArea ServiceArea { get; set; }
        
        /// <summary>
        /// Foreign key for ServiceArea 
        /// </summary>       
        [ForeignKey("ServiceArea")]
        public int? ServiceAreaRefId { get; set; }
        
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
            sb.Append("class LocalArea {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  LocalAreaNumber: ").Append(LocalAreaNumber).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ServiceArea: ").Append(ServiceArea).Append("\n");
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
            return Equals((LocalArea)obj);
        }

        /// <summary>
        /// Returns true if LocalArea instances are equal
        /// </summary>
        /// <param name="other">Instance of LocalArea to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LocalArea other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) &&                 
                (
                    this.LocalAreaNumber == other.LocalAreaNumber ||
                    this.LocalAreaNumber != null &&
                    this.LocalAreaNumber.Equals(other.LocalAreaNumber)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.ServiceArea == other.ServiceArea ||
                    this.ServiceArea != null &&
                    this.ServiceArea.Equals(other.ServiceArea)
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
                if (this.StartDate != null)
                {
                    hash = hash * 59 + this.StartDate.GetHashCode();
                }                if (this.LocalAreaNumber != null)
                {
                    hash = hash * 59 + this.LocalAreaNumber.GetHashCode();
                }                
                                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.ServiceArea != null)
                {
                    hash = hash * 59 + this.ServiceArea.GetHashCode();
                }                if (this.EndDate != null)
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
        public static bool operator ==(LocalArea left, LocalArea right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LocalArea left, LocalArea right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
