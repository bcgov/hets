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
using HETSAPI.Models;

namespace HETSAPI.Models
{
    /// <summary>
    /// An Equipment Attachment associated with a piece of Equipment.
    /// </summary>
        [MetaDataExtension (Description = "An Equipment Attachment associated with a piece of Equipment.")]

    public partial class EquipmentAttachment : AuditableEntity, IEquatable<EquipmentAttachment>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public EquipmentAttachment()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentAttachment" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for an EquipmentAttachment (required).</param>
        /// <param name="Attachment">The attachment type as entered by the user (per the business - no lookup). (required).</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="Description">A description of the equipment attachment if the Equipment Attachment Type is insufficient..</param>
        public EquipmentAttachment(int Id, string Attachment, Equipment Equipment = null, string Description = null)
        {   
            this.Id = Id;
            this.Attachment = Attachment;

            this.Equipment = Equipment;
            this.Description = Description;
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentAttachment
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentAttachment</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for an EquipmentAttachment")]
        public int Id { get; set; }
        
        /// <summary>
        /// The attachment type as entered by the user (per the business - no lookup).
        /// </summary>
        /// <value>The attachment type as entered by the user (per the business - no lookup).</value>
        [MetaDataExtension (Description = "The attachment type as entered by the user (per the business - no lookup).")]
        [MaxLength(255)]
        
        public string Attachment { get; set; }
        
        /// <summary>
        /// Gets or Sets Equipment
        /// </summary>
        public Equipment Equipment { get; set; }
        
        /// <summary>
        /// Foreign key for Equipment 
        /// </summary>       
        [ForeignKey("Equipment")]
        public int? EquipmentRefId { get; set; }
        
        /// <summary>
        /// A description of the equipment attachment if the Equipment Attachment Type is insufficient.
        /// </summary>
        /// <value>A description of the equipment attachment if the Equipment Attachment Type is insufficient.</value>
        [MetaDataExtension (Description = "A description of the equipment attachment if the Equipment Attachment Type is insufficient.")]
        [MaxLength(2048)]
        
        public string Description { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentAttachment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Attachment: ").Append(Attachment).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
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
            return Equals((EquipmentAttachment)obj);
        }

        /// <summary>
        /// Returns true if EquipmentAttachment instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentAttachment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentAttachment other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Attachment == other.Attachment ||
                    this.Attachment != null &&
                    this.Attachment.Equals(other.Attachment)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.Attachment != null)
                {
                    hash = hash * 59 + this.Attachment.GetHashCode();
                }                
                                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                if (this.Description != null)
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
        public static bool operator ==(EquipmentAttachment left, EquipmentAttachment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentAttachment left, EquipmentAttachment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
