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

    public partial class Project : IEquatable<Project>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Project()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="ServiceArea">ServiceArea.</param>
        /// <param name="ProjectNum">ProjectNum.</param>
        /// <param name="JobDesc1">JobDesc1.</param>
        /// <param name="JobDesc2">JobDesc2.</param>
        /// <param name="Requests">Requests.</param>
        /// <param name="PrimaryContact">Link to the designated Primary Contact..</param>
        /// <param name="Contacts">Contacts.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        public Project(int Id, ServiceArea ServiceArea = null, string ProjectNum = null, string JobDesc1 = null, string JobDesc2 = null, List<Request> Requests = null, Contact PrimaryContact = null, List<Contact> Contacts = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null)
        {   
            this.Id = Id;
            this.ServiceArea = ServiceArea;
            this.ProjectNum = ProjectNum;
            this.JobDesc1 = JobDesc1;
            this.JobDesc2 = JobDesc2;
            this.Requests = Requests;
            this.PrimaryContact = PrimaryContact;
            this.Contacts = Contacts;
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
        /// Gets or Sets ServiceArea
        /// </summary>
        public ServiceArea ServiceArea { get; set; }
        
        /// <summary>
        /// Foreign key for ServiceArea 
        /// </summary>       
        [ForeignKey("ServiceArea")]
        public int? ServiceAreaRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets ProjectNum
        /// </summary>
        public string ProjectNum { get; set; }
        
        /// <summary>
        /// Gets or Sets JobDesc1
        /// </summary>
        public string JobDesc1 { get; set; }
        
        /// <summary>
        /// Gets or Sets JobDesc2
        /// </summary>
        public string JobDesc2 { get; set; }
        
        /// <summary>
        /// Gets or Sets Requests
        /// </summary>
        public List<Request> Requests { get; set; }
        
        /// <summary>
        /// Link to the designated Primary Contact.
        /// </summary>
        /// <value>Link to the designated Primary Contact.</value>
        [MetaDataExtension (Description = "Link to the designated Primary Contact.")]
        public Contact PrimaryContact { get; set; }
        
        /// <summary>
        /// Foreign key for PrimaryContact 
        /// </summary>       
        [ForeignKey("PrimaryContact")]
        public int? PrimaryContactRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Contacts
        /// </summary>
        public List<Contact> Contacts { get; set; }
        
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
            sb.Append("class Project {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ServiceArea: ").Append(ServiceArea).Append("\n");
            sb.Append("  ProjectNum: ").Append(ProjectNum).Append("\n");
            sb.Append("  JobDesc1: ").Append(JobDesc1).Append("\n");
            sb.Append("  JobDesc2: ").Append(JobDesc2).Append("\n");
            sb.Append("  Requests: ").Append(Requests).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  Contacts: ").Append(Contacts).Append("\n");
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
            return Equals((Project)obj);
        }

        /// <summary>
        /// Returns true if Project instances are equal
        /// </summary>
        /// <param name="other">Instance of Project to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Project other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.ServiceArea == other.ServiceArea ||
                    this.ServiceArea != null &&
                    this.ServiceArea.Equals(other.ServiceArea)
                ) &&                 
                (
                    this.ProjectNum == other.ProjectNum ||
                    this.ProjectNum != null &&
                    this.ProjectNum.Equals(other.ProjectNum)
                ) &&                 
                (
                    this.JobDesc1 == other.JobDesc1 ||
                    this.JobDesc1 != null &&
                    this.JobDesc1.Equals(other.JobDesc1)
                ) &&                 
                (
                    this.JobDesc2 == other.JobDesc2 ||
                    this.JobDesc2 != null &&
                    this.JobDesc2.Equals(other.JobDesc2)
                ) && 
                (
                    this.Requests == other.Requests ||
                    this.Requests != null &&
                    this.Requests.SequenceEqual(other.Requests)
                ) &&                 
                (
                    this.PrimaryContact == other.PrimaryContact ||
                    this.PrimaryContact != null &&
                    this.PrimaryContact.Equals(other.PrimaryContact)
                ) && 
                (
                    this.Contacts == other.Contacts ||
                    this.Contacts != null &&
                    this.Contacts.SequenceEqual(other.Contacts)
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
                if (this.ServiceArea != null)
                {
                    hash = hash * 59 + this.ServiceArea.GetHashCode();
                }                if (this.ProjectNum != null)
                {
                    hash = hash * 59 + this.ProjectNum.GetHashCode();
                }                
                                if (this.JobDesc1 != null)
                {
                    hash = hash * 59 + this.JobDesc1.GetHashCode();
                }                
                                if (this.JobDesc2 != null)
                {
                    hash = hash * 59 + this.JobDesc2.GetHashCode();
                }                
                                   
                if (this.Requests != null)
                {
                    hash = hash * 59 + this.Requests.GetHashCode();
                }                   
                if (this.PrimaryContact != null)
                {
                    hash = hash * 59 + this.PrimaryContact.GetHashCode();
                }                   
                if (this.Contacts != null)
                {
                    hash = hash * 59 + this.Contacts.GetHashCode();
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
        public static bool operator ==(Project left, Project right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Project left, Project right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
