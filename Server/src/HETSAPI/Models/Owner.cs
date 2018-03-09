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
    /// Owner Database Model
    /// </summary>
    [MetaData (Description = "The person or company to which a piece of construction equipment belongs.")]
    public sealed class Owner : AuditableEntity, IEquatable<Owner>
    {
        /// <summary>
        /// Owner Database Model Constructor (required by entity framework)
        /// </summary>
        public Owner()
        {
            Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Owner" /> class.
        /// </summary>
        /// <param name="id">A system-generated unique identifier for a Owner (required).</param>
        /// <param name="ownerCode">A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &amp;quot;EDW&amp;quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082. (required).</param>
        /// <param name="organizationName">The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company. (required).</param>
        /// <param name="meetsResidency">True to indicate that the owner of the business has confirmed to the HETS Clerk that they meet the residency requirements of the HETS programme. See the published information about the MOTI HETS programme for information on the owner residency requirements. (required).</param>
        /// <param name="localArea">LocalArea (required).</param>
        /// <param name="status">The status of the owner record in the system. Current set of values are &amp;quot;Pending&amp;quot;, &amp;quot;Approved&amp;quot; and &amp;quot;Archived&amp;quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &amp;quot;Approved&amp;quot; is used in all other cases. (required).</param>
        /// <param name="statusComment">A comment field to capture information specific to the change of status.</param>
        /// <param name="doingBusinessAs">An official (per BC Registries) alternate name for an Owner organization under which it does business. The application does not verify the name against any registry&amp;#x2F;lookup.</param>
        /// <param name="registeredCompanyNumber">The BC Registries number under which the business is registered.  The application does not verify the number against any registry&amp;#x2F;lookup.</param>
        /// <param name="primaryContact">Link to the designated Primary Contact.</param>
        /// <param name="isMaintenanceContractor">True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.</param>
        /// <param name="workSafeBcPolicyNumber">The Owner&amp;#39;s WorkSafeBC (aka WCB) Insurance Policy Number.</param>        
        /// <param name="workSafeBcExpiryDate">The expiration of the owner&amp;#39;s current WorkSafeBC (aka WCB) permit.</param>
        /// <param name="givenName">The given name of the contact.</param>
        /// <param name="surname">The surname of the contact.</param>
        /// <param name="address1">Address 1 line of the address.</param>
        /// <param name="address2">Address 2 line of the address.</param>
        /// <param name="city">The City of the address.</param>
        /// <param name="province">The Province of the address.</param>
        /// <param name="postalCode">The postal code of the address.</param>
        /// <param name="cglEndDate">The end date of the owner&amp;#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&amp;#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&amp;#x2F;faxed document.</param>
        /// <param name="cglPolicyNumber">The owner&amp;#39;s Commercial General Liability Policy Number</param>
        /// <param name="archiveCode">TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.</param>
        /// <param name="archiveReason">A text note about why the owner record has been changed to Archived.</param>
        /// <param name="archiveDate">The date the Owner record was changed to Archived and removed from active use in the system.</param>
        /// <param name="contacts">Contacts.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="attachments">Attachments.</param>
        /// <param name="history">History.</param>
        /// <param name="equipmentList">EquipmentList.</param>
        public Owner(int id, string ownerCode, string organizationName, bool meetsResidency, LocalArea localArea, 
            string status, string statusComment = null, string doingBusinessAs = null, string registeredCompanyNumber = null, 
            Contact primaryContact = null,  bool? isMaintenanceContractor = null, string workSafeBcPolicyNumber = null, 
            DateTime? workSafeBcExpiryDate = null, string givenName = null, string surname = null,
            string address1 = null, string address2 = null, string city = null, string province = null, string postalCode = null,
            DateTime? cglEndDate = null, string cglPolicyNumber = null, string archiveCode = null, string archiveReason = null, DateTime? archiveDate = null, 
            List<Contact> contacts = null, List<Note> notes = null, List<Attachment> attachments = null, List<History> history = null, 
            List<Equipment> equipmentList = null)
        {   
            Id = id;
            OwnerCode = ownerCode;
            OrganizationName = organizationName;
            MeetsResidency = meetsResidency;
            LocalArea = localArea;
            Status = status;
            StatusComment = statusComment;
            DoingBusinessAs = doingBusinessAs;
            RegisteredCompanyNumber = registeredCompanyNumber;
            PrimaryContact = primaryContact;
            IsMaintenanceContractor = isMaintenanceContractor;
            WorkSafeBCPolicyNumber = workSafeBcPolicyNumber;
            WorkSafeBCExpiryDate = workSafeBcExpiryDate;
            GivenName = givenName;
            Surname = surname;
            Address1 = address1;
            Address2 = address2;
            City = city;
            Province = province;
            PostalCode = postalCode;
            CGLEndDate = cglEndDate;
            CglPolicyNumber = cglPolicyNumber;
            ArchiveCode = archiveCode;
            ArchiveReason = archiveReason;
            ArchiveDate = archiveDate;
            Contacts = contacts;
            Notes = notes;
            Attachments = attachments;
            History = history;
            EquipmentList = equipmentList;
        }

        #region Owner Verification Pdf Usage Only

        /// <summary>
        /// Pdf Report Date
        /// </summary>
        [NotMapped]
        public string ReportDate { get; set; }

        /// <summary>
        /// Pdf Document Title
        /// </summary>
        [NotMapped]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the District Id
        /// </summary>
        [NotMapped]
        public int DistrictId { get; set; }

        /// <summary>
        /// Gets or Sets the Ministry District Id
        /// </summary>
        [NotMapped]
        public int MinistryDistrictId { get; set; }

        /// <summary>
        /// Gets or Sets the District Name
        /// </summary>
        [NotMapped]
        public string DistrictName { get; set; }

        /// <summary>
        /// Gets or Sets the District Address String
        /// </summary>
        [NotMapped]
        public string DistrictAddress { get; set; }

        /// <summary>
        /// Gets or Sets the District Contact (phone) String
        /// </summary>
        [NotMapped]
        public string DistrictContact { get; set; }

        #endregion

        /// <summary>
        /// A system-generated unique identifier for a Owner
        /// </summary>
        /// <value>A system-generated unique identifier for a Owner</value>
        [MetaData (Description = "A system-generated unique identifier for a Owner")]
        public int Id { get; set; }
        
        /// <summary>
        /// A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.
        /// </summary>
        /// <value>A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.</value>
        [MetaData (Description = "A unique prefix in the system that is used to generate the human-friendly IDs of the equipment. E.g. An owner Edwards might have a prefix &quot;EDW&quot; and their equipment numbered sequentially with that prefix - e.g. EDW-0082.")]
        [MaxLength(20)]        
        public string OwnerCode { get; set; }
        
        /// <summary>
        /// The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.
        /// </summary>
        /// <value>The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.</value>
        [MetaData (Description = "The name of the organization of the Owner. May simply be the First Name, Last Name of the Owner if the Owner is a sole proprietorship, or the name of a company.")]
        [MaxLength(150)]        
        public string OrganizationName { get; set; }
        
        /// <summary>
        /// True to indicate that the owner of the business has confirmed to the HETS Clerk that they meet the residency requirements of the HETS programme. See the published information about the MOTI HETS programme for information on the owner residency requirements.
        /// </summary>
        /// <value>True to indicate that the owner of the business has confirmed to the HETS Clerk that they meet the residency requirements of the HETS programme. See the published information about the MOTI HETS programme for information on the owner residency requirements.</value>
        [MetaData (Description = "True to indicate that the owner of the business has confirmed to the HETS Clerk that they meet the residency requirements of the HETS programme. See the published information about the MOTI HETS programme for information on the owner residency requirements.")]
        public bool MeetsResidency { get; set; }
        
        /// <summary>
        /// Gets or Sets LocalArea
        /// </summary>
        public LocalArea LocalArea { get; set; }
        
        /// <summary>
        /// Foreign key for LocalArea 
        /// </summary>   
        [ForeignKey("LocalArea")]
		[JsonIgnore]		
        public int? LocalAreaId { get; set; }
        
        /// <summary>
        /// The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.
        /// </summary>
        /// <value>The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.</value>
        [MetaData (Description = "The status of the owner record in the system. Current set of values are &quot;Pending&quot;, &quot;Approved&quot; and &quot;Archived&quot;. Pending is used when an owner self-registers and a HETS Clerk has not reviewed and Approved the record. Archived is when the owner is no longer part of the HETS programme. &quot;Approved&quot; is used in all other cases.")]
        [MaxLength(50)]        
        public string Status { get; set; }

        /// <summary>
        /// A comment field to capture information specific to the change of status.
        /// </summary>
        /// <value>A comment field to capture information specific to the change of status.</value>
        [MetaData(Description = "A comment field to capture information specific to the change of status.")]
        [MaxLength(255)]

        public string StatusComment { get; set; }

        /// <summary>
        /// An official (per BC Registries) alternate name for an Owner organization under which it does business. The application does not verify the name against any registry&#x2F;lookup.
        /// </summary>
        /// <value>An official (per BC Registries) alternate name for an Owner organization under which it does business. The application does not verify the name against any registry&#x2F;lookup.</value>
        [MetaData (Description = "An official (per BC Registries) alternate name for an Owner organization under which it does business. The application does not verify the name against any registry&#x2F;lookup.")]
        [MaxLength(150)]        
        public string DoingBusinessAs { get; set; }
        
        /// <summary>
        /// The BC Registries number under which the business is registered.  The application does not verify the number against any registry&#x2F;lookup.
        /// </summary>
        /// <value>The BC Registries number under which the business is registered.  The application does not verify the number against any registry&#x2F;lookup.</value>
        [MetaData (Description = "The BC Registries number under which the business is registered.  The application does not verify the number against any registry&#x2F;lookup.")]
        [MaxLength(150)]        
        public string RegisteredCompanyNumber { get; set; }
        
        /// <summary>
        /// Link to the designated Primary Contact.
        /// </summary>
        /// <value>Link to the designated Primary Contact.</value>
        [MetaData (Description = "Link to the designated Primary Contact.")]
        public Contact PrimaryContact { get; set; }
        
        /// <summary>
        /// Foreign key for PrimaryContact 
        /// </summary>   
        [ForeignKey("PrimaryContact")]
		[JsonIgnore]
		[MetaData (Description = "Link to the designated Primary Contact.")]
        public int? PrimaryContactId { get; set; }
        
        /// <summary>
        /// True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.
        /// </summary>
        /// <value>True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.</value>
        [MetaData (Description = "True if the owner is contracted by MOTI to handle Maintenance activities in the area - e.g. provided services in address unscheduled issues on the roads in the area.")]
        public bool? IsMaintenanceContractor { get; set; }
        
        /// <summary>
        /// The Owner&#39;s WorkSafeBC (aka WCB) Insurance Policy Number.
        /// </summary>
        /// <value>The Owner&#39;s WorkSafeBC (aka WCB) Insurance Policy Number.</value>
        [MetaData (Description = "The Owner&#39;s WorkSafeBC (aka WCB) Insurance Policy Number.")]
        [MaxLength(50)]        
        public string WorkSafeBCPolicyNumber { get; set; }
        
        /// <summary>
        /// The expiration of the owner&#39;s current WorkSafeBC (aka WCB) permit.
        /// </summary>
        /// <value>The expiration of the owner&#39;s current WorkSafeBC (aka WCB) permit.</value>
        [MetaData (Description = "The expiration of the owner&#39;s current WorkSafeBC (aka WCB) permit.")]
        public DateTime? WorkSafeBCExpiryDate { get; set; }

        /// <summary>
        /// The given name of the contact.
        /// </summary>
        /// <value>The given name of the contact.</value>
        [MetaData(Description = "The given name of the contact.")]
        [MaxLength(50)]
        public string GivenName { get; set; }

        /// <summary>
        /// The surname of the contact.
        /// </summary>
        /// <value>The surname of the contact.</value>
        [MetaData(Description = "The surname of the contact.")]
        [MaxLength(50)]
        public string Surname { get; set; }

        /// <summary>
        /// Address 1 line of the address.
        /// </summary>
        /// <value>Address 1 line of the address.</value>
        [MetaData(Description = "Address 1 line of the address.")]
        [MaxLength(80)]
        public string Address1 { get; set; }

        /// <summary>
        /// Address 2 line of the address.
        /// </summary>
        /// <value>Address 2 line of the address.</value>
        [MetaData(Description = "Address 2 line of the address.")]
        [MaxLength(80)]
        public string Address2 { get; set; }

        /// <summary>
        /// The City of the address.
        /// </summary>
        /// <value>The City of the address.</value>
        [MetaData(Description = "The City of the address.")]
        [MaxLength(100)]
        public string City { get; set; }

        /// <summary>
        /// The Province of the address.
        /// </summary>
        /// <value>The Province of the address.</value>
        [MetaData(Description = "The Province of the address.")]
        [MaxLength(50)]
        public string Province { get; set; }

        /// <summary>
        /// The postal code of the address.
        /// </summary>
        /// <value>The postal code of the address.</value>
        [MetaData(Description = "The postal code of the address.")]
        [MaxLength(15)]
        public string PostalCode { get; set; }

        /// <summary>
        /// The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.
        /// </summary>
        /// <value>The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.</value>
        [MetaData (Description = "The end date of the owner&#39;s Commercial General Liability insurance coverage. Coverage is only needed prior to an owner&#39;s piece of equipment starting a rental period (not when in the HETS program but not hired). The details of the coverage can be entered into a Note, or more often - attached as a scanned&#x2F;faxed document.")]
        public DateTime? CGLEndDate { get; set; }

        /// <summary>
        /// The owner&amp;#39;s Commercial General Liability Policy Number
        /// </summary>
        /// <value>The owner&amp;#39;s Commercial General Liability Policy Number</value>
        [MetaData(Description = "The owner&amp;#39;s Commercial General Liability Policy Number")]
        [MaxLength(50)]
        public string CglPolicyNumber { get; set; }        

        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.</value>
        [MetaData (Description = "TO BE REVIEWED WITH THE BUSINESS - IS THIS NEEDED -A coded reason for why an owner record has been moved to Archived.")]
        [MaxLength(50)]        
        public string ArchiveCode { get; set; }
        
        /// <summary>
        /// A text note about why the owner record has been changed to Archived.
        /// </summary>
        /// <value>A text note about why the owner record has been changed to Archived.</value>
        [MetaData (Description = "A text note about why the owner record has been changed to Archived.")]
        [MaxLength(2048)]        
        public string ArchiveReason { get; set; }
        
        /// <summary>
        /// The date the Owner record was changed to Archived and removed from active use in the system.
        /// </summary>
        /// <value>The date the Owner record was changed to Archived and removed from active use in the system.</value>
        [MetaData (Description = "The date the Owner record was changed to Archived and removed from active use in the system.")]
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
            sb.Append("  OwnerCode: ").Append(OwnerCode).Append("\n");
            sb.Append("  OrganizationName: ").Append(OrganizationName).Append("\n");
            sb.Append("  MeetsResidency: ").Append(MeetsResidency).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  StatusComment: ").Append(StatusComment).Append("\n");
            sb.Append("  DoingBusinessAs: ").Append(DoingBusinessAs).Append("\n");
            sb.Append("  RegisteredCompanyNumber: ").Append(RegisteredCompanyNumber).Append("\n");
            sb.Append("  PrimaryContact: ").Append(PrimaryContact).Append("\n");
            sb.Append("  IsMaintenanceContractor: ").Append(IsMaintenanceContractor).Append("\n");
            sb.Append("  WorkSafeBCPolicyNumber: ").Append(WorkSafeBCPolicyNumber).Append("\n");
            sb.Append("  WorkSafeBCExpiryDate: ").Append(WorkSafeBCExpiryDate).Append("\n");
            sb.Append("  GivenName: ").Append(GivenName).Append("\n");
            sb.Append("  Surname: ").Append(Surname).Append("\n");
            sb.Append("  Address1: ").Append(Address1).Append("\n");
            sb.Append("  Address2: ").Append(Address2).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Province: ").Append(Province).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  CGLEndDate: ").Append(CGLEndDate).Append("\n");
            sb.Append("  CglPolicyNumber: ").Append(CglPolicyNumber).Append("\n");            
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
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            return obj.GetType() == GetType() && Equals((Owner)obj);
        }

        /// <summary>
        /// Returns true if Owner instances are equal
        /// </summary>
        /// <param name="other">Instance of Owner to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Owner other)
        {
            if (other is null) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    Id == other.Id ||
                    Id.Equals(other.Id)
                ) &&                 
                (
                    OwnerCode == other.OwnerCode ||
                    OwnerCode != null &&
                    OwnerCode.Equals(other.OwnerCode)
                ) &&                 
                (
                    OrganizationName == other.OrganizationName ||
                    OrganizationName != null &&
                    OrganizationName.Equals(other.OrganizationName)
                ) &&                 
                (
                    MeetsResidency == other.MeetsResidency ||
                    MeetsResidency.Equals(other.MeetsResidency)
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
                    StatusComment == other.StatusComment ||
                    StatusComment != null &&
                    StatusComment.Equals(other.StatusComment)
                ) &&                 
                (
                    DoingBusinessAs == other.DoingBusinessAs ||
                    DoingBusinessAs != null &&
                    DoingBusinessAs.Equals(other.DoingBusinessAs)
                ) &&                 
                (
                    RegisteredCompanyNumber == other.RegisteredCompanyNumber ||
                    RegisteredCompanyNumber != null &&
                    RegisteredCompanyNumber.Equals(other.RegisteredCompanyNumber)
                ) &&                 
                (
                    PrimaryContact == other.PrimaryContact ||
                    PrimaryContact != null &&
                    PrimaryContact.Equals(other.PrimaryContact)
                ) &&                 
                (
                    IsMaintenanceContractor == other.IsMaintenanceContractor ||
                    IsMaintenanceContractor != null &&
                    IsMaintenanceContractor.Equals(other.IsMaintenanceContractor)
                ) &&                 
                (
                    WorkSafeBCPolicyNumber == other.WorkSafeBCPolicyNumber ||
                    WorkSafeBCPolicyNumber != null &&
                    WorkSafeBCPolicyNumber.Equals(other.WorkSafeBCPolicyNumber)
                ) &&                 
                (
                    WorkSafeBCExpiryDate == other.WorkSafeBCExpiryDate ||
                    WorkSafeBCExpiryDate != null &&
                    WorkSafeBCExpiryDate.Equals(other.WorkSafeBCExpiryDate)
                ) &&
                (
                    GivenName == other.GivenName ||
                    GivenName != null &&
                    GivenName.Equals(other.GivenName)
                ) &&
                (
                    Surname == other.Surname ||
                    Surname != null &&
                    Surname.Equals(other.Surname)
                ) &&
                (
                    Address1 == other.Address1 ||
                    Address1 != null &&
                    Address1.Equals(other.Address1)
                ) &&
                (
                    Address2 == other.Address2 ||
                    Address2 != null &&
                    Address2.Equals(other.Address2)
                ) &&
                (
                    City == other.City ||
                    City != null &&
                    City.Equals(other.City)
                ) &&
                (
                    Province == other.Province ||
                    Province != null &&
                    Province.Equals(other.Province)
                ) &&
                (
                    PostalCode == other.PostalCode ||
                    PostalCode != null &&
                    PostalCode.Equals(other.PostalCode)
                ) &&
                (
                    CGLEndDate == other.CGLEndDate ||
                    CGLEndDate != null &&
                    CGLEndDate.Equals(other.CGLEndDate)
                ) &&
                (
                    CglPolicyNumber == other.CglPolicyNumber ||
                    CglPolicyNumber != null &&
                    CglPolicyNumber.Equals(other.CglPolicyNumber)
                ) &&                
                (
                    ArchiveCode == other.ArchiveCode ||
                    ArchiveCode != null &&
                    ArchiveCode.Equals(other.ArchiveCode)
                ) &&                 
                (
                    ArchiveReason == other.ArchiveReason ||
                    ArchiveReason != null &&
                    ArchiveReason.Equals(other.ArchiveReason)
                ) &&                 
                (
                    ArchiveDate == other.ArchiveDate ||
                    ArchiveDate != null &&
                    ArchiveDate.Equals(other.ArchiveDate)
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
                ) && 
                (
                    EquipmentList == other.EquipmentList ||
                    EquipmentList != null &&
                    EquipmentList.SequenceEqual(other.EquipmentList)
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

                if (OwnerCode != null)
                {
                    hash = hash * 59 + OwnerCode.GetHashCode();
                }

                if (OrganizationName != null)
                {
                    hash = hash * 59 + OrganizationName.GetHashCode();
                }                
                                   
                hash = hash * 59 + MeetsResidency.GetHashCode();
                                   
                if (LocalArea != null)
                {
                    hash = hash * 59 + LocalArea.GetHashCode();
                }

                if (Status != null)
                {
                    hash = hash * 59 + Status.GetHashCode();
                }

                if (StatusComment != null)
                {
                    hash = hash * 59 + StatusComment.GetHashCode();
                }

                if (DoingBusinessAs != null)
                {
                    hash = hash * 59 + DoingBusinessAs.GetHashCode();
                }

                if (RegisteredCompanyNumber != null)
                {
                    hash = hash * 59 + RegisteredCompanyNumber.GetHashCode();
                }                
                                   
                if (PrimaryContact != null)
                {
                    hash = hash * 59 + PrimaryContact.GetHashCode();
                }

                if (IsMaintenanceContractor != null)
                {
                    hash = hash * 59 + IsMaintenanceContractor.GetHashCode();
                }

                if (WorkSafeBCPolicyNumber != null)
                {
                    hash = hash * 59 + WorkSafeBCPolicyNumber.GetHashCode();
                }

                if (WorkSafeBCExpiryDate != null)
                {
                    hash = hash * 59 + WorkSafeBCExpiryDate.GetHashCode();
                }

                if (GivenName != null)
                {
                    hash = hash * 59 + GivenName.GetHashCode();
                }

                if (Surname != null)
                {
                    hash = hash * 59 + Surname.GetHashCode();
                }

                if (Address1 != null)
                {
                    hash = hash * 59 + Address1.GetHashCode();
                }

                if (Address2 != null)
                {
                    hash = hash * 59 + Address2.GetHashCode();
                }

                if (City != null)
                {
                    hash = hash * 59 + City.GetHashCode();
                }

                if (Province != null)
                {
                    hash = hash * 59 + Province.GetHashCode();
                }

                if (PostalCode != null)
                {
                    hash = hash * 59 + PostalCode.GetHashCode();
                }

                if (CGLEndDate != null)
                {
                    hash = hash * 59 + CGLEndDate.GetHashCode();
                }

                if (CglPolicyNumber != null)
                {
                    hash = hash * 59 + CglPolicyNumber.GetHashCode();
                }                

                if (ArchiveCode != null)
                {
                    hash = hash * 59 + ArchiveCode.GetHashCode();
                }

                if (ArchiveReason != null)
                {
                    hash = hash * 59 + ArchiveReason.GetHashCode();
                }

                if (ArchiveDate != null)
                {
                    hash = hash * 59 + ArchiveDate.GetHashCode();
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

                if (EquipmentList != null)
                {
                    hash = hash * 59 + EquipmentList.GetHashCode();
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
