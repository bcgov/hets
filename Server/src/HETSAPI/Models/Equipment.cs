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
    /// A piece of equipment in the HETS system. Each piece of equipment is of a specific equipment type, owned by an Owner, and is within a Local Area.
    /// </summary>
        [MetaDataExtension (Description = "A piece of equipment in the HETS system. Each piece of equipment is of a specific equipment type, owned by an Owner, and is within a Local Area.")]

    public partial class Equipment : AuditableEntity, IEquatable<Equipment>
    {
        /// <summary>
        /// Default constructor, required by entity framework
        /// </summary>
        public Equipment()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment" /> class.
        /// </summary>
        /// <param name="Id">A system-generated unique identifier for a Equipment (required).</param>
        /// <param name="LocalArea">A foreign key reference to the system-generated unique identifier for a Local Area (required).</param>
        /// <param name="DistrictEquipmentType">A foreign key reference to the system-generated unique identifier for a Equipment Type (required).</param>
        /// <param name="Owner">A foreign key reference to the system-generated unique identifier for an Owner (required).</param>
        /// <param name="EquipmentCode">A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083. (required).</param>
        /// <param name="Status">The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added. (required).</param>
        /// <param name="ReceivedDate">The date the piece of equipment was first received and recorded in HETS. (required).</param>
        /// <param name="LastVerifiedDate">The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme. (required).</param>
        /// <param name="ApprovedDate">The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date..</param>
        /// <param name="IsInformationUpdateNeeded">Set true if a need to update the information&amp;#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update..</param>
        /// <param name="InformationUpdateNeededReason">A note about why the needed information&amp;#x2F;status update that is needed about the equipment..</param>
        /// <param name="LicencePlate">The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk..</param>
        /// <param name="Make">The make of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Model">The model of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Year">The model year of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Operator">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="PayRate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="RefuseRate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="SerialNumber">The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area..</param>
        /// <param name="Size">The size of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="ToDate">TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?.</param>
        /// <param name="BlockNumber">The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open.</param>
        /// <param name="Seniority">The current seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="NumberInBlock">The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order..</param>
        /// <param name="IsSeniorityOverridden">True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden..</param>
        /// <param name="SeniorityOverrideReason">A text reason for why the piece of equipments underlying data was overridden to change their seniority number..</param>
        /// <param name="SeniorityEffectiveDate">The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated..</param>
        /// <param name="YearsOfService">The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY..</param>
        /// <param name="ServiceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="ServiceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="ServiceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        /// <param name="ArchiveCode">TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived..</param>
        /// <param name="ArchiveReason">An optional comment about why this piece of equipment has been archived..</param>
        /// <param name="ArchiveDate">The date on which a user most recenly marked this piece of equipment as archived..</param>
        /// <param name="DumpTruck">A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck..</param>
        /// <param name="EquipmentAttachments">EquipmentAttachments.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="SeniorityAudit">SeniorityAudit.</param>
        public Equipment(int Id, LocalArea LocalArea, DistrictEquipmentType DistrictEquipmentType, Owner Owner, string EquipmentCode, string Status, DateTime ReceivedDate, DateTime LastVerifiedDate, DateTime? ApprovedDate = null, bool? IsInformationUpdateNeeded = null, string InformationUpdateNeededReason = null, string LicencePlate = null, string Make = null, string Model = null, string Year = null, string Operator = null, float? PayRate = null, string RefuseRate = null, string SerialNumber = null, string Size = null, DateTime? ToDate = null, float? BlockNumber = null, float? Seniority = null, int? NumberInBlock = null, bool? IsSeniorityOverridden = null, string SeniorityOverrideReason = null, DateTime? SeniorityEffectiveDate = null, float? YearsOfService = null, float? ServiceHoursLastYear = null, float? ServiceHoursTwoYearsAgo = null, float? ServiceHoursThreeYearsAgo = null, string ArchiveCode = null, string ArchiveReason = null, DateTime? ArchiveDate = null, DumpTruck DumpTruck = null, List<EquipmentAttachment> EquipmentAttachments = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<SeniorityAudit> SeniorityAudit = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.DistrictEquipmentType = DistrictEquipmentType;
            this.Owner = Owner;
            this.EquipmentCode = EquipmentCode;
            this.Status = Status;
            this.ReceivedDate = ReceivedDate;
            this.LastVerifiedDate = LastVerifiedDate;







            this.ApprovedDate = ApprovedDate;
            this.IsInformationUpdateNeeded = IsInformationUpdateNeeded;
            this.InformationUpdateNeededReason = InformationUpdateNeededReason;
            this.LicencePlate = LicencePlate;
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            this.Operator = Operator;
            this.PayRate = PayRate;
            this.RefuseRate = RefuseRate;
            this.SerialNumber = SerialNumber;
            this.Size = Size;
            this.ToDate = ToDate;
            this.BlockNumber = BlockNumber;
            this.Seniority = Seniority;
            this.NumberInBlock = NumberInBlock;
            this.IsSeniorityOverridden = IsSeniorityOverridden;
            this.SeniorityOverrideReason = SeniorityOverrideReason;
            this.SeniorityEffectiveDate = SeniorityEffectiveDate;
            this.YearsOfService = YearsOfService;
            this.ServiceHoursLastYear = ServiceHoursLastYear;
            this.ServiceHoursTwoYearsAgo = ServiceHoursTwoYearsAgo;
            this.ServiceHoursThreeYearsAgo = ServiceHoursThreeYearsAgo;
            this.ArchiveCode = ArchiveCode;
            this.ArchiveReason = ArchiveReason;
            this.ArchiveDate = ArchiveDate;
            this.DumpTruck = DumpTruck;
            this.EquipmentAttachments = EquipmentAttachments;
            this.Notes = Notes;
            this.Attachments = Attachments;
            this.History = History;
            this.SeniorityAudit = SeniorityAudit;
        }

        /// <summary>
        /// A system-generated unique identifier for a Equipment
        /// </summary>
        /// <value>A system-generated unique identifier for a Equipment</value>
        [MetaDataExtension (Description = "A system-generated unique identifier for a Equipment")]
        public int Id { get; set; }
        
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
        /// A foreign key reference to the system-generated unique identifier for a Equipment Type
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for a Equipment Type</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Equipment Type")]
        public DistrictEquipmentType DistrictEquipmentType { get; set; }
        
        /// <summary>
        /// Foreign key for DistrictEquipmentType 
        /// </summary>   
        [ForeignKey("DistrictEquipmentType")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for a Equipment Type")]
        public int? DistrictEquipmentTypeId { get; set; }
        
        /// <summary>
        /// A foreign key reference to the system-generated unique identifier for an Owner
        /// </summary>
        /// <value>A foreign key reference to the system-generated unique identifier for an Owner</value>
        [MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public Owner Owner { get; set; }
        
        /// <summary>
        /// Foreign key for Owner 
        /// </summary>   
        [ForeignKey("Owner")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A foreign key reference to the system-generated unique identifier for an Owner")]
        public int? OwnerId { get; set; }
        
        /// <summary>
        /// A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.
        /// </summary>
        /// <value>A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.</value>
        [MetaDataExtension (Description = "A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated at record creation time based on the unique Owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.")]
        [MaxLength(25)]
        
        public string EquipmentCode { get; set; }
        
        /// <summary>
        /// The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.
        /// </summary>
        /// <value>The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.</value>
        [MetaDataExtension (Description = "The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.")]
        [MaxLength(50)]
        
        public string Status { get; set; }
        
        /// <summary>
        /// The date the piece of equipment was first received and recorded in HETS.
        /// </summary>
        /// <value>The date the piece of equipment was first received and recorded in HETS.</value>
        [MetaDataExtension (Description = "The date the piece of equipment was first received and recorded in HETS.")]
        public DateTime ReceivedDate { get; set; }
        
        /// <summary>
        /// The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.
        /// </summary>
        /// <value>The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.</value>
        [MetaDataExtension (Description = "The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.")]
        public DateTime LastVerifiedDate { get; set; }
        
        /// <summary>
        /// The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.
        /// </summary>
        /// <value>The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.</value>
        [MetaDataExtension (Description = "The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.")]
        public DateTime? ApprovedDate { get; set; }
        
        /// <summary>
        /// Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.
        /// </summary>
        /// <value>Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.</value>
        [MetaDataExtension (Description = "Set true if a need to update the information&#x2F;status of the equipment is needed. Used during the processing of a request when an update is noted, but the Clerk does not have time to make the update.")]
        public bool? IsInformationUpdateNeeded { get; set; }
        
        /// <summary>
        /// A note about why the needed information&#x2F;status update that is needed about the equipment.
        /// </summary>
        /// <value>A note about why the needed information&#x2F;status update that is needed about the equipment.</value>
        [MetaDataExtension (Description = "A note about why the needed information&#x2F;status update that is needed about the equipment.")]
        [MaxLength(2048)]
        
        public string InformationUpdateNeededReason { get; set; }
        
        /// <summary>
        /// The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.
        /// </summary>
        /// <value>The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.</value>
        [MetaDataExtension (Description = "The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.")]
        [MaxLength(20)]
        
        public string LicencePlate { get; set; }
        
        /// <summary>
        /// The make of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The make of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The make of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(50)]
        
        public string Make { get; set; }
        
        /// <summary>
        /// The model of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The model of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(50)]
        
        public string Model { get; set; }
        
        /// <summary>
        /// The model year of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model year of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The model year of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(15)]
        
        public string Year { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        [MaxLength(255)]
        
        public string Operator { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public float? PayRate { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        [MaxLength(255)]
        
        public string RefuseRate { get; set; }
        
        /// <summary>
        /// The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.
        /// </summary>
        /// <value>The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.</value>
        [MetaDataExtension (Description = "The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.")]
        [MaxLength(100)]
        
        public string SerialNumber { get; set; }
        
        /// <summary>
        /// The size of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The size of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The size of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(128)]
        
        public string Size { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED WITH THE BUSINESS - WHAT IS THIS?")]
        public DateTime? ToDate { get; set; }
        
        /// <summary>
        /// The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open
        /// </summary>
        /// <value>The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open</value>
        [MetaDataExtension (Description = "The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open")]
        public float? BlockNumber { get; set; }
        
        /// <summary>
        /// The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).
        /// </summary>
        /// <value>The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).</value>
        [MetaDataExtension (Description = "The current seniority calculation result for this piece of equipment. The calculation is based on the &quot;numYears&quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo).")]
        public float? Seniority { get; set; }
        
        /// <summary>
        /// The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.
        /// </summary>
        /// <value>The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.</value>
        [MetaDataExtension (Description = "The number in the block of the piece of equipment so that it can be displayed to the user where it will be useful. This saves the user from having to figure out in their head the order when the list is displayed in Rotation Order.")]
        public int? NumberInBlock { get; set; }
        
        /// <summary>
        /// True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.
        /// </summary>
        /// <value>True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.</value>
        [MetaDataExtension (Description = "True if the Seniority for the piece of equipment was manually overridden. Set if a user has gone in and explicitly updated the seniority base information. Indicates that underlying numbers were manually overridden.")]
        public bool? IsSeniorityOverridden { get; set; }
        
        /// <summary>
        /// A text reason for why the piece of equipments underlying data was overridden to change their seniority number.
        /// </summary>
        /// <value>A text reason for why the piece of equipments underlying data was overridden to change their seniority number.</value>
        [MetaDataExtension (Description = "A text reason for why the piece of equipments underlying data was overridden to change their seniority number.")]
        [MaxLength(2048)]
        
        public string SeniorityOverrideReason { get; set; }
        
        /// <summary>
        /// The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.
        /// </summary>
        /// <value>The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.</value>
        [MetaDataExtension (Description = "The time the seniority data in the record went into effect. Used to populate the SeniorityAudit table when the seniority data is next updated.")]
        public DateTime? SeniorityEffectiveDate { get; set; }
        
        /// <summary>
        /// The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.
        /// </summary>
        /// <value>The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.</value>
        [MetaDataExtension (Description = "The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.")]
        public float? YearsOfService { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the previous fiscal year
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the previous fiscal year</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the previous fiscal year")]
        public float? ServiceHoursLastYear { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016")]
        public float? ServiceHoursTwoYearsAgo { get; set; }
        
        /// <summary>
        /// Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015
        /// </summary>
        /// <value>Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015</value>
        [MetaDataExtension (Description = "Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015")]
        public float? ServiceHoursThreeYearsAgo { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.
        /// </summary>
        /// <value>TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.</value>
        [MetaDataExtension (Description = "TO BE REVIEWED - A reason code indicating why a piece of equipment has been archived.")]
        [MaxLength(50)]
        
        public string ArchiveCode { get; set; }
        
        /// <summary>
        /// An optional comment about why this piece of equipment has been archived.
        /// </summary>
        /// <value>An optional comment about why this piece of equipment has been archived.</value>
        [MetaDataExtension (Description = "An optional comment about why this piece of equipment has been archived.")]
        [MaxLength(2048)]
        
        public string ArchiveReason { get; set; }
        
        /// <summary>
        /// The date on which a user most recenly marked this piece of equipment as archived.
        /// </summary>
        /// <value>The date on which a user most recenly marked this piece of equipment as archived.</value>
        [MetaDataExtension (Description = "The date on which a user most recenly marked this piece of equipment as archived.")]
        public DateTime? ArchiveDate { get; set; }
        
        /// <summary>
        /// A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.
        /// </summary>
        /// <value>A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.</value>
        [MetaDataExtension (Description = "A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.")]
        public DumpTruck DumpTruck { get; set; }
        
        /// <summary>
        /// Foreign key for DumpTruck 
        /// </summary>   
        [ForeignKey("DumpTruck")]
		[JsonIgnore]
		[MetaDataExtension (Description = "A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck.")]
        public int? DumpTruckId { get; set; }
        
        /// <summary>
        /// Gets or Sets EquipmentAttachments
        /// </summary>
        public List<EquipmentAttachment> EquipmentAttachments { get; set; }
        
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
        /// Gets or Sets SeniorityAudit
        /// </summary>
        public List<SeniorityAudit> SeniorityAudit { get; set; }
        
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Equipment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  LocalArea: ").Append(LocalArea).Append("\n");
            sb.Append("  DistrictEquipmentType: ").Append(DistrictEquipmentType).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  EquipmentCode: ").Append(EquipmentCode).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ReceivedDate: ").Append(ReceivedDate).Append("\n");
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  ApprovedDate: ").Append(ApprovedDate).Append("\n");
            sb.Append("  IsInformationUpdateNeeded: ").Append(IsInformationUpdateNeeded).Append("\n");
            sb.Append("  InformationUpdateNeededReason: ").Append(InformationUpdateNeededReason).Append("\n");
            sb.Append("  LicencePlate: ").Append(LicencePlate).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Year: ").Append(Year).Append("\n");
            sb.Append("  Operator: ").Append(Operator).Append("\n");
            sb.Append("  PayRate: ").Append(PayRate).Append("\n");
            sb.Append("  RefuseRate: ").Append(RefuseRate).Append("\n");
            sb.Append("  SerialNumber: ").Append(SerialNumber).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  ToDate: ").Append(ToDate).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  NumberInBlock: ").Append(NumberInBlock).Append("\n");
            sb.Append("  IsSeniorityOverridden: ").Append(IsSeniorityOverridden).Append("\n");
            sb.Append("  SeniorityOverrideReason: ").Append(SeniorityOverrideReason).Append("\n");
            sb.Append("  SeniorityEffectiveDate: ").Append(SeniorityEffectiveDate).Append("\n");
            sb.Append("  YearsOfService: ").Append(YearsOfService).Append("\n");
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
            sb.Append("  ArchiveCode: ").Append(ArchiveCode).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  ArchiveDate: ").Append(ArchiveDate).Append("\n");
            sb.Append("  DumpTruck: ").Append(DumpTruck).Append("\n");
            sb.Append("  EquipmentAttachments: ").Append(EquipmentAttachments).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
            sb.Append("  Attachments: ").Append(Attachments).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  SeniorityAudit: ").Append(SeniorityAudit).Append("\n");
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
            return Equals((Equipment)obj);
        }

        /// <summary>
        /// Returns true if Equipment instances are equal
        /// </summary>
        /// <param name="other">Instance of Equipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Equipment other)
        {

            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }

            return                 
                (
                    this.Id == other.Id ||
                    this.Id.Equals(other.Id)
                ) &&                 
                (
                    this.LocalArea == other.LocalArea ||
                    this.LocalArea != null &&
                    this.LocalArea.Equals(other.LocalArea)
                ) &&                 
                (
                    this.DistrictEquipmentType == other.DistrictEquipmentType ||
                    this.DistrictEquipmentType != null &&
                    this.DistrictEquipmentType.Equals(other.DistrictEquipmentType)
                ) &&                 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.EquipmentCode == other.EquipmentCode ||
                    this.EquipmentCode != null &&
                    this.EquipmentCode.Equals(other.EquipmentCode)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) &&                 
                (
                    this.ReceivedDate == other.ReceivedDate ||
                    this.ReceivedDate != null &&
                    this.ReceivedDate.Equals(other.ReceivedDate)
                ) &&                 
                (
                    this.LastVerifiedDate == other.LastVerifiedDate ||
                    this.LastVerifiedDate != null &&
                    this.LastVerifiedDate.Equals(other.LastVerifiedDate)
                ) &&                 
                (
                    this.ApprovedDate == other.ApprovedDate ||
                    this.ApprovedDate != null &&
                    this.ApprovedDate.Equals(other.ApprovedDate)
                ) &&                 
                (
                    this.IsInformationUpdateNeeded == other.IsInformationUpdateNeeded ||
                    this.IsInformationUpdateNeeded != null &&
                    this.IsInformationUpdateNeeded.Equals(other.IsInformationUpdateNeeded)
                ) &&                 
                (
                    this.InformationUpdateNeededReason == other.InformationUpdateNeededReason ||
                    this.InformationUpdateNeededReason != null &&
                    this.InformationUpdateNeededReason.Equals(other.InformationUpdateNeededReason)
                ) &&                 
                (
                    this.LicencePlate == other.LicencePlate ||
                    this.LicencePlate != null &&
                    this.LicencePlate.Equals(other.LicencePlate)
                ) &&                 
                (
                    this.Make == other.Make ||
                    this.Make != null &&
                    this.Make.Equals(other.Make)
                ) &&                 
                (
                    this.Model == other.Model ||
                    this.Model != null &&
                    this.Model.Equals(other.Model)
                ) &&                 
                (
                    this.Year == other.Year ||
                    this.Year != null &&
                    this.Year.Equals(other.Year)
                ) &&                 
                (
                    this.Operator == other.Operator ||
                    this.Operator != null &&
                    this.Operator.Equals(other.Operator)
                ) &&                 
                (
                    this.PayRate == other.PayRate ||
                    this.PayRate != null &&
                    this.PayRate.Equals(other.PayRate)
                ) &&                 
                (
                    this.RefuseRate == other.RefuseRate ||
                    this.RefuseRate != null &&
                    this.RefuseRate.Equals(other.RefuseRate)
                ) &&                 
                (
                    this.SerialNumber == other.SerialNumber ||
                    this.SerialNumber != null &&
                    this.SerialNumber.Equals(other.SerialNumber)
                ) &&                 
                (
                    this.Size == other.Size ||
                    this.Size != null &&
                    this.Size.Equals(other.Size)
                ) &&                 
                (
                    this.ToDate == other.ToDate ||
                    this.ToDate != null &&
                    this.ToDate.Equals(other.ToDate)
                ) &&                 
                (
                    this.BlockNumber == other.BlockNumber ||
                    this.BlockNumber != null &&
                    this.BlockNumber.Equals(other.BlockNumber)
                ) &&                 
                (
                    this.Seniority == other.Seniority ||
                    this.Seniority != null &&
                    this.Seniority.Equals(other.Seniority)
                ) &&                 
                (
                    this.NumberInBlock == other.NumberInBlock ||
                    this.NumberInBlock != null &&
                    this.NumberInBlock.Equals(other.NumberInBlock)
                ) &&                 
                (
                    this.IsSeniorityOverridden == other.IsSeniorityOverridden ||
                    this.IsSeniorityOverridden != null &&
                    this.IsSeniorityOverridden.Equals(other.IsSeniorityOverridden)
                ) &&                 
                (
                    this.SeniorityOverrideReason == other.SeniorityOverrideReason ||
                    this.SeniorityOverrideReason != null &&
                    this.SeniorityOverrideReason.Equals(other.SeniorityOverrideReason)
                ) &&                 
                (
                    this.SeniorityEffectiveDate == other.SeniorityEffectiveDate ||
                    this.SeniorityEffectiveDate != null &&
                    this.SeniorityEffectiveDate.Equals(other.SeniorityEffectiveDate)
                ) &&                 
                (
                    this.YearsOfService == other.YearsOfService ||
                    this.YearsOfService != null &&
                    this.YearsOfService.Equals(other.YearsOfService)
                ) &&                 
                (
                    this.ServiceHoursLastYear == other.ServiceHoursLastYear ||
                    this.ServiceHoursLastYear != null &&
                    this.ServiceHoursLastYear.Equals(other.ServiceHoursLastYear)
                ) &&                 
                (
                    this.ServiceHoursTwoYearsAgo == other.ServiceHoursTwoYearsAgo ||
                    this.ServiceHoursTwoYearsAgo != null &&
                    this.ServiceHoursTwoYearsAgo.Equals(other.ServiceHoursTwoYearsAgo)
                ) &&                 
                (
                    this.ServiceHoursThreeYearsAgo == other.ServiceHoursThreeYearsAgo ||
                    this.ServiceHoursThreeYearsAgo != null &&
                    this.ServiceHoursThreeYearsAgo.Equals(other.ServiceHoursThreeYearsAgo)
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
                    this.DumpTruck == other.DumpTruck ||
                    this.DumpTruck != null &&
                    this.DumpTruck.Equals(other.DumpTruck)
                ) && 
                (
                    this.EquipmentAttachments == other.EquipmentAttachments ||
                    this.EquipmentAttachments != null &&
                    this.EquipmentAttachments.SequenceEqual(other.EquipmentAttachments)
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
                    this.SeniorityAudit == other.SeniorityAudit ||
                    this.SeniorityAudit != null &&
                    this.SeniorityAudit.SequenceEqual(other.SeniorityAudit)
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
                if (this.LocalArea != null)
                {
                    hash = hash * 59 + this.LocalArea.GetHashCode();
                }                   
                if (this.DistrictEquipmentType != null)
                {
                    hash = hash * 59 + this.DistrictEquipmentType.GetHashCode();
                }                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.EquipmentCode != null)
                {
                    hash = hash * 59 + this.EquipmentCode.GetHashCode();
                }                
                                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
                }                
                                   
                if (this.ReceivedDate != null)
                {
                    hash = hash * 59 + this.ReceivedDate.GetHashCode();
                }                   
                if (this.LastVerifiedDate != null)
                {
                    hash = hash * 59 + this.LastVerifiedDate.GetHashCode();
                }                if (this.ApprovedDate != null)
                {
                    hash = hash * 59 + this.ApprovedDate.GetHashCode();
                }                
                                if (this.IsInformationUpdateNeeded != null)
                {
                    hash = hash * 59 + this.IsInformationUpdateNeeded.GetHashCode();
                }                
                                if (this.InformationUpdateNeededReason != null)
                {
                    hash = hash * 59 + this.InformationUpdateNeededReason.GetHashCode();
                }                
                                if (this.LicencePlate != null)
                {
                    hash = hash * 59 + this.LicencePlate.GetHashCode();
                }                
                                if (this.Make != null)
                {
                    hash = hash * 59 + this.Make.GetHashCode();
                }                
                                if (this.Model != null)
                {
                    hash = hash * 59 + this.Model.GetHashCode();
                }                
                                if (this.Year != null)
                {
                    hash = hash * 59 + this.Year.GetHashCode();
                }                
                                if (this.Operator != null)
                {
                    hash = hash * 59 + this.Operator.GetHashCode();
                }                
                                if (this.PayRate != null)
                {
                    hash = hash * 59 + this.PayRate.GetHashCode();
                }                
                                if (this.RefuseRate != null)
                {
                    hash = hash * 59 + this.RefuseRate.GetHashCode();
                }                
                                if (this.SerialNumber != null)
                {
                    hash = hash * 59 + this.SerialNumber.GetHashCode();
                }                
                                if (this.Size != null)
                {
                    hash = hash * 59 + this.Size.GetHashCode();
                }                
                                if (this.ToDate != null)
                {
                    hash = hash * 59 + this.ToDate.GetHashCode();
                }                
                                if (this.BlockNumber != null)
                {
                    hash = hash * 59 + this.BlockNumber.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.NumberInBlock != null)
                {
                    hash = hash * 59 + this.NumberInBlock.GetHashCode();
                }                
                                if (this.IsSeniorityOverridden != null)
                {
                    hash = hash * 59 + this.IsSeniorityOverridden.GetHashCode();
                }                
                                if (this.SeniorityOverrideReason != null)
                {
                    hash = hash * 59 + this.SeniorityOverrideReason.GetHashCode();
                }                
                                if (this.SeniorityEffectiveDate != null)
                {
                    hash = hash * 59 + this.SeniorityEffectiveDate.GetHashCode();
                }                
                                if (this.YearsOfService != null)
                {
                    hash = hash * 59 + this.YearsOfService.GetHashCode();
                }                
                                if (this.ServiceHoursLastYear != null)
                {
                    hash = hash * 59 + this.ServiceHoursLastYear.GetHashCode();
                }                
                                if (this.ServiceHoursTwoYearsAgo != null)
                {
                    hash = hash * 59 + this.ServiceHoursTwoYearsAgo.GetHashCode();
                }                
                                if (this.ServiceHoursThreeYearsAgo != null)
                {
                    hash = hash * 59 + this.ServiceHoursThreeYearsAgo.GetHashCode();
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
                                   
                if (this.DumpTruck != null)
                {
                    hash = hash * 59 + this.DumpTruck.GetHashCode();
                }                   
                if (this.EquipmentAttachments != null)
                {
                    hash = hash * 59 + this.EquipmentAttachments.GetHashCode();
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
                if (this.SeniorityAudit != null)
                {
                    hash = hash * 59 + this.SeniorityAudit.GetHashCode();
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
        public static bool operator ==(Equipment left, Equipment right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Not Equals
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Equipment left, Equipment right)
        {
            return !Equals(left, right);
        }

        #endregion Operators
    }
}
