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
    /// The person or company to which a piece of construction equipment belongs.
    /// </summary>
        [MetaDataExtension (Description = "The person or company to which a piece of construction equipment belongs.")]

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
        /// <param name="Id">A system-generated unique identifier for a Owner (required).</param>
        /// <param name="OwnerEquipmentCodePrefix">A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &amp;quot;EDW&amp;quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082. (required).</param>
        /// <param name="OrganizationName">The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company..</param>
        /// <param name="PrimaryContact">Link to the designated Primary Contact..</param>
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="IsMaintenanceContractor">True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area..</param>
        /// <param name="WCBExpiryDate">The expiration of the owner&amp;#39;s current WCB permit..</param>
        /// <param name="CGLEndDate">The end date of the owner&amp;#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&amp;#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&amp;#x2F;faxed document..</param>
        /// <param name="Status">The status of the owner record in the system. Current set of values are &amp;quot;Pending&amp;quot;, &amp;quot;Approved&amp;quot; and &amp;quot;Archived&amp;quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &amp;quot;Approved&amp;quot; is used in all other cases..</param>
        /// <param name="ArchiveCode">CHECK WITH BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived..</param>
        /// <param name="ArchiveReason">A text note about why the owner record has been changed to Archived..</param>
        /// <param name="ArchiveDate">The date the Owner record was changed to Archived and removed from active use in the system..</param>
        /// <param name="Contacts">Contacts.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="EquipmentList">EquipmentList.</param>
        public Owner(int Id, string OwnerEquipmentCodePrefix, string OrganizationName = null, Contact PrimaryContact = null, LocalArea LocalArea = null, bool? IsMaintenanceContractor = null, DateTime? WCBExpiryDate = null, DateTime? CGLEndDate = null, string Status = null, string ArchiveCode = null, string ArchiveReason = null, DateTime? ArchiveDate = null, List<Contact> Contacts = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<Equipment> EquipmentList = null)
        {   
            this.Id = Id;
            this.OwnerEquipmentCodePrefix = OwnerEquipmentCodePrefix;

            this.OrganizationName = OrganizationName;
            this.PrimaryContact = PrimaryContact;
            this.LocalArea = LocalArea;
            this.IsMaintenanceContractor = IsMaintenanceContractor;
            this.WCBExpiryDate = WCBExpiryDate;
            this.CGLEndDate = CGLEndDate;
            this.Status = Status;
            this.ArchiveCode = ArchiveCode;
            this.ArchiveReason = ArchiveReason;
            this.ArchiveDate = ArchiveDate;
            this.Contacts = Contacts;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.EquipmentList = EquipmentList;
        }

        /// <summary>
        /// A system-generated unique identifier for a Owner
        /// </summary>
        /// <value>A system-generated unique identifier for a Owner</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Owner")]
        public int Id { get; set; }
        
        /// <summary>
        /// A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.
        /// </summary>
        /// <value>A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.</value>
        [MetaDataExtension (Description = "A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.")]
        [MaxLength(20)]
        
        public string OwnerEquipmentCodePrefix { get; set; }
        
        /// <summary>
        /// The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.
        /// </summary>
        /// <value>The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.</value>
        [MetaDataExtension (Description = "The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.")]
        [MaxLength(150)]
        
        public string OrganizationName { get; set; }
        
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
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>       
        [ForeignKey("LocalArea")]
        public int? LocalAreaRefId { get; set; }
        
        /// <summary>
        /// True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.
        /// </summary>
        /// <value>True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.</value>
        [MetaDataExtension (Description = "True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.")]
        public bool? IsMaintenanceContractor { get; set; }
        
        /// <summary>
        /// The expiration of the owner&#39;s current WCB permit.
        /// </summary>
        /// <value>The expiration of the owner&#39;s current WCB permit.</value>
        [MetaDataExtension (Description = "The expiration of the owner&#39;s current WCB permit.")]
        public DateTime? WCBExpiryDate { get; set; }
        
        /// <summary>
        /// The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.
        /// </summary>
        /// <value>The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.</value>
        [MetaDataExtension (Description = "The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.")]
        public DateTime? CGLEndDate { get; set; }
        
        /// <summary>
        /// The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.
        /// </summary>
        /// <value>The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.</value>
        [MetaDataExtension (Description = "The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.")]
        [MaxLength(50)]
        
        public string Status { get; set; }
        
        /// <summary>
        /// CHECK WITH BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.
        /// </summary>
        /// <value>CHECK WITH BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.</value>
        [MetaDataExtension (Description = "CHECK WITH BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.")]
        [MaxLength(50)]
        
        public string ArchiveCode { get; set; }
        
        /// <summary>
        /// A text note about why the owner record has been changed to Archived.
        /// </summary>
        /// <value>A text note about why the owner record has been changed to Archived.</value>
        [MetaDataExtension (Description = "A text note about why the owner record has been changed to Archived.")]
        [MaxLength(2048)]
        
        public string ArchiveReason { get; set; }
        
        /// <summary>
        /// The date the Owner record was changed to Archived and removed from active use in the system.
        /// </summary>
        /// <value>The date the Owner record was changed to Archived and removed from active use in the system.</value>
        [MetaDataExtension (Description = "The date the Owner record was changed to Archived and removed from active use in the system.")]
        public DateTime? ArchiveDate { get; set; }
        
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
            sb.Append("  OwnerEquipmentCodePrefix: ").Append(OwnerEquipmentCodePrefix).Append("\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  IsMaintenanceContractor: ").Append(IsMaintenanceContractor).Append("\n");
            sb.Append("  WCBExpiryDate: ").Append(WCBExpiryDate).Append("\n");
            sb.Append("  CGLEndDate: ").Append(CGLEndDate).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ArchiveCode: ").Append(ArchiveCode).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  ArchiveDate: ").Append(ArchiveDate).Append("\n");
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
                    this.OwnerEquipmentCodePrefix == other.OwnerEquipmentCodePrefix ||
                    this.OwnerEquipmentCodePrefix != null &&
                    this.OwnerEquipmentCodePrefix.Equals(other.OwnerEquipmentCodePrefix)
                ) &&                 
                (
                    this.OrganizationName == other.OrganizationName ||
                    this.OrganizationName != null &&
                    this.OrganizationName.Equals(other.OrganizationName)
                ) &&                 
                (
                    this.PrimaryContact == other.PrimaryContact ||
                    this.PrimaryContact != null &&
                    this.PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.IsMaintenanceContractor == other.IsMaintenanceContractor ||
                    this.IsMaintenanceContractor != null &&
                    this.IsMaintenanceContractor.Equals(other.IsMaintenanceContractor)
                ) &&                 
                (
                    this.WCBExpiryDate == other.WCBExpiryDate ||
                    this.WCBExpiryDate != null &&
                    this.WCBExpiryDate.Equals(other.WCBExpiryDate)
                ) &&                 
                (
                    this.CGLEndDate == other.CGLEndDate ||
                    this.CGLEndDate != null &&
                    this.CGLEndDate.Equals(other.CGLEndDate)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) &&                 
                (
                    this.ArchiveCode == other.ArchiveCode ||
                    this.ArchiveCode != null &&
                    this.ArchiveCode.Equals(other.ArchiveCode)
                ) &&                 
                (
                    this.ArchiveReason == other.ArchiveReason ||
                    this.ArchiveReason != null &&
                    this.ArchiveReason.Equals(other.ArchiveReason)
                ) &&                 
                (
                    this.ArchiveDate == other.ArchiveDate ||
                    this.ArchiveDate != null &&
                    this.ArchiveDate.Equals(other.ArchiveDate)
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
                                   
                hash = hash * 59 + this.Id.GetHashCode();                if (this.OwnerEquipmentCodePrefix != null)
                {
                    hash = hash * 59 + this.OwnerEquipmentCodePrefix.GetHashCode();
                }                
                                if (this.OrganizationName != null)
                {
                    hash = hash * 59 + this.OrganizationName.GetHashCode();
                }                
                                   
                if (this.PrimaryContact != null)
                {
                    hash = hash * 59 + this.PrimaryContact.GetHashCode();
                }                   
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                if (this.IsMaintenanceContractor != null)
                {
                    hash = hash * 59 + this.IsMaintenanceContractor.GetHashCode();
                }                
                                if (this.WCBExpiryDate != null)
                {
                    hash = hash * 59 + this.WCBExpiryDate.GetHashCode();
                }                
                                if (this.CGLEndDate != null)
                {
                    hash = hash * 59 + this.CGLEndDate.GetHashCode();
                }                
                                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
                }                
                                if (this.ArchiveCode != null)
                {
                    hash = hash * 59 + this.ArchiveCode.GetHashCode();
                }                
                                if (this.ArchiveReason != null)
                {
                    hash = hash * 59 + this.ArchiveReason.GetHashCode();
                }                
                                if (this.ArchiveDate != null)
                {
                    hash = hash * 59 + this.ArchiveDate.GetHashCode();
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
