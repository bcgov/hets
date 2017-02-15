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
    /// An Equipment Attachment Type.
    /// </summary>
        [MetaDataExtension (Description = "An Equipment Attachment Type.")]

    public partial class EquipmentAttachmentType : IEquatable<EquipmentAttachmentType>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public EquipmentAttachmentType()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentAttachmentType" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a EquipmentAttachmentType (required).</param>
        /// <param name="Code">TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED - A unique but human-friendly code for the Equipment Attachment Type that can be displayed on screens with little screen real estate available..</param>
        /// <param name="Description">The name of the equipment attachment type, as specified by the HETS Clerk creating the equipment type..</param>
        public EquipmentAttachmentType(int Id, string Code = null, string Description = null)
        {   
            this.Id = Id;
            this.Code = Code;
            this.Description = Description;
        }

        /// <summary>
        /// A system-generated unique identifier for a EquipmentAttachmentType
        /// </summary>
        /// <value>A system-generated unique identifier for a EquipmentAttachmentType</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a EquipmentAttachmentType")]
        public int Id { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED - A unique but human-friendly code for the Equipment Attachment Type that can be displayed on screens with little screen real estate available.
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED - A unique but human-friendly code for the Equipment Attachment Type that can be displayed on screens with little screen real estate available.</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED - A unique but human-friendly code for the Equipment Attachment Type that can be displayed on screens with little screen real estate available.")]
        [MaxLength(50)]
        
        public string Code { get; set; }
        
        /// <summary>
        /// The name of the equipment attachment type, as specified by the HETS Clerk creating the equipment type.
        /// </summary>
        /// <value>The name of the equipment attachment type, as specified by the HETS Clerk creating the equipment type.</value>
        [MetaDataExtension (Description = "The name of the equipment attachment type, as specified by the HETS Clerk creating the equipment type.")]
        [MaxLength(2048)]
        
        public string Description { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentAttachmentType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
            return Equals((EquipmentAttachmentType)obj);
        }

        /// <summary>
        /// Returns true if EquipmentAttachmentType instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentAttachmentType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentAttachmentType other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) &&                 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.Code != null)
                {
                    hash = hash * 59 + this.Code.GetHashCode();
                }                
                                if (this.Description != null)
                {
                    hash = hash * 59 + this.Description.GetHashCode();
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
        public static bool operator ==(EquipmentAttachmentType left, EquipmentAttachmentType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentAttachmentType left, EquipmentAttachmentType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
