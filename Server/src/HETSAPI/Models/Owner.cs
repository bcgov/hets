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
    /// 
    /// </summary>

    public partial class Owner : IEquatable<Owner>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Owner()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Owner" /> class.
        /// </summary>
        /// <param name="Id">Primary Key (required).</param>
        /// <param name="OwnerCd">OwnerCd.</param>
        /// <param name="OwnerFirstName">OwnerFirstName.</param>
        /// <param name="OwnerLastName">OwnerLastName.</param>
        /// <param name="ContactPerson">ContactPerson.</param>
        /// <param name="LocalToArea">LocalToArea.</param>
        /// <param name="MaintenanceContractor">MaintenanceContractor.</param>
        /// <param name="Comment">Comment.</param>
        /// <param name="WCBNum">WCBNum.</param>
        /// <param name="WCBExpiryDate">WCBExpiryDate.</param>
        /// <param name="CGLCompany">CGLCompany.</param>
        /// <param name="CGLPolicy">CGLPolicy.</param>
        /// <param name="CGLStartDate">CGLStartDate.</param>
        /// <param name="CGLEndDate">CGLEndDate.</param>
        /// <param name="StatusCd">StatusCd.</param>
        /// <param name="ArchiveCd">ArchiveCd.</param>
        /// <param name="ArchiveReason">ArchiveReason.</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="PrimaryContact">Link to the designated Primary Contact..</param>
        /// <param name="Contacts">Contacts.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="EquipmentList">EquipmentList.</param>
        public Owner(int Id, string OwnerCd = null, string OwnerFirstName = null, string OwnerLastName = null, string ContactPerson = null, string LocalToArea = null, string MaintenanceContractor = null, string Comment = null, int? WCBNum = null, DateTime? WCBExpiryDate = null, string CGLCompany = null, string CGLPolicy = null, DateTime? CGLStartDate = null, DateTime? CGLEndDate = null, string StatusCd = null, string ArchiveCd = null, string ArchiveReason = null, LocalArea LocalArea = null, Contact PrimaryContact = null, List<Contact> Contacts = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<Equipment> EquipmentList = null)
        {   
            this.Id = Id;
            this.OwnerCd = OwnerCd;
            this.OwnerFirstName = OwnerFirstName;
            this.OwnerLastName = OwnerLastName;
            this.ContactPerson = ContactPerson;
            this.LocalToArea = LocalToArea;
            this.MaintenanceContractor = MaintenanceContractor;
            this.Comment = Comment;
            this.WCBNum = WCBNum;
            this.WCBExpiryDate = WCBExpiryDate;
            this.CGLCompany = CGLCompany;
            this.CGLPolicy = CGLPolicy;
            this.CGLStartDate = CGLStartDate;
            this.CGLEndDate = CGLEndDate;
            this.StatusCd = StatusCd;
            this.ArchiveCd = ArchiveCd;
            this.ArchiveReason = ArchiveReason;
            this.LocalArea = LocalArea;
            this.PrimaryContact = PrimaryContact;
            this.Contacts = Contacts;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.EquipmentList = EquipmentList;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        /// <value>Primary Key</value>
        [MetaDataExtension (Description = "Primary Key")]
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or Sets OwnerCd
        /// </summary>
        [MaxLength(255)]
        
        public string OwnerCd { get; set; }
        
        /// <summary>
        /// Gets or Sets OwnerFirstName
        /// </summary>
        [MaxLength(255)]
        
        public string OwnerFirstName { get; set; }
        
        /// <summary>
        /// Gets or Sets OwnerLastName
        /// </summary>
        [MaxLength(255)]
        
        public string OwnerLastName { get; set; }
        
        /// <summary>
        /// Gets or Sets ContactPerson
        /// </summary>
        [MaxLength(255)]
        
        public string ContactPerson { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalToArea
        /// </summary>
        [MaxLength(255)]
        
        public string LocalToArea { get; set; }
        
        /// <summary>
        /// Gets or Sets MaintenanceContractor
        /// </summary>
        [MaxLength(255)]
        
        public string MaintenanceContractor { get; set; }
        
        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [MaxLength(255)]
        
        public string Comment { get; set; }
        
        /// <summary>
        /// Gets or Sets WCBNum
        /// </summary>
        public int? WCBNum { get; set; }
        
        /// <summary>
        /// Gets or Sets WCBExpiryDate
        /// </summary>
        public DateTime? WCBExpiryDate { get; set; }
        
        /// <summary>
        /// Gets or Sets CGLCompany
        /// </summary>
        [MaxLength(255)]
        
        public string CGLCompany { get; set; }
        
        /// <summary>
        /// Gets or Sets CGLPolicy
        /// </summary>
        [MaxLength(255)]
        
        public string CGLPolicy { get; set; }
        
        /// <summary>
        /// Gets or Sets CGLStartDate
        /// </summary>
        public DateTime? CGLStartDate { get; set; }
        
        /// <summary>
        /// Gets or Sets CGLEndDate
        /// </summary>
        public DateTime? CGLEndDate { get; set; }
        
        /// <summary>
        /// Gets or Sets StatusCd
        /// </summary>
        [MaxLength(255)]
        
        public string StatusCd { get; set; }
        
        /// <summary>
        /// Gets or Sets ArchiveCd
        /// </summary>
        [MaxLength(255)]
        
        public string ArchiveCd { get; set; }
        
        /// <summary>
        /// Gets or Sets ArchiveReason
        /// </summary>
        [MaxLength(255)]
        
        public string ArchiveReason { get; set; }
        
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
        /// Gets or Sets EquipmentList
        /// </summary>
        public List<Equipment> EquipmentList { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Owner {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OwnerCd: ").Append(OwnerCd).Append("\n");
            sb.Append("  OwnerFirstName: ").Append(OwnerFirstName).Append("\n");
            sb.Append("  OwnerLastName: ").Append(OwnerLastName).Append("\n");
            sb.Append("  ContactPerson: ").Append(ContactPerson).Append("\n");
            sb.Append("  LocalToArea: ").Append(LocalToArea).Append("\n");
            sb.Append("  MaintenanceContractor: ").Append(MaintenanceContractor).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  WCBNum: ").Append(WCBNum).Append("\n");
            sb.Append("  WCBExpiryDate: ").Append(WCBExpiryDate).Append("\n");
            sb.Append("  CGLCompany: ").Append(CGLCompany).Append("\n");
            sb.Append("  CGLPolicy: ").Append(CGLPolicy).Append("\n");
            sb.Append("  CGLStartDate: ").Append(CGLStartDate).Append("\n");
            sb.Append("  CGLEndDate: ").Append(CGLEndDate).Append("\n");
            sb.Append("  StatusCd: ").Append(StatusCd).Append("\n");
            sb.Append("  ArchiveCd: ").Append(ArchiveCd).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  Contacts: ").Append(Contacts).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  EquipmentList: ").Append(EquipmentList).Append("\n");
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
            return Equals((Owner)obj);
        }

        /// <summary>
        /// Returns true if Owner instances are equal
        /// </summary>
        /// <param name="other">Instance of Owner to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Owner other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.OwnerCd == other.OwnerCd ||
                    this.OwnerCd != null &&
                    this.OwnerCd.Equals(other.OwnerCd)
                ) &&                 
                (
                    this.OwnerFirstName == other.OwnerFirstName ||
                    this.OwnerFirstName != null &&
                    this.OwnerFirstName.Equals(other.OwnerFirstName)
                ) &&                 
                (
                    this.OwnerLastName == other.OwnerLastName ||
                    this.OwnerLastName != null &&
                    this.OwnerLastName.Equals(other.OwnerLastName)
                ) &&                 
                (
                    this.ContactPerson == other.ContactPerson ||
                    this.ContactPerson != null &&
                    this.ContactPerson.Equals(other.ContactPerson)
                ) &&                 
                (
                    this.LocalToArea == other.LocalToArea ||
                    this.LocalToArea != null &&
                    this.LocalToArea.Equals(other.LocalToArea)
                ) &&                 
                (
                    this.MaintenanceContractor == other.MaintenanceContractor ||
                    this.MaintenanceContractor != null &&
                    this.MaintenanceContractor.Equals(other.MaintenanceContractor)
                ) &&                 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) &&                 
                (
                    this.WCBNum == other.WCBNum ||
                    this.WCBNum != null &&
                    this.WCBNum.Equals(other.WCBNum)
                ) &&                 
                (
                    this.WCBExpiryDate == other.WCBExpiryDate ||
                    this.WCBExpiryDate != null &&
                    this.WCBExpiryDate.Equals(other.WCBExpiryDate)
                ) &&                 
                (
                    this.CGLCompany == other.CGLCompany ||
                    this.CGLCompany != null &&
                    this.CGLCompany.Equals(other.CGLCompany)
                ) &&                 
                (
                    this.CGLPolicy == other.CGLPolicy ||
                    this.CGLPolicy != null &&
                    this.CGLPolicy.Equals(other.CGLPolicy)
                ) &&                 
                (
                    this.CGLStartDate == other.CGLStartDate ||
                    this.CGLStartDate != null &&
                    this.CGLStartDate.Equals(other.CGLStartDate)
                ) &&                 
                (
                    this.CGLEndDate == other.CGLEndDate ||
                    this.CGLEndDate != null &&
                    this.CGLEndDate.Equals(other.CGLEndDate)
                ) &&                 
                (
                    this.StatusCd == other.StatusCd ||
                    this.StatusCd != null &&
                    this.StatusCd.Equals(other.StatusCd)
                ) &&                 
                (
                    this.ArchiveCd == other.ArchiveCd ||
                    this.ArchiveCd != null &&
                    this.ArchiveCd.Equals(other.ArchiveCd)
                ) &&                 
                (
                    this.ArchiveReason == other.ArchiveReason ||
                    this.ArchiveReason != null &&
                    this.ArchiveReason.Equals(other.ArchiveReason)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
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
                ) && 
                (
                    this.EquipmentList == other.EquipmentList ||
                    this.EquipmentList != null &&
                    this.EquipmentList.SequenceEqual(other.EquipmentList)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.OwnerCd != null)
                {
                    hash = hash * 59 + this.OwnerCd.GetHashCode();
                }                
                                if (this.OwnerFirstName != null)
                {
                    hash = hash * 59 + this.OwnerFirstName.GetHashCode();
                }                
                                if (this.OwnerLastName != null)
                {
                    hash = hash * 59 + this.OwnerLastName.GetHashCode();
                }                
                                if (this.ContactPerson != null)
                {
                    hash = hash * 59 + this.ContactPerson.GetHashCode();
                }                
                                if (this.LocalToArea != null)
                {
                    hash = hash * 59 + this.LocalToArea.GetHashCode();
                }                
                                if (this.MaintenanceContractor != null)
                {
                    hash = hash * 59 + this.MaintenanceContractor.GetHashCode();
                }                
                                if (this.Comment != null)
                {
                    hash = hash * 59 + this.Comment.GetHashCode();
                }                
                                if (this.WCBNum != null)
                {
                    hash = hash * 59 + this.WCBNum.GetHashCode();
                }                
                                if (this.WCBExpiryDate != null)
                {
                    hash = hash * 59 + this.WCBExpiryDate.GetHashCode();
                }                
                                if (this.CGLCompany != null)
                {
                    hash = hash * 59 + this.CGLCompany.GetHashCode();
                }                
                                if (this.CGLPolicy != null)
                {
                    hash = hash * 59 + this.CGLPolicy.GetHashCode();
                }                
                                if (this.CGLStartDate != null)
                {
                    hash = hash * 59 + this.CGLStartDate.GetHashCode();
                }                
                                if (this.CGLEndDate != null)
                {
                    hash = hash * 59 + this.CGLEndDate.GetHashCode();
                }                
                                if (this.StatusCd != null)
                {
                    hash = hash * 59 + this.StatusCd.GetHashCode();
                }                
                                if (this.ArchiveCd != null)
                {
                    hash = hash * 59 + this.ArchiveCd.GetHashCode();
                }                
                                if (this.ArchiveReason != null)
                {
                    hash = hash * 59 + this.ArchiveReason.GetHashCode();
                }                
                                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
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
                if (this.EquipmentList != null)
                {
                    hash = hash * 59 + this.EquipmentList.GetHashCode();
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
        public static bool operator ==(Owner left, Owner right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Owner left, Owner right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
