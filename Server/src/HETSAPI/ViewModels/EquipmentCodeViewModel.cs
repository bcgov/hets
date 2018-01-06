using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Equipment Code View Model
    /// </summary>
    [MetaData (Description = "Equipment Code is a combination of the Owner Equipment Prefix and the numeric identifier for the next piece of equipment.")]
    [DataContract]
    public sealed class EquipmentCodeViewModel : IEquatable<EquipmentCodeViewModel>
    {
        /// <summary>
        /// Equipment Code View Model Constructor
        /// </summary>
        public EquipmentCodeViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentCodeViewModel" /> class.
        /// </summary>
        /// <param name="equipmentCode">The Equipment Code.</param>
        public EquipmentCodeViewModel(string equipmentCode)
        {
            EquipmentCode = equipmentCode;
        }

        /// <summary>
        /// The Equipment Code
        /// </summary>
        /// <value>The Equipment Code</value>
        [DataMember(Name="equipmentCode")]
        [MetaData (Description = "The Equipment Code")]
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
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((EquipmentCodeViewModel)obj);
        }

        /// <summary>
        /// Returns true if EquipmentCodeViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentCodeViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentCodeViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    EquipmentCode == other.EquipmentCode ||
                    EquipmentCode != null &&
                    EquipmentCode.Equals(other.EquipmentCode)
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
                if (EquipmentCode != null)
                {
                    hash = hash * 59 + EquipmentCode.GetHashCode();
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
