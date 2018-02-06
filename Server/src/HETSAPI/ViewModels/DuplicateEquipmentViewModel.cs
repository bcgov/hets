using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Duplicate Equipment View Model
    /// </summary>
    [MetaData (Description = "A piece of duplicate equipment in the HETS system. Active equipment with the same Serial Number.")]
    [DataContract]
    public sealed class DuplicateEquipmentViewModel : IEquatable<DuplicateEquipmentViewModel>
    {
        /// <summary>
        /// Equipment View Model Constructor
        /// </summary>
        public DuplicateEquipmentViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEquipmentViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Duplicate Equipment.</param>
        /// <param name="districtName">Name of the District.</param>
        /// <param name="serialNumber">The serial number of the piece of equipment as provided by the Equipment Owner..</param>
        /// <param name="duplicteEquipment">Duplicate Equipment.</param>
        public DuplicateEquipmentViewModel(int id, string districtName = null, string serialNumber = null, 
            Equipment duplicteEquipment = null)
        {   
            Id = id;
            DistrictName = districtName;
            SerialNumber = serialNumber;
            DuplicateEquipment = duplicteEquipment;
        }

        /// <summary>
        /// A system-generated unique identifier for a Equipment
        /// </summary>
        /// <value>A system-generated unique identifier for a Equipment</value>
        [DataMember(Name = "id")]
        [MetaData(Description = "A system-generated unique identifier for a Equipment")]
        public int Id { get; set; }

        /// <summary>
        /// Used to sort the Equipment by Seniority in the UI
        /// </summary>
        [DataMember(Name = "districtName")]
        public string DistrictName { get; set; }

        /// <summary>
        /// The serial number of the piece of equipment.
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; set; }        

        /// <summary>
        /// Duplicate Equipment.
        /// </summary>
        [DataMember(Name = "duplicteEquipment")]
        public Equipment DuplicateEquipment { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class DuplicateEquipmentViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DistrictName: ").Append(DistrictName).Append("\n");
            sb.Append("  SerialNumber: ").Append(SerialNumber).Append("\n");
            sb.Append("  DuplicateEquipment: ").Append(DuplicateEquipment).Append("\n");                    
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
            return obj.GetType() == GetType() && Equals((DuplicateEquipmentViewModel)obj);
        }

        /// <summary>
        /// Returns true if DuplicateEquipmentViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of DuplicateEquipmentViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(DuplicateEquipmentViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    DistrictName == other.DistrictName ||
                    DistrictName != null &&
                    DistrictName.Equals(other.DistrictName)
                ) &&                 
                (
                    SerialNumber == other.SerialNumber ||
                    SerialNumber != null &&
                    SerialNumber.Equals(other.SerialNumber)
                ) &&                 
                (
                    DuplicateEquipment == other.DuplicateEquipment ||
                    DuplicateEquipment != null &&
                    DuplicateEquipment.Equals(other.DuplicateEquipment)
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
                hash = hash * 59 + Id.GetHashCode();   
                
                if (DistrictName != null)
                {
                    hash = hash * 59 + DistrictName.GetHashCode();
                }

                if (SerialNumber != null)
                {
                    hash = hash * 59 + SerialNumber.GetHashCode();
                }

                if (DuplicateEquipment != null)
                {
                    hash = hash * 59 + DuplicateEquipment.GetHashCode();
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
        public static bool operator ==(DuplicateEquipmentViewModel left, DuplicateEquipmentViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DuplicateEquipmentViewModel left, DuplicateEquipmentViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
