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

    public partial class EquipmentAttachment : IEquatable<EquipmentAttachment>
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
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="Type">The type of the equipment attachment..</param>
        /// <param name="SeqNum">SeqNum.</param>
        /// <param name="Description">Description.</param>
        public EquipmentAttachment(int Id, Equipment Equipment = null, EquipmentAttachmentType Type = null, int? SeqNum = null, string Description = null)
        {   
            this.Id = Id;
            this.Equipment = Equipment;
            this.Type = Type;
            this.SeqNum = SeqNum;
            this.Description = Description;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
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
        /// The type of the equipment attachment.
        /// </summary>
        /// <value>The type of the equipment attachment.</value>
        [MetaDataExtension (Description = "The type of the equipment attachment.")]
        public EquipmentAttachmentType Type { get; set; }
        
        /// <summary>
        /// Foreign key for Type 
        /// </summary>       
        [ForeignKey("Type")]
        public int? TypeRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets SeqNum
        /// </summary>
        public int? SeqNum { get; set; }
        
        /// <summary>
        /// Gets or Sets Description
        /// </summary>
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
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  SeqNum: ").Append(SeqNum).Append("\n");
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
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) &&                 
                (
                    this.SeqNum == other.SeqNum ||
                    this.SeqNum != null &&
                    this.SeqNum.Equals(other.SeqNum)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                   
                if (this.Type != null)
                {
                    hash = hash * 59 + this.Type.GetHashCode();
                }                if (this.SeqNum != null)
                {
                    hash = hash * 59 + this.SeqNum.GetHashCode();
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
