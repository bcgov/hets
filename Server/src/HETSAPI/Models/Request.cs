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

namespace HETSAPI.Models
{
    /// <summary>
    /// A request from a Project for one or more of a type of equipment from a specific Local Area.
    /// </summary>
        [MetaDataExtension (Description = "A request from a Project for one or more of a type of equipment from a specific Local Area.")]

    public partial class Request : IEquatable<Request>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Request()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Request" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a Request (required).</param>
        /// <param name="Project">Project.</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentType">EquipmentType.</param>
        /// <param name="EquipmentCount">The number of pieces of the equipment type wanted for hire as part of this request..</param>
        /// <param name="ExpectedHours">The expected number of rental hours for each piece equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="ExpectedStartDate">The expected start date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="ExpectedEndDate">The expected end date of each piece of equipment hired against this request, as provided by the Project Manager making the request..</param>
        /// <param name="FirstOnRotationList">The first piece of equipment on the rotation list at the time of the creation of the request..</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="RequestRotationList">RequestRotationList.</param>
        public Request(int Id, Project Project = null, LocalArea LocalArea = null, EquipmentType EquipmentType = null, int? EquipmentCount = null, int? ExpectedHours = null, DateTime? ExpectedStartDate = null, DateTime? ExpectedEndDate = null, Equipment FirstOnRotationList = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<RequestRotationList> RequestRotationList = null)
        {   
            this.Id = Id;
            this.Project = Project;
            this.LocalArea = LocalArea;
            this.EquipmentType = EquipmentType;
            this.EquipmentCount = EquipmentCount;
            this.ExpectedHours = ExpectedHours;
            this.ExpectedStartDate = ExpectedStartDate;
            this.ExpectedEndDate = ExpectedEndDate;
            this.FirstOnRotationList = FirstOnRotationList;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.RequestRotationList = RequestRotationList;
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
        public int? ProjectRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>       
        [ForeignKey("LocalArea")]
        public int? LocalAreaRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentType
        /// </summary>
        public EquipmentType EquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for EquipmentType 
        /// </summary>       
        [ForeignKey("EquipmentType")]
        public int? EquipmentTypeRefId { get; set; }
        
        /// <summary>
        /// The number of pieces of the equipment type wanted for hire as part of this request.
        /// </summary>
        /// <value>The number of pieces of the equipment type wanted for hire as part of this request.</value>
        [MetaDataExtension (Description = "The number of pieces of the equipment type wanted for hire as part of this request.")]
        public int? EquipmentCount { get; set; }
        
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
        public int? FirstOnRotationListRefId { get; set; }
        
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
        /// Gets or Sets RequestRotationList
        /// </summary>
        public List<RequestRotationList> RequestRotationList { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Request {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Project: ").Append(Project).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  EquipmentCount: ").Append(EquipmentCount).Append("\n");
            sb.Append("  ExpectedHours: ").Append(ExpectedHours).Append("\n");
            sb.Append("  ExpectedStartDate: ").Append(ExpectedStartDate).Append("\n");
            sb.Append("  ExpectedEndDate: ").Append(ExpectedEndDate).Append("\n");
            sb.Append("  FirstOnRotationList: ").Append(FirstOnRotationList).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  RequestRotationList: ").Append(RequestRotationList).Append("\n");
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
            return Equals((Request)obj);
        }

        /// <summary>
        /// Returns true if Request instances are equal
        /// </summary>
        /// <param name="other">Instance of Request to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Request other)
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
                    this.EquipmentType == other.EquipmentType ||
                    this.EquipmentType != null &&
                    this.EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    this.EquipmentCount == other.EquipmentCount ||
                    this.EquipmentCount != null &&
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
                    this.RequestRotationList == other.RequestRotationList ||
                    this.RequestRotationList != null &&
                    this.RequestRotationList.SequenceEqual(other.RequestRotationList)
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
                }                   
                if (this.EquipmentType != null)
                {
                    hash = hash * 59 + this.EquipmentType.GetHashCode();
                }                if (this.EquipmentCount != null)
                {
                    hash = hash * 59 + this.EquipmentCount.GetHashCode();
                }                
                                if (this.ExpectedHours != null)
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
                if (this.RequestRotationList != null)
                {
                    hash = hash * 59 + this.RequestRotationList.GetHashCode();
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
        public static bool operator ==(Request left, Request right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Request left, Request right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
