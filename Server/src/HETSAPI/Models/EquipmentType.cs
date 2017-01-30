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

    public partial class EquipmentType : IEquatable<EquipmentType>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public EquipmentType()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentType" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="Code">Code.</param>
        /// <param name="Description">Description.</param>
        /// <param name="EquipRentalRateNo">EquipRentalRateNo.</param>
        /// <param name="EquipRentalRatePage">EquipRentalRatePage.</param>
        /// <param name="MaxHours">MaxHours.</param>
        /// <param name="ExtendHours">ExtendHours.</param>
        /// <param name="MaxHoursSub">MaxHoursSub.</param>
        /// <param name="SecondBlk">SecondBlk.</param>
        public EquipmentType(int Id, LocalArea LocalArea = null, string Code = null, string Description = null, float? EquipRentalRateNo = null, float? EquipRentalRatePage = null, float? MaxHours = null, float? ExtendHours = null, float? MaxHoursSub = null, string SecondBlk = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.Code = Code;
            this.Description = Description;
            this.EquipRentalRateNo = EquipRentalRateNo;
            this.EquipRentalRatePage = EquipRentalRatePage;
            this.MaxHours = MaxHours;
            this.ExtendHours = ExtendHours;
            this.MaxHoursSub = MaxHoursSub;
            this.SecondBlk = SecondBlk;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>       
        [ForeignKey("LocalArea")]
        public int? LocalAreaRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [MaxLength(255)]
        
        public string Code { get; set; }
        
        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [MaxLength(255)]
        
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipRentalRateNo
        /// </summary>
        public float? EquipRentalRateNo { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipRentalRatePage
        /// </summary>
        public float? EquipRentalRatePage { get; set; }
        
        /// <summary>
        /// Gets or Sets MaxHours
        /// </summary>
        public float? MaxHours { get; set; }
        
        /// <summary>
        /// Gets or Sets ExtendHours
        /// </summary>
        public float? ExtendHours { get; set; }
        
        /// <summary>
        /// Gets or Sets MaxHoursSub
        /// </summary>
        public float? MaxHoursSub { get; set; }
        
        /// <summary>
        /// Gets or Sets SecondBlk
        /// </summary>
        [MaxLength(255)]
        
        public string SecondBlk { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  EquipRentalRateNo: ").Append(EquipRentalRateNo).Append("\n");
            sb.Append("  EquipRentalRatePage: ").Append(EquipRentalRatePage).Append("\n");
            sb.Append("  MaxHours: ").Append(MaxHours).Append("\n");
            sb.Append("  ExtendHours: ").Append(ExtendHours).Append("\n");
            sb.Append("  MaxHoursSub: ").Append(MaxHoursSub).Append("\n");
            sb.Append("  SecondBlk: ").Append(SecondBlk).Append("\n");
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
            return Equals((EquipmentType)obj);
        }

        /// <summary>
        /// Returns true if EquipmentType instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentType to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentType other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
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
                ) &&                 
                (
                    this.EquipRentalRateNo == other.EquipRentalRateNo ||
                    this.EquipRentalRateNo != null &&
                    this.EquipRentalRateNo.Equals(other.EquipRentalRateNo)
                ) &&                 
                (
                    this.EquipRentalRatePage == other.EquipRentalRatePage ||
                    this.EquipRentalRatePage != null &&
                    this.EquipRentalRatePage.Equals(other.EquipRentalRatePage)
                ) &&                 
                (
                    this.MaxHours == other.MaxHours ||
                    this.MaxHours != null &&
                    this.MaxHours.Equals(other.MaxHours)
                ) &&                 
                (
                    this.ExtendHours == other.ExtendHours ||
                    this.ExtendHours != null &&
                    this.ExtendHours.Equals(other.ExtendHours)
                ) &&                 
                (
                    this.MaxHoursSub == other.MaxHoursSub ||
                    this.MaxHoursSub != null &&
                    this.MaxHoursSub.Equals(other.MaxHoursSub)
                ) &&                 
                (
                    this.SecondBlk == other.SecondBlk ||
                    this.SecondBlk != null &&
                    this.SecondBlk.Equals(other.SecondBlk)
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
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                if (this.Code != null)
                {
                    hash = hash * 59 + this.Code.GetHashCode();
                }                
                                if (this.Description != null)
                {
                    hash = hash * 59 + this.Description.GetHashCode();
                }                
                                if (this.EquipRentalRateNo != null)
                {
                    hash = hash * 59 + this.EquipRentalRateNo.GetHashCode();
                }                
                                if (this.EquipRentalRatePage != null)
                {
                    hash = hash * 59 + this.EquipRentalRatePage.GetHashCode();
                }                
                                if (this.MaxHours != null)
                {
                    hash = hash * 59 + this.MaxHours.GetHashCode();
                }                
                                if (this.ExtendHours != null)
                {
                    hash = hash * 59 + this.ExtendHours.GetHashCode();
                }                
                                if (this.MaxHoursSub != null)
                {
                    hash = hash * 59 + this.MaxHoursSub.GetHashCode();
                }                
                                if (this.SecondBlk != null)
                {
                    hash = hash * 59 + this.SecondBlk.GetHashCode();
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
        public static bool operator ==(EquipmentType left, EquipmentType right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentType left, EquipmentType right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
