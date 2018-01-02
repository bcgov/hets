using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class RentalRequestSearchResultViewModel : IEquatable<RentalRequestSearchResultViewModel>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalRequestSearchResultViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestSearchResultViewModel" /> class.
        /// </summary>
        /// <param name="Id">Id (required).</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentCount">EquipmentCount.</param>
        /// <param name="EquipmentTypeName">EquipmentTypeName.</param>
        /// <param name="ProjectName">ProjectName.</param>
        /// <param name="PrimaryContact">PrimaryContact.</param>
        /// <param name="Status">Project status.</param>
        /// <param name="ProjectId">ProjectId.</param>
        /// <param name="ExpectedStartDate">ExpectedStartDate.</param>
        /// <param name="ExpectedEndDate">ExpectedEndDate.</param>
        public RentalRequestSearchResultViewModel(int Id, LocalArea LocalArea = null, int? EquipmentCount = null, string EquipmentTypeName = null, string ProjectName = null, Contact PrimaryContact = null, string Status = null, int? ProjectId = null, DateTime? ExpectedStartDate = null, DateTime? ExpectedEndDate = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.EquipmentCount = EquipmentCount;
            this.EquipmentTypeName = EquipmentTypeName;
            this.ProjectName = ProjectName;
            this.PrimaryContact = PrimaryContact;
            this.Status = Status;
            this.ProjectId = ProjectId;
            this.ExpectedStartDate = ExpectedStartDate;
            this.ExpectedEndDate = ExpectedEndDate;
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
        [MetaDataExtension (Description = "Project status")]
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((RentalRequestSearchResultViewModel)obj);
        }

        /// <summary>
        /// Returns true if RentalRequestSearchResultViewModel instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequestSearchResultViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestSearchResultViewModel other)
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
                    this.EquipmentCount == other.EquipmentCount ||
                    this.EquipmentCount != null &&
                    this.EquipmentCount.Equals(other.EquipmentCount)
                ) &&                 
                (
                    this.EquipmentTypeName == other.EquipmentTypeName ||
                    this.EquipmentTypeName != null &&
                    this.EquipmentTypeName.Equals(other.EquipmentTypeName)
                ) &&                 
                (
                    this.ProjectName == other.ProjectName ||
                    this.ProjectName != null &&
                    this.ProjectName.Equals(other.ProjectName)
                ) &&                 
                (
                    this.PrimaryContact == other.PrimaryContact ||
                    this.PrimaryContact != null &&
                    this.PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) &&                 
                (
                    this.ProjectId == other.ProjectId ||
                    this.ProjectId != null &&
                    this.ProjectId.Equals(other.ProjectId)
                ) &&                 
                (
                    this.ExpectedStartDate == other.ExpectedStartDate ||
                    this.ExpectedStartDate != null &&
                    this.ExpectedStartDate.Equals(other.ExpectedStartDate)
                ) &&                 
                (
                    this.ExpectedEndDate == other.ExpectedEndDate ||
                    this.ExpectedEndDate != null &&
                    this.ExpectedEndDate.Equals(other.ExpectedEndDate)
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
                }                if (this.EquipmentCount != null)
                {
                    hash = hash * 59 + this.EquipmentCount.GetHashCode();
                }                
                                if (this.EquipmentTypeName != null)
                {
                    hash = hash * 59 + this.EquipmentTypeName.GetHashCode();
                }                
                                if (this.ProjectName != null)
                {
                    hash = hash * 59 + this.ProjectName.GetHashCode();
                }                
                                   
                if (this.PrimaryContact != null)
                {
                    hash = hash * 59 + this.PrimaryContact.GetHashCode();
                }                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
                }                
                                if (this.ProjectId != null)
                {
                    hash = hash * 59 + this.ProjectId.GetHashCode();
                }                
                                if (this.ExpectedStartDate != null)
                {
                    hash = hash * 59 + this.ExpectedStartDate.GetHashCode();
                }                
                                if (this.ExpectedEndDate != null)
                {
                    hash = hash * 59 + this.ExpectedEndDate.GetHashCode();
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
