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
    /// The collection of all equipment tracked in the system. Each piece of equipment is of a specific type, is linked to an Equipment Owner, and is managed within a Local Area.
    /// </summary>
        [MetaDataExtension (Description = "The collection of all equipment tracked in the system. Each piece of equipment is of a specific type, is linked to an Equipment Owner, and is managed within a Local Area.")]

    public partial class Equipment : IEquatable<Equipment>
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
        /// <param name="LocalArea">LocalArea.</param>
        /// <param name="EquipmentType">EquipmentType.</param>
        /// <param name="DumpTruckDetails">DumpTruckDetails.</param>
        /// <param name="Owner">Owner.</param>
        /// <param name="EquipCode">A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083..</param>
        /// <param name="Status">The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added..</param>
        /// <param name="ApprovedDate">The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date..</param>
        /// <param name="ReceivedDate">The date the piece of equipment was first received and recorded in HETS..</param>
        /// <param name="AddressLine1">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="AddressLine2">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="AddressLine3">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="AddressLine4">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="City">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="Postal">TO BE REMOVED - display primary contact of owner.</param>
        /// <param name="Comment">TO BE REMOVED - REPLACE WITH NOTES.</param>
        /// <param name="CycleHrsWrk">TO BE REMOVED - CALCULATED FROM TIME RECORDS.</param>
        /// <param name="FrozenOut">TO BE REMOVED - NO LONGER USED.</param>
        /// <param name="LastVerifiedDate">The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme..</param>
        /// <param name="LicencePlate">The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk..</param>
        /// <param name="Make">The make of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Model">The model of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Year">The model year of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="Type">TO BE REVIEWED - WHAT IS THIS?.</param>
        /// <param name="Operator">TO BE REVIEWED - IS THIS NEEDED?.</param>
        /// <param name="PayRate">TO BE REVIEWED - IS THIS NEEDED?.</param>
        /// <param name="RefuseRate">TO BE REVIEWED - IS THIS NEEDED?.</param>
        /// <param name="SerialNum">The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area..</param>
        /// <param name="Size">The size of the piece of equipment, as provided by the Equipment Owner..</param>
        /// <param name="ToDate">WHAT IS THIS?.</param>
        /// <param name="Working">TO BE REMOVED - CALCULATED FROM RENTAL AGREEMENT RECORDS.</param>
        /// <param name="YearEndReg">TO BE REMOVED - BASED ON OLD METHOD OF TRACKING ACTIVE EQUIPMENT.</param>
        /// <param name="PrevRegArea">TO BE REMOVED - AVAILABLE IN THE HISTORY OF THE EQUIPMENT.</param>
        /// <param name="BlockNumber">The current block number for the piece of equipment as calculated by the Seniority Algorthm for this equipment type in the local area. As currently defined y the business  - 1, 2 or Open.</param>
        /// <param name="Seniority">The current seniority calculation result for this piece of equipment. The calculation is based on the &amp;quot;numYears&amp;quot; of service + average hours of service over the last three fiscal years - as stored in the related fields (serviceHoursLastYear, serviceHoursTwoYearsAgo serviceHoursThreeYearsAgo)..</param>
        /// <param name="NumYears">The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY..</param>
        /// <param name="YTD">TO BE REMOVED - Sum of Hours in time cards from the current fiscal year..</param>
        /// <param name="ServiceHoursLastYear">Number of hours of service by this piece of equipment in the previous fiscal year.</param>
        /// <param name="ServiceHoursTwoYearsAgo">Number of hours of service by this piece of equipment in the fiscal year before the last one - e.g. if current year is FY2018 then hours in FY2016.</param>
        /// <param name="ServiceHoursThreeYearsAgo">Number of hours of service by this piece of equipment in the fiscal year three years ago - e.g. if current year is FY2018 then hours in FY2015.</param>
        /// <param name="ArchiveCode">A reason code indicating why a piece of equipment has been archived..</param>
        /// <param name="ArchiveReason">An optional comment about why this piece of equipment has been archived..</param>
        /// <param name="ArchiveDate">The date on which a user most recenly marked this piece of equipment as archived..</param>
        /// <param name="DraftBlockNum">TO BE REMOVED.</param>
        /// <param name="DumpTruck">A link to a dump truck set if this piece of equipment is an equipment type flagged as a dump truck..</param>
        /// <param name="EquipmentAttachments">EquipmentAttachments.</param>
        /// <param name="Notes">Notes.</param>
        /// <param name="Attachments">Attachments.</param>
        /// <param name="History">History.</param>
        /// <param name="SeniorityAudit">SeniorityAudit.</param>
        public Equipment(int Id, LocalArea LocalArea = null, EquipmentType EquipmentType = null, DumpTruck DumpTruckDetails = null, Owner Owner = null, string EquipCode = null, string Status = null, DateTime? ApprovedDate = null, DateTime? ReceivedDate = null, string AddressLine1 = null, string AddressLine2 = null, string AddressLine3 = null, string AddressLine4 = null, string City = null, string Postal = null, string Comment = null, float? CycleHrsWrk = null, string FrozenOut = null, DateTime? LastVerifiedDate = null, string LicencePlate = null, string Make = null, string Model = null, string Year = null, string Type = null, string Operator = null, float? PayRate = null, string RefuseRate = null, string SerialNum = null, string Size = null, DateTime? ToDate = null, string Working = null, string YearEndReg = null, string PrevRegArea = null, float? BlockNumber = null, float? Seniority = null, float? NumYears = null, float? YTD = null, float? ServiceHoursLastYear = null, float? ServiceHoursTwoYearsAgo = null, float? ServiceHoursThreeYearsAgo = null, string ArchiveCode = null, string ArchiveReason = null, DateTime? ArchiveDate = null, float? DraftBlockNum = null, DumpTruck DumpTruck = null, List<EquipmentAttachment> EquipmentAttachments = null, List<Note> Notes = null, List<Attachment> Attachments = null, List<History> History = null, List<SeniorityAudit> SeniorityAudit = null)
        {   
            this.Id = Id;
            this.LocalArea = LocalArea;
            this.EquipmentType = EquipmentType;
            this.DumpTruckDetails = DumpTruckDetails;
            this.Owner = Owner;
            this.EquipCode = EquipCode;
            this.Status = Status;
            this.ApprovedDate = ApprovedDate;
            this.ReceivedDate = ReceivedDate;
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.AddressLine3 = AddressLine3;
            this.AddressLine4 = AddressLine4;
            this.City = City;
            this.Postal = Postal;
            this.Comment = Comment;
            this.CycleHrsWrk = CycleHrsWrk;
            this.FrozenOut = FrozenOut;
            this.LastVerifiedDate = LastVerifiedDate;
            this.LicencePlate = LicencePlate;
            this.Make = Make;
            this.Model = Model;
            this.Year = Year;
            this.Type = Type;
            this.Operator = Operator;
            this.PayRate = PayRate;
            this.RefuseRate = RefuseRate;
            this.SerialNum = SerialNum;
            this.Size = Size;
            this.ToDate = ToDate;
            this.Working = Working;
            this.YearEndReg = YearEndReg;
            this.PrevRegArea = PrevRegArea;
            this.BlockNumber = BlockNumber;
            this.Seniority = Seniority;
            this.NumYears = NumYears;
            this.YTD = YTD;
            this.ServiceHoursLastYear = ServiceHoursLastYear;
            this.ServiceHoursTwoYearsAgo = ServiceHoursTwoYearsAgo;
            this.ServiceHoursThreeYearsAgo = ServiceHoursThreeYearsAgo;
            this.ArchiveCode = ArchiveCode;
            this.ArchiveReason = ArchiveReason;
            this.ArchiveDate = ArchiveDate;
            this.DraftBlockNum = DraftBlockNum;
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
        /// Gets or Sets DumpTruckDetails
        /// </summary>
        public DumpTruck DumpTruckDetails { get; set; }
        
        /// <summary>
        /// Foreign key for DumpTruckDetails 
        /// </summary>       
        [ForeignKey("DumpTruckDetails")]
        public int? DumpTruckDetailsRefId { get; set; }
        
        /// <summary>
        /// Gets or Sets Owner
        /// </summary>
        public Owner Owner { get; set; }
        
        /// <summary>
        /// Foreign key for Owner 
        /// </summary>       
        [ForeignKey("Owner")]
        public int? OwnerRefId { get; set; }
        
        /// <summary>
        /// A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.
        /// </summary>
        /// <value>A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.</value>
        [MetaDataExtension (Description = "A human-visible unique code for the piece of equipment, referenced for convenience by the system users - HETS Clerks and Equipment Owners. Generated based on a unique Equipment owner prefix (e.g. EDW) and a zero-filled unique number - resulting in a code like EDW-0083.")]
        [MaxLength(255)]
        
        public string EquipCode { get; set; }
        
        /// <summary>
        /// The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.
        /// </summary>
        /// <value>The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.</value>
        [MetaDataExtension (Description = "The current status of the equipment in a UI-controlled string. Initial values are Pending, Approved and Archived, but other values may be added.")]
        [MaxLength(255)]
        
        public string Status { get; set; }
        
        /// <summary>
        /// The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.
        /// </summary>
        /// <value>The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.</value>
        [MetaDataExtension (Description = "The date the piece of equipment was first approved in HETS. Part of the seniority calculation for a piece of equipment is based on this date.")]
        public DateTime? ApprovedDate { get; set; }
        
        /// <summary>
        /// The date the piece of equipment was first received and recorded in HETS.
        /// </summary>
        /// <value>The date the piece of equipment was first received and recorded in HETS.</value>
        [MetaDataExtension (Description = "The date the piece of equipment was first received and recorded in HETS.")]
        public DateTime? ReceivedDate { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string AddressLine1 { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string AddressLine2 { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string AddressLine3 { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string AddressLine4 { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string City { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - display primary contact of owner
        /// </summary>
        /// <value>TO BE REMOVED - display primary contact of owner</value>
        [MetaDataExtension (Description = "TO BE REMOVED - display primary contact of owner")]
        [MaxLength(255)]
        
        public string Postal { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - REPLACE WITH NOTES
        /// </summary>
        /// <value>TO BE REMOVED - REPLACE WITH NOTES</value>
        [MetaDataExtension (Description = "TO BE REMOVED - REPLACE WITH NOTES")]
        [MaxLength(255)]
        
        public string Comment { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - CALCULATED FROM TIME RECORDS
        /// </summary>
        /// <value>TO BE REMOVED - CALCULATED FROM TIME RECORDS</value>
        [MetaDataExtension (Description = "TO BE REMOVED - CALCULATED FROM TIME RECORDS")]
        public float? CycleHrsWrk { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - NO LONGER USED
        /// </summary>
        /// <value>TO BE REMOVED - NO LONGER USED</value>
        [MetaDataExtension (Description = "TO BE REMOVED - NO LONGER USED")]
        [MaxLength(255)]
        
        public string FrozenOut { get; set; }
        
        /// <summary>
        /// The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.
        /// </summary>
        /// <value>The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.</value>
        [MetaDataExtension (Description = "The date the equipment was last verified by the HETS Clerk as being still in service in the Local Area and available for the HETS Programme.")]
        public DateTime? LastVerifiedDate { get; set; }
        
        /// <summary>
        /// The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.
        /// </summary>
        /// <value>The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.</value>
        [MetaDataExtension (Description = "The licence plate (if any) of the piece of equipment, as entered by the HETS Clerk.")]
        [MaxLength(255)]
        
        public string LicencePlate { get; set; }
        
        /// <summary>
        /// The make of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The make of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The make of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(255)]
        
        public string Make { get; set; }
        
        /// <summary>
        /// The model of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The model of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(255)]
        
        public string Model { get; set; }
        
        /// <summary>
        /// The model year of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The model year of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The model year of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(255)]
        
        public string Year { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED - WHAT IS THIS?
        /// </summary>
        /// <value>TO BE REVIEWED - WHAT IS THIS?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED - WHAT IS THIS?")]
        [MaxLength(255)]
        
        public string Type { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED - IS THIS NEEDED?
        /// </summary>
        /// <value>TO BE REVIEWED - IS THIS NEEDED?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED - IS THIS NEEDED?")]
        [MaxLength(255)]
        
        public string Operator { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED - IS THIS NEEDED?
        /// </summary>
        /// <value>TO BE REVIEWED - IS THIS NEEDED?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED - IS THIS NEEDED?")]
        public float? PayRate { get; set; }
        
        /// <summary>
        /// TO BE REVIEWED - IS THIS NEEDED?
        /// </summary>
        /// <value>TO BE REVIEWED - IS THIS NEEDED?</value>
        [MetaDataExtension (Description = "TO BE REVIEWED - IS THIS NEEDED?")]
        [MaxLength(255)]
        
        public string RefuseRate { get; set; }
        
        /// <summary>
        /// The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.
        /// </summary>
        /// <value>The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.</value>
        [MetaDataExtension (Description = "The serial number of the piece of equipment as provided by the Equipment Owner. Used to detect and reconcile pieces of equipment moved between Local Areas. Duplicate serial numbers are flagged in the system but permitted. The duplicates are flagged in the UI until the HETS Clerks reconcile the differences - either correcting the serial number or archiving a piece of equipment moved to a new local area.")]
        [MaxLength(255)]
        
        public string SerialNum { get; set; }
        
        /// <summary>
        /// The size of the piece of equipment, as provided by the Equipment Owner.
        /// </summary>
        /// <value>The size of the piece of equipment, as provided by the Equipment Owner.</value>
        [MetaDataExtension (Description = "The size of the piece of equipment, as provided by the Equipment Owner.")]
        [MaxLength(255)]
        
        public string Size { get; set; }
        
        /// <summary>
        /// WHAT IS THIS?
        /// </summary>
        /// <value>WHAT IS THIS?</value>
        [MetaDataExtension (Description = "WHAT IS THIS?")]
        public DateTime? ToDate { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - CALCULATED FROM RENTAL AGREEMENT RECORDS
        /// </summary>
        /// <value>TO BE REMOVED - CALCULATED FROM RENTAL AGREEMENT RECORDS</value>
        [MetaDataExtension (Description = "TO BE REMOVED - CALCULATED FROM RENTAL AGREEMENT RECORDS")]
        [MaxLength(255)]
        
        public string Working { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - BASED ON OLD METHOD OF TRACKING ACTIVE EQUIPMENT
        /// </summary>
        /// <value>TO BE REMOVED - BASED ON OLD METHOD OF TRACKING ACTIVE EQUIPMENT</value>
        [MetaDataExtension (Description = "TO BE REMOVED - BASED ON OLD METHOD OF TRACKING ACTIVE EQUIPMENT")]
        [MaxLength(255)]
        
        public string YearEndReg { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - AVAILABLE IN THE HISTORY OF THE EQUIPMENT
        /// </summary>
        /// <value>TO BE REMOVED - AVAILABLE IN THE HISTORY OF THE EQUIPMENT</value>
        [MetaDataExtension (Description = "TO BE REMOVED - AVAILABLE IN THE HISTORY OF THE EQUIPMENT")]
        [MaxLength(255)]
        
        public string PrevRegArea { get; set; }
        
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
        /// The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.
        /// </summary>
        /// <value>The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.</value>
        [MetaDataExtension (Description = "The number of years of active service of this piece of equipment at the time seniority is calculated - April 1 of the current FY.")]
        public float? NumYears { get; set; }
        
        /// <summary>
        /// TO BE REMOVED - Sum of Hours in time cards from the current fiscal year.
        /// </summary>
        /// <value>TO BE REMOVED - Sum of Hours in time cards from the current fiscal year.</value>
        [MetaDataExtension (Description = "TO BE REMOVED - Sum of Hours in time cards from the current fiscal year.")]
        public float? YTD { get; set; }
        
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
        /// A reason code indicating why a piece of equipment has been archived.
        /// </summary>
        /// <value>A reason code indicating why a piece of equipment has been archived.</value>
        [MetaDataExtension (Description = "A reason code indicating why a piece of equipment has been archived.")]
        [MaxLength(255)]
        
        public string ArchiveCode { get; set; }
        
        /// <summary>
        /// An optional comment about why this piece of equipment has been archived.
        /// </summary>
        /// <value>An optional comment about why this piece of equipment has been archived.</value>
        [MetaDataExtension (Description = "An optional comment about why this piece of equipment has been archived.")]
        [MaxLength(255)]
        
        public string ArchiveReason { get; set; }
        
        /// <summary>
        /// The date on which a user most recenly marked this piece of equipment as archived.
        /// </summary>
        /// <value>The date on which a user most recenly marked this piece of equipment as archived.</value>
        [MetaDataExtension (Description = "The date on which a user most recenly marked this piece of equipment as archived.")]
        public DateTime? ArchiveDate { get; set; }
        
        /// <summary>
        /// TO BE REMOVED
        /// </summary>
        /// <value>TO BE REMOVED</value>
        [MetaDataExtension (Description = "TO BE REMOVED")]
        public float? DraftBlockNum { get; set; }
        
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
        public int? DumpTruckRefId { get; set; }
        
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
            sb.Append("  EquipmentType: ").Append(EquipmentType).Append("\n");
            sb.Append("  DumpTruckDetails: ").Append(DumpTruckDetails).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  EquipCode: ").Append(EquipCode).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  ApprovedDate: ").Append(ApprovedDate).Append("\n");
            sb.Append("  ReceivedDate: ").Append(ReceivedDate).Append("\n");
            sb.Append("  AddressLine1: ").Append(AddressLine1).Append("\n");
            sb.Append("  AddressLine2: ").Append(AddressLine2).Append("\n");
            sb.Append("  AddressLine3: ").Append(AddressLine3).Append("\n");
            sb.Append("  AddressLine4: ").Append(AddressLine4).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  Postal: ").Append(Postal).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  CycleHrsWrk: ").Append(CycleHrsWrk).Append("\n");
            sb.Append("  FrozenOut: ").Append(FrozenOut).Append("\n");
            sb.Append("  LastVerifiedDate: ").Append(LastVerifiedDate).Append("\n");
            sb.Append("  LicencePlate: ").Append(LicencePlate).Append("\n");
            sb.Append("  Make: ").Append(Make).Append("\n");
            sb.Append("  Model: ").Append(Model).Append("\n");
            sb.Append("  Year: ").Append(Year).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Operator: ").Append(Operator).Append("\n");
            sb.Append("  PayRate: ").Append(PayRate).Append("\n");
            sb.Append("  RefuseRate: ").Append(RefuseRate).Append("\n");
            sb.Append("  SerialNum: ").Append(SerialNum).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  ToDate: ").Append(ToDate).Append("\n");
            sb.Append("  Working: ").Append(Working).Append("\n");
            sb.Append("  YearEndReg: ").Append(YearEndReg).Append("\n");
            sb.Append("  PrevRegArea: ").Append(PrevRegArea).Append("\n");
            sb.Append("  BlockNumber: ").Append(BlockNumber).Append("\n");
            sb.Append("  Seniority: ").Append(Seniority).Append("\n");
            sb.Append("  NumYears: ").Append(NumYears).Append("\n");
            sb.Append("  YTD: ").Append(YTD).Append("\n");
            sb.Append("  ServiceHoursLastYear: ").Append(ServiceHoursLastYear).Append("\n");
            sb.Append("  ServiceHoursTwoYearsAgo: ").Append(ServiceHoursTwoYearsAgo).Append("\n");
            sb.Append("  ServiceHoursThreeYearsAgo: ").Append(ServiceHoursThreeYearsAgo).Append("\n");
            sb.Append("  ArchiveCode: ").Append(ArchiveCode).Append("\n");
            sb.Append("  ArchiveReason: ").Append(ArchiveReason).Append("\n");
            sb.Append("  ArchiveDate: ").Append(ArchiveDate).Append("\n");
            sb.Append("  DraftBlockNum: ").Append(DraftBlockNum).Append("\n");
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
                    this.EquipmentType == other.EquipmentType ||
                    this.EquipmentType != null &&
                    this.EquipmentType.Equals(other.EquipmentType)
                ) &&                 
                (
                    this.DumpTruckDetails == other.DumpTruckDetails ||
                    this.DumpTruckDetails != null &&
                    this.DumpTruckDetails.Equals(other.DumpTruckDetails)
                ) &&                 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) &&                 
                (
                    this.EquipCode == other.EquipCode ||
                    this.EquipCode != null &&
                    this.EquipCode.Equals(other.EquipCode)
                ) &&                 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) &&                 
                (
                    this.ApprovedDate == other.ApprovedDate ||
                    this.ApprovedDate != null &&
                    this.ApprovedDate.Equals(other.ApprovedDate)
                ) &&                 
                (
                    this.ReceivedDate == other.ReceivedDate ||
                    this.ReceivedDate != null &&
                    this.ReceivedDate.Equals(other.ReceivedDate)
                ) &&                 
                (
                    this.AddressLine1 == other.AddressLine1 ||
                    this.AddressLine1 != null &&
                    this.AddressLine1.Equals(other.AddressLine1)
                ) &&                 
                (
                    this.AddressLine2 == other.AddressLine2 ||
                    this.AddressLine2 != null &&
                    this.AddressLine2.Equals(other.AddressLine2)
                ) &&                 
                (
                    this.AddressLine3 == other.AddressLine3 ||
                    this.AddressLine3 != null &&
                    this.AddressLine3.Equals(other.AddressLine3)
                ) &&                 
                (
                    this.AddressLine4 == other.AddressLine4 ||
                    this.AddressLine4 != null &&
                    this.AddressLine4.Equals(other.AddressLine4)
                ) &&                 
                (
                    this.City == other.City ||
                    this.City != null &&
                    this.City.Equals(other.City)
                ) &&                 
                (
                    this.Postal == other.Postal ||
                    this.Postal != null &&
                    this.Postal.Equals(other.Postal)
                ) &&                 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) &&                 
                (
                    this.CycleHrsWrk == other.CycleHrsWrk ||
                    this.CycleHrsWrk != null &&
                    this.CycleHrsWrk.Equals(other.CycleHrsWrk)
                ) &&                 
                (
                    this.FrozenOut == other.FrozenOut ||
                    this.FrozenOut != null &&
                    this.FrozenOut.Equals(other.FrozenOut)
                ) &&                 
                (
                    this.LastVerifiedDate == other.LastVerifiedDate ||
                    this.LastVerifiedDate != null &&
                    this.LastVerifiedDate.Equals(other.LastVerifiedDate)
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
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
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
                    this.SerialNum == other.SerialNum ||
                    this.SerialNum != null &&
                    this.SerialNum.Equals(other.SerialNum)
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
                    this.Working == other.Working ||
                    this.Working != null &&
                    this.Working.Equals(other.Working)
                ) &&                 
                (
                    this.YearEndReg == other.YearEndReg ||
                    this.YearEndReg != null &&
                    this.YearEndReg.Equals(other.YearEndReg)
                ) &&                 
                (
                    this.PrevRegArea == other.PrevRegArea ||
                    this.PrevRegArea != null &&
                    this.PrevRegArea.Equals(other.PrevRegArea)
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
                    this.NumYears == other.NumYears ||
                    this.NumYears != null &&
                    this.NumYears.Equals(other.NumYears)
                ) &&                 
                (
                    this.YTD == other.YTD ||
                    this.YTD != null &&
                    this.YTD.Equals(other.YTD)
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
                    this.DraftBlockNum == other.DraftBlockNum ||
                    this.DraftBlockNum != null &&
                    this.DraftBlockNum.Equals(other.DraftBlockNum)
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
                if (this.EquipmentType != null)
                {
                    hash = hash * 59 + this.EquipmentType.GetHashCode();
                }                   
                if (this.DumpTruckDetails != null)
                {
                    hash = hash * 59 + this.DumpTruckDetails.GetHashCode();
                }                   
                if (this.Owner != null)
                {
                    hash = hash * 59 + this.Owner.GetHashCode();
                }                if (this.EquipCode != null)
                {
                    hash = hash * 59 + this.EquipCode.GetHashCode();
                }                
                                if (this.Status != null)
                {
                    hash = hash * 59 + this.Status.GetHashCode();
                }                
                                if (this.ApprovedDate != null)
                {
                    hash = hash * 59 + this.ApprovedDate.GetHashCode();
                }                
                                if (this.ReceivedDate != null)
                {
                    hash = hash * 59 + this.ReceivedDate.GetHashCode();
                }                
                                if (this.AddressLine1 != null)
                {
                    hash = hash * 59 + this.AddressLine1.GetHashCode();
                }                
                                if (this.AddressLine2 != null)
                {
                    hash = hash * 59 + this.AddressLine2.GetHashCode();
                }                
                                if (this.AddressLine3 != null)
                {
                    hash = hash * 59 + this.AddressLine3.GetHashCode();
                }                
                                if (this.AddressLine4 != null)
                {
                    hash = hash * 59 + this.AddressLine4.GetHashCode();
                }                
                                if (this.City != null)
                {
                    hash = hash * 59 + this.City.GetHashCode();
                }                
                                if (this.Postal != null)
                {
                    hash = hash * 59 + this.Postal.GetHashCode();
                }                
                                if (this.Comment != null)
                {
                    hash = hash * 59 + this.Comment.GetHashCode();
                }                
                                if (this.CycleHrsWrk != null)
                {
                    hash = hash * 59 + this.CycleHrsWrk.GetHashCode();
                }                
                                if (this.FrozenOut != null)
                {
                    hash = hash * 59 + this.FrozenOut.GetHashCode();
                }                
                                if (this.LastVerifiedDate != null)
                {
                    hash = hash * 59 + this.LastVerifiedDate.GetHashCode();
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
                                if (this.Type != null)
                {
                    hash = hash * 59 + this.Type.GetHashCode();
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
                                if (this.SerialNum != null)
                {
                    hash = hash * 59 + this.SerialNum.GetHashCode();
                }                
                                if (this.Size != null)
                {
                    hash = hash * 59 + this.Size.GetHashCode();
                }                
                                if (this.ToDate != null)
                {
                    hash = hash * 59 + this.ToDate.GetHashCode();
                }                
                                if (this.Working != null)
                {
                    hash = hash * 59 + this.Working.GetHashCode();
                }                
                                if (this.YearEndReg != null)
                {
                    hash = hash * 59 + this.YearEndReg.GetHashCode();
                }                
                                if (this.PrevRegArea != null)
                {
                    hash = hash * 59 + this.PrevRegArea.GetHashCode();
                }                
                                if (this.BlockNumber != null)
                {
                    hash = hash * 59 + this.BlockNumber.GetHashCode();
                }                
                                if (this.Seniority != null)
                {
                    hash = hash * 59 + this.Seniority.GetHashCode();
                }                
                                if (this.NumYears != null)
                {
                    hash = hash * 59 + this.NumYears.GetHashCode();
                }                
                                if (this.YTD != null)
                {
                    hash = hash * 59 + this.YTD.GetHashCode();
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
                                if (this.DraftBlockNum != null)
                {
                    hash = hash * 59 + this.DraftBlockNum.GetHashCode();
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
