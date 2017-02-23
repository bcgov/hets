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
    /// A global and local area list of types of equipment for which a request can be generated and for which owners can register their equipment. If the local area is NULL, the type is visible Province-wide. In the previous instance of this application, all equipment types were assigned to the local area and transitioning to a province-wide model is being attempted (and may not work for the end users). An attempt to achieve a &amp;quot;mostly province-wide&amp;quot; list is being made during the data conversion process - mapping local area equipment types to provincial or local area types as part of data conversion.
    /// </summary>
        [MetaDataExtension (Description = "A global and local area list of types of equipment for which a request can be generated and for which owners can register their equipment. If the local area is NULL, the type is visible Province-wide. In the previous instance of this application, all equipment types were assigned to the local area and transitioning to a province-wide model is being attempted (and may not work for the end users). An attempt to achieve a &amp;quot;mostly province-wide&amp;quot; list is being made during the data conversion process - mapping local area equipment types to provincial or local area types as part of data conversion.")]

    public partial class EquipmentType : AuditableEntity, IEquatable<EquipmentType>
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
        /// <param name="Id">A system-generated unique identifier for an EquipmentType (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="Name">A human-friendly name for the Equipment Type that is displayed on screens with limited screen real estate available..</param>
        /// <param name="Description">A extended description of the equipment type..</param>
        /// <param name="EquipRentalRateNo">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="EquipRentalRatePage">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="MaxHours">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="ExtendHours">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="MaxHoursSub">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="Blocks">The number of blocks defined for this equipment type. Currently, either 2 for Dump Truck equipment and 1 for non-Dump Truck equipment. Unlikely to change, particularly without a significant impact on code..</param>
        public EquipmentType(int Id, LocalArea LocalArea = null, string Name = null, string Description = null, float? EquipRentalRateNo = null, float? EquipRentalRatePage = null, float? MaxHours = null, float? ExtendHours = null, float? MaxHoursSub = null, int? Blocks = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.Name = Name;
            this.Description = Description;
            this.EquipRentalRateNo = EquipRentalRateNo;
            this.EquipRentalRatePage = EquipRentalRatePage;
            this.MaxHours = MaxHours;
            this.ExtendHours = ExtendHours;
            this.MaxHoursSub = MaxHoursSub;
            this.Blocks = Blocks;
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentType
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentType</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for an EquipmentType")]
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
        /// A human-friendly name for the Equipment Type that is displayed on screens with limited screen real estate available.
        /// </summary>
        /// <value>A human-friendly name for the Equipment Type that is displayed on screens with limited screen real estate available.</value>
        [MetaDataExtension (Description = "A human-friendly name for the Equipment Type that is displayed on screens with limited screen real estate available.")]
        [MaxLength(50)]
        
        public string Name { get; set; }
        
        /// <summary>
        /// A extended description of the equipment type.
        /// </summary>
        /// <value>A extended description of the equipment type.</value>
        [MetaDataExtension (Description = "A extended description of the equipment type.")]
        [MaxLength(2048)]
        
        public string Description { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? EquipRentalRateNo { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? EquipRentalRatePage { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? MaxHours { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? ExtendHours { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? MaxHoursSub { get; set; }
        
        /// <summary>
        /// The number of blocks defined for this equipment type. Currently, either 2 for Dump Truck equipment and 1 for non-Dump Truck equipment. Unlikely to change, particularly without a significant impact on code.
        /// </summary>
        /// <value>The number of blocks defined for this equipment type. Currently, either 2 for Dump Truck equipment and 1 for non-Dump Truck equipment. Unlikely to change, particularly without a significant impact on code.</value>
        [MetaDataExtension (Description = "The number of blocks defined for this equipment type. Currently, either 2 for Dump Truck equipment and 1 for non-Dump Truck equipment. Unlikely to change, particularly without a significant impact on code.")]
        public int? Blocks { get; set; }
        
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
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  EquipRentalRateNo: ").Append(EquipRentalRateNo).Append("\n");
            sb.Append("  EquipRentalRatePage: ").Append(EquipRentalRatePage).Append("\n");
            sb.Append("  MaxHours: ").Append(MaxHours).Append("\n");
            sb.Append("  ExtendHours: ").Append(ExtendHours).Append("\n");
            sb.Append("  MaxHoursSub: ").Append(MaxHoursSub).Append("\n");
            sb.Append("  Blocks: ").Append(Blocks).Append("\n");
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
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
                    this.Blocks == other.Blocks ||
                    this.Blocks != null &&
                    this.Blocks.Equals(other.Blocks)
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
                }                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
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
                                if (this.Blocks != null)
                {
                    hash = hash * 59 + this.Blocks.GetHashCode();
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
