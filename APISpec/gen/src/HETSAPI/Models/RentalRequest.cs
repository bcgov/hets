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
    /// A request from a Project for one or more of a type of equipment from a specific Local Area.
    /// </summary>
        [MetaDataExtension (Description = "A request from a Project for one or more of a type of equipment from a specific Local Area.")]

    public partial class RentalRequest : AuditableEntity, IEquatable<RentalRequest>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public RentalRequest()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalRequest" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a Request (required).</param>
        /// <param name="Project">Project (required).</param>
        /// <param name="LocalArea">A foreign key reference to the system-generated unique identifier for a Local Area (required).</param>
        /// <param name="Status">The status of the Rental Request - whether it in progress, completed or was cancelled. (required).</param>
        /// <param name="DistrictEquipmentType">A foreign key reference to the system-generated unique identifier for an Equipment Type (required).</param>
        /// <param name="EquipmentCount">The number of pieces of the equipment type wanted for hire as part of this request. (required).</param>
        /// <param name="ExpectedHours">The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="ExpectedStartDate">The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="ExpectedEndDate">The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="FirstOnRotationList">The first piece of equipment on the rotation list at the time of the creation of the request..</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="RentalRequestRotationList">RentalRequestRotationList.</param>
        public RentalRequest(int Id, Project Project, LocalArea LocalArea, string Status, DistrictEquipmentType DistrictEquipmentType, int EquipmentCount, int? ExpectedHours = null, DateTime? ExpectedStartDate = null, DateTime? ExpectedEndDate = null, Equipment FirstOnRotationList = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<RentalRequestRotationList> RentalRequestRotationList = null)
        {   
            this.Id = Id;
            this.Project = Project;
            this.LocalArea = LocalArea;
            this.Status = Status;
            this.DistrictEquipmentType = DistrictEquipmentType;
            this.EquipmentCount = EquipmentCount;





            this.ExpectedHours = ExpectedHours;
            this.ExpectedStartDate = ExpectedStartDate;
            this.ExpectedEndDate = ExpectedEndDate;
            this.FirstOnRotationList = FirstOnRotationList;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.RentalRequestRotationList = RentalRequestRotationList;
        }

        /// <summary>
        /// A system-generated unique identifier for a Request
        /// </summary>
        /// <value>A system-generated unique identifier for a Request</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Request")]
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
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>   
        [ForeignKey("LocalArea")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Local Area")]
        public int? LocalAreaId { get; set; }
        
        /// <summary>
        /// The status of the Rental Request - whether it in progress, completed or was cancelled.
        /// </summary>
        /// <value>The status of the Rental Request - whether it in progress, completed or was cancelled.</value>
        [MetaDataExtension (Description = "The status of the Rental Request - whether it in progress, completed or was cancelled.")]
        [MaxLength(50)]
        
        public string Status { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Equipment Type
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Equipment Type</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public DistrictEquipmentType DistrictEquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for DistrictEquipmentType 
        /// </summary>   
        [ForeignKey("DistrictEquipmentType")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Equipment Type")]
        public int? DistrictEquipmentTypeId { get; set; }
        
        /// <summary>
        /// The number of pieces of the equipment type wanted for hire as part of this request.
        /// </summary>
        /// <value>The number of pieces of the equipment type wanted for hire as part of this request.</value>
        [MetaDataExtension (Description = "The number of pieces of the equipment type wanted for hire as part of this request.")]
        public int EquipmentCount { get; set; }
        
        /// <summary>
        /// The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaDataExtension (Description = "The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request.")]
        public int? ExpectedHours { get; set; }
        
        /// <summary>
        /// The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaDataExtension (Description = "The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request.")]
        public DateTime? ExpectedStartDate { get; set; }
        
        /// <summary>
        /// The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.
        /// </summary>
        /// <value>The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.</value>
        [MetaDataExtension (Description = "The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request.")]
        public DateTime? ExpectedEndDate { get; set; }
        
        /// <summary>
        /// The first piece of equipment on the rotation list at the time of the creation of the request.
        /// </summary>
        /// <value>The first piece of equipment on the rotation list at the time of the creation of the request.</value>
        [MetaDataExtension (Description = "The first piece of equipment on the rotation list at the time of the creation of the request.")]
        public Equipment FirstOnRotationList { get; set; }
        
        /// <summary>
        /// Foreign key for FirstOnRotationList 
        /// </summary>   
        [ForeignKey("FirstOnRotationList")]
		[JsonIgnore]
		[MetaDataExtension (Description = "The first piece of equipment on the rotation list at the time of the creation of the request.")]
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
        /// Gets or Sets RentalRequestRotationList
        /// </summary>
        public List<RentalRequestRotationList> RentalRequestRotationList { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class RentalRequest {\n");
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
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((RentalRequest)obj);
        }

        /// <summary>
        /// Returns true if RentalRequest instances are equal
        /// </summary>
        /// <param name="other">Instance of RentalRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RentalRequest other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.Project == other.Project ||
                    this.Project != null &&
                    this.Project.Equals(other.Project)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) &&                 
                (
                    this.DistrictEquipmentType == other.DistrictEquipmentType ||
                    this.DistrictEquipmentType != null &&
                    this.DistrictEquipmentType.Equals(other.DistrictEquipmentType)
                ) &&                 
                (
                    this.EquipmentCount == other.EquipmentCount ||
                    this.EquipmentCount.Equals(other.EquipmentCount)
                ) &&                 
                (
                    this.ExpectedHours == other.ExpectedHours ||
                    this.ExpectedHours != null &&
                    this.ExpectedHours.Equals(other.ExpectedHours)
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
                ) &&                 
                (
                    this.FirstOnRotationList == other.FirstOnRotationList ||
                    this.FirstOnRotationList != null &&
                    this.FirstOnRotationList.Equals(other.FirstOnRotationList)
                ) && 
                (
                    this.Notes == other.Notes ||
                    this.Notes != null &&
                    this.Notes.SequenceEqual(other.Notes)
                ) && 
                (
                    this.Attachments == other.Attachments ||
                    this.Attachments != null &&
                    this.Attachments.SequenceEqual(other.Attachments)
                ) && 
                (
                    this.History == other.History ||
                    this.History != null &&
                    this.History.SequenceEqual(other.History)
                ) && 
                (
                    this.RentalRequestRotationList == other.RentalRequestRotationList ||
                    this.RentalRequestRotationList != null &&
                    this.RentalRequestRotationList.SequenceEqual(other.RentalRequestRotationList)
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
                if (this.Project != null)
                {
                    hash = hash * 59 + this.Project.GetHashCode();
                }                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
                }                
                                   
                if (this.DistrictEquipmentType != null)
                {
                    hash = hash * 59 + this.DistrictEquipmentType.GetHashCode();
                }                                   
                hash = hash * 59 + this.EquipmentCount.GetHashCode();                if (this.ExpectedHours != null)
                {
                    hash = hash * 59 + this.ExpectedHours.GetHashCode();
                }                
                                if (this.ExpectedStartDate != null)
                {
                    hash = hash * 59 + this.ExpectedStartDate.GetHashCode();
                }                
                                if (this.ExpectedEndDate != null)
                {
                    hash = hash * 59 + this.ExpectedEndDate.GetHashCode();
                }                
                                   
                if (this.FirstOnRotationList != null)
                {
                    hash = hash * 59 + this.FirstOnRotationList.GetHashCode();
                }                   
                if (this.Notes != null)
                {
                    hash = hash * 59 + this.Notes.GetHashCode();
                }                   
                if (this.Attachments != null)
                {
                    hash = hash * 59 + this.Attachments.GetHashCode();
                }                   
                if (this.History != null)
                {
                    hash = hash * 59 + this.History.GetHashCode();
                }                   
                if (this.RentalRequestRotationList != null)
                {
                    hash = hash * 59 + this.RentalRequestRotationList.GetHashCode();
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
        public static bool operator ==(RentalRequest left, RentalRequest right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RentalRequest left, RentalRequest right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
