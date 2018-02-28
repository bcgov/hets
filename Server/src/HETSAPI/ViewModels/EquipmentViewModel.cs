using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Equipment View Model
    /// </summary>
    [DataContract]
    public sealed class EquipmentViewModel : IEquatable<EquipmentViewModel>
    {
        /// <summary>
        /// Equipment View Model Constructor
        /// </summary>
        public EquipmentViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Equipment (required).</param>
        /// <param name="equipmentType">District Equipment Type</param>
        /// <param name="ownerName">Owner Name</param>        
        /// <param name="isHired">Is Hired</param>   
        /// <param name="seniorityString">Seniority String</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="size">Size</param>
        /// <param name="attachmentCount">Attachment Count</param>
        /// <param name="lastVerifiedDate">Last verified Date</param>        
        public EquipmentViewModel(int id, string equipmentType = null, string ownerName = null, bool isHired = false, 
            string seniorityString = null, string make = null, string model = null, string size = null, 
            int attachmentCount = 0, DateTime? lastVerifiedDate = null)
        {   
            Id = id;
            EquipmentType = equipmentType;
            OwnerName = ownerName;
            IsHired = isHired;
            SeniorityString = seniorityString;
            Make = make;
            Model = model;
            Size = size;
            AttachmentCount = attachmentCount;
            LastVerifiedDate = lastVerifiedDate;                       
        }

        /// <summary>
        /// Create/format Seniority String
        /// </summary>
        public string FormatSeniorityString(float seniority = 0F, int blockNumber = 0, int numberOfBlocks = 3)
        {
            // E.g. For equipment with 3 blocks
            // 1 - 133.277
            // 2 - 323.333
            // Open - 21.333
            // The last block is always open

            if (blockNumber < numberOfBlocks)
            {
                return string.Format("{0} - {1:0.###}", blockNumber, seniority);
            }

            return string.Format("Open - {0:0.###}", seniority);
        }

        /// <summary>
        /// Function to create a sortable value for the seniority column
        /// Calculate "seniority sort order" & round the seniority value (3 decimal places)
        /// </summary>
        public int CalculateSenioritySortOrder(int blockNumber = 0, int numberInBlock = 0)
        {
            return (blockNumber * 100) + numberInBlock;
        }

        /// <summary>
        /// Function to determine if thsi piece of equipment is hired
        /// </summary>
        public bool CheckIsHired(List<RentalAgreement> rentalAgreements)
        {
            int? count = rentalAgreements?.Count(x => x.Status == "Active");

            return count > 0;
        }

        /// <summary>
        /// Calculate attachment count
        /// </summary>
        public int CalculateAttachmentCount(List<EquipmentAttachment> attachments)
        {
            return attachments?.Count ?? 0;
        }

        //*******************************************************************
        // Equipment Search Screen - Data Attributes
        //*******************************************************************

        /// <summary>
        /// A system-generated unique identifier for a Equipment
        /// </summary>
        [DataMember(Name = "id")]
        [MetaData(Description = "A system-generated unique identifier for a Equipment")]
        public int Id { get; set; }

        /// <summary>
        /// Equipment Type (from District Equipment Type)
        /// </summary>
        [DataMember(Name = "equipmentType")]
        public string EquipmentType { get; set; }

        /// <summary>
        /// Owner Name (from Owner)
        /// </summary>
        [DataMember(Name = "ownerName")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Identify if this equipment is currenty on an active Rental Agreement
        /// </summary>
        [DataMember(Name = "isHired")]
        public bool IsHired { get; set; }

        /// <summary>
        /// Seniority String
        /// </summary>
        [DataMember(Name = "seniorityString")]
        public string SeniorityString { get; set; }

        /// <summary>
        /// Equipment Make
        /// </summary>
        [DataMember(Name = "make")]
        public string Make { get; set; }

        /// <summary>
        /// Equipment model
        /// </summary>
        [DataMember(Name = "model")]
        public string Model { get; set; }

        /// <summary>
        /// Equipment size
        /// </summary>
        [DataMember(Name = "size")]
        public string Size { get; set; }

        /// <summary>
        /// Attachment Count (Equipment Attachments)
        /// </summary>
        [DataMember(Name = "attachmentCount")]
        public int AttachmentCount { get; set; }

        /// <summary>
        /// Equipment last verified date
        /// </summary>
        [DataMember(Name = "lastVerifiedDate")]
        public DateTime? LastVerifiedDate { get; set; }

        //*******************************************************************
        // Equipment Search Screen - Additional Attributes
        //*******************************************************************

        /// <summary>
        /// Used to sort the Equipment by Seniority in the UI
        /// </summary>
        [DataMember(Name = "senioritySortOrder")]
        public int SenioritySortOrder { get; set; }

        //*******************************************************************
        // Standard HETS Model Methods
        //*******************************************************************

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();            

            sb.Append("class EquipmentViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  OwnerName: ").Append(OwnerName).Append("\n");
            sb.Append("  IsHired: ").Append(IsHired).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  AttachmentCount: ").Append(AttachmentCount).Append("\n");
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  SenioritySortOrder: ").Append(SenioritySortOrder).Append("\n");
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
            return obj.GetType() == GetType() && Equals((EquipmentViewModel)obj);
        }

        /// <summary>
        /// Returns true if EquipmentViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of EquipmentViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EquipmentViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    EquipmentType == other.EquipmentType ||
                    EquipmentType != null &&
                    EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    OwnerName == other.OwnerName ||
                    OwnerName != null &&
                    OwnerName.Equals(other.OwnerName)
                ) &&                 
                (
                    IsHired == other.IsHired ||
                    IsHired.Equals(other.IsHired)
                ) &&                 
                (
                    SeniorityString == other.SeniorityString ||
                    SeniorityString != null &&
                    SeniorityString.Equals(other.SeniorityString)
                ) &&
                (
                    Make == other.Make ||
                    Make != null &&
                    Make.Equals(other.Make)
                ) &&
                (
                    Model == other.Model ||
                    Model != null &&
                    Model.Equals(other.Model)
                ) &&
                (
                    Size == other.Size ||
                    Size != null &&
                    Size.Equals(other.Size)
                ) &&
                (
                    AttachmentCount == other.AttachmentCount ||
                    AttachmentCount.Equals(other.AttachmentCount)
                ) &&
                (
                    LastVerifiedDate == other.LastVerifiedDate ||
                    LastVerifiedDate != null &&
                    LastVerifiedDate.Equals(other.LastVerifiedDate)
                ) &&                 
                (
                    SenioritySortOrder == other.SenioritySortOrder ||
                    SenioritySortOrder.Equals(other.SenioritySortOrder)
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

                /*
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  SenioritySortOrder: ").Append(SenioritySortOrder).Append("\n");
                 */

                // Suitable nullity checks                                   
                hash = hash * 59 + Id.GetHashCode();   
                
                if (EquipmentType != null)
                {
                    hash = hash * 59 + EquipmentType.GetHashCode();
                }

                if (OwnerName != null)
                {
                    hash = hash * 59 + OwnerName.GetHashCode();
                }

                hash = hash * 59 + IsHired.GetHashCode();

                if (Make != null)
                {
                    hash = hash * 59 + Make.GetHashCode();
                }

                if (Model != null)
                {
                    hash = hash * 59 + Model.GetHashCode();
                }

                if (Size != null)
                {
                    hash = hash * 59 + Size.GetHashCode();
                }

                hash = hash * 59 + AttachmentCount.GetHashCode();
                
                if (LastVerifiedDate != null)
                {
                    hash = hash * 59 + LastVerifiedDate.GetHashCode();
                }

                hash = hash * 59 + SenioritySortOrder.GetHashCode();                                          
                
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
        public static bool operator ==(EquipmentViewModel left, EquipmentViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(EquipmentViewModel left, EquipmentViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
