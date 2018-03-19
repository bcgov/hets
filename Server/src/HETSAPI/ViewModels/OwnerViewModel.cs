using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Owner View Model
    /// </summary>
    [DataContract]
    public sealed class OwnerViewModel : IEquatable<OwnerViewModel>
    {
        /// <summary>
        /// Owner Database Model Constructor (required by entity framework)
        /// </summary>
        public OwnerViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Owner (required).</param>        
        /// <param name="ownerCode">A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &amp;quot;EDW&amp;quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082. (required).</param>
        /// <param name="organizationName">The name of the organization of the Owner</param>
        /// <param name="localAreaName">Local Area thiw owner is associated with</param>
        /// <param name="primaryContactName">The name of the Owners Primary Contacts</param>
        /// <param name="equipmentCount">Count of (non-Archived) Equipment</param>
        /// <param name="status">The status of the Owner</param>        
        public OwnerViewModel(int id, string ownerCode, string organizationName, string localAreaName = null,             
            string primaryContactName = null, int equipmentCount = 0, string status = null)
        {   
            Id = id;
            OwnerCode = ownerCode;
            OrganizationName = organizationName;
            LocalAreaName = localAreaName;
            PrimaryContactName = primaryContactName;
            EquipmentCount = equipmentCount;
            Status = status;                 
        }

        /// <summary>
        /// Function to populate equipment count for this owner
        /// </summary>
        public void CalculateEquipmentCount(List<Equipment> equipmentList)
        {
            foreach (Equipment equipment in equipmentList)
            {
                if (equipment.Status != Equipment.StatusArchived)
                {
                    ++EquipmentCount;
                }
            }
        }

        /// <summary>
        /// A system-generated unique identifier for a Owner
        /// </summary>
        [DataMember(Name = "id")]
        [MetaData (Description = "A system-generated unique identifier for a Owner")]
        public int Id { get; set; }

        /// <summary>
        /// Human-friendly owner ID
        /// </summary>
        [DataMember(Name = "ownerCode")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// The name of the organization of the Owner
        /// </summary>
        [DataMember(Name = "organizationName")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// The name of the Local Area this Owner is associated with
        /// </summary>
        [DataMember(Name = "localAreaName")]
        public string LocalAreaName { get; set; }

        /// <summary>
        /// The name of the Owners Primary Contacts
        /// </summary>
        [DataMember(Name = "primaryContactName")]
        public string PrimaryContactName { get; set; }

        /// <summary>
        /// Count of (non-Archived) Equipment
        /// </summary>
        [DataMember(Name = "equipmentCount")]
        public int EquipmentCount { get; set; }

        /// <summary>
        /// The status of the Owner
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class Owner {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OwnerCode: ").Append(OwnerCode).Append("\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  LocalAreaName: ").Append(LocalAreaName).Append("\n");
            sb.Append("  PrimaryContactName: ").Append(PrimaryContactName).Append("\n");
            sb.Append("  EquipmentCount: ").Append(EquipmentCount).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
            return obj.GetType() == GetType() && Equals((OwnerViewModel)obj);
        }

        /// <summary>
        /// Returns true if Owner instances are equal
        /// </summary>
        /// <param name="other">Instance of Owner to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OwnerViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    OwnerCode == other.OwnerCode ||
                    OwnerCode != null &&
                    OwnerCode.Equals(other.OwnerCode)
                ) &&                 
                (
                    OrganizationName == other.OrganizationName ||
                    OrganizationName != null &&
                    OrganizationName.Equals(other.OrganizationName)
                ) &&
                (
                    LocalAreaName == other.LocalAreaName ||
                    LocalAreaName != null &&
                    LocalAreaName.Equals(other.LocalAreaName)
                ) &&
                (
                    PrimaryContactName == other.PrimaryContactName ||
                    PrimaryContactName != null &&
                    PrimaryContactName.Equals(other.PrimaryContactName)
                ) &&
                (
                    EquipmentCount == other.EquipmentCount ||
                    EquipmentCount.Equals(other.EquipmentCount)
                ) &&
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
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

                if (OwnerCode != null)
                {
                    hash = hash * 59 + OwnerCode.GetHashCode();
                }

                if (OrganizationName != null)
                {
                    hash = hash * 59 + OrganizationName.GetHashCode();
                }

                if (LocalAreaName != null)
                {
                    hash = hash * 59 + LocalAreaName.GetHashCode();
                }

                if (PrimaryContactName != null)
                {
                    hash = hash * 59 + PrimaryContactName.GetHashCode();
                }

                hash = hash * 59 + EquipmentCount.GetHashCode();
                
                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
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
        public static bool operator ==(OwnerViewModel left, OwnerViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(OwnerViewModel left, OwnerViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
