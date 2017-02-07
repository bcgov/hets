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
    /// The set of Provincial Construction Projects by Service Area that contact HETS Clerks to request equipment. The same Construction Project may be in the table multiple times - once per affected Service Area. Limited information is maintained on the projects - primarily a name, a collection of contacts and a collections of equipment rentals.
    /// </summary>
        [MetaDataExtension (Description = "The set of Provincial Construction Projects by Service Area that contact HETS Clerks to request equipment. The same Construction Project may be in the table multiple times - once per affected Service Area. Limited information is maintained on the projects - primarily a name, a collection of contacts and a collections of equipment rentals.")]

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
        /// <param name="Id">A system-generated unique identifier for a Project (required).</param>
        /// <param name="ServiceArea">The service Area for which this project has requested hired equipment..</param>
        /// <param name="ProvincialProjectNumber">TO BE VERIFIED - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project..</param>
        /// <param name="Description">A description entered by the HETS Clerk creating the Project record about the project. The description may contain other metadata about the project needed to process the requests related to the project..</param>
        /// <param name="Requests">Requests.</param>
        /// <param name="PrimaryContact">Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment..</param>
        /// <param name="Contacts">Contacts.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        public Project(int Id, ServiceArea ServiceArea = null, string ProvincialProjectNumber = null, string Description = null, List<Request> Requests = null, Contact PrimaryContact = null, List<Contact> Contacts = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null)
        {   
            this.Id = Id;
            this.ServiceArea = ServiceArea;
            this.ProvincialProjectNumber = ProvincialProjectNumber;
            this.Description = Description;
            this.Requests = Requests;
            this.PrimaryContact = PrimaryContact;
            this.Contacts = Contacts;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
        }

        /// <summary>
        /// A system-generated unique identifier for a Project
        /// </summary>
        /// <value>A system-generated unique identifier for a Project</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Project")]
        public int Id { get; set; }
        
        /// <summary>
        /// The service Area for which this project has requested hired equipment.
        /// </summary>
        /// <value>The service Area for which this project has requested hired equipment.</value>
        [MetaDataExtension (Description = "The service Area for which this project has requested hired equipment.")]
        public ServiceArea ServiceArea { get; set; }
        
        /// <summary>
        /// Foreign key for ServiceArea 
        /// </summary>       
        [ForeignKey("ServiceArea")]
        public int? ServiceAreaRefId { get; set; }
        
        /// <summary>
        /// TO BE VERIFIED - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.
        /// </summary>
        /// <value>TO BE VERIFIED - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.</value>
        [MetaDataExtension (Description = "TO BE VERIFIED - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.")]
        [MaxLength(255)]
        
        public string ProvincialProjectNumber { get; set; }
        
        /// <summary>
        /// A description entered by the HETS Clerk creating the Project record about the project. The description may contain other metadata about the project needed to process the requests related to the project.
        /// </summary>
        /// <value>A description entered by the HETS Clerk creating the Project record about the project. The description may contain other metadata about the project needed to process the requests related to the project.</value>
        [MetaDataExtension (Description = "A description entered by the HETS Clerk creating the Project record about the project. The description may contain other metadata about the project needed to process the requests related to the project.")]
        [MaxLength(2048)]
        
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or Sets Requests
        /// </summary>
        public List<Request> Requests { get; set; }
        
        /// <summary>
        /// Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment.
        /// </summary>
        /// <value>Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment.</value>
        [MetaDataExtension (Description = "Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment.")]
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
            sb.Append("  ProvincialProjectNumber: ").Append(ProvincialProjectNumber).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
                    this.ProvincialProjectNumber == other.ProvincialProjectNumber ||
                    this.ProvincialProjectNumber != null &&
                    this.ProvincialProjectNumber.Equals(other.ProvincialProjectNumber)
                ) &&                 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
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
                }                if (this.ProvincialProjectNumber != null)
                {
                    hash = hash * 59 + this.ProvincialProjectNumber.GetHashCode();
                }                
                                if (this.Description != null)
                {
                    hash = hash * 59 + this.Description.GetHashCode();
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
