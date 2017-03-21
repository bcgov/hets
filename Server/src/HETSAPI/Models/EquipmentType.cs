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
    /// A provincial-wide Equipment Type, the related Blue Book Chapter Section and related usage attributes.
    /// </summary>
        [MetaDataExtension (Description = "A provincial-wide Equipment Type, the related Blue Book Chapter Section and related usage attributes.")]

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
        /// <param name="Id">A system-generated unique identifier for an EquipmentName.</param>
        /// <param name="Name">The generic name of an equipment type - e.g. Dump Truck, Excavator and so on..</param>
        /// <param name="IsDumpTruck">True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes..</param>
        /// <param name="BlueBookSection">The section of the Blue Book that is related to equipment types of this name..</param>
        /// <param name="BlueBookRateNumber">The rate number in the Blue Book that is related to equipment types of this name..</param>
        /// <param name="NumberOfBlocks">The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks..</param>
        /// <param name="MaximumHours">The maximum number of hours per year that equipment types of this name&amp;#x2F;Blue Book section can work in a year.</param>
        /// <param name="ExtendHours">The number of extended hours per year that equipment types of this name&amp;#x2F;Blue Book section can work..</param>
        /// <param name="MaxHoursSub">The number of substitute hours per year that equipment types of this name&amp;#x2F;Blue Book section can work..</param>
        public EquipmentType(int? Id = null, string Name = null, bool? IsDumpTruck = null, float? BlueBookSection = null, float? BlueBookRateNumber = null, int? NumberOfBlocks = null, float? MaximumHours = null, float? ExtendHours = null, float? MaxHoursSub = null)
        {               this.Id = Id;
            this.Name = Name;
            this.IsDumpTruck = IsDumpTruck;
            this.BlueBookSection = BlueBookSection;
            this.BlueBookRateNumber = BlueBookRateNumber;
            this.NumberOfBlocks = NumberOfBlocks;
            this.MaximumHours = MaximumHours;
            this.ExtendHours = ExtendHours;
            this.MaxHoursSub = MaxHoursSub;
        }

        /// <summary>
        /// A system-generated unique identifier for an EquipmentName
        /// </summary>
        /// <value>A system-generated unique identifier for an EquipmentName</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for an EquipmentName")]
        public int? Id { get; set; }
        
        /// <summary>
        /// The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.
        /// </summary>
        /// <value>The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.</value>
        [MetaDataExtension (Description = "The generic name of an equipment type - e.g. Dump Truck, Excavator and so on.")]
        [MaxLength(50)]
        
        public string Name { get; set; }
        
        /// <summary>
        /// True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.
        /// </summary>
        /// <value>True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.</value>
        [MetaDataExtension (Description = "True if the Equipment Type is a Dump Truck. Equipment of this type will have a related Dump Truck record containing dump truck-related attributes.")]
        public bool? IsDumpTruck { get; set; }
        
        /// <summary>
        /// The section of the Blue Book that is related to equipment types of this name.
        /// </summary>
        /// <value>The section of the Blue Book that is related to equipment types of this name.</value>
        [MetaDataExtension (Description = "The section of the Blue Book that is related to equipment types of this name.")]
        public float? BlueBookSection { get; set; }
        
        /// <summary>
        /// The rate number in the Blue Book that is related to equipment types of this name.
        /// </summary>
        /// <value>The rate number in the Blue Book that is related to equipment types of this name.</value>
        [MetaDataExtension (Description = "The rate number in the Blue Book that is related to equipment types of this name.")]
        public float? BlueBookRateNumber { get; set; }
        
        /// <summary>
        /// The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.
        /// </summary>
        /// <value>The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.</value>
        [MetaDataExtension (Description = "The number of blocks defined for the equipment of this name and Blue Book section. In general Dump Truck-class equipment types have 3 blocks, while non-Dump Truck equipment types have 2 blocks.")]
        public int? NumberOfBlocks { get; set; }
        
        /// <summary>
        /// The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year
        /// </summary>
        /// <value>The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year</value>
        [MetaDataExtension (Description = "The maximum number of hours per year that equipment types of this name&#x2F;Blue Book section can work in a year")]
        public float? MaximumHours { get; set; }
        
        /// <summary>
        /// The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.
        /// </summary>
        /// <value>The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.</value>
        [MetaDataExtension (Description = "The number of extended hours per year that equipment types of this name&#x2F;Blue Book section can work.")]
        public float? ExtendHours { get; set; }
        
        /// <summary>
        /// The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.
        /// </summary>
        /// <value>The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.</value>
        [MetaDataExtension (Description = "The number of substitute hours per year that equipment types of this name&#x2F;Blue Book section can work.")]
        public float? MaxHoursSub { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentType {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsDumpTruck: ").Append(IsDumpTruck).Append("\n");
            sb.Append("  BlueBookSection: ").Append(BlueBookSection).Append("\n");
            sb.Append("  BlueBookRateNumber: ").Append(BlueBookRateNumber).Append("\n");
            sb.Append("  NumberOfBlocks: ").Append(NumberOfBlocks).Append("\n");
            sb.Append("  MaximumHours: ").Append(MaximumHours).Append("\n");
            sb.Append("  ExtendHours: ").Append(ExtendHours).Append("\n");
            sb.Append("  MaxHoursSub: ").Append(MaxHoursSub).Append("\n");
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
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) &&                 
                (
                    this.IsDumpTruck == other.IsDumpTruck ||
                    this.IsDumpTruck != null &&
                    this.IsDumpTruck.Equals(other.IsDumpTruck)
                ) &&                 
                (
                    this.BlueBookSection == other.BlueBookSection ||
                    this.BlueBookSection != null &&
                    this.BlueBookSection.Equals(other.BlueBookSection)
                ) &&                 
                (
                    this.BlueBookRateNumber == other.BlueBookRateNumber ||
                    this.BlueBookRateNumber != null &&
                    this.BlueBookRateNumber.Equals(other.BlueBookRateNumber)
                ) &&                 
                (
                    this.NumberOfBlocks == other.NumberOfBlocks ||
                    this.NumberOfBlocks != null &&
                    this.NumberOfBlocks.Equals(other.NumberOfBlocks)
                ) &&                 
                (
                    this.MaximumHours == other.MaximumHours ||
                    this.MaximumHours != null &&
                    this.MaximumHours.Equals(other.MaximumHours)
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
                if (this.Id != null)
                {
                    hash = hash * 59 + this.Id.GetHashCode();
                }                
                                if (this.Name != null)
                {
                    hash = hash * 59 + this.Name.GetHashCode();
                }                
                                if (this.IsDumpTruck != null)
                {
                    hash = hash * 59 + this.IsDumpTruck.GetHashCode();
                }                
                                if (this.BlueBookSection != null)
                {
                    hash = hash * 59 + this.BlueBookSection.GetHashCode();
                }                
                                if (this.BlueBookRateNumber != null)
                {
                    hash = hash * 59 + this.BlueBookRateNumber.GetHashCode();
                }                
                                if (this.NumberOfBlocks != null)
                {
                    hash = hash * 59 + this.NumberOfBlocks.GetHashCode();
                }                
                                if (this.MaximumHours != null)
                {
                    hash = hash * 59 + this.MaximumHours.GetHashCode();
                }                
                                if (this.ExtendHours != null)
                {
                    hash = hash * 59 + this.ExtendHours.GetHashCode();
                }                
                                if (this.MaxHoursSub != null)
                {
                    hash = hash * 59 + this.MaxHoursSub.GetHashCode();
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
