using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HETSAPI.Models;

namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Rental Request Database Model
    /// </summary>
    [MetaData (Description = "A request from a Project for one or more of a type of equipment from a specific Local Area.")]
    public sealed class RentalRequestViewModel : IEquatable<RentalRequestViewModel>
    {
        /// <summary>
        /// Rental Request Database Model Constructor (required by entity framework)
        /// </summary>
        public RentalRequestViewModel()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequestViewModel" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Request (required).</param>
        /// <param name="project">Project (required).</param>
        /// <param name="localArea">A foreign key reference to the system-generated unique identifier for a Local Area (required).</param>
        /// <param name="status">The status of the Rental Request - whether it in progress, completed or was cancelled. (required).</param>
        /// <param name="districtEquipmentType">A foreign key reference to the system-generated unique identifier for an Equipment Type (required).</param>
        /// <param name="equipmentCount">The number of pieces of the equipment type wanted for hire as part of this request. (required).</param>
        /// <param name="expectedHours">The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="expectedStartDate">The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="expectedEndDate">The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="firstOnRotationList">The first piece of equipment on the rotation list at the time of the creation of the request..</param>
        /// <param name="notes">Notes.</param>
        /// <param name="attachments">Attachments.</param>
        /// <param name="history">History.</param>
        /// <param name="rentalRequestAttachments">RentalRequestAttachments.</param>
        /// <param name="rentalRequestRotationList">RentalRequestRotationList.</param>
        public RentalRequestViewModel(int id, Project project, LocalArea localArea, string status, DistrictEquipmentType districtEquipmentType, 
            int equipmentCount, int? expectedHours = null, DateTime? expectedStartDate = null, DateTime? expectedEndDate = null, 
            Equipment firstOnRotationList = null, List<Note> notes = null, List<Attachment> attachments = null, 
            List<History> history = null, List<RentalRequestAttachment> rentalRequestAttachments = null, 
            List<RentalRequestRotationList> rentalRequestRotationList = null)
        {   
            Id = id;
            Project = project;
            LocalArea = localArea;
            Status = status;
            DistrictEquipmentType = districtEquipmentType;
            EquipmentCount = equipmentCount;
            ExpectedHours = expectedHours;
            ExpectedStartDate = expectedStartDate;
            ExpectedEndDate = expectedEndDate;
            FirstOnRotationList = firstOnRotationList;
            Notes = notes;
            Attachments = attachments;
            History = history;
            RentalRequestAttachments = rentalRequestAttachments;
            RentalRequestRotationList = rentalRequestRotationList;

            // calculate the Yes Count based on the RentalRequestList
            CalculateYesCount();
        }

        /// <summary>
        /// The count of yes responses from Equipment Owners (calculated field)
        /// </summary>
        /// <value>A system-generated unique identifier for a Request</value>
        [MetaData(Description = "The count of yes responses from Equipment Owners")]
        public int YesCount { get; set; }        

        /// <summary>
        /// Check how many Yes' we currently have from Owners
        /// </summary>
        /// <returns></returns>
        public int CalculateYesCount()
        {
            int temp = 0;

            if (RentalRequestRotationList != null)
            {
                foreach (RentalRequestRotationList equipment in RentalRequestRotationList)
                {
                    if (equipment.OfferResponse != null &&
                        equipment.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        temp++;
                    }
                }
            }

            return temp;
        }       

        #region Standard Rental Request Model Properties

        /// <summary>
        /// A system-generated unique identifier for a Request
        /// </summary>
        /// <value>A system-generated unique identifier for a Request</value>
        [MetaData (Description = "A system-generated unique identifier for a Request")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets Project
        /// </summary>
        public Project Project { get; set; }
        
        /// <summary>
        /// Foreign key for Project 
        /// </summary>   
        [ForeignKey("Project")]
		[JsonIgnore]		
        public int? ProjectId { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for a Local Area
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Local Area</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>   
        [ForeignKey("LocalArea")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public int? LocalAreaId { get; set; }
        
        /// <summary>
        /// The status of the Rental Request - whether it in progress, completed or was cancelled.
        /// </summary>
        /// <value>The status of the Rental Request - whether it in progress, completed or was cancelled.</value>
        [MetaData (Description = "The status of the Rental Request - whether it in progress, completed or was cancelled.")]
        [MaxLength(50)]        
        public string Status { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Equipment Type
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Equipment Type</value>
        [MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public DistrictEquipmentType DistrictEquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for DistrictEquipmentType 
        /// </summary>   
        [ForeignKey("DistrictEquipmentType")]
		[JsonIgnore]
		[MetaData (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public int? DistrictEquipmentTypeId { get; set; }
        
        /// <summary>
        /// The number of pieces of the equipment type wanted for hire as part of this request.
        /// </summary>
        /// <value>The number of pieces of the equipment type wanted for hire as part of this request.</value>
        [MetaData (Description = "The number of pieces of the equipment type wanted for hire as part of this request.")]
        public int EquipmentCount { get; set; }
        
        /// <summary>
        /// The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaData (Description = "The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.")]
        public int? ExpectedHours { get; set; }
        
        /// <summary>
        /// The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaData (Description = "The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.")]
        public DateTime? ExpectedStartDate { get; set; }
        
        /// <summary>
        /// The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaData (Description = "The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.")]
        public DateTime? ExpectedEndDate { get; set; }
        
        /// <summary>
        /// The first piece of equipment on the rotation list at the time of the creation of the request.
        /// </summary>
        /// <value>The first piece of equipment on the rotation list at the time of the creation of the request.</value>
        [MetaData (Description = "The first piece of equipment on the rotation list at the time of the creation of the request.")]
        public Equipment FirstOnRotationList { get; set; }
        
        /// <summary>
        /// Foreign key for FirstOnRotationList 
        /// </summary>   
        [ForeignKey("FirstOnRotationList")]
		[JsonIgnore]
		[MetaData (Description = "The first piece of equipment on the rotation list at the time of the creation of the request.")]
        public int? FirstOnRotationListId { get; set; }
        
        /// <summary>
        /// Gets or Sets Notes
        /// </summary>
        public List<Note> Notes { get; set; }
        
        /// <summary>
        /// Gets or Sets Attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }
        
        /// <summary>
        /// Gets or Sets History
        /// </summary>
        public List<History> History { get; set; }
        
        /// <summary>
        /// Gets or Sets RentalRequestAttachments
        /// </summary>
        public List<RentalRequestAttachment> RentalRequestAttachments { get; set; }
        
        /// <summary>
        /// Gets or Sets RentalRequestRotationList
        /// </summary>
        public List<RentalRequestRotationList> RentalRequestRotationList { get; set; }

        #endregion

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("class RentalRequestViewModel {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  DistrictEquipmentType: ").Append(DistrictEquipmentType).Append("\n");
            sb.Append("  EquipmentCount: ").Append(EquipmentCount).Append("\n");
            sb.Append("  ExpectedHours: ").Append(ExpectedHours).Append("\n");
            sb.Append("  ExpectedStartDate: ").Append(ExpectedStartDate).Append("\n");
            sb.Append("  ExpectedEndDate: ").Append(ExpectedEndDate).Append("\n");
            sb.Append("  FirstOnRotationList: ").Append(FirstOnRotationList).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  RentalRequestAttachments: ").Append(RentalRequestAttachments).Append("\n");
            sb.Append("  RentalRequestRotationList: ").Append(RentalRequestRotationList).Append("\n");
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
            return obj.GetType() == GetType() && Equals((RentalRequestViewModel)obj);
        }

        /// <summary>
        /// Returns true if RentalRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequestViewModel other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    Project == other.Project ||
                    Project != null &&
                    Project.Equals(other.Project)
                ) &&                 
                (
                    LocalArea == other.LocalArea ||
                    LocalArea != null &&
                    LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) &&                 
                (
                    DistrictEquipmentType == other.DistrictEquipmentType ||
                    DistrictEquipmentType != null &&
                    DistrictEquipmentType.Equals(other.DistrictEquipmentType)
                ) &&                 
                (
                    EquipmentCount == other.EquipmentCount ||
                    EquipmentCount.Equals(other.EquipmentCount)
                ) &&                 
                (
                    ExpectedHours == other.ExpectedHours ||
                    ExpectedHours != null &&
                    ExpectedHours.Equals(other.ExpectedHours)
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
                ) &&                 
                (
                    FirstOnRotationList == other.FirstOnRotationList ||
                    FirstOnRotationList != null &&
                    FirstOnRotationList.Equals(other.FirstOnRotationList)
                ) && 
                (
                    Notes == other.Notes ||
                    Notes != null &&
                    Notes.SequenceEqual(other.Notes)
                ) && 
                (
                    Attachments == other.Attachments ||
                    Attachments != null &&
                    Attachments.SequenceEqual(other.Attachments)
                ) && 
                (
                    History == other.History ||
                    History != null &&
                    History.SequenceEqual(other.History)
                ) && 
                (
                    RentalRequestAttachments == other.RentalRequestAttachments ||
                    RentalRequestAttachments != null &&
                    RentalRequestAttachments.SequenceEqual(other.RentalRequestAttachments)
                ) && 
                (
                    RentalRequestRotationList == other.RentalRequestRotationList ||
                    RentalRequestRotationList != null &&
                    RentalRequestRotationList.SequenceEqual(other.RentalRequestRotationList)
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
                
                if (Project != null)
                {
                    hash = hash * 59 + Project.GetHashCode();
                }

                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }                
                                   
                if (DistrictEquipmentType != null)
                {
                    hash = hash * 59 + DistrictEquipmentType.GetHashCode();
                }

                hash = hash * 59 + EquipmentCount.GetHashCode();

                if (ExpectedHours != null)
                {
                    hash = hash * 59 + ExpectedHours.GetHashCode();
                }

                if (ExpectedStartDate != null)
                {
                    hash = hash * 59 + ExpectedStartDate.GetHashCode();
                }

                if (ExpectedEndDate != null)
                {
                    hash = hash * 59 + ExpectedEndDate.GetHashCode();
                }                
                                   
                if (FirstOnRotationList != null)
                {
                    hash = hash * 59 + FirstOnRotationList.GetHashCode();
                }

                if (Notes != null)
                {
                    hash = hash * 59 + Notes.GetHashCode();
                }

                if (Attachments != null)
                {
                    hash = hash * 59 + Attachments.GetHashCode();
                }

                if (History != null)
                {
                    hash = hash * 59 + History.GetHashCode();
                }

                if (RentalRequestAttachments != null)
                {
                    hash = hash * 59 + RentalRequestAttachments.GetHashCode();
                }

                if (RentalRequestRotationList != null)
                {
                    hash = hash * 59 + RentalRequestRotationList.GetHashCode();
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
        public static bool operator ==(RentalRequestViewModel left, RentalRequestViewModel right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequestViewModel left, RentalRequestViewModel right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
