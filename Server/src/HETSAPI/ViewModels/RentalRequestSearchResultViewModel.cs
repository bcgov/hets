using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Rental Request Search Result View Model
    /// </summary>
    [DataContract]
    public sealed class RentalRequestSearchResultViewModel : IEquatable<RentalRequestSearchResultViewModel>
    {
        /// <summary>
        /// Rental Request Search Result View Model Constructor
        /// </summary>
        public RentalRequestSearchResultViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestSearchResultViewModel" /> class.
        /// </summary>
        /// <param name="id">Id (required).</param>
        /// <param name="localArea">LocalArea.</param>
        /// <param name="equipmentCount">EquipmentCount.</param>
        /// <param name="equipmentTypeName">EquipmentTypeName.</param>
        /// <param name="projectName">ProjectName.</param>
        /// <param name="primaryContact">PrimaryContact.</param>
        /// <param name="status">Project status.</param>
        /// <param name="projectId">ProjectId.</param>
        /// <param name="expectedStartDate">ExpectedStartDate.</param>
        /// <param name="expectedEndDate">ExpectedEndDate.</param>
        public RentalRequestSearchResultViewModel(int id, LocalArea localArea = null, int? equipmentCount = null, 
            string equipmentTypeName = null, string projectName = null, Contact primaryContact = null, string status = null, 
            int? projectId = null, DateTime? expectedStartDate = null, DateTime? expectedEndDate = null)
        {   
            Id = id;
            LocalArea = localArea;
            EquipmentCount = equipmentCount;
            EquipmentTypeName = equipmentTypeName;
            ProjectName = projectName;
            PrimaryContact = primaryContact;
            Status = status;
            ProjectId = projectId;
            ExpectedStartDate = expectedStartDate;
            ExpectedEndDate = expectedEndDate;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        [DataMember(Name="localArea")]
        public LocalArea LocalArea { get; set; }

        /// <summary>
        /// Gets or Sets EquipmentCount
        /// </summary>
        [DataMember(Name="equipmentCount")]
        public int? EquipmentCount { get; set; }

        /// <summary>
        /// Gets or Sets EquipmentTypeName
        /// </summary>
        [DataMember(Name="equipmentTypeName")]
        public string EquipmentTypeName { get; set; }

        /// <summary>
        /// Gets or Sets ProjectName
        /// </summary>
        [DataMember(Name="projectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or Sets PrimaryContact
        /// </summary>
        [DataMember(Name="primaryContact")]
        public Contact PrimaryContact { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        /// <value>Project status</value>
        [DataMember(Name="status")]
        [MetaData (Description = "Project status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets ProjectId
        /// </summary>
        [DataMember(Name="projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or Sets ExpectedStartDate
        /// </summary>
        [DataMember(Name="expectedStartDate")]
        public DateTime? ExpectedStartDate { get; set; }

        /// <summary>
        /// Gets or Sets ExpectedEndDate
        /// </summary>
        [DataMember(Name="expectedEndDate")]
        public DateTime? ExpectedEndDate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RentalRequestSearchResultViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  EquipmentCount: ").Append(EquipmentCount).Append("\n");
            sb.Append("  EquipmentTypeName: ").Append(EquipmentTypeName).Append("\n");
            sb.Append("  ProjectName: ").Append(ProjectName).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ProjectId: ").Append(ProjectId).Append("\n");
            sb.Append("  ExpectedStartDate: ").Append(ExpectedStartDate).Append("\n");
            sb.Append("  ExpectedEndDate: ").Append(ExpectedEndDate).Append("\n");
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
            return obj.GetType() == GetType() && Equals((RentalRequestSearchResultViewModel)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestSearchResultViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestSearchResultViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestSearchResultViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    LocalArea == other.LocalArea ||
                    LocalArea != null &&
                    LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    EquipmentCount == other.EquipmentCount ||
                    EquipmentCount != null &&
                    EquipmentCount.Equals(other.EquipmentCount)
                ) &&                 
                (
                    EquipmentTypeName == other.EquipmentTypeName ||
                    EquipmentTypeName != null &&
                    EquipmentTypeName.Equals(other.EquipmentTypeName)
                ) &&                 
                (
                    ProjectName == other.ProjectName ||
                    ProjectName != null &&
                    ProjectName.Equals(other.ProjectName)
                ) &&                 
                (
                    PrimaryContact == other.PrimaryContact ||
                    PrimaryContact != null &&
                    PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) &&                 
                (
                    ProjectId == other.ProjectId ||
                    ProjectId != null &&
                    ProjectId.Equals(other.ProjectId)
                ) &&                 
                (
                    ExpectedStartDate == other.ExpectedStartDate ||
                    ExpectedStartDate != null &&
                    ExpectedStartDate.Equals(other.ExpectedStartDate)
                ) &&                 
                (
                    ExpectedEndDate == other.ExpectedEndDate ||
                    ExpectedEndDate != null &&
                    ExpectedEndDate.Equals(other.ExpectedEndDate)
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

                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (EquipmentCount != null)
                {
                    hash = hash * 59 + EquipmentCount.GetHashCode();
                }

                if (EquipmentTypeName != null)
                {
                    hash = hash * 59 + EquipmentTypeName.GetHashCode();
                }

                if (ProjectName != null)
                {
                    hash = hash * 59 + ProjectName.GetHashCode();
                }                
                                   
                if (PrimaryContact != null)
                {
                    hash = hash * 59 + PrimaryContact.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }

                if (ProjectId != null)
                {
                    hash = hash * 59 + ProjectId.GetHashCode();
                }

                if (ExpectedStartDate != null)
                {
                    hash = hash * 59 + ExpectedStartDate.GetHashCode();
                }

                if (ExpectedEndDate != null)
                {
                    hash = hash * 59 + ExpectedEndDate.GetHashCode();
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
        public static bool operator ==(RentalRequestSearchResultViewModel left, RentalRequestSearchResultViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestSearchResultViewModel left, RentalRequestSearchResultViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
