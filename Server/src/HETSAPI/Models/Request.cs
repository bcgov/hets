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

namespace HETSAPI.Models
{
    /// <summary>
    /// 
    /// </summary>

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
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="Project">Project.</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentType">EquipmentType.</param>
        /// <param name="EquipmentCount">EquipmentCount.</param>
        /// <param name="ExpectedHours">ExpectedHours.</param>
        /// <param name="ExpectedStartDate">ExpectedStartDate.</param>
        /// <param name="ExpectedEndDate">ExpectedEndDate.</param>
        /// <param name="RotationList">RotationList.</param>
        /// <param name="HireOffers">HireOffers.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        public Request(int Id, Project Project = null, LocalArea LocalArea = null, EquipmentType EquipmentType = null, int? EquipmentCount = null, int? ExpectedHours = null, DateTime? ExpectedStartDate = null, DateTime? ExpectedEndDate = null, RotationList RotationList = null, List<HireOffer> HireOffers = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null)
        {   
            this.Id = Id;
            this.Project = Project;
            this.LocalArea = LocalArea;
            this.EquipmentType = EquipmentType;
            this.EquipmentCount = EquipmentCount;
            this.ExpectedHours = ExpectedHours;
            this.ExpectedStartDate = ExpectedStartDate;
            this.ExpectedEndDate = ExpectedEndDate;
            this.RotationList = RotationList;
            this.HireOffers = HireOffers;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
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
        /// Gets or Sets EquipmentCount
        /// </summary>
        public int? EquipmentCount { get; set; }
        
        /// <summary>
        /// Gets or Sets ExpectedHours
        /// </summary>
        public int? ExpectedHours { get; set; }
        
        /// <summary>
        /// Gets or Sets ExpectedStartDate
        /// </summary>
        public DateTime? ExpectedStartDate { get; set; }
        
        /// <summary>
        /// Gets or Sets ExpectedEndDate
        /// </summary>
        public DateTime? ExpectedEndDate { get; set; }
        
        /// <summary>
        /// Gets or Sets RotationList
        /// </summary>
        public RotationList RotationList { get; set; }
        
        /// <summary>
        /// Foreign key for RotationList 
        /// </summary>       
        [ForeignKey("RotationList")]
        public int? RotationListRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets HireOffers
        /// </summary>
        public List<HireOffer> HireOffers { get; set; }
        
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
            sb.Append("  RotationList: ").Append(RotationList).Append("\n");
            sb.Append("  HireOffers: ").Append(HireOffers).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
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
                    this.RotationList == other.RotationList ||
                    this.RotationList != null &&
                    this.RotationList.Equals(other.RotationList)
                ) && 
                (
                    this.HireOffers == other.HireOffers ||
                    this.HireOffers != null &&
                    this.HireOffers.SequenceEqual(other.HireOffers)
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
                                   
                if (this.RotationList != null)
                {
                    hash = hash * 59 + this.RotationList.GetHashCode();
                }                   
                if (this.HireOffers != null)
                {
                    hash = hash * 59 + this.HireOffers.GetHashCode();
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
