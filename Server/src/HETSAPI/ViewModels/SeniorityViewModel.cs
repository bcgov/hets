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
    /// Seniority View Model
    /// </summary>
    [DataContract]
    public sealed class SeniorityViewModel : IEquatable<SeniorityViewModel>
    {
        /// <summary>
        /// Seniority View Model Constructor
        /// </summary>
        public SeniorityViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeniorityViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Equipment (required).</param>
        /// <param name="equipmentType">District Equipment Type</param>
        /// <param name="ownerName">Owner Name</param>        
        /// <param name="ownerId">Owner Id</param>        
        /// <param name="seniorityString">Seniority String (including block)</param>
        /// <param name="seniority">Seniority</param>
        /// <param name="make">Make</param>
        /// <param name="model">Model</param>
        /// <param name="size">Size</param>
        /// <param name="equipmentCode">Equipment Code</param>
        /// <param name="lastCalled">Last called (Y or blank)</param> 
        /// <param name="yearsRegistered">Years Registered</param>       
        /// <param name="ytdHours">YTD Hours</param>        
        /// <param name="hoursYearMinus1">Hours for Year Minus 1</param>        
        /// <param name="hoursYearMinus2">Hours for Year Minus 2</param>    
        /// <param name="hoursYearMinus3">Hours for Year Minus 3</param>         
        public SeniorityViewModel(int id, string equipmentType = null, string ownerName = null, 
            int? ownerId = null, string seniorityString = null, string seniority = null,
            string make = null, string model = null, string size = null, string equipmentCode = null,
            string lastCalled = null, string yearsRegistered = null, string ytdHours = null,
            string hoursYearMinus1 = null, string hoursYearMinus2 = null, string hoursYearMinus3 = null)
        {   
            Id = id;
            EquipmentType = equipmentType;
            OwnerName = ownerName;
            OwnerId = ownerId;
            SeniorityString = seniorityString;
            Seniority = seniority;
            Make = make;
            Model = model;
            Size = size;
            EquipmentCode = equipmentCode;
            LastCalled = lastCalled;
            YearsRegistered = yearsRegistered;
            YtdHours = ytdHours;
            HoursYearMinus1 = hoursYearMinus1;
            HoursYearMinus2 = hoursYearMinus2;
            HoursYearMinus3 = hoursYearMinus3;
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
        // Seniority List Pdf - Data Attributes
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
        /// Owner Id (from Owner)
        /// </summary>
        [DataMember(Name = "ownerId")]
        public int? OwnerId { get; set; }        

        /// <summary>
        /// Seniority String
        /// </summary>
        [DataMember(Name = "seniorityString")]
        public string SeniorityString { get; set; }

        /// <summary>
        /// Seniority
        /// </summary>
        [DataMember(Name = "seniority")]
        public string Seniority { get; set; }

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
        /// Equipment code
        /// </summary>
        [DataMember(Name = "equipmentCode")]
        public string EquipmentCode { get; set; }

        /// <summary>
        /// Last called (Y or blank)
        /// </summary>
        [DataMember(Name = "lastCalled")]
        public string LastCalled { get; set; }

        /// <summary>
        /// Years Registered
        /// </summary>
        [DataMember(Name = "yearsRegistered")]
        public string YearsRegistered { get; set; }

        /// <summary>
        /// Year to Date Hours
        /// </summary>
        [DataMember(Name = "ytdHours")]
        public string YtdHours { get; set; }

        /// <summary>
        /// Hours from Year minus 1
        /// </summary>
        [DataMember(Name = "hoursYearMinus1")]
        public string HoursYearMinus1 { get; set; }

        /// <summary>
        /// Hours from Year minus 2
        /// </summary>
        [DataMember(Name = "hoursYearMinus2")]
        public string HoursYearMinus2 { get; set; }

        /// <summary>
        /// Hours from Year minus 3
        /// </summary>
        [DataMember(Name = "hoursYearMinus3")]
        public string HoursYearMinus3 { get; set; }  

        //*******************************************************************
        // Additional Attributes
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

            sb.Append("class SeniorityViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  OwnerName: ").Append(OwnerName).Append("\n");
            sb.Append("  OwnerId: ").Append(OwnerId).Append("\n");
            sb.Append("  SeniorityString: ").Append(SeniorityString).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  EquipmentCode: ").Append(EquipmentCode).Append("\n");
            sb.Append("  LastCalled: ").Append(LastCalled).Append("\n");
            sb.Append("  YearsRegistered: ").Append(YearsRegistered).Append("\n");
            sb.Append("  YtdHours: ").Append(YtdHours).Append("\n");
            sb.Append("  HoursYearMinus1: ").Append(HoursYearMinus1).Append("\n");
            sb.Append("  HoursYearMinus2: ").Append(HoursYearMinus2).Append("\n");
            sb.Append("  HoursYearMinus3: ").Append(HoursYearMinus3).Append("\n");
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
            return obj.GetType() == GetType() && Equals((SeniorityViewModel)obj);
        }

        /// <summary>
        /// Returns true if SeniorityViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of SeniorityViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SeniorityViewModel other)
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
                    OwnerId == other.OwnerId ||
                    OwnerId != null &&
                    OwnerId.Equals(other.OwnerId)
                ) &&                
                (
                    SeniorityString == other.SeniorityString ||
                    SeniorityString != null &&
                    SeniorityString.Equals(other.SeniorityString)
                ) &&
                (
                    Seniority == other.Seniority ||
                    Seniority != null &&
                    Seniority.Equals(other.Seniority)
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
                    EquipmentCode == other.EquipmentCode ||
                    EquipmentCode != null &&
                    EquipmentCode.Equals(other.EquipmentCode)
                ) &&
                (
                    LastCalled == other.LastCalled ||
                    LastCalled != null &&
                    LastCalled.Equals(other.LastCalled)
                ) &&
                (
                    YearsRegistered == other.YearsRegistered ||
                    YearsRegistered != null &&
                    YearsRegistered.Equals(other.YearsRegistered)
                ) &&
                (
                    YtdHours == other.YtdHours ||
                    YtdHours != null &&
                    YtdHours.Equals(other.YtdHours)
                ) &&
                (
                    HoursYearMinus1 == other.HoursYearMinus1 ||
                    HoursYearMinus1 != null &&
                    HoursYearMinus1.Equals(other.HoursYearMinus1)
                ) &&
                (
                    HoursYearMinus2 == other.HoursYearMinus2 ||
                    HoursYearMinus2 != null &&
                    HoursYearMinus2.Equals(other.HoursYearMinus2)
                ) &&
                (
                    HoursYearMinus3 == other.HoursYearMinus3 ||
                    HoursYearMinus3 != null &&
                    HoursYearMinus3.Equals(other.HoursYearMinus3)
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

                if (OwnerId != null)
                {
                    hash = hash * 59 + OwnerId.GetHashCode();
                }

                if (SeniorityString != null)
                {
                    hash = hash * 59 + SeniorityString.GetHashCode();
                }

                if (Seniority != null)
                {
                    hash = hash * 59 + Seniority.GetHashCode();
                }

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

                if (LastCalled != null)
                {
                    hash = hash * 59 + LastCalled.GetHashCode();
                }

                if (YearsRegistered != null)
                {
                    hash = hash * 59 + YearsRegistered.GetHashCode();
                }

                if (YtdHours != null)
                {
                    hash = hash * 59 + YtdHours.GetHashCode();
                }

                if (HoursYearMinus1 != null)
                {
                    hash = hash * 59 + HoursYearMinus1.GetHashCode();
                }

                if (HoursYearMinus2 != null)
                {
                    hash = hash * 59 + HoursYearMinus2.GetHashCode();
                }

                if (HoursYearMinus3 != null)
                {
                    hash = hash * 59 + HoursYearMinus3.GetHashCode();
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
        public static bool operator ==(SeniorityViewModel left, SeniorityViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(SeniorityViewModel left, SeniorityViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
