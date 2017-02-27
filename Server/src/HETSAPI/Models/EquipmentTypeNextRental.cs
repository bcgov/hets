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
    /// 
    /// </summary>

    public partial class EquipmentTypeNextRental : AuditableEntity, IEquatable<EquipmentTypeNextRental>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public EquipmentTypeNextRental()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentTypeNextRental" /> class.
        /// </summary>
        /// <param name="Id">Id (required).</param>
        /// <param name="EquipmentType">EquipmentType.</param>
        /// <param name="AskNextBlock1">The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1..</param>
        /// <param name="AskNextBlock2">The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2..</param>
        /// <param name="AskNextBlockOpen">The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open..</param>
        public EquipmentTypeNextRental(int Id, EquipmentType EquipmentType = null, Equipment AskNextBlock1 = null, Equipment AskNextBlock2 = null, Equipment AskNextBlockOpen = null)
        {   
            this.Id = Id;
            this.EquipmentType = EquipmentType;
            this.AskNextBlock1 = AskNextBlock1;
            this.AskNextBlock2 = AskNextBlock2;
            this.AskNextBlockOpen = AskNextBlockOpen;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentType
        /// </summary>
        public EquipmentType EquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for EquipmentType 
        /// </summary>   
        [ForeignKey("EquipmentType")]
		[JsonIgnore]
        public int? EquipmentTypeId { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.
        /// </summary>
        /// <value>The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.</value>
        [MetaDataExtension (Description = "The id of the next piece of Block 1 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 1.")]
        public Equipment AskNextBlock1 { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlock1 
        /// </summary>   
        [ForeignKey("AskNextBlock1")]
		[JsonIgnore]
        public int? AskNextBlock1Id { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.
        /// </summary>
        /// <value>The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.</value>
        [MetaDataExtension (Description = "The id of the next piece of Block 2 Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block 2.")]
        public Equipment AskNextBlock2 { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlock2 
        /// </summary>   
        [ForeignKey("AskNextBlock2")]
		[JsonIgnore]
        public int? AskNextBlock2Id { get; set; }
        
        /// <summary>
        /// The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.
        /// </summary>
        /// <value>The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.</value>
        [MetaDataExtension (Description = "The id of the next piece of Block Open Equipment to be asked for a Rental Request. If null, start from the first piece of equipment in Block Open.")]
        public Equipment AskNextBlockOpen { get; set; }
        
        /// <summary>
        /// Foreign key for AskNextBlockOpen 
        /// </summary>   
        [ForeignKey("AskNextBlockOpen")]
		[JsonIgnore]
        public int? AskNextBlockOpenId { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentTypeNextRental {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  AskNextBlock1: ").Append(AskNextBlock1).Append("\n");
            sb.Append("  AskNextBlock2: ").Append(AskNextBlock2).Append("\n");
            sb.Append("  AskNextBlockOpen: ").Append(AskNextBlockOpen).Append("\n");
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
            return Equals((EquipmentTypeNextRental)obj);
        }

        /// <summary>
        /// Returns true if EquipmentTypeNextRental instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentTypeNextRental to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentTypeNextRental other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.EquipmentType == other.EquipmentType ||
                    this.EquipmentType != null &&
                    this.EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    this.AskNextBlock1 == other.AskNextBlock1 ||
                    this.AskNextBlock1 != null &&
                    this.AskNextBlock1.Equals(other.AskNextBlock1)
                ) &&                 
                (
                    this.AskNextBlock2 == other.AskNextBlock2 ||
                    this.AskNextBlock2 != null &&
                    this.AskNextBlock2.Equals(other.AskNextBlock2)
                ) &&                 
                (
                    this.AskNextBlockOpen == other.AskNextBlockOpen ||
                    this.AskNextBlockOpen != null &&
                    this.AskNextBlockOpen.Equals(other.AskNextBlockOpen)
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
                if (this.EquipmentType != null)
                {
                    hash = hash * 59 + this.EquipmentType.GetHashCode();
                }                   
                if (this.AskNextBlock1 != null)
                {
                    hash = hash * 59 + this.AskNextBlock1.GetHashCode();
                }                   
                if (this.AskNextBlock2 != null)
                {
                    hash = hash * 59 + this.AskNextBlock2.GetHashCode();
                }                   
                if (this.AskNextBlockOpen != null)
                {
                    hash = hash * 59 + this.AskNextBlockOpen.GetHashCode();
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
        public static bool operator ==(EquipmentTypeNextRental left, EquipmentTypeNextRental right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentTypeNextRental left, EquipmentTypeNextRental right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
