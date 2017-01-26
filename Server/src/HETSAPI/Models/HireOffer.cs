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

    public partial class HireOffer : IEquatable<HireOffer>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public HireOffer()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HireOffer" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="Request">Request.</param>
        /// <param name="Equipment">Equipment.</param>
        /// <param name="RentalAgreement">The rental agreement created for this hire offer. Available only if status code is Yes.</param>
        /// <param name="IsForceHire">IsForceHire.</param>
        /// <param name="Asked">Asked.</param>
        /// <param name="AskedDate">AskedDate.</param>
        /// <param name="AcceptedOffer">The response about the equipment. Either a No (move to next on the list) or Yes (move to on to the Rental Agreement).</param>
        /// <param name="RefuseReason">RefuseReason.</param>
        /// <param name="Note">Note.</param>
        /// <param name="EquipmentVerifiedActive">EquipmentVerifiedActive.</param>
        /// <param name="FlagEquipmentUpdate">FlagEquipmentUpdate.</param>
        /// <param name="EquipmentUpdateReason">EquipmentUpdateReason.</param>
        public HireOffer(int Id, Request Request = null, Equipment Equipment = null, RentalAgreement RentalAgreement = null, bool? IsForceHire = null, bool? Asked = null, DateTime? AskedDate = null, bool? AcceptedOffer = null, string RefuseReason = null, string Note = null, bool? EquipmentVerifiedActive = null, bool? FlagEquipmentUpdate = null, string EquipmentUpdateReason = null)
        {   
            this.Id = Id;
            this.Request = Request;
            this.Equipment = Equipment;
            this.RentalAgreement = RentalAgreement;
            this.IsForceHire = IsForceHire;
            this.Asked = Asked;
            this.AskedDate = AskedDate;
            this.AcceptedOffer = AcceptedOffer;
            this.RefuseReason = RefuseReason;
            this.Note = Note;
            this.EquipmentVerifiedActive = EquipmentVerifiedActive;
            this.FlagEquipmentUpdate = FlagEquipmentUpdate;
            this.EquipmentUpdateReason = EquipmentUpdateReason;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Request
        /// </summary>
        public Request Request { get; set; }
        
        /// <summary>
        /// Foreign key for Request 
        /// </summary>       
        [ForeignKey("Request")]
        public int? RequestRefId { get; set; }
        
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
        /// The rental agreement created for this hire offer. Available only if status code is Yes
        /// </summary>
        /// <value>The rental agreement created for this hire offer. Available only if status code is Yes</value>
        [MetaDataExtension (Description = "The rental agreement created for this hire offer. Available only if status code is Yes")]
        public RentalAgreement RentalAgreement { get; set; }
        
        /// <summary>
        /// Foreign key for RentalAgreement 
        /// </summary>       
        [ForeignKey("RentalAgreement")]
        public int? RentalAgreementRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets IsForceHire
        /// </summary>
        public bool? IsForceHire { get; set; }
        
        /// <summary>
        /// Gets or Sets Asked
        /// </summary>
        public bool? Asked { get; set; }
        
        /// <summary>
        /// Gets or Sets AskedDate
        /// </summary>
        public DateTime? AskedDate { get; set; }
        
        /// <summary>
        /// The response about the equipment. Either a No (move to next on the list) or Yes (move to on to the Rental Agreement)
        /// </summary>
        /// <value>The response about the equipment. Either a No (move to next on the list) or Yes (move to on to the Rental Agreement)</value>
        [MetaDataExtension (Description = "The response about the equipment. Either a No (move to next on the list) or Yes (move to on to the Rental Agreement)")]
        public bool? AcceptedOffer { get; set; }
        
        /// <summary>
        /// Gets or Sets RefuseReason
        /// </summary>
        public string RefuseReason { get; set; }
        
        /// <summary>
        /// Gets or Sets Note
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentVerifiedActive
        /// </summary>
        public bool? EquipmentVerifiedActive { get; set; }
        
        /// <summary>
        /// Gets or Sets FlagEquipmentUpdate
        /// </summary>
        public bool? FlagEquipmentUpdate { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentUpdateReason
        /// </summary>
        public string EquipmentUpdateReason { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class HireOffer {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Request: ").Append(Request).Append("\n");
            sb.Append("  Equipment: ").Append(Equipment).Append("\n");
            sb.Append("  RentalAgreement: ").Append(RentalAgreement).Append("\n");
            sb.Append("  IsForceHire: ").Append(IsForceHire).Append("\n");
            sb.Append("  Asked: ").Append(Asked).Append("\n");
            sb.Append("  AskedDate: ").Append(AskedDate).Append("\n");
            sb.Append("  AcceptedOffer: ").Append(AcceptedOffer).Append("\n");
            sb.Append("  RefuseReason: ").Append(RefuseReason).Append("\n");
            sb.Append("  Note: ").Append(Note).Append("\n");
            sb.Append("  EquipmentVerifiedActive: ").Append(EquipmentVerifiedActive).Append("\n");
            sb.Append("  FlagEquipmentUpdate: ").Append(FlagEquipmentUpdate).Append("\n");
            sb.Append("  EquipmentUpdateReason: ").Append(EquipmentUpdateReason).Append("\n");
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
            return Equals((HireOffer)obj);
        }

        /// <summary>
        /// Returns true if HireOffer instances are equal
        /// </summary>
        /// <param name="other">Instance of HireOffer to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(HireOffer other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Request == other.Request ||
                    this.Request != null &&
                    this.Request.Equals(other.Request)
                ) &&                 
                (
                    this.Equipment == other.Equipment ||
                    this.Equipment != null &&
                    this.Equipment.Equals(other.Equipment)
                ) &&                 
                (
                    this.RentalAgreement == other.RentalAgreement ||
                    this.RentalAgreement != null &&
                    this.RentalAgreement.Equals(other.RentalAgreement)
                ) &&                 
                (
                    this.IsForceHire == other.IsForceHire ||
                    this.IsForceHire != null &&
                    this.IsForceHire.Equals(other.IsForceHire)
                ) &&                 
                (
                    this.Asked == other.Asked ||
                    this.Asked != null &&
                    this.Asked.Equals(other.Asked)
                ) &&                 
                (
                    this.AskedDate == other.AskedDate ||
                    this.AskedDate != null &&
                    this.AskedDate.Equals(other.AskedDate)
                ) &&                 
                (
                    this.AcceptedOffer == other.AcceptedOffer ||
                    this.AcceptedOffer != null &&
                    this.AcceptedOffer.Equals(other.AcceptedOffer)
                ) &&                 
                (
                    this.RefuseReason == other.RefuseReason ||
                    this.RefuseReason != null &&
                    this.RefuseReason.Equals(other.RefuseReason)
                ) &&                 
                (
                    this.Note == other.Note ||
                    this.Note != null &&
                    this.Note.Equals(other.Note)
                ) &&                 
                (
                    this.EquipmentVerifiedActive == other.EquipmentVerifiedActive ||
                    this.EquipmentVerifiedActive != null &&
                    this.EquipmentVerifiedActive.Equals(other.EquipmentVerifiedActive)
                ) &&                 
                (
                    this.FlagEquipmentUpdate == other.FlagEquipmentUpdate ||
                    this.FlagEquipmentUpdate != null &&
                    this.FlagEquipmentUpdate.Equals(other.FlagEquipmentUpdate)
                ) &&                 
                (
                    this.EquipmentUpdateReason == other.EquipmentUpdateReason ||
                    this.EquipmentUpdateReason != null &&
                    this.EquipmentUpdateReason.Equals(other.EquipmentUpdateReason)
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
                if (this.Request != null)
                {
                    hash = hash * 59 + this.Request.GetHashCode();
                }                   
                if (this.Equipment != null)
                {
                    hash = hash * 59 + this.Equipment.GetHashCode();
                }                   
                if (this.RentalAgreement != null)
                {
                    hash = hash * 59 + this.RentalAgreement.GetHashCode();
                }                if (this.IsForceHire != null)
                {
                    hash = hash * 59 + this.IsForceHire.GetHashCode();
                }                
                                if (this.Asked != null)
                {
                    hash = hash * 59 + this.Asked.GetHashCode();
                }                
                                if (this.AskedDate != null)
                {
                    hash = hash * 59 + this.AskedDate.GetHashCode();
                }                
                                if (this.AcceptedOffer != null)
                {
                    hash = hash * 59 + this.AcceptedOffer.GetHashCode();
                }                
                                if (this.RefuseReason != null)
                {
                    hash = hash * 59 + this.RefuseReason.GetHashCode();
                }                
                                if (this.Note != null)
                {
                    hash = hash * 59 + this.Note.GetHashCode();
                }                
                                if (this.EquipmentVerifiedActive != null)
                {
                    hash = hash * 59 + this.EquipmentVerifiedActive.GetHashCode();
                }                
                                if (this.FlagEquipmentUpdate != null)
                {
                    hash = hash * 59 + this.FlagEquipmentUpdate.GetHashCode();
                }                
                                if (this.EquipmentUpdateReason != null)
                {
                    hash = hash * 59 + this.EquipmentUpdateReason.GetHashCode();
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
        public static bool operator ==(HireOffer left, HireOffer right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HireOffer left, HireOffer right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
