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
    /// 
    /// </summary>

    public partial class LocalArea : IEquatable<LocalArea>
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
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="Name">The full name of the Local Area.</param>
        /// <param name="ServiceArea">The Service Area in which the Local Area is found..</param>
        public LocalArea(int Id, string Name = null, ServiceArea ServiceArea = null)
        {   
            this.Id = Id;
            this.Name = Name;
            this.ServiceArea = ServiceArea;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// The full name of the Local Area
        /// </summary>
        /// <value>The full name of the Local Area</value>
        [MetaDataExtension (Description = "The full name of the Local Area")]
        [MaxLength(255)]
        
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LocalArea {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ServiceArea: ").Append(ServiceArea).Append("\n");
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.ServiceArea == other.ServiceArea ||
                    this.ServiceArea != null &&
                    this.ServiceArea.Equals(other.ServiceArea)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                   
                if (this.ServiceArea != null)
                {
                    hash = hash * 59 + this.ServiceArea.GetHashCode();
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
