using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HETSAPI.Models
{
    /// <summary>
    /// Project Database Model
    /// </summary>
    [MetaDataExtension (Description = "A Provincial Project that my from time to time request equipment under the HETS programme from a Service Area.")]

    public partial class Project : AuditableEntity, IEquatable<Project>
    {
        /// <summary>
        /// Project Database Model Constructor (required by entity framework)
        /// </summary>
        public Project()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Project" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Project (required).</param>
        /// <param name="district">The District associated with this Project record. (required).</param>
        /// <param name="name">A descriptive name for the Project, useful to the HETS Clerk and Project Manager. (required).</param>
        /// <param name="status">The status of the project to determine if it is listed when creating new requests (required).</param>
        /// <param name="provincialProjectNumber">TO BE REVIEWED WITH THE BUSINESS - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project..</param>
        /// <param name="information">Information about the Project needed by the HETS Clerks. Used for capturing varying (project by project) metadata needed to process requests related to the project..</param>
        /// <param name="rentalRequests">The Rental Requests associated with this Project.</param>
        /// <param name="rentalAgreements">The Rental Agreements associated with this Project.</param>
        /// <param name="primaryContact">Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment..</param>
        /// <param name="contacts">Contacts.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="attachments">Attachments.</param>
        /// <param name="history">History.</param>
        public Project(int id, District district, string name, string status, string provincialProjectNumber = null, 
            string information = null, List<RentalRequest> rentalRequests = null, List<RentalAgreement> rentalAgreements = null, 
            Contact primaryContact = null, List<Contact> contacts = null, List<Note> notes = null, List<Attachment> attachments = null, 
            List<History> history = null)
        {   
            Id = id;
            District = district;
            Name = name;
            Status = status;
            ProvincialProjectNumber = provincialProjectNumber;
            Information = information;
            RentalRequests = rentalRequests;
            RentalAgreements = rentalAgreements;
            PrimaryContact = primaryContact;
            Contacts = contacts;
            Notes = notes;
            Attachments = attachments;
            History = history;
        }

        /// <summary>
        /// A system-generated unique identifier for a Project
        /// </summary>
        /// <value>A system-generated unique identifier for a Project</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Project")]
        public int Id { get; set; }
        
        /// <summary>
        /// The District associated with this Project record.
        /// </summary>
        /// <value>The District associated with this Project record.</value>
        [MetaDataExtension (Description = "The District associated with this Project record.")]
        public District District { get; set; }
        
        /// <summary>
        /// Foreign key for District 
        /// </summary>   
        [ForeignKey("District")]
		[JsonIgnore]
		[MetaDataExtension (Description = "The District associated with this Project record.")]
        public int? DistrictId { get; set; }
        
        /// <summary>
        /// A descriptive name for the Project, useful to the HETS Clerk and Project Manager.
        /// </summary>
        /// <value>A descriptive name for the Project, useful to the HETS Clerk and Project Manager.</value>
        [MetaDataExtension (Description = "A descriptive name for the Project, useful to the HETS Clerk and Project Manager.")]
        [MaxLength(100)]        
        public string Name { get; set; }
        
        /// <summary>
        /// The status of the project to determine if it is listed when creating new requests
        /// </summary>
        /// <value>The status of the project to determine if it is listed when creating new requests</value>
        [MetaDataExtension (Description = "The status of the project to determine if it is listed when creating new requests")]
        [MaxLength(50)]        
        public string Status { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - The Provincial charge code for the equipment hiring related to this project. This will be the same across multiple service areas that provide equipment for the same Project.")]
        [MaxLength(150)]        
        public string ProvincialProjectNumber { get; set; }
        
        /// <summary>
        /// Information about the Project needed by the HETS Clerks. Used for capturing varying (project by project) metadata needed to process requests related to the project.
        /// </summary>
        /// <value>Information about the Project needed by the HETS Clerks. Used for capturing varying (project by project) metadata needed to process requests related to the project.</value>
        [MetaDataExtension (Description = "Information about the Project needed by the HETS Clerks. Used for capturing varying (project by project) metadata needed to process requests related to the project.")]
        [MaxLength(2048)]        
        public string Information { get; set; }
        
        /// <summary>
        /// The Rental Requests associated with this Project
        /// </summary>
        /// <value>The Rental Requests associated with this Project</value>
        [MetaDataExtension (Description = "The Rental Requests associated with this Project")]
        public List<RentalRequest> RentalRequests { get; set; }
        
        /// <summary>
        /// The Rental Agreements associated with this Project
        /// </summary>
        /// <value>The Rental Agreements associated with this Project</value>
        [MetaDataExtension (Description = "The Rental Agreements associated with this Project")]
        public List<RentalAgreement> RentalAgreements { get; set; }
        
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
		[JsonIgnore]
		[MetaDataExtension (Description = "Link to the designated Primary Contact for the Project - usually the Project Manager requesting to hire equipment.")]
        public int? PrimaryContactId { get; set; }
        
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
            sb.Append("  District: ").Append(District).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ProvincialProjectNumber: ").Append(ProvincialProjectNumber).Append("\n");
            sb.Append("  Information: ").Append(Information).Append("\n");
            sb.Append("  RentalRequests: ").Append(RentalRequests).Append("\n");
            sb.Append("  RentalAgreements: ").Append(RentalAgreements).Append("\n");
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
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    District == other.District ||
                    District != null &&
                    District.Equals(other.District)
                ) &&                 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) &&                 
                (
                    Status == other.Status ||
                    Status != null &&
                    Status.Equals(other.Status)
                ) &&                 
                (
                    ProvincialProjectNumber == other.ProvincialProjectNumber ||
                    ProvincialProjectNumber != null &&
                    ProvincialProjectNumber.Equals(other.ProvincialProjectNumber)
                ) &&                 
                (
                    Information == other.Information ||
                    Information != null &&
                    Information.Equals(other.Information)
                ) && 
                (
                    RentalRequests == other.RentalRequests ||
                    RentalRequests != null &&
                    RentalRequests.SequenceEqual(other.RentalRequests)
                ) && 
                (
                    RentalAgreements == other.RentalAgreements ||
                    RentalAgreements != null &&
                    RentalAgreements.SequenceEqual(other.RentalAgreements)
                ) &&                 
                (
                    PrimaryContact == other.PrimaryContact ||
                    PrimaryContact != null &&
                    PrimaryContact.Equals(other.PrimaryContact)
                ) && 
                (
                    Contacts == other.Contacts ||
                    Contacts != null &&
                    Contacts.SequenceEqual(other.Contacts)
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

                if (District != null)
                {
                    hash = hash * 59 + District.GetHashCode();
                }

                if (Name != null)
                {
                    hash = hash * 59 + Name.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }

                if (ProvincialProjectNumber != null)
                {
                    hash = hash * 59 + ProvincialProjectNumber.GetHashCode();
                }

                if (Information != null)
                {
                    hash = hash * 59 + Information.GetHashCode();
                }                
                                   
                if (RentalRequests != null)
                {
                    hash = hash * 59 + RentalRequests.GetHashCode();
                }          
                
                if (RentalAgreements != null)
                {
                    hash = hash * 59 + RentalAgreements.GetHashCode();
                }   
                
                if (PrimaryContact != null)
                {
                    hash = hash * 59 + PrimaryContact.GetHashCode();
                }               
                
                if (Contacts != null)
                {
                    hash = hash * 59 + Contacts.GetHashCode();
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
