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

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Equipment Code is a combination of the Owner Equipment Prefix and the numeric identifier for the next piece of equipment.
    /// </summary>
        [MetaDataExtension (Description = "Equipment Code is a combination of the Owner Equipment Prefix and the numeric identifier for the next piece of equipment.")]
    [DataContract]
    public partial class EquipmentCodeViewModel : IEquatable<EquipmentCodeViewModel>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public EquipmentCodeViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentCodeViewModel" /> class.
        /// </summary>
        /// <param name="EquipmentCode">The Equipment Code.</param>
        public EquipmentCodeViewModel(string EquipmentCode = null)
        {               this.EquipmentCode = EquipmentCode;
        }

        /// <summary>
        /// The Equipment Code
        /// </summary>
        /// <value>The Equipment Code</value>
        [DataMember(Name="equipmentCode")]
        [MetaDataExtension (Description = "The Equipment Code")]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EquipmentCodeViewModel {\n");
            sb.Append("  EquipmentCode: ").Append(EquipmentCode).Append("\n");
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
            return Equals((EquipmentCodeViewModel)obj);
        }

        /// <summary>
        /// Returns true if EquipmentCodeViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentCodeViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentCodeViewModel other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.EquipmentCode == other.EquipmentCode ||
                    this.EquipmentCode != null &&
                    this.EquipmentCode.Equals(other.EquipmentCode)
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
                if (this.EquipmentCode != null)
                {
                    hash = hash * 59 + this.EquipmentCode.GetHashCode();
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
        public static bool operator ==(EquipmentCodeViewModel left, EquipmentCodeViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentCodeViewModel left, EquipmentCodeViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
